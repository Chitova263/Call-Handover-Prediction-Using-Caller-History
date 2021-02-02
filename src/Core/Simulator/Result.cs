using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerticalHandoverPrediction
{
    public sealed class Result
    {
        public int VoiceCallsAdmitted { get; private set; }
        public int DataCallsAdmitted { get; private set; }
        public int VideoCallsAdmitted { get; private set; }
        public int VoiceCallsDropped { get; private set; }
        public int DataCallsDropped { get; private set; }
        public int VideoCallsDropped { get; private set; }
        public int SuccessfulVerticalHandovers { get; private set; }
        public int FailedVerticalHandovers { get; private set; }
        public int Sessions { get; private set; }

        public void LogVerticalHandover(bool isSuccess)
        {
            if (isSuccess)
                SuccessfulVerticalHandovers++;
            else
                FailedVerticalHandovers++;
        }
        
        public void LogAdmittedCall(Service service)
        {
            switch (service)
            {
                case Service.Voice:
                    VoiceCallsAdmitted++;
                    break;
                case Service.Data:
                    DataCallsAdmitted++;
                    break;
                case Service.Video:
                    VoiceCallsAdmitted++;
                    break;
                default:
                    break;
            }
        }

        public void LogDroppedCall(Service service)
        {
            switch (service)
            {
                case Service.Voice:
                    VoiceCallsAdmitted++;
                    break;
                case Service.Data:
                    DataCallsAdmitted++;
                    break;
                case Service.Video:
                    VoiceCallsAdmitted++;
                    break;
                default:
                    break;
            }
        }
    }
}
