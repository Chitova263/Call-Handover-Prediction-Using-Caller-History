namespace VerticalHandoverPrediction.CallAdimissionControl
{
    using System.Linq;
    using VerticalHandoverPrediction.Mobile;
    using VerticalHandoverPrediction.Network;
    using VerticalHandoverPrediction.CallSession;
    using System;
    using static MoreLinq.Extensions.StartsWithExtension;
    using System.Collections.Generic;
    using VerticalHandoverPrediction.Utils;
    using VerticalHandoverPrediction.Simulator.Events;
    using Serilog;

    public class Cac
    {
        public bool Predictive { get; set; }

        public Cac(bool predictive)
        {
            Predictive = predictive;
        }

        public void AdmitCall(CallStartedEvent evt)
        {
            if (evt is null)
                throw new VerticalHandoverPredictionException($"{nameof(evt)} is null");

            var mobileTerminal = HetNet.Instance.MobileTerminals
                .FirstOrDefault(x => x.MobileTerminalId == evt.Call.MobileTerminalId);
            
            if(mobileTerminal.State != MobileTerminalState.Idle)
            {
                var session = HetNet.Instance.Rats
                    .SelectMany(x => x.OngoingSessions)
                    .FirstOrDefault(x => x.SessionId == mobileTerminal.SessionId);
                
                //------------- Cant have a voice call admitted when we have another voice call active in same session -----------------
                var services = session.ActiveCalls.Select(x => x.Service);
                if(services.Contains(evt.Call.Service))
                {
                    HetNet.Instance.CallStartedEventsRejectedWhenNotIdle++;
                    //Log.Warning($"There is a @{evt.Call.Service} call active in session @{session.SessionId}");
                    return;
                }

                Simulator.NetworkSimulator.Instance.Events.Add(evt);
                HetNet.Instance.CallsGenerated++;

                var rat = HetNet.Instance.Rats
                    .FirstOrDefault(x => x.RatId == session.RatId);

                if(rat.CanAdmitNewCallToOngoingSession(session, evt.Call, mobileTerminal))
                {
                    rat.CanAdmitNewCallToOngoingSession(session, evt.Call, mobileTerminal);
                    rat.AdmitNewCallToOngoingSession(session, evt.Call, mobileTerminal);
                    return;
                }

                HetNet.Instance.Handover(evt.Call, session, mobileTerminal, rat);

                return;
            }
            //If Idle
            else
            {
                if(mobileTerminal.IsActive)
                {
                    //Log.Warning("Session Already Terminated");
                    HetNet.Instance.CallStartedEventsRejectedWhenIdle++;
                    return;
                }

                //Mobile Terminal is now active
                mobileTerminal.SetActive(true);

                //Call successfuly generated
                Simulator.NetworkSimulator.Instance.Events.Add(evt);
                HetNet.Instance.CallsGenerated++;

                if(Predictive)
                {
                    //Number of calls to be used for prediction -- use this to compute success rate --
                    HetNet.Instance.CallsToBePredictedInitialRatSelection++;
                    RunPredictiveAlgorithm(evt, mobileTerminal);
                }
                else
                {
                    RunNonPredictiveAlgorithm(evt, mobileTerminal);
                }        
            }
        }

        private void RunPredictiveAlgorithm(CallStartedEvent evt, IMobileTerminal mobileTerminal)
        {
            var history = CsvUtils._Instance.Read<CallLogMap,CallLog>($"{Environment.CurrentDirectory}/calllogs.csv").ToList();
            
            var nextState =  evt.Call.Service.GetState();

            if(history.Any())
            {    
                var group = history
                    .Where(x => x.UserId == mobileTerminal.MobileTerminalId)
                    .Select(x => x.SessionSequence)
                    .Select(x => x.ToList().Select(x =>(MobileTerminalState)(int.Parse(x.ToString()))))
                    .Select(x => x.Skip(1).Take(2))
                    .Where(x => x.StartsWith(new List<MobileTerminalState>{evt.Call.Service.GetState()})                  )
                    .SelectMany(x => x.Skip(1))
                    .Where(x => x != MobileTerminalState.Idle)
                    .GroupBy(x => x);
                   
                //If group is empty it means prediction has failed
                if(!group.Any()) 
                {
                    HetNet.Instance.FailedPredictions++;
                }
                else
                {
                    //continue to compute frequency table
                    var prediction = default(IGrouping<MobileTerminalState, MobileTerminalState>);
                    var max = 0;

                    //If There are ties it takes the last item in the history : Fix this, what decision is made in that case
                    
                    //group.Dump();

                    foreach(var grp in group )
                    {
                        if(grp.Count() > max) 
                        {
                            prediction = grp;
                            max = prediction.Count();
                        }
                        //Console.WriteLine( $"next state is {grp.Key}, Frequency: {grp.Count()}");
                    }
                    nextState = prediction.Key;
                }
            }

            var services = new List<Service>{evt.Call.Service};
            foreach (var service in nextState.SupportedServices())
            {
                if(!services.Contains(service)) services.Add(service);
            }

            var rats = HetNet.Instance.Rats
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
                    if(evt.Call.Service.GetState() != nextState) 
                    {
                        //Successful Prediction means 1. predicted state was not idle 2. call ends up being admited as predicted
                        HetNet.Instance.SuccessfulPredictions++;
                        //Log.Information("----- Successful prediction");
                    } 
                    return;
                }
            }
            //All Possible Rats Cannot Admit Call i.e [call in its predicted state]
            HetNet.Instance.BlockedUsingPredictiveScheme++;

            //Try just accommodating the incoming call without predicting before blocking it
            RunNonPredictiveAlgorithm(evt, mobileTerminal);
        }

        private void RunNonPredictiveAlgorithm(CallStartedEvent evt, IMobileTerminal mobileTerminal)
        {
            var rats = HetNet.Instance.Rats
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
            HetNet.Instance.BlockedCalls++;
        }

        private void StartNewSessionAndAdmitCall(CallStartedEvent evt, IMobileTerminal mobileTerminal, IRat rat)
        {
            var session = new Session(rat.RatId, evt.Time);

            mobileTerminal.SetSessionId(session.SessionId);

            rat.TakeNetworkResources(evt.Call.Service.ComputeRequiredNetworkResources());

            session.AddToActiveCalls(evt.Call);

            var state = mobileTerminal.UpdateMobileTerminalState(session);

            session.AddToSessionSequence(state);

            rat.AddSession(session);
        }
    }
}