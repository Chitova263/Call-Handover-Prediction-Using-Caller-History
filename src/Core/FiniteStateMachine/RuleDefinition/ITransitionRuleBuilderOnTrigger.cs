namespace FiniteStateMachine.RuleDefinition;
public interface ITransitionRuleBuilderOnTrigger<TState, TTrigger> where TState : Enum where TTrigger : Enum
{
    ITransitionRuleBuilderTransitionTo<TState, TTrigger> OnTrigger(TTrigger trigger);
}