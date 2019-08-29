using System;

namespace HandoverPrediction
{
    public class Call 
    {
        public Guid SessionId { get; }
        public MobileTerminal MobileTerminal { get; set; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public Service Service { get; }

        ///
        public Call(MobileTerminal mobileTerminal, Service service)
        {
            SessionId = Guid.NewGuid();
            StartTime = DateTime.Now;
            MobileTerminal = mobileTerminal ?? throw new ArgumentNullException(nameof(mobileTerminal));
            Service = service;
        }

        //compute call duration from the state
        //get the bandwidth usage of the type of call,  derived from the state 
        //what RAT is accommodating the call

    }
}

