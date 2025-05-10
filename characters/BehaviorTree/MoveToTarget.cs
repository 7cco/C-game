namespace C__game;

public class MoveToTarget : BTNode
{
    private readonly ICollisionChecker _collisionChecker;
    private readonly float _arrivalDistance = 5f;
    private readonly float _speed = 200f;

    public MoveToTarget(ICollisionChecker collisionChecker)
    {
        _collisionChecker = collisionChecker;
    }

    public override BTStatus Execute(GameContext context, Bot bot)
    {
        if (bot.Target == null) return BTStatus.Failure;
        
        Vector2 direction = bot.Target.Position - bot.Position;
        float distance = direction.Length();
        
        if (distance <= _arrivalDistance)
        {
            bot.SetDirection(Vector2.Zero);
            return BTStatus.Success;
        }

        direction = Vector2.Normalize(direction);
        Vector2[] possibleDirections = new Vector2[]
        {
            direction,
            new Vector2(direction.Y, -direction.X),
            new Vector2(-direction.Y, direction.X),
            new Vector2(direction.X, 0),
            new Vector2(0, direction.Y)
        };

        foreach (Vector2 testDirection in possibleDirections)
        {
            Vector2 testPosition = bot.Position + testDirection * _speed * (float)context.TotalSeconds;
            Rectangle testBounds = new Rectangle(
                (int)testPosition.X,
                (int)testPosition.Y,
                bot.Bounds.Width,
                bot.Bounds.Height
            );

            if (!_collisionChecker.CheckCollision(testBounds))
            {
                bot.SetDirection(testDirection);
                bot.Position = testPosition;
                return BTStatus.Running;
            }
        }

        bot.SetDirection(Vector2.Zero);
        return BTStatus.Failure;
    }
}