using System;

namespace VerticalHandoverPrediction
{
    public interface ICall
    {
        Guid CallId { get; set; }
        MobileTerminal MobileTerminal { get; set; }
        Service Service { get; set; }
    }
}