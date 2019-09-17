namespace VerticalHandoverPrediction.Network
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using VerticalHandoverPrediction.Cac;
    using VerticalHandoverPrediction.CallSession;
    using VerticalHandoverPrediction.Mobile;

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
            get => CallStartedEventsRejectedWhenIdle + CallStartedEventsRejectedWhenNotIdle + BlockedCalls;
            private set{}
        }

        public int CallEndedEventsRejected { get; set; }
        public int TotalSessions { get; set; }
       
        private HetNet()
        {
            HetNetId = Guid.NewGuid();
            _rats = new List<IRat>();
            _mobileTerminals = new List<IMobileTerminal>();
        }

        public static HetNet Instance
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

        public void Handover(ICall call, ISession session, IMobileTerminal mobileTerminal, IRat source)
        {
            if (call == null)
            {
                throw new VerticalHandoverPredictionException($"{nameof(call)} is invalid");
            }

            if (session == null)
            {
                throw new VerticalHandoverPredictionException($"{nameof(session)} is invalid");
            }

            if (mobileTerminal == null)
            {
                throw new VerticalHandoverPredictionException($"{nameof(mobileTerminal)} is invalid");
            }

            if (source == null)
            {
                throw new VerticalHandoverPredictionException($"{nameof(source)} is invalid");
            }

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
                    VerticalHandovers++;

                    source.RealeaseNetworkResources(requiredNetworkResouces - call.Service.ComputeRequiredNetworkResources());
                    source.RemoveSession(session);

                    session.SetRatId(target.RatId);

                    target.TakeNetworkResources(requiredNetworkResouces);

                    session.AddToActiveCalls(call);

                    var state = mobileTerminal.UpdateMobileTerminalState(session);
            
                    session.AddToSessionSequence(state);

                    target.AddSession(session);

                    return;
                }
            }
            BlockedCalls++;
            return;
        }

        public void GenerateUsers(int users)
        {
            if (users <= 0) 
            {
                throw new VerticalHandoverPredictionException();
            }
           
            for (int i = 0; i < users; i++)
            {
                _mobileTerminals.Add(new MobileTerminal());
            }
        }

        public void GenerateRats()
        {
            var rats = new List<Rat>
            {
                new Rat(new List<Service>
                {
                    Service.Voice
                }, 100,  "RAT 1 (Voice)"),
                new Rat(new List<Service>
                {
                    Service.Data
                }, 100, "RAT 2 (Data)"),
                new Rat(new List<Service>
                {
                    Service.Voice, Service.Data
                }, 100,  "RAT 3 (Voice - Data)"),
                new Rat(new List<Service>
                {
                    Service.Voice, Service.Video, Service.Data
                }, 100,  "RAT 4 (Voice - Data - Video)"),
            };

            _rats.AddRange(rats);
        }
    }
}

