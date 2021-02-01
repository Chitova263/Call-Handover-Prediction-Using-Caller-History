using System;
using System.Collections.Generic;
using System.Linq;

namespace VerticalHandoverPrediction
{
    public sealed class Rat
    {
        public Guid RatId { get; }
        public string Name { get; }
        public int Priority { get; }
        public int Capacity { get; }
        public int CurrentLoad { get; private set; }
        public Dictionary<Guid, Session> OngoingSessions { get; }
        public Service SupportedServices { get; }

        private Rat(Service supportedServices, int capacity, string name, int priority)
        {
            RatId = Guid.NewGuid();
            SupportedServices = supportedServices;
            Capacity = capacity;
            CurrentLoad = 0;
            Name = name;
            Priority = priority;
            OngoingSessions = new Dictionary<Guid, Session>();
        }

        public static Rat CreateRat(Service supportedServices, int capacity, string name, int priority)
        {
            return new Rat(supportedServices, capacity, name, priority);
        }

       

        private void Admit(Call call, BasicBandwidthUnits basicBandwidthUnits)
        {
            CurrentLoad += call.Service.GetRequiredBasicBandwidthUnits(basicBandwidthUnits);
            var session = Session.CreateSession(call.StartTime, call);
            bool added = OngoingSessions.TryAdd(session.SessionId, session);
           
            if (!added) throw new Exception();

        }

        public void AdmitInitialCall(Call call, BasicBandwidthUnits basicBandwidthUnits)
        {
            // Increase load
            CurrentLoad += call.Service.GetRequiredBasicBandwidthUnits(basicBandwidthUnits);
            // Create new session
            var session = Session.CreateSession(call.StartTime, call);
            bool successful = OngoingSessions.TryAdd(session.SessionId, session);
            if (!successful)
            {
                throw new VerticalHandoverPredictionException("duplicate call session ids");
            }
        }

        public bool CanAdmitCall(Service requiredService, BasicBandwidthUnits basicBandwidthUnits) 
            => SupportsRequiredService(requiredService) && HasEnoughCapacity(requiredService, basicBandwidthUnits);

        private bool SupportsRequiredService(Service requiredService) => 
            SupportedServices.HasFlag(requiredService);

        private bool HasEnoughCapacity(Service requiredService, BasicBandwidthUnits basicBandwidthUnits) =>
            Capacity >= CurrentLoad + requiredService.GetRequiredBasicBandwidthUnits(basicBandwidthUnits);

        public void AdmitIncomingCallToOngoingSession(Call call, Session session, int resources)
        {
            CurrentLoad += resources;
            Session ongoingSession;
            if (!OngoingSessions.TryGetValue(session.SessionId, out ongoingSession))
            {
                throw new VerticalHandoverPredictionException("Session not found");
            }
            if (!ongoingSession.ActiveCalls.TryAdd(call.CallId, call))
            {
                throw new VerticalHandoverPredictionException("Duplicate callId not found");
            }
        }
    }
}