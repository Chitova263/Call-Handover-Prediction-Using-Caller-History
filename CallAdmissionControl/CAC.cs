using VerticalHandoverPrediction.CallSession;
using VerticalHandoverPrediction.Network;
using System.Linq;
using VerticalHandoverPrediction.Mobile;
using static MoreLinq.Extensions.StartsWithExtension;
using System.Collections.Generic;
using System;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using MediatR;
using VerticalHandoverPrediction.Simulator;

//Converted to a singleton
namespace VerticalHandoverPrediction.CallAdmissionControl
{

    public class CAC : ICAC
    {
        private CAC()
        {
            
        }

        public static ICAC StartCACAlgorithm()
        {
            return new CAC();
        }
        
        public void AdmitCall(ICall call)
        {
            //Find the mobile terminal involved in call [From The HetNet]
            var mobileTerminal = HetNet._HetNet.MobileTerminals
                .FirstOrDefault(m => m.MobileTerminalId == call.MobileTerminalId);

            IRat currentRat = default(IRat);

            //If incoming call is on a mobile terminal on an active session
            if(mobileTerminal.State != MobileTerminalState.Idle)
            {
                //Find the Rat accommodating the current session
                var currentSession = HetNet._HetNet.Rats
                    .SelectMany(x => x.OngoingSessions)
                    .FindSessionWithId(mobileTerminal.SessionId);
                
                //--------------------------------------------------------------------
                //Discard Call if there is already an active call of the type on mobile
                var services = currentSession.ActiveCalls
                    .Select(x => x.Service)
                    .ToList();

                if(services.Contains(call.Service)) return;
                //---------------------------------------------------------------------

                //call generation successful
                HetNet._HetNet.CallsGenerated++;

                currentRat = HetNet._HetNet.Rats
                    .FirstOrDefault(x => x.RatId == currentSession.RatId);
                
                //Check if the current Rat can admit the incoming call
                if(currentRat.CanAccommodateCall(call))
                {
                    //Accommodate incoming call to the currentSession
                    currentRat.AdmitIncomingCallToOngoingSession(call,currentSession,mobileTerminal);
                    
                    //Call Admission Successful End CAC
                    //Publish event
                    var mediator = DIContainer._Container.Container.GetRequiredService<IMediator>();
                    var @event = new CallStartedEvent(DateTime.Now.AddMinutes(1), call.CallId, call.MobileTerminalId, call.SessionId);
                    mediator.Publish(@event).Wait();

                    return; //return true here
                }
                //If current Rat cannot accomodate the call we need to handover the session to another RAT
                else
                {
                    StartHandoverProcess(call, currentSession, currentRat, mobileTerminal);
                }
            } 
            //new call, no ongoing session
            else
            {
                //Update Calls Generated
                HetNet._HetNet.CallsGenerated++;

                //NonPredictiveAlgorithm(call, mobileTerminal);
                PredictiveAlgorithm(call, mobileTerminal);
            }
            
        }

        private static void NonPredictiveAlgorithm(ICall call, IMobileTerminal mobileTerminal)
        {
            //Non Predictive algorithm : => Swap this with predictive code
            var candidateRats = HetNet._HetNet.Rats
                .Where(x => x.Services.Contains(call.Service))
                .OrderBy(x => x.Services.Count)
                .ToList();

            foreach (var rat in candidateRats)
            {
                if (rat.CanAccommodateCall(call))
                {
                    rat.AdmitIncomingCallToNewSession(call, mobileTerminal);
                    break;
                }
                else
                {
                    //All the RATs cant accommodate new call
                    HetNet._HetNet.BlockedCalls++;
                }
            }
        }

        public void StartHandoverProcess(ICall call, ISession currentSession, IRat sourceRat, IMobileTerminal mobileTerminal)
        {
            //Find services in ongoing session
            var services = currentSession.ActiveCalls
                .Select(x => x.Service)
                .ToList();
            //Add the service of new call
            services.Add(call.Service);

            //Find Candidate Rats that support required services
            var candidateRats = HetNet._HetNet.Rats
                .Where(x => x.RatId != currentSession.RatId
                    && x.Services.ToHashSet().IsSupersetOf(services))
                .OrderBy(x => x.Services.Count())
                .ToList();
            
            //From the candidate Rats find rat that has enough capacity to admit new session
            var requiredBbu = 0;
            foreach (var service in services)
            {
                requiredBbu += service.ComputeRequiredCapacity();
            }

            //NB: session keeps the same SessionId for tracking through RATs
            //1. Transfer ongoing session to target RAT
            //2. Add new call to ongoing session at target rat
            foreach (var destinationRat in candidateRats)
            {
                //If there is Rat with enough network resources
                if (destinationRat.AvailableCapacity() >= requiredBbu)
                {
                    InitiateHandover(call, sourceRat, destinationRat, currentSession, mobileTerminal);
                    //Handover is successfull
                    HetNet._HetNet.VerticalHandovers++;

                    
                    return;  //return true and publish event to queue
                }
            }
            //If you reach here block the incoming call
            HetNet._HetNet.BlockedCalls++;
            return; //return false dont publish to queue 
        }

        private void InitiateHandover(ICall call, IRat sourceRat, IRat destinationRat, ISession currentSession, IMobileTerminal mobileTerminal)
        {
            //Remove session from source Rat
            sourceRat.RemoveSessionFromRat(currentSession);
            //Transfer session to target Rat
            destinationRat.TransferSessionToRat(currentSession);
            //Admit the call new call to 
            destinationRat.AdmitIncomingCallToOngoingSession(call, currentSession, mobileTerminal);
        }

        public void PredictiveAlgorithm(ICall call, IMobileTerminal mobileTerminal)
        {
            //obtain call history from mobile termina
            var callHistory = mobileTerminal.CallHistoryLogs
                .Select(x => x.SessionSequence)
                .ToList();

            var nextState = default(MobileTerminalState);
            
            if(callHistory.Count() == 0) nextState = call.Service.GetState();
            else
            {
                //Predict the next state
                var group = callHistory
                    .Select(x => x.Skip(1).Take(2))
                    .Where(x => x.StartsWith(new List<MobileTerminalState>{call.Service.GetState()}))
                    .SelectMany(x => x.Skip(1))
                    .GroupBy(x => x);

                IGrouping<MobileTerminalState, MobileTerminalState> prediction = default(IGrouping<MobileTerminalState, MobileTerminalState>);
                int max = 0;
                foreach(var grp in group )
                {
                    if(grp.Count() > max) 
                    {
                        prediction = grp;
                        max = prediction.Count();
                    }
                // Console.WriteLine( $"next state is {grp.Key}, Frequency: {grp.Count()}");
                }

                foreach( var grp in group )
                {
                    Console.WriteLine( $"next state is {grp.Key}, Frequency: {grp.Count()}");
                }
                nextState = prediction.Key;
                Log.Information($"---- The predicted state is @{nextState.ToString()} @{nameof(nextState)}");
            }

            //Find the services required for the current call and predicted state
            var services = new List<Service>{call.Service};
            foreach (var service in nextState.SupportedServices())
            {
                if(!services.Contains(service)) services.Add(service);
            }

            //Find the candidate RATs From HetNet to accomodate only the predicted states
            var candidateRats = HetNet._HetNet.Rats
                .Where(x => x.Services.ToHashSet().IsSupersetOf(services))
                .OrderBy(x => x.Services.Count)
                .ToList();

            //NB: There is no allocation of bandwidth in advance
            foreach (var rat in candidateRats)
            {
                if(rat.CanAccommodateServices(services))   
                {
                    //Start new session
                    rat.AdmitIncomingCallToNewSession(call, mobileTerminal);
                    
                    //If Accommodated via a prediction
                    if(callHistory.Count() != 0) {
                        HetNet._HetNet.SuccessfulPredictions++;
                        Log.Information($"----- VHO Prediction Successful Rat:  @{rat.RatId}, Total Successful Predictions : @{HetNet._HetNet.SuccessfulPredictions} ");
                    }
                      
                    break;
                }
                else 
                {
                    //If prediction fails
                    if(callHistory.Count() != 0) {
                        HetNet._HetNet.FailedPredictions++;
                        Log.Information($"---- VHO Prediction Failled, Total Failed Predictions : @{HetNet._HetNet.FailedPredictions} ");
                    }

                    //Try accommodating call using non predictive scheme
                    NonPredictiveAlgorithm(call, mobileTerminal);
                   
                    //No RAT can accommodate the incoming call so drop call
                    HetNet._HetNet.BlockedCalls++;
                    Log.Information($"---- Incoming Call Blocked, Total Calls Blocked : @{HetNet._HetNet.BlockedCalls} ");
                }
            }
        }
    }
}
