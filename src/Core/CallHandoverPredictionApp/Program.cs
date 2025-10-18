// See https://aka.ms/new-console-template for more information

using CallHandoverPredictionAlgorithm.Models;
using FiniteStateMachine;
using FiniteStateMachine.RuleDefinition;

var mobileTerminalStateMachine = FiniteStateMachineBuilder<MobileTerminalState, MobileTerminalStateMachineTrigger>
    .Define(MobileTerminalState.Idle)
    .AddRule(new TransitionRule<MobileTerminalState, MobileTerminalStateMachineTrigger>
    {
        Trigger = MobileTerminalStateMachineTrigger.StartVoiceCall,
        FromState = MobileTerminalState.Idle,
        TransitionToState = MobileTerminalState.Voice
    })
    .AddRule(new TransitionRule<MobileTerminalState, MobileTerminalStateMachineTrigger>
    {
        Trigger = MobileTerminalStateMachineTrigger.StartVoiceCall,
        FromState = MobileTerminalState.Data,
        TransitionToState = MobileTerminalState.Data | MobileTerminalState.Voice
    })
    .AddRule(TransitionRuleBuilder<MobileTerminalState, MobileTerminalStateMachineTrigger>
        .Builder()
        .WhenInState(MobileTerminalState.Idle)
        .OnTrigger(MobileTerminalStateMachineTrigger.StartDataCall)
        .TransitionTo(MobileTerminalState.Data)
        .Build()
    )
    .Build();
            
            
Console.WriteLine(mobileTerminalStateMachine.CurrentState);