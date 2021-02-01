using System;
using System.Collections.Generic;

namespace VerticalHandoverPrediction
{
    public class NetworkBuilder
    {
        private Dictionary<Guid, MobileTerminal> _mobileTerminals;
        private Dictionary<Guid, Rat> _rats;

        private int _ratPriority;

        public NetworkBuilder()
        {
            _mobileTerminals = new Dictionary<Guid, MobileTerminal>();
            _rats = new Dictionary<Guid, Rat>();
            _ratPriority = -1;
        }

        public Network Build()
        {
            return Network.Initialize(_mobileTerminals, _rats);
        }

        public NetworkBuilder RegisterMobileTerminals(int count = 1000)
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

        public NetworkBuilder RegisterRat(Action<RatConfiguration> action)
        {
            var config = new RatConfiguration();
            action(config);

            var rat = Rat.CreateRat(config.SupportedServices, config.Capacity, config.Name, ++_ratPriority);
            var success = _rats.TryAdd(rat.RatId, rat);
            if (!success)
                throw new Exception();

            return this;
        }
    }
}