using FiniteStateMachine;
using FiniteStateMachine.RuleDefinition;
using Simulator.MultiModeMobileTerminal;
using Simulator.Rat;

namespace Simulator.Network;

public class NetworkSimulatorBuilder
{
    private NetworkConfigurationOptions? _networkConfigurationOptions;
    private readonly List<RadioAccessTechnologyOptions> _radioAccessTechnologyOptions;
    private static readonly FiniteStateMachine<MobileTerminalState,MobileTerminalEvent> MobileTerminalStateMachine = GetDefaultMobileTerminalFiniteStateMachine();
    private int _mobileTerminalCount;
    
    private NetworkSimulatorBuilder()
    {
        _radioAccessTechnologyOptions = [];
        _mobileTerminalCount = 10;
    }
    
    public static NetworkSimulatorBuilder Configure()
    {
        return new NetworkSimulatorBuilder();
    }

    public NetworkSimulatorBuilder NumberOfMultiModeMobileTerminals(int mobileTerminalCount)
    {
        _mobileTerminalCount = mobileTerminalCount;
        return this;
    }

    public NetworkSimulatorBuilder ConfigureRadioAccessTechnology(Action<RadioAccessTechnologyOptionsBuilder> configureRadioAccessTechnologyOptions)
    {
        var radioAccessTechnologyOptionsBuilder = new RadioAccessTechnologyOptionsBuilder();
        configureRadioAccessTechnologyOptions(radioAccessTechnologyOptionsBuilder);
        _radioAccessTechnologyOptions.Add(radioAccessTechnologyOptionsBuilder.Build());
        return this;
    }
    
    public NetworkSimulatorBuilder ConfigureNetworkParameters(Action<NetworkConfigurationOptionsBuilder> configureNetworkOptions)
    {
        var networkConfigurationOptionsBuilder = new NetworkConfigurationOptionsBuilder();
        configureNetworkOptions(networkConfigurationOptionsBuilder);
        _networkConfigurationOptions = networkConfigurationOptionsBuilder.Build();
        return this;
    }
    
    private static FiniteStateMachine<MobileTerminalState, MobileTerminalEvent> GetDefaultMobileTerminalFiniteStateMachine()
    {
        return FiniteStateMachineBuilder<MobileTerminalState, MobileTerminalEvent>
            .Define(initialState: MobileTerminalState.Idle)
            .AddRule(TransitionRuleBuilder<MobileTerminalState, MobileTerminalEvent>
                .Builder()
                .WhenInState(MobileTerminalState.Idle)
                .OnTrigger(MobileTerminalEvent.StartVoiceCall)
                .TransitionTo(MobileTerminalState.VoiceCallActive)
                .Build()
            )
            .AddRule(TransitionRuleBuilder<MobileTerminalState, MobileTerminalEvent>
                .Builder()
                .WhenInState(MobileTerminalState.VoiceCallActive)
                .SelfTransitionOn(MobileTerminalEvent.StartVoiceCall)
            )
            .AddRule(TransitionRuleBuilder<MobileTerminalState, MobileTerminalEvent>
                .Builder()
                .WhenInState(MobileTerminalState.DataCallActive)
                .OnTrigger(MobileTerminalEvent.StartVoiceCall)
                .TransitionTo(MobileTerminalState.DataCallActive | MobileTerminalState.VoiceCallActive)
                .Build()
            )
            .AddRule(TransitionRuleBuilder<MobileTerminalState, MobileTerminalEvent>
                .Builder()
                .WhenInState(MobileTerminalState.Idle)
                .OnTrigger(MobileTerminalEvent.StartDataCall)
                .TransitionTo(MobileTerminalState.DataCallActive)
                .Build()
            )
            .Build();
    }

    public NetworkSimulator Build()
    {
        var heterogeneousNetwork = HeterogeneousNetwork.Create(
            BuildMobileTerminals(),
            BuildRadioAccessTechnologies(),
            _networkConfigurationOptions ?? throw new Exception("No network configuration options specified."));
        return NetworkSimulator.Create(heterogeneousNetwork, new PoissonCallEventGenerator(10D, 100));
    }

    private Dictionary<Guid, RadioAccessTechnology> BuildRadioAccessTechnologies()
    {
        Dictionary<Guid, RadioAccessTechnology> radioAccessTechnologies = [];
        foreach (var option in _radioAccessTechnologyOptions)
        {
            HashSet<MobileTerminalEvent> events = [];
            if (option.IsVoiceEnabled)
            {
                events.Add(MobileTerminalEvent.StartVoiceCall);
            }
            if (option.IsDataEnabled)
            {
                events.Add(MobileTerminalEvent.StartDataCall);
            }
            if (option.IsVideoEnabled)
            {
                events.Add(MobileTerminalEvent.StartVideoCall);
            }
            var radioAccessTechnology = RadioAccessTechnology.Create(events,  option.MaxBasicBandwidthUnits);
            radioAccessTechnologies.Add(radioAccessTechnology.RadioAccessTechnologyId, radioAccessTechnology);
        }
        return radioAccessTechnologies;
    }

    private Dictionary<Guid, MobileTerminal> BuildMobileTerminals()
    {
        var mobileTerminals = new Dictionary<Guid, MobileTerminal>();
        for (var i = 0; i < _mobileTerminalCount; i++)
        {
            var mobileTerminal = MobileTerminal.Create(MobileTerminalStateMachine);
            mobileTerminals.Add(mobileTerminal.MobileTerminalId, mobileTerminal);
        }
        return mobileTerminals;
    }
}