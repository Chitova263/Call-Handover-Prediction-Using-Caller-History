namespace VerticalHandoverPrediction.CallAdimissionControl
{
    using System.Collections.Generic;
    using VerticalHandoverPrediction.CallSession;
    using VerticalHandoverPrediction.Mobile;

    public static class LinqExtensions
    {
        public static int ComputeRequiredNetworkResources(this Service service)
        {
            var utlizedCapacity = 0;
            switch (service)
            {
                case Service.Voice:
                    utlizedCapacity += 1;
                    break;
                case Service.Data:
                    utlizedCapacity += 2;
                    break;
                case Service.Video:
                    utlizedCapacity += 2;
                    break;
            }
            return utlizedCapacity;
        }

        public static MobileTerminalState GetState(this Service service)
        {
            MobileTerminalState state = default(MobileTerminalState);
            switch (service)
            {
                case Service.Voice:
                    state = MobileTerminalState.Voice;
                    break;
                case Service.Video:
                    state = MobileTerminalState.Video;
                    break;
                case Service.Data:
                    state = MobileTerminalState.Data;
                    break;
            }
            return state;
        }

        public static IList<Service> SupportedServices(this MobileTerminalState state)
        {
            var services = new List<Service>();
            switch (state)
            {
                case MobileTerminalState.Idle:    
                    break;
                case MobileTerminalState.Data:
                    services.Add(Service.Data);
                    break;
                case MobileTerminalState.Video:
                    services.Add(Service.Video);
                    break;
                case MobileTerminalState.Voice:
                    services.Add(Service.Voice);
                    break;
                case MobileTerminalState.VideoData:
                    services.Add(Service.Video);
                    services.Add(Service.Data);
                    break;
                case MobileTerminalState.VoiceData:
                    services.Add(Service.Data);
                    services.Add(Service.Voice);
                    break;
                case MobileTerminalState.VoiceVideo:
                    services.Add(Service.Voice);
                    services.Add(Service.Video);
                    break;
                case MobileTerminalState.VoiceDataVideo:
                    services.Add(Service.Data);
                    services.Add(Service.Voice);
                    services.Add(Service.Video);
                    break;      
            }
            
            return services;
        }
    }
}