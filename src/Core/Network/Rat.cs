namespace VerticalHandoverPrediction.Network
{
    using System;
    using System.Collections.Generic;
    using VerticalHandoverPrediction.CallAdimissionControl;
    using VerticalHandoverPrediction.CallSession;
    using VerticalHandoverPrediction.Mobile;

    public class Rat : IRat
    {
        public Guid RatId { get; private set; }
        public string Name { get; private set; }
        public int Capacity { get; private set; }
        public int UsedNetworkResources { get; private set; }
        private readonly List<ISession> _ongoingSessions;
        public IReadOnlyCollection<ISession> OngoingSessions => _ongoingSessions;
        public IList<Service> Services { get; set; }

        public Rat(IList<Service> services, int capacity, string name)
        {
            if (services is null)
            {
                throw new VerticalHandoverPredictionException($"{nameof(services)} is invalid");
            }

            RatId = Guid.NewGuid();
            Services = services;
            Capacity = capacity;
            Name = name;
            _ongoingSessions = new List<ISession>();
        }

        public void RemoveSession(ISession session)
        {
            if (session == null)
                throw new VerticalHandoverPredictionException($"{nameof(session)} is invalid");

            _ongoingSessions.Remove(session);
        }

        public void AddSession(ISession session)
        {
            if (session == null)
                throw new VerticalHandoverPredictionException($"{nameof(session)} is invalid");

            _ongoingSessions.Add(session);
        }
        public void TakeNetworkResources(int resources) => UsedNetworkResources = UsedNetworkResources + resources;

        public void RealeaseNetworkResources(int resources) => UsedNetworkResources = UsedNetworkResources - resources;

        public int AvailableNetworkResources() => Capacity - UsedNetworkResources;

        public bool CanAdmitNewCallToOngoingSession(ISession session, ICall call, IMobileTerminal mobileTerminal)
        {
            if (session == null)
                throw new VerticalHandoverPredictionException($"{nameof(session)} is invalid");

            if (call == null)
                throw new VerticalHandoverPredictionException($"{nameof(call)} is invalid");
    
            if (mobileTerminal == null)
                throw new VerticalHandoverPredictionException($"{nameof(mobileTerminal)} is invalid");
    
            if (!this.Services.Contains(call.Service))
                return false;

            var requiredNetworkResources = call.Service.ComputeRequiredNetworkResources();
            return requiredNetworkResources <= AvailableNetworkResources();
        }

        public void AdmitNewCallToOngoingSession(ISession session, ICall call, IMobileTerminal mobileTerminal)
        {
            if (session == null)
                throw new VerticalHandoverPredictionException($"{nameof(session)} is invalid");

            if (call == null)
                throw new VerticalHandoverPredictionException($"{nameof(call)} is invalid");

            if (mobileTerminal == null)
                throw new VerticalHandoverPredictionException($"{nameof(mobileTerminal)} is invalid");

            TakeNetworkResources(call.Service.ComputeRequiredNetworkResources());

            session.AddToActiveCalls(call);

            var state = mobileTerminal.UpdateMobileTerminalStateWhenAdmitingNewCallToOngoingSession(session.ActiveCalls);

            session.AddToSessionSequence(state);
        }
    }
}