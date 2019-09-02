using System;
using System.Collections.Generic;
using System.Linq;
using VerticalHandoverPrediction.CallAdmissionControl;
using VerticalHandoverPrediction.CallSession;
using VerticalHandoverPrediction.Mobile;

namespace VerticalHandoverPrediction.Network
{
    public class Rat : IRat
    {
        public Guid RatId { get; private set; }
        public int Capacity { get; private set; }
        public int UsedCapacity { get; private set; }
        public IList<Service> Services { get; set; }
        public IList<ISession> OngoingSessions { get; set; }

        private Rat(IList<Service> services, int capacity)
        {
            RatId = Guid.NewGuid();
            Services = services;
            Capacity = capacity;
            OngoingSessions = new List<ISession>();
        }

        public static Rat CreateRat(IList<Service> services, int capacity)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            return new Rat(services, capacity);
        }

        public int AvailableCapacity() => Capacity - UsedCapacity;

        public void RemoveSessionFromRat(ISession session)
        {
            //Remove the session from Rat's ongoing sessions
            OngoingSessions.Remove(session);

            //Free up resources
            var services = session.ActiveCalls
                .Select(x => x.Service)
                .ToList();
            
            foreach (var service in services)
            {
                UsedCapacity -= service.ComputeRequiredCapacity();
            }
            //Session Removed this Rat
        }

        public void TransferSessionToRat(ISession currentSession)
        {
            //Change the RatId of the session to identify its new Rat
            currentSession.SetRatId(RatId);

            //Add session to list of ongoing sessions
            OngoingSessions.Add(currentSession);

            //Take up the used capacity by the session
            var services = currentSession.ActiveCalls
                .Select(x => x.Service)
                .ToList();

            foreach (var service in services)
            {
                UsedCapacity += service.ComputeRequiredCapacity();
            }
            //Session Successfuly transfered to this Rat
        }

        public void SetUsedCapacity(int bbu) => UsedCapacity = bbu;

        //Admits incoming call on this ongoing session
        public void AdmitIncomingCallToOngoingSession(ICall call, ISession session, IMobileTerminal mobileTerminal)
        {
            //Update the state of the mobile terminal involved
            var state = mobileTerminal.UpdateMobileTerminalState(call.Service);
            //Update session sequence with new state
            session.SessionSequence.Add(state);
            //Update call sessionId property
            call.SetSessionId(session.SessionId);
            //Add call to list of active calls
            session.ActiveCalls.Add(call);
            //Update the used resources
            UsedCapacity += call.Service.ComputeRequiredCapacity();
        }

        public void AdmitIncomingCallToNewSession(ICall call, IMobileTerminal mobileTerminal)
        {
            //Start new session
            var session = Session.StartSession(RatId);
            //Add call & mobileTerminal to the new session
            mobileTerminal.SetSessionId(session.SessionId);
            call.SetSessionId(session.SessionId);
            //Add call to list of active calls in session
            session.ActiveCalls.Add(call);
            //Update the mobile terminal state
            var state = mobileTerminal.UpdateMobileTerminalState(call.Service);
            //Update the session sequence
            session.SessionSequence.Add(state);
            //Add session to list of ongoing sessions
            OngoingSessions.Add(session);
            //Update used resources extra call
            UsedCapacity += call.Service.ComputeRequiredCapacity();
        }

        public bool CanAccommodateCall(ICall call)
        {
            //Can rat support the incoming call service
            var supported = Services.Contains(call.Service);
            if(!supported) return false;
             /* If supported check if the is enough capacity to accommodate session with new call */
            var requiredBbu = UsedCapacity + call.Service.ComputeRequiredCapacity();
            return requiredBbu <= Capacity;
        }
    }
}