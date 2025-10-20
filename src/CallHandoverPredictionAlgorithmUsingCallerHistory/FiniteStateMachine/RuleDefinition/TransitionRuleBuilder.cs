namespace FiniteStateMachine.RuleDefinition;

public sealed class TransitionRuleBuilder<TState, TTrigger> :
    ITransitionRuleBuilderWhenInState<TState, TTrigger>, 
    ITransitionRuleBuilderOnTrigger<TState, TTrigger>, 
    ITransitionRuleBuilderTransitionTo<TState, TTrigger>, 
    ITransitionRuleBuilder<TState, TTrigger> 
    where TState : Enum
    where TTrigger : Enum
{
    private TTrigger? _trigger;
    private TState? _inState;
    private TState? _transitionTo;

    public static ITransitionRuleBuilderWhenInState<TState, TTrigger> Builder()
    {
        return new TransitionRuleBuilder<TState, TTrigger>();
    }

    public TransitionRule<TState, TTrigger> AllowSelfTransition(TState fromState, TTrigger trigger)
    {
        return new TransitionRule<TState, TTrigger>
        {
            Trigger =  _trigger ?? throw new Exception("Trigger is null"),
            FromState = _inState ?? throw new Exception("In state is null"),
            TransitionToState = _transitionTo  ?? throw new Exception("In state is null"),
        };
    }

    public TransitionRule<TState, TTrigger> Build()
    {
        return new TransitionRule<TState, TTrigger>
        {
            Trigger = _trigger ?? throw new Exception("Trigger is null"),
            FromState = _inState ?? throw new Exception("In state is null"),
            TransitionToState = _transitionTo ?? throw new Exception("In state is null"),
        };
    }

    public ITransitionRuleBuilderOnTrigger<TState, TTrigger> WhenInState(TState state)
    {
        ArgumentNullException.ThrowIfNull(state);
        _inState = state;
        return this;
    }

    public ITransitionRuleBuilderTransitionTo<TState, TTrigger> OnTrigger(TTrigger trigger)
    {
        ArgumentNullException.ThrowIfNull(trigger);
        _trigger = trigger;
        return this;
    }

    public TransitionRule<TState, TTrigger> SelfTransitionOn(TTrigger trigger)
    {
        return new TransitionRule<TState, TTrigger>
        {
            Trigger =  trigger ?? throw new Exception("Trigger is null"),
            FromState = _inState ?? throw new Exception("In state is null"),
            TransitionToState = _inState  ?? throw new Exception("In state is null"),
        };
    }

    public ITransitionRuleBuilder<TState, TTrigger> TransitionTo(TState state)
    {
        ArgumentNullException.ThrowIfNull(state);
        _transitionTo = state;
        return this;
    }
}