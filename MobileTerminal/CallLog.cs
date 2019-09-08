using System;

namespace VerticalHandoverPrediction.Mobile
{
    public class CallLog
    {
        public Guid UserId { get; set; }
        public Guid SessionId { get; set; }
        public Guid RatId { get; set; }
        public TimeSpan Duration { get; set; }
        public string SessionSequence { get; set; }
    }
}