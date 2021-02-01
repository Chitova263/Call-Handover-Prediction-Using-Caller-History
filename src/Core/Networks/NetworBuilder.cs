using System;
using System.Collections.Generic;
using VerticalHandoverPrediction.Networks;
using System.Linq;

namespace VerticalHandoverPrediction
{
    public class NetworBuilder
    {
        private Dictionary<Guid, MobileTerminal> _mobileTerminals;
        private Dictionary<Guid, Rat> _rats;

        public NetworBuilder()
        {
        }

        public Network Build()
        {
            return new Network(_mobileTerminals, _rats);
        }

        public NetworBuilder RegisterMobileTerminals(int count = 1000)
        {
            var mobileTerminals = MobileTerminal.GenerateMobileTerminals(count);
            foreach (var mobileTerminal in mobileTerminals)
            {
                var success = _mobileTerminals.TryAdd(mobileTerminal.MobileTerminalId, mobileTerminal);
                if (!success)
                    throw new Exception();
            }
           
            return this;
        }

        public NetworBuilder RegisterRat(Action<RatConfiguration> action)
        {
            var config = RatConfiguration.CreateDefaultConfiguration();
            action(config);

            var rat = Rat.CreateRat(config.SupportedServices, config.Capacity, config.Name);
            var success = _rats.TryAdd(rat.RatId, rat);
            if (!success)
                throw new Exception();

            return this;
        }
    }
}