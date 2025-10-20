namespace FiniteStateMachine.RuleDefinition;

public interface ITransitionRuleBuilder<TState, TTrigger> where TState : Enum where TTrigger : Enum
{
    TransitionRule<TState, TTrigger> Build();
}