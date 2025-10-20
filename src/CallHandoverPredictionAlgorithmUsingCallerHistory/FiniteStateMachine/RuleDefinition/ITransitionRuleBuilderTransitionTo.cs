namespace FiniteStateMachine.RuleDefinition;

public interface ITransitionRuleBuilderTransitionTo<TState, TTrigger> where TState : Enum where TTrigger : Enum
{
    ITransitionRuleBuilder<TState, TTrigger> TransitionTo(TState state);
}