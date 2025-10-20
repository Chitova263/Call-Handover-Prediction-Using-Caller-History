namespace FiniteStateMachine.RuleDefinition;

public interface ITransitionRuleBuilderWhenInState<TState, TTrigger> where TState : Enum where TTrigger : Enum
{
    ITransitionRuleBuilderOnTrigger<TState, TTrigger> WhenInState(TState state);
    
}