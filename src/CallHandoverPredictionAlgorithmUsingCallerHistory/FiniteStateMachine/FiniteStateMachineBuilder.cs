using FiniteStateMachine.RuleDefinition;

namespace FiniteStateMachine;

public class FiniteStateMachineBuilder<TState, TTrigger>
    where TState : Enum
    where TTrigger : Enum
{
    private readonly TState _initialState;
    private readonly IList<TransitionRule<TState, TTrigger>> _rules;
    
    private FiniteStateMachineBuilder(TState initialState)
    {
        _initialState = initialState;
        _rules = [];
    }

    public static FiniteStateMachineBuilder<TState, TTrigger> Define(TState initialState)
    {
        ArgumentNullException.ThrowIfNull(initialState);
        return new FiniteStateMachineBuilder<TState, TTrigger>(initialState);
    }

    public FiniteStateMachine<TState, TTrigger> Build()
    {
        var ruleSet = TransitionRuleSet<TState, TTrigger>.EmptyRuleSet();
        foreach (var rule in _rules)
        {
            var ruleSetKey = new TransitionRuleSetKey<TState, TTrigger>(rule.FromState, rule.Trigger);
            ruleSet.AddRule(ruleSetKey, rule);
        }
        return FiniteStateMachine<TState, TTrigger>.Create(_initialState, ruleSet);
    }

    public FiniteStateMachineBuilder<TState, TTrigger> AddRule(TransitionRule<TState, TTrigger> rule)
    {
        ArgumentNullException.ThrowIfNull(rule);
        _rules.Add(rule);
        return this;
    }
}