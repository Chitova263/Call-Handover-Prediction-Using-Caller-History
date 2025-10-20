using Simulator.CallAdmissionControlAlgorithm;
using Simulator.Network;

var simulator = NetworkSimulatorBuilder
    .Configure()
    .NumberOfMultiModeMobileTerminals(200)
    .ConfigureRadioAccessTechnology(options => options.WithDataEnabled().HasMaxBasicBandwidthUnits(2))
    .ConfigureRadioAccessTechnology(options => options.WithDataEnabled().HasMaxBasicBandwidthUnits(5))
    .ConfigureRadioAccessTechnology(options => options.WithVoiceEnabled().WithDataEnabled().HasMaxBasicBandwidthUnits(8))
    .ConfigureRadioAccessTechnology(options => options.WithVoiceEnabled().WithDataEnabled().WithVideoEnabled().HasMaxBasicBandwidthUnits(10))
    .ConfigureNetworkParameters(options => options
        .SetVoiceBandwidthRequirement(1)
        .SetDataBandwidthRequirement(2)
        .SetVideoBandwidthRequirement(3)
        .SetCallArrivalRate(20)
    )
    .Build();

simulator.Run(new NonPredictiveCallHandoverAlgorithm());    