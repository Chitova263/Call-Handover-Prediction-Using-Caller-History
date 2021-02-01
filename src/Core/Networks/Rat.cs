using System;
using System.Collections.Generic;
using VerticalHandoverPrediction.CallAdimissionControl;
using VerticalHandoverPrediction.CallSession;

namespace VerticalHandoverPrediction
{
    public sealed class Rat
    {
        public Guid RatId { get; private set; }
        public string Name { get; private set; }
        public int Capacity { get; private set; }
        public int UsedNetworkResources { get; private set; }
        private readonly List<Session> _ongoingSessions;
        public IReadOnlyCollection<Session> OngoingSessions => _ongoingSessions;
        public Service Services { get; set; }

        public static Rat CreateRat(Service services, int capacity, string name)
        {
            return new Rat(services, capacity, name);
        }


        public Rat(Service services, int capacity, string name)
        {
       
            RatId = Guid.NewGuid();
            Services = services;
            Capacity = capacity;
            Name = name;
            _ongoingSessions = new List<Session>();
        }

        public void RemoveSession(Session session)
        {
            if (session == null)
                throw new VerticalHandoverPredictionException($"{nameof(session)} is invalid");

            _ongoingSessions.Remove(session);
        }

        public void AddSession(Session session)
        {
            if (session == null)
                throw new VerticalHandoverPredictionException($"{nameof(session)} is invalid");

            _ongoingSessions.Add(session);
        }
        public void TakeNetworkResources(int resources) => UsedNetworkResources = UsedNetworkResources + resources;

        public void RealeaseNetworkResources(int resources) => UsedNetworkResources = UsedNetworkResources - resources;

        public int AvailableNetworkResources() => Capacity - UsedNetworkResources;

        public bool CanAdmitNewCallToOngoingSession(Session session, Call call, MobileTerminal mobileTerminal)
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

        public void AdmitNewCallToOngoingSession(Session session, Call call, MobileTerminal mobileTerminal)
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