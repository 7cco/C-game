namespace C__game;

public class Patrol : BTNode
{
    private readonly float _patrolDistance;
    private readonly float _speed;
    private Vector2 _startPosition;
    private Vector2 _patrolDirection;
    private readonly Random _random;
    private float _timeSinceLastRandomChange;
    private readonly float _randomChangeInterval;
    private readonly float _randomChangeChance;
    private readonly ICollisionChecker _collisionChecker;

    public Patrol(
        ICollisionChecker collisionChecker,
        float patrolDistance,
        float speed = 200f)
    {
        _collisionChecker = collisionChecker;
        _patrolDistance = patrolDistance;
        _speed = speed;
        _random = new Random();
        _timeSinceLastRandomChange = 0f;
        _randomChangeInterval = 2f;
        _randomChangeChance = 0.2f;
    }

    public override BTStatus Execute(GameContext context, Bot bot)
    {
        // Сбрасываем боевой режим при патрулировании
        bot.SetCombatMode(false);
        
        if (_startPosition == Vector2.Zero)
        {
            _startPosition = bot.Position;
            _patrolDirection = _random.Next(2) == 0 
                ? new Vector2(1, 0) 
                : new Vector2(0, 1);
        }

        _timeSinceLastRandomChange += (float)context.TotalSeconds;

        if (_timeSinceLastRandomChange >= _randomChangeInterval)
        {
            _timeSinceLastRandomChange = 0f;
            if (_random.NextDouble() < _randomChangeChance)
            {
                _patrolDirection = _random.Next(2) == 0 
                    ? new Vector2(1, 0) 
                    : new Vector2(0, 1);
                if (_random.Next(2) == 0)
                {
                    _patrolDirection = -_patrolDirection;
                }
            }
        }

        Vector2 previousPosition = bot.Position;
        bot.SetDirection(_patrolDirection);
        bot.Position += _patrolDirection * _speed * (float)context.TotalSeconds;

        Rectangle testBounds = new Rectangle(
            (int)bot.Position.X,
            (int)bot.Position.Y,
            bot.Bounds.Width,
            bot.Bounds.Height
        );

        if (_collisionChecker.CheckCollision(testBounds))
        {
            bot.Position = previousPosition;
            
            Vector2[] possibleDirections = new Vector2[]
            {
                new Vector2(1, 0),
                new Vector2(-1, 0),
                new Vector2(0, 1),
                new Vector2(0, -1)
            };

            ShuffleDirections(possibleDirections);

            foreach (Vector2 direction in possibleDirections)
            {
                bot.Position = previousPosition + direction * _speed * (float)context.TotalSeconds;
                testBounds = new Rectangle(
                    (int)bot.Position.X,
                    (int)bot.Position.Y,
                    bot.Bounds.Width,
                    bot.Bounds.Height
                );

                if (!_collisionChecker.CheckCollision(testBounds))
                {
                    _patrolDirection = direction;
                    return BTStatus.Running;
                }
                bot.Position = previousPosition;
            }
        }

        return BTStatus.Running;
    }

    private void ShuffleDirections(Vector2[] directions)
    {
        for (int i = 0; i < directions.Length; i++)
        {
            int randomIndex = _random.Next(i, directions.Length);
            Vector2 temp = directions[i];
            directions[i] = directions[randomIndex];
            directions[randomIndex] = temp;
        }
    }
}