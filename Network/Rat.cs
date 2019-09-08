using System;
using System.Collections.Generic;
using VerticalHandoverPrediction.CallAdmissionControl;
using VerticalHandoverPrediction.CallSession;
using VerticalHandoverPrediction.Mobile;

namespace VerticalHandoverPrediction.Network
{

    public class Rat : IRat
    {
        public Guid RatId { get; private set; }
        public string Name { get; private set; }
        public int Capacity { get; private set; }
        public int UsedNetworkResources { get; private set; }
        private readonly List<ISession> _ongoingSessions;
        public IReadOnlyCollection<ISession> OngoingSessions => _ongoingSessions;
        public IList<Service> Services { get; set; }

        private Rat(IList<Service> services, int capacity, string name)
        {
            RatId = Guid.NewGuid();
            Services = services;
            Capacity = capacity;
            Name = name;
            _ongoingSessions = new List<ISession>();
        }

        public static Rat CreateRat(IList<Service> services, int capacity, string name)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            return new Rat(services, capacity, name);
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
        public void TakeNetworkResources(int resources) => UsedNetworkResources = UsedNetworkResources + resources;

        public void RealeaseNetworkResources(int resources) => UsedNetworkResources = UsedNetworkResources - resources;

        public int AvailableNetworkResources() => Capacity - UsedNetworkResources;

        public bool CanAdmitNewCallToOngoingSession(ISession session, ICall call, IMobileTerminal mobileTerminal)
        {
            if (!this.Services.Contains(call.Service)) return false;
            var requiredNetworkResources = call.Service.ComputeRequiredNetworkResources();
            return requiredNetworkResources <= AvailableNetworkResources();
        }

        public void AdmitNewCallToOngoingSession(ISession session, ICall call, IMobileTerminal mobileTerminal)
        {
            this.TakeNetworkResources(call.Service.ComputeRequiredNetworkResources());
            session.ActiveCalls.Add(call);

            var state = mobileTerminal.
                UpdateMobileTerminalStateWhenAdmitingNewCallToOngoingSession(session.ActiveCalls);

            session.SessionSequence.Add(state);
        }
    }
}