using System;
using System.Collections.Generic;
using VerticalHandoverPrediction.CallSession;
using VerticalHandoverPrediction.Mobile;

namespace VerticalHandoverPrediction.Network
{
    public interface IRat
    {
        Guid RatId { get; }
        int Capacity { get; }
        int UsedCapacity { get; }
        IList<Service> Services { get; set; }
        IList<ISession> OngoingSessions { get; set; }

        int AvailableCapacity();
        void RemoveSessionFromRat(ISession currentSession);
        void TransferSessionToRat(ISession currentSession);
        void AdmitIncomingCallToNewSession(ICall call, IMobileTerminal mobileTerminal);
        void AdmitIncomingCallToOngoingSession(ICall call, ISession session, IMobileTerminal mobileTerminal);
        bool CanAccommodateCall(ICall call);
    }
}