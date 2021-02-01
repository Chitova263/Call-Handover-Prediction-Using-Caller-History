using System;
using System.Collections.Generic;
using System.Linq;

namespace VerticalHandoverPrediction
{
    public class NonPredictiveAlgorithm : Algorithm
    {
        public override void Admit(
            IEvent @event,
            Network network,
            BasicBandwidthUnits basicBandwidthUnits,
            HashSet<Guid> IgnoreEvents)
        {
            // Handle CallStarted Events
            if(@event is CallStartedEvent evt)
            {
               
            }
            // Handle CallEnded Events
            else
            {

            }
        }


        private bool HandleCallStartedEvent(
            CallStartedEvent @event,
            Network network,
            BasicBandwidthUnits basicBandwidthUnits,
            HashSet<Guid> IgnoreEvents)
        {
            MobileTerminal mobileTerminal;

            if (!network.MobileTerminals.TryGetValue(@event.MobileTerminal.MobileTerminalId, out mobileTerminal))
            {
                throw new VerticalHandoverPredictionException("mobile terminal not found");
            }

            if (mobileTerminal.State == MobileTerminalState.Idle)
            {
                return HandleIncomingCallForIdleTerminal(@event, network, basicBandwidthUnits, IgnoreEvents);
            } 
            else
            {
                return false;
            }
        }


        private bool HandleIncomingCallForIdleTerminal(
            CallStartedEvent @event,
            Network network,
            BasicBandwidthUnits basicBandwidthUnits,
            HashSet<Guid> IgnoreEvents)
        {
            MobileTerminal mobileTerminal;
            if (!network.MobileTerminals.TryGetValue(@event.MobileTerminal.MobileTerminalId, out mobileTerminal))
            {
                throw new VerticalHandoverPredictionException("mobile terminal not found");
            }

            var call = Call.CreateCall(@event.Service, @event.Timestamp);

            IEnumerable<Rat> rats = network.GetCompatibleRats(@event.Service);
            foreach (var rat in rats)
            {
                if (rat.CanAdmitCall(call.Service, basicBandwidthUnits))
                {
                    rat.AdmitInitialCall(call, basicBandwidthUnits);
                    //*************
                    // RECORD: 1. Call Admitted 2. Type of Call Dropped
                    //*************
                    return true;
                }
            }
            // If you reach here it means there is no available RAT to handle call, call is dropped
            //*************
            //RECORD:  1. Call Dropped 2. Type of Call Dropped
            //*************
            IgnoreEvents.Add(@event.EventId);
            return false;
        }

        private bool HandleIncomingCallForActiveTerminal(
            CallStartedEvent @event,
            Network network,
            BasicBandwidthUnits basicBandwidthUnits,
            HashSet<Guid> IgnoreEvents)
        {
            MobileTerminal mobileTerminal;
            if (!network.MobileTerminals.TryGetValue(@event.MobileTerminal.MobileTerminalId, out mobileTerminal))
            {
                throw new VerticalHandoverPredictionException("mobile terminal not found");
            }

            var call = Call.CreateCall(@event.Service, @event.Timestamp);

            // RAT with ongoing call
            var rat = network.Rats
                    .FirstOrDefault(o => o.Value.OngoingSessions.Any(o => o.Key == mobileTerminal.Session.SessionId))
                    .Value;

            var requiredService = mobileTerminal.Session.ActiveCalls
                         .Select(o => o.Value.Service)
                         .Aggregate(call.Service, (acc, s) => acc | s);

            // Can RAT admit the incoming call
            if (rat.CanAdmitCall(requiredService, basicBandwidthUnits))
            {
                int resources = requiredService.GetRequiredBasicBandwidthUnits(basicBandwidthUnits);
                rat.AdmitIncomingCallToOngoingSession(call, mobileTerminal.Session, resources);
                return true;
            }
            // Find Compatible RATs and Attempt Vertical Handover
            else
            {
                // Exclude current RAT from selection, Ordered by priority
                var rats = network.GetCompatibleRats(requiredService)
                    .Where(r => r.RatId != rat.RatId);

                foreach (var possibleRat in rats)
                {
                    if(possibleRat.CanAdmitCall(requiredService, basicBandwidthUnits))
                    {
                        //Network Performs A Handover Action


                        //**************************************
                        // Record  1. Successfull Vertical Handover
                        //**************************************
                        return true;
                    }
                }
                // If you reach here failed handover, incoming call dropped
                //**************************************
                // Record  1. Failled Vertical Handover
                //**************************************
            }
            return false;
        }
    }
}
