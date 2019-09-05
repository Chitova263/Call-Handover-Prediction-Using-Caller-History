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

        void AdmitIncomingCallToNewSession(ICall call, IMobileTerminal mobileTerminal);
        void AdmitIncomingCallToOngoingSession(ICall call, ISession session, IMobileTerminal mobileTerminal);
        int AvailableCapacity();
        bool CanAccommodateCall(ICall call);
        bool CanAccommodateServices(List<Service> services);
        void RemoveSessionFromRat(ISession session);
        void SetUsedCapacity(int bbu);
        void TransferSessionToRat(ISession currentSession);
    }
}