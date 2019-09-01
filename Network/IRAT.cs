using System;
using System.Collections.Generic;

namespace VerticalHandoverPrediction
{
    public interface IRAT
    {
        Guid RATId { get; }
        int Capacity { get; }
        int UsedCapacity { get; }
        IList<Service> Services { get; set; }
        IList<ISession> OngoingSessions { get; set; }

        void AdmitIncomingCallToNewSession(ICall call);
        void AdmitIncomingCallToOngoingSession(ICall call);
        int AvailableCapacity();
        bool CanAccommodateCall(ICall call);
        void RemoveSessionFromSourceRAT(ISession session);
        void TerminateSession(Guid sessionId);
        void TransferSessionTo(ISession currentSession, ICall call);
    }
}
