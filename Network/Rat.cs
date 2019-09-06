using System;
using System.Collections.Generic;
using Serilog;
using VerticalHandoverPrediction.CallAdmissionControl;
using VerticalHandoverPrediction.CallSession;
using VerticalHandoverPrediction.Mobile;
using VerticalHandoverPrediction.Utils;

namespace VerticalHandoverPrediction.Network
{

    public class Rat : IRat
    {
        public Guid RatId { get; private set; }
        public int Capacity { get; private set; }
        public int UsedResources { get; private set; }
        private readonly List<ISession> _ongoingSessions;
        public IReadOnlyCollection<ISession> OngoingSessions => _ongoingSessions;
        public IList<Service> Services { get; set; }


        private Rat(IList<Service> services, int capacity)
        {
            RatId = Guid.NewGuid();
            Services = services;
            Capacity = capacity;
            _ongoingSessions = new List<ISession>();
        }

        public static Rat CreateRat(IList<Service> services, int capacity)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            return new Rat(services, capacity);
        }

        public void RemoveSession(ISession session)
        {
            if (session is null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            this._ongoingSessions.Remove(session);
        }

        public void AddSession(ISession session)
        {
            if (session is null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            this._ongoingSessions.Add(session);
        }

        public void SetRatId(Guid id)
        {
            RatId = id;
        }

        public void SetUsedResources(int bbu)
        {
            UsedResources = bbu;
        }


        public int AvailableCapacity() => Capacity - UsedResources;


        //Admits incoming call on this ongoing session
        public void AdmitIncomingCallToOngoingSession(ICall call, ISession session, IMobileTerminal mobileTerminal)
        {
            session.SessionSequence.Add(mobileTerminal.State);
            session.ActiveCalls.Add(call);
            this.UsedResources += call.Service.ComputeRequiredCapacity();
        }

        public void AdmitIncomingCallToNewSession(ICall call, IMobileTerminal mobileTerminal)
        {
            Log.Information($"----Starting new session rat: @{RatId}; service: @{call.Service}");

            var session = Session.StartSession(RatId);
            mobileTerminal.SetSessionId(session.SessionId);
            session.ActiveCalls.Add(call);
            mobileTerminal.UpdateMobileTerminalState(call.Service);
            session.SessionSequence.Add(mobileTerminal.State);
            this.AddSession(session);
            UsedResources += call.Service.ComputeRequiredCapacity();

            Log.Information($"---- New call admitted rat: @{RatId}; service: @{call.Service}");
        }

        public bool CanAccommodateCall(ICall call)
        {
            //Can rat support the incoming call service
            var supported = Services.Contains(call.Service);
            if (!supported) return false;
            /* If supported check if the is enough capacity to accommodate session with new call */
            var requiredBbu = UsedResources + call.Service.ComputeRequiredCapacity();
            return requiredBbu <= AvailableCapacity();
        }

        public bool CanAccommodateServices(List<Service> services)
        {
            var requiredBbu = 0;
            foreach (var service in services)
            {
                requiredBbu += service.ComputeRequiredCapacity();
            }
            return requiredBbu <= AvailableCapacity();
        }
    }
}