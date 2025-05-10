namespace C__game;

public class Sequence : BTNode
{
    private readonly BTNode[] _children;

    public Sequence(params BTNode[] children)
    {
        _children = children;
    }

    public override BTStatus Execute(GameContext context, Bot bot)
    {
        foreach (var child in _children)
        {
            var status = child.Execute(context, bot);
            if (status != BTStatus.Success)
                return status;
        }
        return BTStatus.Success;
    }
}