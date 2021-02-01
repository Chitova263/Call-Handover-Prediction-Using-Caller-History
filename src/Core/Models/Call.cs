using System;

namespace VerticalHandoverPrediction
{
    public sealed class Call
    {
        public Guid CallId { get; }
        public DateTime StartTime { get; }
        public Service Service { get; }

        private Call(Service service, DateTime startTime)
        {
            CallId = Guid.NewGuid();
            Service = service;
            StartTime = startTime;
        }

        public static Call CreateCall(Service service, DateTime startTime)
        {
            return new Call(service, startTime);
        }
    }
}