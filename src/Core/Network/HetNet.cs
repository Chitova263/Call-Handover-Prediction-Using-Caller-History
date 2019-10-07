namespace VerticalHandoverPrediction.Network
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using VerticalHandoverPrediction.CallAdimissionControl;
    using VerticalHandoverPrediction.CallSession;
    using VerticalHandoverPrediction.Mobile;
    using VerticalHandoverPrediction.Utils;

    public sealed class HetNet 
    {
        private static HetNet instance = null;
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
        public int CallsGenerated { get; set; }
        public int VoiceCallsGenerated { get; set; }
        public int VoiceHandovers { get; set; }
        public int VideoCallsGenerated { get; set; }
        public int VideoHandovers { get; set; }
        public int DataCallsGenerated { get; set; }
        public int DataHandovers { get; set; }
        public int TotalSessions { get; set; }
        public IList<CallLog> CallerHistory { get; set; }
       
        private HetNet()
        {
            HetNetId = Guid.NewGuid();
            _rats = new List<IRat>();
            _mobileTerminals = new List<IMobileTerminal>();
            CallerHistory = new List<CallLog>();
        }

        public void LoadHistory()
        {
            CallerHistory = CsvUtils._Instance.Read<CallLogMap,CallLog>($"{Environment.CurrentDirectory}/calllogs.csv").ToList();
        }

        public static HetNet Instance
        {
            get
            {
                if (instance == null)
                    instance = new HetNet();

                return instance;
            }
        }

        public void Reset() 
        {
            BlockedUsingPredictiveScheme =0;
            BlockedCalls = 0;
            CallsGenerated =0;
            CallsToBePredictedInitialRatSelection = 0;
            FailedPredictions = 0;
            SuccessfulPredictions = 0;
            TotalSessions = 0;
            VerticalHandovers = 0;
            VoiceCallsGenerated = 0;
            VideoCallsGenerated = 0;
            DataCallsGenerated = 0;
            VoiceHandovers = 0;
            VideoHandovers = 0;
            DataHandovers = 0;
        }

        public void UpdateHandovers(Service service)
        {
            VerticalHandovers++;

            switch (service)
            {
                case Service.Data:
                    DataHandovers++;
                    break;
                case Service.Voice:
                    VoiceHandovers++;
                    break;
                case Service.Video:
                    VideoHandovers++;
                    break;
            }
        }

        public void UpdateGeneratedCalls(Service service)
        {
            CallsGenerated ++;

            switch (service)
            {
                case Service.Data:
                    DataCallsGenerated++;
                    break;
                case Service.Voice:
                    VoiceCallsGenerated++;
                    break;
                case Service.Video:
                    VideoCallsGenerated++;
                    break;
            }
        } 

        public void Handover(ICall call, ISession session, IMobileTerminal mobileTerminal, IRat source)
        {
            if (call == null)
                throw new VerticalHandoverPredictionException($"{nameof(call)} is invalid");
            
            if (session == null)
                throw new VerticalHandoverPredictionException($"{nameof(session)} is invalid");
            
            if (mobileTerminal == null)
                throw new VerticalHandoverPredictionException($"{nameof(mobileTerminal)} is invalid");
            
            if (source == null)
                throw new VerticalHandoverPredictionException($"{nameof(source)} is invalid");

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
                    UpdateHandovers(call.Service);

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
            //If call cannot be handed over then the incoming call is blocked
            BlockedCalls++;
        }

        public void GenerateMobileTerminals(int numOfUsers)
        {  
            for (int i = 0; i < numOfUsers; i++)
                _mobileTerminals.Add(new MobileTerminal());
        }

        public void GenerateRats(int c1, int c2, int c3, int c4)
        {
            var rats = new List<Rat>
            {
                new Rat(new List<Service> { Service.Voice }, c1,  "RAT 1 (Voice)"),
                new Rat(new List<Service> { Service.Data }, c2, "RAT 2 (Data)"),
                new Rat(new List<Service> { Service.Voice, Service.Data }, c3,  "RAT 3 (Voice - Data)"),
                new Rat(new List<Service> { Service.Voice, Service.Video, Service.Data }, c4,  "RAT 4 (Voice - Data - Video)"),
            };

            _rats.AddRange(rats);
        }
    }
}

