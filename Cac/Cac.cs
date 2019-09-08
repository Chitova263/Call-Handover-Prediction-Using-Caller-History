using System.Linq;
using VerticalHandoverPrediction.Mobile;
using VerticalHandoverPrediction.Network;
using VerticalHandoverPrediction.Simulator;
using VerticalHandoverPrediction.CallAdmissionControl;
using VerticalHandoverPrediction.CallSession;
using Serilog;

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
                RunNonPredictiveAlgorithm(evt, mobileTerminal);
                return;
            }
        }

        private void RunNonPredictiveAlgorithm(CallStartedEvent evt, IMobileTerminal mobileTerminal)
        {
            HetNet._HetNet.CallsGenerated++;

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