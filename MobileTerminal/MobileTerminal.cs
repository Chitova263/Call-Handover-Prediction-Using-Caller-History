using System;
using System.Collections.Generic;
using System.Linq;
using Serilog;
using VerticalHandoverPrediction.CallAdmissionControl;
using VerticalHandoverPrediction.CallSession;
using VerticalHandoverPrediction.Events;
using VerticalHandoverPrediction.Network;
using VerticalHandoverPrediction.Utils;

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

        public void TerminateCall(CallEndedEvent evt)
        {
            //Find the current session and obtain the RatId
            var session = HetNet._HetNet.Rats
                .SelectMany(x => x.OngoingSessions)
                .FindSessionWithId(SessionId);

            System.Console.WriteLine("------------ Session ------------");
            session.Dump();

            //Call never existed
            if (session is null)
            {
                evt = null;
                return; 
            }

            var rat = HetNet._HetNet.Rats
                .FirstOrDefault(x => x.RatId == session.RatId);

            //Find the call in the list of active calls
            var call = session.ActiveCalls
                .FirstOrDefault(x => x.CallId == evt.CallId);

            System.Console.WriteLine("------------ Call ------------");
            session.Dump();

            //There is an existing session but call never existed
            if (call is null)
            {
                evt = null;
                return; 
            } 

            Log.Information($"Terminating call service: @{call.Service}; session: @{this.SessionId}");
            
            session.ActiveCalls.Remove(call);

            //Must be called after removing the call from active calls
            var state = GetStateFromCurrentSession(session.ActiveCalls);

            SetState(state);

            rat.SetUsedResources(rat.UsedResources - call.Service.ComputeRequiredCapacity());

            if(state == MobileTerminalState.Idle)
            {
                //Remove the session from the Rat 
                rat.RemoveSession(session);
                session.SetEndTime(evt.Time);
                session.SessionSequence.Add(State);
                this.SetSessionId(Guid.Empty);
                var callHistory = new CallLog
                {
                    SessionId = session.SessionId,
                    Start = session.Start,
                    RatId = session.RatId,
                    End = session.End,
                    SessionSequence = session.SessionSequence
                };

                Log.Information($"---- Saving history");

                CallHistoryLogs.Add(callHistory);

                Log.Information($"---- Session ended @{session.SessionId}");

                session = null;

                return;
            }

            session.SessionSequence.Add(state);

            Log.Information($"Call terminated service: @{call.Service}; session: @{this.SessionId}");

            session.Dump();

            call = null;
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