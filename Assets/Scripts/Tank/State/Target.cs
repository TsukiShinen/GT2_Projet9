public class Target : IState<Tank>
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

    public void Update(Tank Entity)
    {

    }

    public IState<Tank> Handle(Tank Entity)
    {
        if (Entity.NextState == "GoTo") { Entity.NextState = ""; return Entity.States.Goto; }

        return this;
    }
}