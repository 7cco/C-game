namespace C__game;

public class Selector : BTNode
{
    private readonly BTNode[] _children;

    public Selector(params BTNode[] children)
    {
        _children = children;
    }

    public override BTStatus Execute(GameContext context, Bot bot)
    {
        foreach (var child in _children)
        {
            var status = child.Execute(context, bot);
            if (status != BTStatus.Failure)
                return status;
        }
        return BTStatus.Failure;
    }
}