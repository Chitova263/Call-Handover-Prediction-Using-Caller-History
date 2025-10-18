using VerticalHandoverPrediction.Exceptions;
using VerticalHandoverPrediction.Extensions;
using VerticalHandoverPrediction.Models;
using VerticalHandoverPrediction.Simulator;
using VerticalHandoverPrediction.Simulator.Events;

namespace VerticalHandoverPrediction.CallAdmissionControlAlgorithms
{
    public sealed class NonPredictiveAlgorithm : Algorithm
    {
      
        public override void Admit(
            IEvent @event,
            Network network,
            BasicBandwidthUnits basicBandwidthUnits,
            HashSet<Guid> ignoreEvents)
        {
            // Handle CallStarted Events
            if (@event is CallStartedEvent evt)
            {
                if (@evt.MobileTerminal.State == MobileTerminalState.Idle)
                    HandleIncomingCallForIdleTerminal(evt, network, basicBandwidthUnits, ignoreEvents);
                else
                    HandleIncomingCallForActiveTerminal(evt, network, basicBandwidthUnits, ignoreEvents);
            }
            // Handle CallEnded Events
            else
                HandleCallEndedEvent(@event as CallEndedEvent, network, basicBandwidthUnits);
        }
        
        private bool HandleIncomingCallForIdleTerminal(
            CallStartedEvent @event,
            Network network,
            BasicBandwidthUnits basicBandwidthUnits,
            HashSet<Guid> ignoreEvents)
        {
            MobileTerminal mobileTerminal;
            if (!network.MobileTerminals.TryGetValue(@event.MobileTerminal.MobileTerminalId, out mobileTerminal))
            {
                throw new VerticalHandoverPredictionException("mobile terminal not found");
            }

            var call = Call.CreateCall(@event.EventId, @event.Service, @event.Timestamp);

            IEnumerable<Rat> rats = network.GetCompatibleRats(@event.Service);
            foreach (var rat in rats)
            {
                if (rat.CanAdmitCall(call.Service, basicBandwidthUnits))
                {
                    rat.AdmitInitialCall(call, basicBandwidthUnits);
                   
                    _result.LogAdmittedCall(call.Service);

                    return true;
                }
            }
            // If you reach here it means there is no available RAT to handle call, call is dropped
            _result.LogDroppedCall(call.Service);

            ignoreEvents.Add(@event.EventId);
            return false;
        }

        private bool HandleIncomingCallForActiveTerminal(
            CallStartedEvent @event,
            Network network,
            BasicBandwidthUnits basicBandwidthUnits,
            HashSet<Guid> ignoreEvents)
        {
            MobileTerminal mobileTerminal;
            if (!network.MobileTerminals.TryGetValue(@event.MobileTerminal.MobileTerminalId, out mobileTerminal))
            {
                throw new VerticalHandoverPredictionException("mobile terminal not found");
            }

            var call = Call.CreateCall(@event.EventId, @event.Service, @event.Timestamp);

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

                _result.LogAdmittedCall(call.Service);

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
                        network.ExecuteHandover(
                            sourceRat: rat, 
                            targetRat: possibleRat, 
                            currentSession: mobileTerminal.Session,
                            call: call,
                            requiredResources: requiredService.GetRequiredBasicBandwidthUnits(basicBandwidthUnits));
                        
                        // Successfull Vertical Handover
                        _result.LogAdmittedCall(call.Service);
                        _result.LogVerticalHandover(true);

                        return true;
                    }
                }
            }
            // If you reach here failed handover, incoming call dropped
            // Failled Vertical Handover
            _result.LogDroppedCall(call.Service);
            _result.LogVerticalHandover(false);
            
            return false;
        }

        private void HandleCallEndedEvent(
            CallEndedEvent @event,
            Network network,
            BasicBandwidthUnits basicBandwidthUnits)
        {
            MobileTerminal mobileTerminal;
            if (!network.MobileTerminals.TryGetValue(@event.MobileTerminal.MobileTerminalId, out mobileTerminal))
            {
                throw new VerticalHandoverPredictionException("mobile terminal not found");
            }

            // Find RAT with the ongoing session
            var rat = network.Rats
                .Select(o => o.Value)
                .FirstOrDefault(o => o.OngoingSessions.ContainsKey(mobileTerminal.Session.SessionId));
            
            rat.EndCall(@event.CallStartedEventId, mobileTerminal.Session, basicBandwidthUnits);
        }
    }
}
