using VerticalHandoverPrediction.Exceptions;
using VerticalHandoverPrediction.Models;
using VerticalHandoverPrediction.Simulator;

namespace VerticalHandoverPrediction.Extensions
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

        public static MobileTerminalState DeriveMobileTerminalState(this Service service)
        {
            MobileTerminalState mobileTerminalState;
            switch (service)
            {
                case Service.Voice:
                    mobileTerminalState = MobileTerminalState.Voice;
                    break;
                case Service.Data:
                    mobileTerminalState = MobileTerminalState.Data;
                    break;
                case Service.Video:
                    mobileTerminalState = MobileTerminalState.Video;
                    break;
                default:
                    throw new VerticalHandoverPredictionException("Something wrong happenned");
            }
            return mobileTerminalState;
        }

        public static Enum GetRandomEnumValue(this Type t)
        {
            return Enum.GetValues(t)
                .OfType<Enum>()
                .OrderBy(e => Guid.NewGuid())
                .FirstOrDefault();
        }

        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            return source.PickRandom(1).Single();
        }

        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }
    }
}
