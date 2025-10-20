namespace FiniteStateMachine.RuleDefinition;

public sealed record TransitionRule<TState, TTrigger> 
    where TState : Enum
    where TTrigger : Enum
{
    public required TTrigger Trigger { get; init; }
    public required TState FromState { get; init; }
    public required TState TransitionToState { get; init; }
}