using System;
using System.Collections.Generic;

namespace HandoverPrediction
{
    public class RAT: IRAT
    {
        //Unique identifier of RATs
        public Guid RATid { get; private set; }
        public int Capacity { get; private set; }
        //Utilized capacity must be updated whenever a new call is admitted to RAT or Terminated
        public int UtilizedCapacity { get; private set; }
        public List<Service> Services { get; private set; }
        //whenever a call is admitted it is added to the list of ongoing sessions, whenever call is terminated its removed from this list of call sessions
        public List<CallSession> OngoingSessions { get; private set; } // change this to a hashmap for efficiency

        private RAT(int capacity, List<Service> services)
        {
            Capacity = capacity;
            Services = services;
        }

        //Factory method to create RAT
        public static RAT CreateRAT(int capacity, List<Service> services)
        {
            return new RAT(capacity, services);
        }

        public bool AdmitNewCall(Call call)
        {
            throw new NotImplementedException();
        }

        public int AvailableCapacity()
        {
            throw new NotImplementedException();
        }
    }
}
