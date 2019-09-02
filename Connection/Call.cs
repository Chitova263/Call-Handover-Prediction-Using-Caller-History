using System;
using System.Linq;

namespace VerticalHandoverPrediction
{
    public class Call : ICall
    {
        public Guid CallId { get; private set; }
        public IMobileTerminal MobileTerminal { get; private set; }
        public Service Service { get; private set; }

        //Consider moving sessionId to this object

        private Call(IMobileTerminal mobileTerminal, Service service)
        {
            CallId = Guid.NewGuid();
            MobileTerminal = mobileTerminal;
            Service = service;
        }

        public static Call InitiateCall(IMobileTerminal mobileTerminal, Service service, IJCAC jcac)
        {
            var call = new Call(mobileTerminal, service);
            
            //Handle Handover Algorithm whenever a new call is initiated
            jcac.AdmitCall(call);

            return call;
        }

        public void TerminateCall(ICall call)
        {
            //Must be called by the MT / call object ??? remodel MT and Call Objects
            //Remove call From Session
            //Free Up AssociatedRAT Bandwidth
            throw new NotImplementedException();
        }
    }
}