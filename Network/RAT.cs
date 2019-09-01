using System;
using System.Collections.Generic;
using System.Linq;

namespace VerticalHandoverPrediction
{

    public class RAT : IRAT
    {
        public Guid RATId { get; private set; }
        public int Capacity { get; private set; }
        public int UsedCapacity { get; private set; }
        public IList<Service> Services { get; set; }
        public IList<ISession> OngoingSessions { get; set; }

        private RAT(IList<Service> services, int capacity)
        {
            RATId = Guid.NewGuid();
            Services = services;
            Capacity = capacity;
            OngoingSessions = new List<ISession>();
        }

        public static RAT CreateRAT(IList<Service> services, int capacity) => new RAT(services, capacity);

        //Check if RAT can accommodate call
        public bool CanAccommodateCall(ICall call)
        {
            ISession ongoingSession;
            // Can RAT support the incoming call
            var supported = Services.Contains(call.Service);
            if (!supported) return false;

            /* If supported check if the is enough capacity to accommodate session with new call */
            // Retrieve ongoing session
            ongoingSession = OngoingSessions.FirstOrDefault(x => x.SessionId == call.MobileTerminal.SessionId);

            //if ongoingSession is not available,
            var requiredBbu = (ongoingSession == null) ? call.ComputeRequiredCapacity()
                : call.ComputeRequiredCapacity() + ongoingSession.ComputeRequiredCapacity();

            return Capacity >= requiredBbu;
        }

        public void AdmitIncomingCallToOngoingSession(ICall call)
        {
            //find the ongoing session
            var ongoingSession = OngoingSessions.FirstOrDefault(x => x.SessionId == call.MobileTerminal.SessionId);
            //update the current state of mobile terminal
            var state = call.MobileTerminal.UpdateMobileTerminalState(call.Service);
            //update the session sequence
            ongoingSession.UpdateCallSessionSequence(state);
            //add new call to list of session's active calls
            ongoingSession.ActiveCalls.Add(call);
            //update the used resources by subtracting resources used by new call
            UsedCapacity += call.ComputeRequiredCapacity();

        }

        public void AdmitIncomingCallToNewSession(ICall call)
        {
            //Initiate new session
            var session = Session.InitiateSession(call, this.RATId);
            //Add session to list of ongoing sessions
            this.OngoingSessions.Add(session);
            //Update the used resources
            UsedCapacity += session.ComputeRequiredCapacity();
        }

        public int AvailableCapacity() => Capacity - UsedCapacity;

        public void TransferSessionTo(ISession currentSession, ICall call)
        {
            //Update RATId of Session before saving
            currentSession.SetRATId(this.RATId);
            call.MobileTerminal.SetRATId(this.RATId);

            OngoingSessions.Add(currentSession);

             var services = currentSession.ActiveCalls
                .Select(x => x.Service)
                .ToList();

            foreach (var service in services)
            {
                this.UsedCapacity += service.ComputeRequiredCapacity();
            }  
        }

        public void RemoveSessionFromSourceRAT(ISession session)
        {
            //Remove Session
            OngoingSessions.Remove(session);
            //Free Up Resources
            var services = session.ActiveCalls
                .Select(x => x.Service)
                .ToList();
           
            foreach (var service in services)
            {
                this.UsedCapacity -= service.ComputeRequiredCapacity();
            } 
        }

        public void TerminateSession(Guid sessionId)
        {
            //Free up resources
            var services = OngoingSessions
                .FirstOrDefault(x => x.SessionId == sessionId)
                .ActiveCalls
                .Select(x => x.Service)
                .ToList();

            foreach (var service in services)
            {
                this.UsedCapacity -= service.ComputeRequiredCapacity();
            } 

            var session = OngoingSessions.FirstOrDefault(x => x.SessionId == sessionId);
            OngoingSessions.Remove(session);

            session = null; //Removed by garbage collector
        }
    }
}
