namespace FiniteStateMachine.RuleDefinition;

public sealed record TransitionRuleSetKey<TState,  TTrigger>(TState FromState, TTrigger Trigger);