using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

//Make sessionId and RatId to null upon termination


namespace VerticalHandoverPrediction
{

    [JsonObject(IsReference = true)]
    public class MobileTerminal : IMobileTerminal
    {
        public Guid MobileTerminalId { get; private set; }
        public Guid SessionId { get; private set; }
        public Guid RATId { get; private set; }
        public IList<ISession> CallHistoryLog { get; private set; }
        public MobileTerminalState State { get; private set; }
        public MobileTerminalModality Mode { get; private set; }

        private MobileTerminal(MobileTerminalModality mode)
        {
            MobileTerminalId = Guid.NewGuid();
            State = MobileTerminalState.Idle;
            Mode = mode;
            CallHistoryLog = new List<ISession>();
        }

        public static MobileTerminal CreateMobileTerminal(MobileTerminalModality mode)
        {
            return new MobileTerminal(mode);
        }

        public void SetSessionId(Guid sessionId) => SessionId = sessionId;

        public void UpdateCallHistoryLog(ISession session)
        {
            CallHistoryLog.Add(session);
        }

        public void SetRATId(Guid ratId) => RATId = ratId;

        public void SetMobileTerminalState(MobileTerminalState state) => State = state;

        public void TerminateSession(IHetNet hetNet)
        {
            var session = hetNet.RATs
                .FirstOrDefault(x => x.RATId == this.RATId)
                .OngoingSessions
                .FirstOrDefault(x => x.SessionId == this.SessionId);

            session.TerminateSession((IMobileTerminal)this, hetNet);
            this.SessionId = this.RATId = default(Guid);
        }

        public MobileTerminalState UpdateMobileTerminalState(Service service)
        {
            // Refactor Code
            switch (State)
            {
                case MobileTerminalState.Idle:
                    if (service.Equals(Service.Voice)) State = MobileTerminalState.Voice;
                    if (service.Equals(Service.Data)) State = MobileTerminalState.Data;
                    if (service.Equals(Service.Video)) State = MobileTerminalState.Video;
                    break;
                case MobileTerminalState.Data:
                    if (service.Equals(Service.Voice)) State = MobileTerminalState.VoiceData;
                    if (service.Equals(Service.Data)) throw new HandoverPredictionException();
                    if (service.Equals(Service.Video)) State = MobileTerminalState.VideoData;
                    break;
                case MobileTerminalState.Video:
                    if (service.Equals(Service.Voice)) State = MobileTerminalState.VoiceVideo;
                    if (service.Equals(Service.Data)) State = MobileTerminalState.VideoData;
                    if (service.Equals(Service.Video)) throw new HandoverPredictionException();
                    break;
                case MobileTerminalState.Voice:
                    if (service.Equals(Service.Voice)) throw new HandoverPredictionException();
                    if (service.Equals(Service.Data)) State = MobileTerminalState.VoiceData;
                    if (service.Equals(Service.Video)) State = MobileTerminalState.VoiceVideo;
                    break;
                case MobileTerminalState.VideoData:
                    if (service.Equals(Service.Voice)) State = MobileTerminalState.VoiceDataVideo;
                    if (service.Equals(Service.Data)) throw new HandoverPredictionException();
                    if (service.Equals(Service.Video)) throw new HandoverPredictionException();
                    break;
                case MobileTerminalState.VoiceData:
                    if (service.Equals(Service.Voice)) throw new HandoverPredictionException();
                    if (service.Equals(Service.Data)) throw new HandoverPredictionException();
                    if (service.Equals(Service.Video)) State = MobileTerminalState.VoiceDataVideo;
                    break;
                case MobileTerminalState.VoiceVideo:
                    if (service.Equals(Service.Voice)) throw new HandoverPredictionException();
                    if (service.Equals(Service.Data)) State = MobileTerminalState.VoiceDataVideo;
                    if (service.Equals(Service.Video)) throw new HandoverPredictionException();
                    break;
                case MobileTerminalState.VoiceDataVideo:
                    throw new HandoverPredictionException();
            }
            return State;
        }


    }
}