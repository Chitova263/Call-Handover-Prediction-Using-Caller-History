using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerticalHandoverPrediction
{
    public class Network
    {
        public Dictionary<Guid, MobileTerminal> MobileTerminals { get; }
        public Dictionary<Guid, Rat> Rats { get; }

        public Network(Dictionary<Guid, MobileTerminal> mobileTerminals, Dictionary<Guid, Rat> rats)
        {
            MobileTerminals = mobileTerminals;
            Rats = rats;
        }

    }
}
