namespace VerticalHandoverPrediction.Mobile
{
    using System;
    using CsvHelper.Configuration;

    public class CallLog
    {
        public Guid UserId { get; set; }
        public Guid SessionId { get; set; }
        public TimeSpan Duration { get; set; }
        public string SessionSequence { get; set; }
    }

    public class CallLogMap: ClassMap<CallLog>
    {
        public CallLogMap() => AutoMap();
    }
}