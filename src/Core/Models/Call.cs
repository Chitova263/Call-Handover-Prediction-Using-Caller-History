namespace VerticalHandoverPrediction.Models
{
    public sealed class Call
    {
        public Guid CallId { get; }
        public DateTime StartTime { get; }
        public Service Service { get; }

        private Call(Guid callId, Service service, DateTime startTime)
        {
            CallId = callId;
            Service = service;
            StartTime = startTime;
        }

        public static Call CreateCall(Guid callId, Service service, DateTime startTime)
        {
            return new Call(callId, service, startTime);
        }
    }
}