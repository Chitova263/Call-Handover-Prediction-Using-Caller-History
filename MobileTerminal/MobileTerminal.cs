using System;
using System.Collections.Generic;
using System.Linq;
using Serilog;
using VerticalHandoverPrediction.CallAdmissionControl;
using VerticalHandoverPrediction.CallSession;
using VerticalHandoverPrediction.Network;

namespace VerticalHandoverPrediction.Mobile
{

    public class MobileTerminal : IMobileTerminal
    {
        public Guid MobileTerminalId { get; private set; }
        public Guid SessionId { get; private set; }
        public Modality Modality { get; private set; }
        public MobileTerminalState State { get; private set; }
        public IList<CallLog> CallHistoryLogs { get; private set; }

        private MobileTerminal(Modality modality)
        {
            MobileTerminalId = Guid.NewGuid();
            Modality = modality;
            CallHistoryLogs = new List<CallLog>();
            State = MobileTerminalState.Idle;
        }

        public static MobileTerminal CreateMobileTerminal(Modality modality)
        {
            return new MobileTerminal(modality);
        }

        public MobileTerminalState ChangeStateTo(MobileTerminalState state)
        {
            State = state;
            //Return the updated state
            return State;
        }

        public void SetSessionId(Guid sessionId)
        {
            SessionId = sessionId;//set when we initiate new session
        }


        public MobileTerminalState UpdateMobileTerminalState(Service service)
        {
            return this.UpdateStateExtension(service);
        }

        public void TerminateSession()
        {

            //Find the current session and obtain the RatId
            var session = HetNet._HetNet.Rats
                .SelectMany(x => x.OngoingSessions)
                .FindSessionWithId(SessionId);

            //Find the Rat handling session
            var rat = HetNet._HetNet.Rats
                .FirstOrDefault(x => x.RatId == session.RatId);

            Log.Information($"---- Session Termination Started @{session.SessionId}");
            //Free up the resources used by session
            var services = session.ActiveCalls
                .Select(x => x.Service);

            var bbuToRelease = 0;
            foreach (var service in services)
            {
                bbuToRelease -= service.ComputeRequiredCapacity();
            }
            rat.SetUsedCapacity(rat.UsedCapacity - bbuToRelease);

            //Remove Session from list of ongoing sessions on rat
            rat.OngoingSessions.Remove(session);

            //update mobile terminal
            SetSessionId(Guid.Empty);
            var state = SetState(MobileTerminalState.Idle);

            //Update session sequence
            session.SessionSequence.Add(state);

            //save session to callLogHistory : Consider changing this to a hashmap
            var callHistory = new CallLog
            {
                SessionId = session.SessionId,
                Start = session.Start,
                RatId = session.RatId,
                End = session.End,
                SessionSequence = session.SessionSequence
                //No need to save the list of active calls on the history data store
            };

            Log.Information($"---- Saving Session @{session.SessionId}");
            CallHistoryLogs.Add(callHistory);

            Log.Information($"---- Terminating Session @{session.SessionId}");

            //set the end time of the session
            session.SetEndTime(DateTime.Now);

            //Set refference to session object to null
            session = null;

            Log.Information($"----Session Termination Successful");
        }

        public void TerminateCall(Guid callId)
        {
            //Find the current session and obtain the RatId
            var session = HetNet._HetNet.Rats
                .SelectMany(x => x.OngoingSessions)
                .FindSessionWithId(SessionId);

            //Find the Rat handling session
            var rat = HetNet._HetNet.Rats
                .FirstOrDefault(x => x.RatId == session.RatId);

            //Find the call in the list of active calls
            var call = session.ActiveCalls
                .FirstOrDefault(x => x.CallId == callId);

            //Remove call from the list of Active calls
            session.ActiveCalls.Remove(call);

            //Free up resources in Rat used by the service of call
            rat.SetUsedCapacity(
                rat.UsedCapacity - call.Service.ComputeRequiredCapacity()
                );

            var services = session.ActiveCalls
                .Select(x => x.Service)
                .ToList();

            //Update the mobile state based on remaining services
            DeriveStateFromServices(services);

            //update the session sequence

            //Delete call
            call = null;
        }

        private MobileTerminalState DeriveStateFromServices(IList<Service> services)
        {
            throw new NotImplementedException();  //Edge cases
        }

        private MobileTerminalState SetState(MobileTerminalState state)
        {
            State = state;
            return state;
        }
    }
}