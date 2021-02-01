using System;
using System.Collections.Generic;

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

        public bool AdmitInitialCall(Call call, BasicBandwidthUnits basicBandwidthUnits)
        {
            if (Capacity < CurrentLoad + call.Service.GetRequiredBasicBandwidthUnits(basicBandwidthUnits))
                return false;
            Admit(call, basicBandwidthUnits);
            return true;
        }

        private void Admit(Call call, BasicBandwidthUnits basicBandwidthUnits)
        {
            CurrentLoad += call.Service.GetRequiredBasicBandwidthUnits(basicBandwidthUnits);
            var session = Session.CreateSession(call.StartTime, call);
            bool added = OngoingSessions.TryAdd(session.SessionId, session);
           
            if (!added) throw new Exception();

        }

        public bool CanAdmitToOngoingSession(Call call, BasicBandwidthUnits basicBandwidthUnits)
        {
            // Does current RAT support incoming call
            if (!SupportedServices.HasFlag(call.Service))
            {
                return false;
            }

            return true;
        }
    }
}