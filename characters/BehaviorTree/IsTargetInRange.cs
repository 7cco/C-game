namespace C__game;

public class IsTargetInRange : BTNode
{
    private readonly float _range;
    private readonly ICollisionChecker _collisionChecker;

    public IsTargetInRange(ICollisionChecker collisionChecker, float range)
    {
        _collisionChecker = collisionChecker;
        _range = range;
    }

    public override BTStatus Execute(GameContext context, Bot bot)
    {
        if (bot.Target == null) return BTStatus.Failure;
        
        float distance = Vector2.Distance(bot.Position, bot.Target.Position);
        if (distance <= _range && _collisionChecker.HasLineOfSight(bot.Position, bot.Target.Position))
        {
            return BTStatus.Success;
        }
        return BTStatus.Failure;
    }
}