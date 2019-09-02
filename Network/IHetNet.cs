using System;
using System.Collections.Generic;

namespace VerticalHandoverPrediction
{
    public interface IHetNet
    {
        Guid HetNetId { get; }
        IList<RAT> RATs { get; }

        bool InitiateHandoverProcess(ICall call);
        bool InitiateHandoverPrediction(ICall call);
    }
}
