﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace VerticalHandoverPrediction
{
    public sealed class Network
    {
        public Dictionary<Guid, MobileTerminal> MobileTerminals { get; }
        public Dictionary<Guid, Rat> Rats { get; }

        private Network(Dictionary<Guid, MobileTerminal> mobileTerminals, Dictionary<Guid, Rat> rats)
        {
            MobileTerminals = mobileTerminals;
            Rats = rats;
        }

        public static Network Initialize(Dictionary<Guid, MobileTerminal> mobileTerminals, Dictionary<Guid, Rat> rats)
        {
            return new Network(mobileTerminals, rats);
        }

        public IEnumerable<Rat> GetCompatibleRats(Service service)
        {
            return Rats.Where(r => r.Value.SupportedServices.HasFlag(service))
                 .OrderByDescending(r => r.Value.Priority)
                 .Select(o => o.Value);
        }
    }
}
