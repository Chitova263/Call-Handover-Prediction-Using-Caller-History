using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerticalHandoverPrediction.CallSession;

namespace VerticalHandoverPrediction.Networks
{
    public class RatConfiguration
    {
        public int Capacity { get; set; }
        public string Name { get; set; }
        public Service SupportedServices { get; set; }

        private RatConfiguration()
        {

        }
        
        public static RatConfiguration CreateDefaultConfiguration()
        {
            return new RatConfiguration();
        }
    }
}
