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

            //set the end time of the session
            session.SetEndTime(DateTime.Now);

            var bbuToRelease = 0;
            foreach (var service in services)
            {
                bbuToRelease += service.ComputeRequiredCapacity();
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



            //Set refference to session object to null
            session = null;

            Log.Information($"----Session Termination Successful");
        }

        public void TerminateCall(Guid callId)
        {
            Log.Information($"Terminating call id: @{callId}; terminal: @{this.MobileTerminalId}; session: @{this.SessionId}");

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

            //Update the mobile state based on the remaining active calls
            var state = GetStateFromCurrentSession(session.ActiveCalls);

            //If state is idle terminate session
            if (state == MobileTerminalState.Idle)
            {
                this.TerminateSession();
                return;
            }

            //Set the new Mobile Terminal State
            this.SetState(state);

            //Update the session sequence
            session.SessionSequence.Add(state);

            //Delete call
            call = null;

            Log.Information($"Call Ended Successfully call id: @{callId}; terminal: @{this.MobileTerminalId}; session: @{this.SessionId}");
        }

        private MobileTerminalState GetStateFromCurrentSession(IList<ICall> activeCalls)
        {
            var state = MobileTerminalState.Idle;
            if (activeCalls.Count == 3) return MobileTerminalState.VoiceDataVideo;
            if (activeCalls.Count == 1)
            {
                switch (activeCalls.ElementAt(0).Service)
                {
                    case Service.Voice:
                        return MobileTerminalState.Voice;
                    case Service.Data:
                        return MobileTerminalState.Data;
                    case Service.Video:
                        return MobileTerminalState.Video;
                }
            }
            if (activeCalls.Count == 2)
            {
                var services = new List<Service>();
                foreach (var call in activeCalls)
                {
                    services.Add(call.Service);
                }
                if (services.Intersect(new List<Service> { Service.Voice, Service.Data }).Count() == services.Count())
                {
                    return MobileTerminalState.VoiceData;
                }
                if (services.Intersect(new List<Service> { Service.Voice, Service.Video }).Count() == services.Count())
                {
                    return MobileTerminalState.VoiceData;
                }
                if (services.Intersect(new List<Service> { Service.Video, Service.Data }).Count() == services.Count())
                {
                    return MobileTerminalState.VideoData;
                }
            }
            return state; //If state is idle go on to terminate the session
        }

        private MobileTerminalState SetState(MobileTerminalState state)
        {
            State = state;
            return state;
        }
    }
}