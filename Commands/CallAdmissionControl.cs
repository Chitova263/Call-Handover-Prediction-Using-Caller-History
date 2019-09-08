using System;
using System.Collections.Generic;
using System.Linq;
using static MoreLinq.Extensions.StartsWithExtension;
using VerticalHandoverPrediction.CallAdmissionControl;
using VerticalHandoverPrediction.CallSession;
using VerticalHandoverPrediction.Mobile;
using VerticalHandoverPrediction.Network;
using Serilog;

namespace VerticalHandoverPrediction.Commands
{
    /*
    public class JCallAdmissionControl
    {
        private JCallAdmissionControl()
        {
            
        }

        public static JCallAdmissionControl StartCAC(){
            return new JCallAdmissionControl();
        }

        public void AdmitCall(ICall call)
        {
            var mobileTerminal = HetNet._HetNet.MobileTerminals
                .FirstOrDefault(m => m.MobileTerminalId == call.MobileTerminalId);
            
            //No prediction occurs on this path
            if(mobileTerminal.State != MobileTerminalState.Idle)
            {
                var session = HetNet._HetNet.Rats
                    .SelectMany(x => x.OngoingSessions)
                    .FindSessionWithId(mobileTerminal.SessionId);
                
                var isTypeOfCallAlreadyInSession = session.ActiveCalls
                    .Select(x => x.Service)
                    .Contains(call.Service);

                if(isTypeOfCallAlreadyInSession) return;

                HetNet._HetNet.CallsGenerated++;

                var rat = HetNet._HetNet.Rats
                    .FirstOrDefault(x => x.RatId == session.RatId);
                
                //Try Admiting call on the ongoing session on current Rat otherwise try to handover to another Rat

                if(rat.CanAccommodateCall(call))
                {
                    rat.AdmitIncomingCallToOngoingSession(call, session, mobileTerminal);
                }
                else
                {
                    PerfomHandover(call, session, rat, mobileTerminal);
                    return;
                }
            }
            else
            {
                HandoverPrediction(call, mobileTerminal);
            }
        }

        





        //Helper methods
        private void PerfomHandover(ICall call, ISession session, IRat srcRat, IMobileTerminal mobileTerminal)
        {
            var callsInActiveSession = session.ActiveCalls
                .Select(x => x.Service)
                .ToList();
            
            callsInActiveSession.Add(call.Service);

            var rats = HetNet._HetNet.Rats
                .Where(x => x.RatId != session.RatId
                    && x.Services.ToHashSet().IsSupersetOf(callsInActiveSession))
                .OrderBy(x => x.Services.Count());
            
            var requiredResources = 0;
            foreach (var service in callsInActiveSession)
            {
                requiredResources += service.ComputeRequiredCapacity();
            }

            //Check if there is a Rat with enough resources to handover the session
            foreach (var destRat in rats)
            {
                if(requiredResources <= destRat.AvailableCapacity())
                {
                    HetNet._HetNet.HandoverSessionToNewRat(call, session, srcRat, destRat, mobileTerminal);
                    HetNet._HetNet.VerticalHandovers++;
                    return;
                }
            }

            //if you exit this it means there is no Rat that has enough resources to accommodate the handover session
            HetNet._HetNet.BlockedCalls++;    
        }

        private void HandoverPrediction(ICall call, IMobileTerminal mobileTerminal)
        {
            if (call is null)
            {
                throw new ArgumentNullException(nameof(call));
            }

            if (mobileTerminal is null)
            {
                throw new ArgumentNullException(nameof(mobileTerminal));
            }

            var history = mobileTerminal.CallHistoryLogs;

            if(!history.Any())
            {
                //Just Find Any Suitable Rat To Admit The Call
                RunNonPredictiveAlgorithm(call, mobileTerminal);
                return;
            }
            else
            {
                //Predict the next state
                var sessionSequenceHistory = history.Select(x => x.SessionSequence);

                var group = sessionSequenceHistory
                    .Select(x => x.Skip(1).Take(2))
                    .Where(x => x.StartsWith(new List<MobileTerminalState>{call.Service.GetState()}))
                    .SelectMany(x => x.Skip(1))
                    .GroupBy(x => x);
                
                //if no match with incoming call means group will be empty
                if(group.Count() == 0)
                {
                    HetNet._HetNet.FailedPredictions++;
                    //First Find Some Other Rat to accommodate call in its 
                    RunNonPredictiveAlgorithm(call, mobileTerminal);
                    return;
                }
                else
                {
                    //predict the next state
                    IGrouping<MobileTerminalState, MobileTerminalState> prediction = default(IGrouping<MobileTerminalState, MobileTerminalState>);
                    int max = 0;
                    foreach(var grp in group )
                    {
                        if(grp.Count() > max) 
                        {
                            prediction = grp;
                            max = prediction.Count();
                        }
                    }
                    var nextState = prediction.Key;
                    
                    Log.Information($"---- Predicted state: @{nextState.ToString()}");

                    HetNet._HetNet.SuccessfulPredictions++;

                    var services = new List<Service>{call.Service};
                    foreach (var service in nextState.SupportedServices())
                    {
                        if(!services.Contains(service)) services.Add(service);
                    }

                    var requiredResources = 0;
                    
                    foreach (var service in services)
                    {
                        requiredResources += service.ComputeRequiredCapacity();
                    }

                    var rats = HetNet._HetNet.Rats
                        .Where(x => x.Services.ToHashSet().IsSupersetOf(services))
                        .OrderBy(x => x.Services.Count);
                    
                    //Find Rats capable of admiting call in its predicted state
                    foreach (var rat in rats)
                    {
                        if(requiredResources <= rat.AvailableCapacity())
                        {
                            rat.AdmitIncomingCallToNewSession(call, mobileTerminal);
                            return;
                        }
                    }
                    //If cant accommodate in predicted state, try normal admission of the call
                    RunNonPredictiveAlgorithm(call, mobileTerminal);
                    return;
                }
            }
        }

        private void RunNonPredictiveAlgorithm(ICall call, IMobileTerminal mobileTerminal)
        {
            var rats = HetNet._HetNet.Rats
                    .Where(x => x.Services.Contains(call.Service))
                    .OrderBy(x => x.Services.Count);

                foreach (var rat in rats)
                {
                    if(rat.CanAccommodateCall(call))
                    {
                        rat.AdmitIncomingCallToNewSession(call, mobileTerminal);
                        HetNet._HetNet.CallsGenerated++;
                        return;
                    }
                }
                //No rats have enough resources to get new call in
                HetNet._HetNet.BlockedCalls++;
        }
    }
     */
}