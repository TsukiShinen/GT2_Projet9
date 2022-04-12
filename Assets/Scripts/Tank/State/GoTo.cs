using UnityEngine;

public class Goto : IState<Tank>
{
    public void Enter(Tank Entity)
    {

    }

    public void Exit(Tank Entity)
    {

    }

    public void FixedUpdate(Tank Entity)
    {

    }

    public IState<Tank> Handle(Tank Entity)
    {
        if (Entity.NextState == "Target") { Entity.NextState = ""; return TankStates.Target; }
        if (Entity.Movement.ArrivedAtDestination) { Entity.NextState = ""; return TankStates.Idle; }
        if (Entity.NextState == "Idle") { Entity.NextState = ""; return TankStates.Idle; }
        return this;
    }

    public void Update(Tank Entity)
    {
        Entity.Movement.Move();
    }
}