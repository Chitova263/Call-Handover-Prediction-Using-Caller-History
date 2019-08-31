using System;
using System.Collections.Generic;

namespace VerticalHandoverPrediction
{
    public class RAT
    {
        public Guid RATid { get; private set; }
        public int Capacity { get; private set; }
        //Utilized capacity must be updated whenever a new call is admitted to RAT or Terminated
        public int UtilizedCapacity { get; private set; }
        public List<Service> Services { get; private set; }
        //whenever a call is admitted it is added to the list of ongoing sessions, whenever call is terminated its removed from this list of call sessions
        public List<CallSession> OngoingSessions { get; private set; } // change this to a hashmap for efficiency
    }
}