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

        public void AdmitSessionDuringHandover(Session session, int requiredResources)
        {
            if (!OngoingSessions.TryAdd(session.SessionId, session))
            {
                throw new VerticalHandoverPredictionException("duplicate session id");
            }
            CurrentLoad += requiredResources;
        }

        public Session Release(Session currentSession, int requiredResources)
        {
            CurrentLoad -= requiredResources;
            Session session;
            if (!OngoingSessions.TryGetValue(currentSession.SessionId, out session))
            {
                throw new VerticalHandoverPredictionException("session not found");
            }
            OngoingSessions.Remove(currentSession.SessionId);
            return session;
        }

        public void AdmitInitialCall(Call call, BasicBandwidthUnits basicBandwidthUnits)
        {
            CurrentLoad += call.Service.GetRequiredBasicBandwidthUnits(basicBandwidthUnits);
            var session = Session.CreateSession(call.StartTime, call);
            if (!OngoingSessions.TryAdd(session.SessionId, session))
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

            session.UpdateMobileTerminalStateSequence(call.Service);
        }

        public void EndCall(Guid callId, Session session, BasicBandwidthUnits basicBandwidthUnits)
        {
            if (OngoingSessions.TryGetValue(session.SessionId, out Session ongoingSession))
                throw new VerticalHandoverPredictionException("Session not found");

            if (!ongoingSession.ActiveCalls.TryGetValue(callId, out Call callToTerminate))
                throw new VerticalHandoverPredictionException("Call not found");

            // Free up resources
            CurrentLoad -=  callToTerminate.Service.GetRequiredBasicBandwidthUnits(basicBandwidthUnits);

            ongoingSession.ActiveCalls.Remove(callId);

            // Update session sequence state
            var nextState = ongoingSession.MobileTerminalStateSequence.Last.Value &= ~callToTerminate.Service.DeriveMobileTerminalState();
            ongoingSession.MobileTerminalStateSequence.AddLast(nextState);

            // Terminate session if there are no more active calls
            if (!ongoingSession.ActiveCalls.Any())
                OngoingSessions.Remove(ongoingSession.SessionId);

        }
    }
}