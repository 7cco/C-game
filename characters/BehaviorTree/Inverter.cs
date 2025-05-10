namespace C__game;

public class Inverter : BTNode
{
    private readonly BTNode _child;

    public Inverter(BTNode child)
    {
        _child = child;
    }

    public override BTStatus Execute(GameContext context, Bot bot)
    {
        var status = _child.Execute(context, bot);
        return status switch
        {
            BTStatus.Success => BTStatus.Failure,
            BTStatus.Failure => BTStatus.Success,
            _ => BTStatus.Running
        };
    }
}