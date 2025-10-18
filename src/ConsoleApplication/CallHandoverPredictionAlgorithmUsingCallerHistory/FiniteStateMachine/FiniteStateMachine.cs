
using FiniteStateMachine.RuleDefinition;

namespace FiniteStateMachine;

public class FiniteStateMachine<TState, TTrigger>
    where TState : Enum
    where TTrigger : Enum
{
    public TState CurrentState { get; private set; }
    private readonly TransitionRuleSet<TState, TTrigger> _ruleSet;

    private FiniteStateMachine(TState initialState,  TransitionRuleSet<TState, TTrigger> ruleSet)
    {
        CurrentState = initialState;
        _ruleSet = ruleSet;
    }

    internal static FiniteStateMachine<TState, TTrigger> Create(TState initialState, TransitionRuleSet<TState, TTrigger> ruleSet)
    {
        ArgumentNullException.ThrowIfNull(ruleSet);
        ArgumentNullException.ThrowIfNull(initialState);
        return new FiniteStateMachine<TState, TTrigger>(initialState, ruleSet);
    }

    public bool IsValidStateTransition(TTrigger trigger)
    {
        return _ruleSet.TryGetRule(new TransitionRuleSetKey<TState, TTrigger>(
            CurrentState,
            trigger), out _);
    }
    
    public void Trigger(TTrigger trigger)
    {
        ArgumentNullException.ThrowIfNull(trigger);
        var ruleSetKey = new TransitionRuleSetKey<TState, TTrigger>(
            CurrentState,
            trigger);
        var rule = _ruleSet.GetRule(ruleSetKey);
        TransitionToNextState(rule);
    }
    
    public IReadOnlyDictionary<TransitionRuleSetKey<TState, TTrigger>, TransitionRule<TState, TTrigger>>  GetRuleSet() => _ruleSet.GetRuleSet();


    private void TransitionToNextState(TransitionRule<TState, TTrigger> rule)
    {
        CurrentState = rule.TransitionToState;
    }
}