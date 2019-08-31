using System;
using System.Collections.Generic;

namespace VerticalHandoverPrediction
{
    public interface IRAT
    {
        Guid RATId { get; }
        int Capacity { get; }
        int UtilizedCapacity { get; }
        IList<Service> Services { get; }
        IList<ICallSession> OngoingSessions { get; }

        IList<ICallSession> AdmitCall(ICallSession session);
        IList<ICallSession> AdmitCallSession(ICallSession session);
        int AvailableBandwidthBasebandUnits();
        IList<ICallSession> DismissCall(ICallSession session);
        IList<ICallSession> DropCallSession(ICallSession session);
    }
}
