using System;

namespace VerticalHandoverPrediction
{
    public class Call : ICall
    {
        public Guid CallId { get; set; }
        public MobileTerminal MobileTerminal { get; set; }
        public Service Service { get; set; }

        private Call(MobileTerminal mobileTerminal, Service service)
        {
            CallId = Guid.NewGuid();
            MobileTerminal = mobileTerminal;
            Service = service;
        }

        public static Call InitiateCall(MobileTerminal mobileTerminal, Service service)
        {
            var call = new Call(mobileTerminal, service);

            //****************************************** */
            //Perform the Algorithm RAT Selection Algorithm Here Before Recording The Call

            //Follow The FlowChart
            //What happens if history is empty
            //if history is not empty, do the prediction here

            //****************************************** */

            if (!mobileTerminal.IsOnActiveSession())
            {

                mobileTerminal.SetMobileTerminalCurrentState(service);
                mobileTerminal.CurrentSession = CallSession.InitiateSession(mobileTerminal);
            }
            else
            {
                //if on an active session
                mobileTerminal.SetMobileTerminalCurrentState(service);
                mobileTerminal.CurrentSession.UpdateCallSessionSequence(mobileTerminal.CurrentState);
            }
            mobileTerminal.CurrentSession.ActiveCalls.Add(call);
            return call;
        }

    }
}