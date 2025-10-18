namespace FiniteStateMachine.RuleDefinition;

internal class TransitionRuleSet<TState, TTrigger>
    where TState : Enum
    where TTrigger : Enum
{
    private readonly Dictionary<TransitionRuleSetKey<TState, TTrigger>, TransitionRule<TState, TTrigger>> _ruleSet;

    private TransitionRuleSet()
    {
        _ruleSet = new Dictionary<TransitionRuleSetKey<TState, TTrigger>, TransitionRule<TState, TTrigger>>();
    }

    public static TransitionRuleSet<TState, TTrigger> EmptyRuleSet()
    {
        return new TransitionRuleSet<TState, TTrigger>();
    }

    public void AddRule(TransitionRuleSetKey<TState, TTrigger> ruleSetKey, TransitionRule<TState, TTrigger> rule)
    {
        _ruleSet.Add(ruleSetKey, rule);
    }

    public bool TryGetRule(TransitionRuleSetKey<TState, TTrigger> ruleSetKey, out TransitionRule<TState, TTrigger>? rule)
    {
        return _ruleSet.TryGetValue(ruleSetKey, out rule);
    }
    
    public TransitionRule<TState, TTrigger> GetRule(TransitionRuleSetKey<TState, TTrigger> ruleSetKey)
    {
        var exists = _ruleSet.TryGetValue(ruleSetKey, out var rule);
        return exists && rule is not null 
            ? rule 
            : throw new KeyNotFoundException($"RuleSetKey {ruleSetKey} does not exist");
    }
    
    public IReadOnlyDictionary<TransitionRuleSetKey<TState, TTrigger>, TransitionRule<TState, TTrigger>>  GetRuleSet() => _ruleSet.AsReadOnly();
}