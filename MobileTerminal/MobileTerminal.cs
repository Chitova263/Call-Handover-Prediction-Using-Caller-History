using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VerticalHandoverPrediction
{
    [JsonObject(IsReference = true)]
    public class MobileTerminal : IMobileTerminal
    {
        public Guid MobileTerminalId { get; set; }
        public List<CallSession> CallHistoryLog { get; set; } = new List<CallSession>();
        public MobileTerminalState CurrentState { get; set; }
        public CallSession CurrentSession { get; set; }
        public MobileTerminalModality Mode { get; set; }

        private MobileTerminal()
        {
            MobileTerminalId = Guid.NewGuid();
            CurrentState = MobileTerminalState.Idle;
        }

        public static MobileTerminal CreateMobileTerminal()
        {
            return new MobileTerminal();
        }

        public bool IsOnActiveSession()
        {
            return !CurrentState.Equals(MobileTerminalState.Idle);
        }

        public MobileTerminalState SetMobileTerminalCurrentState(Service service)
        {
            // Refactor Code
            switch (CurrentState)
            {
                case MobileTerminalState.Idle:
                    if (service.Equals(Service.Voice)) CurrentState = MobileTerminalState.Voice;
                    if (service.Equals(Service.Data)) CurrentState = MobileTerminalState.Data;
                    if (service.Equals(Service.Video)) CurrentState = MobileTerminalState.Video;
                    break;
                case MobileTerminalState.Data:
                    if (service.Equals(Service.Voice)) CurrentState = MobileTerminalState.VoiceData;
                    if (service.Equals(Service.Data)) throw new HandoverPredictionException();
                    if (service.Equals(Service.Video)) CurrentState = MobileTerminalState.VideoData;
                    break;
                case MobileTerminalState.Video:
                    if (service.Equals(Service.Voice)) CurrentState = MobileTerminalState.VoiceVideo;
                    if (service.Equals(Service.Data)) CurrentState = MobileTerminalState.VideoData;
                    if (service.Equals(Service.Video)) throw new HandoverPredictionException();
                    break;
                case MobileTerminalState.Voice:
                    if (service.Equals(Service.Voice)) throw new HandoverPredictionException();
                    if (service.Equals(Service.Data)) CurrentState = MobileTerminalState.VoiceData;
                    if (service.Equals(Service.Video)) CurrentState = MobileTerminalState.VoiceVideo;
                    break;
                case MobileTerminalState.VideoData:
                    if (service.Equals(Service.Voice)) CurrentState = MobileTerminalState.VoiceDataVideo;
                    if (service.Equals(Service.Data)) throw new HandoverPredictionException();
                    if (service.Equals(Service.Video)) throw new HandoverPredictionException();
                    break;
                case MobileTerminalState.VoiceData:
                    if (service.Equals(Service.Voice)) throw new HandoverPredictionException();
                    if (service.Equals(Service.Data)) throw new HandoverPredictionException();
                    if (service.Equals(Service.Video)) CurrentState = MobileTerminalState.VoiceDataVideo;
                    break;
                case MobileTerminalState.VoiceVideo:
                    if (service.Equals(Service.Voice)) throw new HandoverPredictionException();
                    if (service.Equals(Service.Data)) CurrentState = MobileTerminalState.VoiceDataVideo;
                    if (service.Equals(Service.Video)) throw new HandoverPredictionException();
                    break;
                case MobileTerminalState.VoiceDataVideo:
                    throw new HandoverPredictionException();
            }
            return CurrentState;
        }


    }
}