using System.Linq;
using VerticalHandoverPrediction.Mobile;
using VerticalHandoverPrediction.Network;
using VerticalHandoverPrediction.Simulator;
using VerticalHandoverPrediction.CallAdmissionControl;
using VerticalHandoverPrediction.CallSession;
using Serilog;
using System;
using static MoreLinq.Extensions.StartsWithExtension;
using System.Collections.Generic;
using VerticalHandoverPrediction.Utils;
using System.IO;
using CsvHelper;

namespace VerticalHandoverPrediction.Cac
{
    public class Cac
    {
        public Cac()
        {
            
        }

        public void AdmitCall(CallStartedEvent evt)
        {
            var mobileTerminal = HetNet._HetNet.MobileTerminals
                .FirstOrDefault(x => x.MobileTerminalId == evt.Call.MobileTerminalId);
            
            //IF mobile terminal is not idle
            if(mobileTerminal.State != MobileTerminalState.Idle)
            {
                var session = HetNet._HetNet.Rats
                    .SelectMany(x => x.OngoingSessions)
                    .FirstOrDefault(x => x.SessionId == mobileTerminal.SessionId);
                
                //------------- Cant have a voice call admitted when we have another voice call active in same session -----------------
                var services = session.ActiveCalls.Select(x => x.Service);
                if(services.Contains(evt.Call.Service))
                {
                    HetNet._HetNet.CallStartedEventsRejectedWhenNotIdle++;
                    Log.Warning($"There is a @{evt.Call.Service} call active in session @{session.SessionId}");
                    return;
                }

                //------------------------------------------------------------------------------------------------


                HetNet._HetNet.CallsGenerated++;

                var rat = HetNet._HetNet.Rats
                    .FirstOrDefault(x => x.RatId == session.RatId);

                if(rat.CanAdmitNewCallToOngoingSession(session, evt.Call, mobileTerminal))
                {
                    rat.CanAdmitNewCallToOngoingSession(session, evt.Call, mobileTerminal);
                    rat.AdmitNewCallToOngoingSession(session, evt.Call, mobileTerminal);
                    return;
                }

                HetNet._HetNet.Handover(evt.Call, session, mobileTerminal, rat);

                return;
            }
            //IF mobile terminal was idle
            else
            {
                //----------------
                if(mobileTerminal.Activated)
                {
                    Log.Warning("Session Already Terminated");
                    HetNet._HetNet.CallStartedEventsRejectedWhenIdle++;
                    return;
                    //throw new VerticalHandoverPredictionException(" Wrong state");
                }
                //----------------

                mobileTerminal.Activated = true;

                HetNet._HetNet.CallsGenerated++;
                HetNet._HetNet.CallsToBePredictedInitialRatSelection++;
                
                //---------------- Refactor to choose scheme to use when simulator is started
                //RunNonPredictiveAlgorithm(evt, mobileTerminal);
                RunPredictiveAlgorithm(evt, mobileTerminal);
                //---------------- 
                
                return;
            }
        }

        private void RunPredictiveAlgorithm(CallStartedEvent evt, IMobileTerminal mobileTerminal)
        {
            var callHistory = mobileTerminal.CallHistoryLogs
                .Select(x => x.SessionSequence)
                .ToList();
            
            var nextState =  default(MobileTerminalState);
            if(callHistory.Any()) 
            {
                nextState = evt.Call.Service.GetState();
            }
            else
            {
                var x = new List<CallLog>();
                var str = @"/Users/DjMadd/Documents/Thesis/VerticalHandoverPrediction";
                using (var reader = new StreamReader($"{str}/jkby.csv"))
                using (var csv = new CsvReader(reader))
                {
                    var records = csv.GetRecords<CallLog>();
                    x = records.ToList();
                }
               

                //Algorithm to predict next state
                //var group = callHistory
                var group = x
                    .Where(x => x.UserId == mobileTerminal.MobileTerminalId)
                    .Select(x => x.SessionSequence)
                    .Select(x => x.ToList().Select(x =>(MobileTerminalState)(int.Parse(x.ToString()))))
                    .Select(x => x.Skip(1).Take(2))
                    .Where(x => x.StartsWith(new List<MobileTerminalState>{evt.Call.Service.GetState()}))
                    .SelectMany(x => x.Skip(1))
                    .GroupBy(x => x);
                
                //If group is empty it means prediction has failed
                if(!group.Any()) 
                {
                    HetNet._HetNet.FailedPredictions++;
                    nextState = evt.Call.Service.GetState();
                }
                else
                {
                    var prediction = default(IGrouping<MobileTerminalState, MobileTerminalState>);
                    var max = 0;

                    //If There are ties it takes the last item in the history : Fix this, what decision is made in that case
                    
                    group.Dump();

                    foreach(var grp in group )
                    {
                        if(grp.Count() > max) 
                        {
                            prediction = grp;
                            max = prediction.Count();
                        }
                        Console.WriteLine( $"next state is {grp.Key}, Frequency: {grp.Count()}");
                    }

                    foreach( var grp in group )
                    {
                        //Generate Excell File With Frequency Table
                        Console.WriteLine( $"next state is {grp.Key}, Frequency: {grp.Count()}");
                    }

                    nextState = prediction.Key;
                }
            
                //What happens if nextState is Idle
                if(nextState == MobileTerminalState.Idle)
                {
                    //Fix program goes into an unstable state
                    throw new VerticalHandoverPredictionException("Next state predicted is idle");
                }
            }

            var services = new List<Service>{evt.Call.Service};
            foreach (var service in nextState.SupportedServices())
            {
                if(!services.Contains(service)) services.Add(service);
            }

            var rats = HetNet._HetNet.Rats
                .Where(x => x.Services.ToHashSet().IsSupersetOf(services))
                .OrderBy(x => x.Services.Count)
                .ToList();
            
            //This scheme reserves bandwidth in advance, I considered all the services
            var requiredNetworkResources = 0;
            foreach (var service in services)
            {
                requiredNetworkResources += service.ComputeRequiredNetworkResources();
            }

            foreach (var rat in rats)
            {
                if(requiredNetworkResources <= rat.AvailableNetworkResources())
                {
                    StartNewSessionAndAdmitCall(evt, mobileTerminal, rat);
                    if(!callHistory.Any()) 
                    {
                        HetNet._HetNet.SuccessfulPredictions++;
                        Log.Information("----- Successful prediction");
                    } 
                    return;
                }
            }
            //All Possible Rats Cannot Admit Call i.e [call in its predicted state]
            HetNet._HetNet.BlockedUsingPredictiveScheme++;

            //Try just accommodating the incoming call without predicting before blocking it
            RunNonPredictiveAlgorithm(evt, mobileTerminal);
            return;
        }

        private void RunNonPredictiveAlgorithm(CallStartedEvent evt, IMobileTerminal mobileTerminal)
        {
            var rats = HetNet._HetNet.Rats
                .Where(x => x.Services.Contains(evt.Call.Service))
                .OrderBy(x => x.Services.Count)
                .ToList();
            
            foreach (var rat in rats)
            {
                if(evt.Call.Service.ComputeRequiredNetworkResources() <= rat.AvailableNetworkResources())
                {
                    StartNewSessionAndAdmitCall(evt, mobileTerminal, rat);
                    return;
                }
            }
            HetNet._HetNet.BlockedCalls++;
            return;
        }

        private void StartNewSessionAndAdmitCall(CallStartedEvent evt, IMobileTerminal mobileTerminal, IRat rat)
        {
            var session = Session.StartSession(rat.RatId, evt.Time);

            mobileTerminal.SetSessionId(session.SessionId);

            rat.TakeNetworkResources(evt.Call.Service.ComputeRequiredNetworkResources());

            session.ActiveCalls.Add(evt.Call);

            var state = mobileTerminal.UpdateMobileTerminalState(session);

            session.SessionSequence.Add(state);

            rat.AddSession(session);
        }
    }
}