using System;
using System.Collections.Generic;
using System.Linq;
using VerticalHandoverPrediction.CallAdmissionControl;
using VerticalHandoverPrediction.CallSession;
using VerticalHandoverPrediction.Mobile;

namespace VerticalHandoverPrediction.Network
{

    public sealed class HetNet : IHetNet
    {
        private static IHetNet instance = null;
        private static readonly object padlock = new object();
        public Guid HetNetId { get; private set; }
        private readonly List<IRat> _rats;
        public IReadOnlyCollection<IRat> Rats => _rats;
        private readonly List<IMobileTerminal> _mobileTerminals;
        public IReadOnlyCollection<IMobileTerminal> MobileTerminals => _mobileTerminals;
        public int VerticalHandovers { get; set; }
        public int BlockedCalls { get; set; }
        public int FailedPredictions { get; set; }
        public int SuccessfulPredictions { get; set; }
        public int CallsGenerated { get; set; }

        private HetNet()
        {
            HetNetId = Guid.NewGuid();

            _rats = new List<IRat>();

            _mobileTerminals = new List<IMobileTerminal>();
        }

        public static IHetNet _HetNet
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

        public void AddRats(IEnumerable<IRat> rats)
        {
            if (rats is null)
            {
                throw new System.ArgumentNullException(nameof(rats));
            }

            _rats.AddRange(rats);
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
                }, 100),
                Rat.CreateRat(new List<Service>
                {
                    Service.Data
                }, 100),
                Rat.CreateRat(new List<Service>
                {
                    Service.Voice, Service.Data
                }, 100),
                Rat.CreateRat(new List<Service>
                {
                    Service.Voice, Service.Video, Service.Data
                }, 100),
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
                networkResources += service.ComputeRequiredCapacity();
            }

            srcRat.SetUsedResources(srcRat.UsedResources - networkResources);

            session.SetRatId(destRat.RatId);

            destRat.SetUsedResources(destRat.UsedResources + networkResources);

            destRat.AddSession(session);

            destRat.AdmitIncomingCallToOngoingSession(call, session, mobileTerminal);
        }
    }
}

