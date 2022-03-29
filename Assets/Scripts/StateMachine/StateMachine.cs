using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T> : MonoBehaviour where T : StateMachine<T>
{
    protected IState<T> _currentState;

    private T Entity => (T)this;

    public virtual void Update()
    {
        if (_currentState == null) { return; }
        ChangeState(_currentState.Handle(Entity));
        _currentState.Update(Entity);
    }

    public virtual void FixedUpdate()
    {
        if (_currentState == null) { return; }
        _currentState.FixedUpdate(Entity);
    }

    private void ChangeState(IState<T> newState)
    {
        if (_currentState == newState) { return; }
        if (_currentState != null)
        {
            _currentState.Exit(Entity);
        }
        _currentState = newState;
        _currentState.Enter(Entity);
    }
}
