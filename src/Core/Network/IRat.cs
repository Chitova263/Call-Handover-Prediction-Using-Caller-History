namespace VerticalHandoverPrediction.Network
{
    using System;
    using System.Collections.Generic;
    using VerticalHandoverPrediction.CallSession;
    using VerticalHandoverPrediction.Mobile;

    public interface IRat
    {
        Guid RatId { get; }
        string Name { get; }
        int Capacity { get; }
        int UsedNetworkResources { get; }
        IReadOnlyCollection<ISession> OngoingSessions { get; }
        IList<Service> Services { get; set; }

        void AddSession(ISession session);
        void AdmitNewCallToOngoingSession(ISession session, ICall call, IMobileTerminal mobileTerminal);
        int AvailableNetworkResources();
        bool CanAdmitNewCallToOngoingSession(ISession session, ICall call, IMobileTerminal mobileTerminal);
        void RealeaseNetworkResources(int resources);
        void RemoveSession(ISession session);
        void TakeNetworkResources(int resources);
    }
}