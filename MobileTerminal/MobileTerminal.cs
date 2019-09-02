using System;
using System.Collections.Generic;
using VerticalHandoverPrediction.CallSession;

namespace VerticalHandoverPrediction.Mobile
{

    public class MobileTerminal : IMobileTerminal
    {
        public Guid MobileTerminalId { get; private set; }
        public Guid SessionId { get; private set; }
        public Modality Modality { get; private set; }
        public MobileTerminalState State { get; private set; }
        public IList<ISession> CallHistoryLogs { get; private set; }

        private MobileTerminal(Modality modality)
        {
            MobileTerminalId = Guid.NewGuid();
            Modality = modality;
            CallHistoryLogs = new List<ISession>();
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

    }
}