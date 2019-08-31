using System;
using System.Collections.Generic;
using System.Linq;

namespace VerticalHandoverPrediction
{

    public class RAT : IRAT
    {
        public Guid RATId { get; private set; }
        public int Capacity { get; private set; }
        public int UtilizedCapacity { get; private set; }
        public IList<Service> Services { get; private set; } //JCAC takes care of checking if call can be admitted
        public IList<ICallSession> OngoingSessions { get; private set; } = new List<ICallSession>(); // change this to a hashmap for efficiency

        private RAT(IList<Service> services, int capacity)
        {
            RATId = Guid.NewGuid();
            Services = services;
            Capacity = capacity;
            UtilizedCapacity = 0;
        }

        public static RAT CreateRAT(IList<Service> services, int capacity)
        {
            var rat = new RAT(services, capacity);
            /*
                Do heavy lifting object creation
            */
            return rat;
        }

        public IList<ICallSession> AdmitCallSession(ICallSession session)
        {
            if (session is null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            OngoingSessions.Add(session);
            //Update the utilized capacity
            UtilizedCapacity -= session.ComputeUtilizedCapacity();
            //return updated list of ongoing sessions
            return OngoingSessions;
        }

        public IList<ICallSession> DropCallSession(ICallSession session)
        {
            var sessionToDrop = OngoingSessions
                .FirstOrDefault(x => x.CallSessionId == session.CallSessionId);
            //update utilized capacity
            UtilizedCapacity += session.ComputeUtilizedCapacity();
            OngoingSessions.Remove(sessionToDrop);
            //return updated list of ongoing sessions
            return OngoingSessions;
        }

        public IList<ICallSession> DismissCall(ICallSession session)
        {
            var sessionToUpdate = OngoingSessions
                .FirstOrDefault(x => x.CallSessionId == session.CallSessionId);
            //Refactor extract this to extension method
            var realeasedBandwidth = Math.Abs(session.ComputeUtilizedCapacity()
                - sessionToUpdate.ComputeUtilizedCapacity());
            UtilizedCapacity -= realeasedBandwidth;
            sessionToUpdate = session;
            return OngoingSessions;
        }

        public IList<ICallSession> AdmitCall(ICallSession session)
        {
            var sessionToUpdate = OngoingSessions
                .FirstOrDefault(x => x.CallSessionId == session.CallSessionId);
            //Refactor extract this to extension method
            var addedBandwidth = Math.Abs(session.ComputeUtilizedCapacity()
                - sessionToUpdate.ComputeUtilizedCapacity());
            UtilizedCapacity += addedBandwidth;
            sessionToUpdate = session;
            return OngoingSessions;
        }

        public int AvailableBandwidthBasebandUnits() => Capacity - UtilizedCapacity;
    }
}

/*

---- What happens when single call is removed from the session

Keeps a record of all MT connected to the RAT, 
All the ongoing Sessions running on RAT,
The amount of network resources being used
 */