using System;
using System.Collections.Generic;
using System.Linq;
using VerticalHandoverPrediction.CallAdmissionControl;
using VerticalHandoverPrediction.CallSession;
using VerticalHandoverPrediction.Mobile;

namespace VerticalHandoverPrediction.Network
{

    public sealed class HetNet 
    {
        private static HetNet instance = null;
        private static readonly object padlock = new object();
        public Guid HetNetId { get; private set; }
        private readonly List<IRat> _rats;
        public IReadOnlyCollection<IRat> Rats => _rats;
        private readonly List<IMobileTerminal> _mobileTerminals;
        public IReadOnlyCollection<IMobileTerminal> MobileTerminals => _mobileTerminals;
        public int VerticalHandovers { get; set; }
        public int BlockedCalls { get; set; }
        public int FailedPredictions { get; set; }
        public int CallsToBePredictedInitialRatSelection { get; set; }
        public int SuccessfulPredictions { get; set; }
        public int BlockedUsingPredictiveScheme { get; set; }
        public int RandomCallsGenerated { get; set; }
        public int CallsGenerated { get; set; }
        public int CallStartedEventsRejectedWhenIdle { get; set; }
        public int CallStartedEventsRejectedWhenNotIdle { get; set; }
        public int CallStartedEventsRejected
        {
            get
            {
                return CallStartedEventsRejectedWhenIdle + CallStartedEventsRejectedWhenNotIdle + BlockedCalls;
            }
            set{}
        }

        public int CallEndedEventsRejected { get; set; }
        public int TotalSessions { get; set; }
       

        private HetNet()
        {
            HetNetId = Guid.NewGuid();

            _rats = new List<IRat>();

            _mobileTerminals = new List<IMobileTerminal>();
        }

        public static HetNet _HetNet
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new HetNet();
                    }
                    return instance;
                }
            }
        }

        public void Reset() 
        {
            BlockedUsingPredictiveScheme =0;
            BlockedCalls = 0;
            CallEndedEventsRejected = 0;
            CallStartedEventsRejectedWhenIdle =0;
            CallStartedEventsRejectedWhenNotIdle =0;
            CallsGenerated =0;
            CallsToBePredictedInitialRatSelection = 0;
            FailedPredictions = 0;
            RandomCallsGenerated = 0;
            SuccessfulPredictions = 0;
            TotalSessions = 0;
            VerticalHandovers = 0;
            CallStartedEventsRejected = 0;
        } 

        public void AddRats(IEnumerable<IRat> rats)
        {
            if (rats is null)
            {
                throw new System.ArgumentNullException(nameof(rats));
            }

            _rats.AddRange(rats);
        }

        public void Handover(ICall call, ISession session, IMobileTerminal mobileTerminal, IRat source)
        {
            var callsToHandedOverToTargetRat = session.ActiveCalls.Select(x => x.Service).ToList();
            
            callsToHandedOverToTargetRat.Add(call.Service);

            var rats = Rats
                .Where(x => x.RatId != session.RatId
                    && x.Services.ToHashSet().IsSupersetOf(callsToHandedOverToTargetRat))
                .OrderBy(x => x.Services.Count())
                .ToList();
            
            var requiredNetworkResouces = 0;
            foreach (var service in callsToHandedOverToTargetRat)
            {
                requiredNetworkResouces += service.ComputeRequiredNetworkResources();
            }

            foreach (var target in rats)
            {
                if(requiredNetworkResouces <= target.AvailableNetworkResources())
                {
                    HetNet._HetNet.VerticalHandovers++;

                    source.RealeaseNetworkResources(requiredNetworkResouces - call.Service.ComputeRequiredNetworkResources());
                    source.RemoveSession(session);

                    session.SetRatId(target.RatId);

                    target.TakeNetworkResources(requiredNetworkResouces);

                    session.ActiveCalls.Add(call);

                    var state = mobileTerminal.UpdateMobileTerminalState(session);
            
                    session.SessionSequence.Add(state);

                    target.AddSession(session);

                    return;
                }
            }
            HetNet._HetNet.BlockedCalls++;
            return;
        }

        public void AddMobileTerminals(IEnumerable<IMobileTerminal> mobileTerminals)
        {
            if (mobileTerminals is null)
            {
                throw new ArgumentNullException(nameof(mobileTerminals));
            }

            _mobileTerminals.AddRange(mobileTerminals);
        }

        public void GenerateUsers(int users)
        {
            if (users <= 0) throw new VerticalHandoverPredictionException();

            for (int i = 0; i < users; i++)
            {
                _mobileTerminals.Add(MobileTerminal.CreateMobileTerminal(Modality.TrippleMode));
            }
        }

        public void GenerateRats()
        {
            var rats = new List<Rat>
            {
                Rat.CreateRat(new List<Service>
                {
                    Service.Voice
                }, 100,  "RAT 1 (Voice)"),
                Rat.CreateRat(new List<Service>
                {
                    Service.Data
                }, 100, "RAT 2 (Data)"),
                Rat.CreateRat(new List<Service>
                {
                    Service.Voice, Service.Data
                }, 100,  "RAT 3 (Voice - Data)"),
                Rat.CreateRat(new List<Service>
                {
                    Service.Voice, Service.Video, Service.Data
                }, 100,  "RAT 4 (Voice - Data - Video)"),
            };

            AddRats(rats);
        }

        public void HandoverSessionToNewRat(ICall call, ISession session, IRat srcRat, IRat destRat, IMobileTerminal mobileTerminal)
        {
            if (call is null)
            {
                throw new ArgumentNullException(nameof(call));
            }

            if (session is null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            if (srcRat is null)
            {
                throw new ArgumentNullException(nameof(srcRat));
            }

            if (destRat is null)
            {
                throw new ArgumentNullException(nameof(destRat));
            }

            if (mobileTerminal is null)
            {
                throw new ArgumentNullException(nameof(mobileTerminal));
            }

            srcRat.RemoveSession(session);

            var services = session.ActiveCalls
                .Select(x => x.Service);

            var networkResources = 0;

            foreach (var service in services)
            {
                networkResources += service.ComputeRequiredNetworkResources();
            }

            //srcRat.SetUsedResources(srcRat.UsedNetworkResources - networkResources);

            //session.SetRatId(destRat.RatId);

            //destRat.SetUsedResources(destRat.UsedNetworkResources + networkResources);

            //destRat.AddSession(session);

            //destRat.AdmitIncomingCallToOngoingSession(call, session, mobileTerminal);
        }
    }
}

