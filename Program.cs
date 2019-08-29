using System.Collections.Generic;
using Handoverprediction.VerticalHandover;

namespace Handoverprediction
{
    class Program
    {
        static void Main(string[] args)
        {
            //existing users
            var users = new List<User>
            {
                new User{ UserId = 1},
                new User{ UserId = 2},
                new User{ UserId = 3},
                new User{ UserId = 4},
            };

            //RAT-1 --Voice 
            var RAT1 = new RAT
            {
                //Max number of calls supported
                Capacity = 100,
                Service = Service.Voice,
                BasebandUnits = 1
            };

            var RAT2 = new RAT
            {
                //Max number of calls supported
                Capacity = 100,
                Service = Service.Data,
                BasebandUnits = 2,
            };

            var RAT3 = new RAT
            {
                //Max number of calls supported
                Capacity = 100, 
                Service = Service.VoiceData,
                BasebandUnits = 2,
            };

            var RAT4 = new RAT
            {
                //Max number of calls supported
                Capacity = 100,
                Service = Service.VoiceVideoData,
                BasebandUnits = 2,
            };
        }
    }
}
