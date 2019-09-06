using VerticalHandoverPrediction.CallSession;

namespace VerticalHandoverPrediction.Mobile
{
    public static class MobileTerminalExtensions
    {
        public static MobileTerminalState UpdateStateExtension(this IMobileTerminal mt, Service service)
        {
            switch (mt.State)
            {
                case MobileTerminalState.Idle:
                    if (service.Equals(Service.Voice)) mt.ChangeStateTo(MobileTerminalState.Voice);
                    if (service.Equals(Service.Data)) mt.ChangeStateTo(MobileTerminalState.Data);
                    if (service.Equals(Service.Video)) mt.ChangeStateTo(MobileTerminalState.Video);
                    break;
                case MobileTerminalState.Data:
                    if (service.Equals(Service.Voice)) mt.ChangeStateTo(MobileTerminalState.VoiceData);
                    if (service.Equals(Service.Data)) throw new HandoverPredictionException();
                    if (service.Equals(Service.Video)) mt.ChangeStateTo(MobileTerminalState.VideoData);
                    break;
                case MobileTerminalState.Video:
                    if (service.Equals(Service.Voice)) mt.ChangeStateTo(MobileTerminalState.VoiceVideo);
                    if (service.Equals(Service.Data)) mt.ChangeStateTo(MobileTerminalState.VideoData);
                    if (service.Equals(Service.Video)) throw new HandoverPredictionException();
                    break;
                case MobileTerminalState.Voice:
                    if (service.Equals(Service.Voice)) throw new HandoverPredictionException();
                    if (service.Equals(Service.Data)) mt.ChangeStateTo(MobileTerminalState.VoiceData);
                    if (service.Equals(Service.Video)) mt.ChangeStateTo(MobileTerminalState.VoiceVideo);
                    break;
                case MobileTerminalState.VideoData:
                    if (service.Equals(Service.Voice)) mt.ChangeStateTo(MobileTerminalState.VoiceDataVideo);
                    if (service.Equals(Service.Data)) throw new HandoverPredictionException();
                    if (service.Equals(Service.Video)) throw new HandoverPredictionException();
                    break;
                case MobileTerminalState.VoiceData:
                    if (service.Equals(Service.Voice)) throw new HandoverPredictionException();
                    if (service.Equals(Service.Data)) throw new HandoverPredictionException();
                    if (service.Equals(Service.Video)) mt.ChangeStateTo(MobileTerminalState.VoiceDataVideo);
                    break;
                case MobileTerminalState.VoiceVideo:
                    if (service.Equals(Service.Voice)) throw new HandoverPredictionException();
                    if (service.Equals(Service.Data)) mt.ChangeStateTo(MobileTerminalState.VoiceDataVideo);
                    if (service.Equals(Service.Video)) throw new HandoverPredictionException();
                    break;
                case MobileTerminalState.VoiceDataVideo:
                    throw new HandoverPredictionException();
            }
            return mt.State;
        }
    }
}