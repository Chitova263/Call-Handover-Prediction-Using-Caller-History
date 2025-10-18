namespace FiniteStateMachine.RuleDefinition;

internal sealed record TransitionRuleSetKey<TState,  TTrigger>(TState FromState, TTrigger Trigger);