using System;
using System.Collections.Generic;

namespace HandoverPrediction
{
    public class MobileTerminal: IMobileTerminal
    {
        public Guid MobileTerminalId { get; private set; }
        public List<Call> CallHistoryLog { get; private set; }
        public MobileTerminalState CurrentState { get; private set; }

        //Each mobile terminal has a session -------------  remodel

        // Add method to check if MT is on an active session!!!! for preceeding calls

        public MobileTerminal(Guid mobileTerminalId)
        {
            MobileTerminalId = mobileTerminalId;
            CurrentState = MobileTerminalState.Idle;
        }

        public MobileTerminalState SetMobileTerminalState(MobileTerminalState state)
        {
            CurrentState = state;
            return CurrentState;
        }

        //Get the MT current state given the new call and current state
        public MobileTerminalState ComputeMobileTerminalCurrentState(Service service)
        {
            // Refactor Code
            switch (CurrentState)
            {
                case MobileTerminalState.Idle:
                    if(service.Equals(Service.Voice)) CurrentState = MobileTerminalState.Voice;
                    if(service.Equals(Service.Data)) CurrentState = MobileTerminalState.Data;
                    if(service.Equals(Service.Video)) CurrentState = MobileTerminalState.Video; 
                    break;
                case MobileTerminalState.Data:
                    if(service.Equals(Service.Voice)) CurrentState = MobileTerminalState.VoiceData;
                    if(service.Equals(Service.Data)) throw new HandoverPredictionException();
                    if(service.Equals(Service.Video)) CurrentState = MobileTerminalState.VideoData; 
                    break;
                case MobileTerminalState.Video:
                    if(service.Equals(Service.Voice)) CurrentState = MobileTerminalState.VoiceVideo;
                    if(service.Equals(Service.Data)) CurrentState = MobileTerminalState.VideoData;
                    if(service.Equals(Service.Video)) throw new HandoverPredictionException(); 
                    break;
                case MobileTerminalState.Voice:
                    if(service.Equals(Service.Voice)) throw new HandoverPredictionException();
                    if(service.Equals(Service.Data)) CurrentState = MobileTerminalState.VoiceData;
                    if(service.Equals(Service.Video)) CurrentState = MobileTerminalState.VoiceVideo;
                    break;
                case MobileTerminalState.VideoData:
                    if(service.Equals(Service.Voice)) CurrentState = MobileTerminalState.VoiceDataVideo; 
                    if(service.Equals(Service.Data)) throw new HandoverPredictionException();
                    if(service.Equals(Service.Video)) throw new HandoverPredictionException();
                    break;
                case MobileTerminalState.VoiceData:
                    if(service.Equals(Service.Voice))  throw new HandoverPredictionException();
                    if(service.Equals(Service.Data)) throw new HandoverPredictionException();
                    if(service.Equals(Service.Video)) CurrentState = MobileTerminalState.VoiceDataVideo;
                    break;
                case MobileTerminalState.VoiceVideo:
                    if(service.Equals(Service.Voice))  throw new HandoverPredictionException();
                    if(service.Equals(Service.Data)) CurrentState = MobileTerminalState.VoiceDataVideo;
                    if(service.Equals(Service.Video)) throw new HandoverPredictionException();
                    break;
                case MobileTerminalState.VoiceDataVideo:
                    throw new HandoverPredictionException();          
            }
            return CurrentState;
        }

    }
}

