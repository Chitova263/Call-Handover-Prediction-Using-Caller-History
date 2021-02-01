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
            if (@event is CallStartedEvent evt)
            {
                MobileTerminal mobileTerminal;
                var found = network.MobileTerminals.TryGetValue(evt.MobileTerminal.MobileTerminalId, out mobileTerminal);
                
                if (!found)
                    throw new VerticalHandoverPredictionException("mobile terminal not found");
                
                // Check if its the first call
                if (mobileTerminal.State == MobileTerminalState.Idle)
                {
                    // Find The Suitable RAT to Admit the call, 
                    // If none then block the call & mark the respective CallEndedEvent from the priority Queue
                    var call = Call.CreateCall(evt.Service, evt.Timestamp);

                    var compatibleRats = network.GetCompatibleRats(evt.Service); 

                    foreach (var rat in compatibleRats)
                    {
                        bool admitted = rat.Value.AdmitInitialCall(call, basicBandwidthUnits);

                        // Admitted no handovers involved
                        if (admitted)
                        {
                            // Record call admitted
                            return;
                        }
                    }
                    // If you reach here call is dropped

                    // 1. Record call dropped
                    // 2. Mark respective CallEndedEvent to be ignored
                    IgnoreEvents.Add(evt.EventId);
                }
                else
                {
                    var call = Call.CreateCall(evt.Service, evt.Timestamp);
                    // Mobile Terminal Has An Ongoing Session
                    // Find The RAT having this session
                    var rat = network.Rats
                        .FirstOrDefault(o => o.Value.OngoingSessions.Any(o => o.Key == mobileTerminal.Session.SessionId))
                        .Value;

                    if (!rat.CanAdmitToOngoingSession(call, basicBandwidthUnits))
                    {
                        var requiredService = mobileTerminal.Session.ActiveCalls
                            .Select(o => o.Value.Service)
                            .Aggregate(call.Service, (acc, s) => acc | s);

                        var compatibleRats = network.GetCompatibleRats(requiredService);

                        foreach (var compatibleRat in compatibleRats)
                        {
                            bool admitted = compatibleRat.Value.CanAdmitToOngoingSession(call, basicBandwidthUnits);
                            if (admitted)
                            {
                                // Perform Handover Action
                            }
                        }
                    }
                }
            }
            else
            {

            }
        }
    }
}
