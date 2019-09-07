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
        int UsedNetworkResources { get; }
        IReadOnlyCollection<ISession> OngoingSessions { get; }
        IList<Service> Services { get; set; }

        void AddSession(ISession session);
        void AdmitIncomingCallToNewSession(ICall call, IMobileTerminal mobileTerminal);
        void AdmitIncomingCallToOngoingSession(ICall call, ISession session, IMobileTerminal mobileTerminal);
        int AvailableNetworkResources();
        bool CanAccommodateCall(ICall call);
        bool CanAccommodateServices(List<Service> services);
        void RealeaseNetworkResources(int resources);
        void RemoveSession(ISession session);
        void SetRatId(Guid id);
    }
}