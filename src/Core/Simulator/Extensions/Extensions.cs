using System;

namespace VerticalHandoverPrediction
{
    public static class Extensions
    {
        public static int GetRequiredBasicBandwidthUnits(
            this Service service,
            BasicBandwidthUnits basicBandwidthUnits)
        {
            int bbu;
            switch (service)
            {
                case Service.Voice:
                    bbu = basicBandwidthUnits.Voice;
                    break;
                case Service.Data:
                    bbu = basicBandwidthUnits.Data;
                    break;
                case Service.Video:
                    bbu = basicBandwidthUnits.Video;
                    break;
                default:
                    throw new Exception();
            }

            return bbu;
        }
    }
}
