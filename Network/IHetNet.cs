using System;
using System.Collections.Generic;
using VerticalHandoverPrediction.CallSession;
using VerticalHandoverPrediction.Mobile;

namespace VerticalHandoverPrediction.Network
{
    public interface IHetNet
    {
        Guid HetNetId { get; }
        IReadOnlyCollection<IRat> Rats { get; }
        IReadOnlyCollection<IMobileTerminal> MobileTerminals { get; }
        int VerticalHandovers { get; set; }
        int BlockedCalls { get; set; }
        int FailedPredictions { get; set; }
        int SuccessfulPredictions { get; set; }
        int CallsGenerated { get; set; }

        void AddMobileTerminals(IEnumerable<IMobileTerminal> mobileTerminals);
        void AddRats(IEnumerable<IRat> rats);
        void GenerateRats();
        void GenerateUsers(int users);
        void HandoverSessionToNewRat(ICall call, ISession session, IRat srcRat, IRat destRat, IMobileTerminal mobileTerminal);
    }
}

