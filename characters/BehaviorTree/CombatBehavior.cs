namespace C__game;

public class CombatBehavior : BTNode
{
    private readonly ICollisionChecker _collisionChecker;
    private readonly float _arrivalDistance; // Дистанция, на которой бот будет стрелять
    private readonly float _minDistance; // Минимальная дистанция для сближения
    private readonly Vector2[] _retreatDirections; // Направления отступления

    public CombatBehavior(ICollisionChecker collisionChecker, float arrivalDistance = 150f, float minDistance = 100f)
    {
        _collisionChecker = collisionChecker;
        _arrivalDistance = arrivalDistance;
        _minDistance = minDistance;
        

        _retreatDirections = new Vector2[]
        {
            new Vector2(-1, 0),  
            new Vector2(1, 0),   
            new Vector2(0, -1),  
            new Vector2(0, 1),   
            new Vector2(-1, -1), 
            new Vector2(1, -1), 
            new Vector2(-1, 1),  
            new Vector2(1, 1)   
        };
    }

    public override BTStatus Execute(GameContext context, Bot bot)
    {
        if (bot.Target == null) return BTStatus.Failure;
        
        Vector2 direction = bot.Target.Position - bot.Position;
        float distance = direction.Length();
        
        // Устанавливаем боевой режим
        bot.SetCombatMode(true);

        // Если мы слишком близко, пробуем отступить в разных направлениях
        if (distance < _minDistance)
        {
            Vector2 retreatDirection = -Vector2.Normalize(direction);
            bool canRetreat = false;

            // Сначала пробуем отступить в основном направлении
            Vector2 testPosition = bot.Position + retreatDirection * bot.CurrentSpeed * (float)context.TotalSeconds;
            Rectangle testBounds = new Rectangle(
                (int)testPosition.X,
                (int)testPosition.Y,
                bot.Bounds.Width,
                bot.Bounds.Height
            );

            if (!_collisionChecker.CheckCollision(testBounds))
            {
                bot.SetDirection(retreatDirection);
                bot.Position = testPosition;
                canRetreat = true;
            }
            else
            {
                // Если основное направление заблокировано, пробуем другие направления
                foreach (var dir in _retreatDirections)
                {
                    testPosition = bot.Position + dir * bot.CurrentSpeed * (float)context.TotalSeconds;
                    testBounds = new Rectangle(
                        (int)testPosition.X,
                        (int)testPosition.Y,
                        bot.Bounds.Width,
                        bot.Bounds.Height
                    );

                    if (!_collisionChecker.CheckCollision(testBounds))
                    {
                        bot.SetDirection(dir);
                        bot.Position = testPosition;
                        canRetreat = true;
                        break;
                    }
                }
            }

            // Если не удалось отступить ни в одном направлении, пробуем стрелять
            if (!canRetreat)
            {
                bot.SetDirection(Vector2.Zero);
                bot.TryShoot();
            }
        }
        // Если мы на оптимальной дистанции, стреляем
        else if (distance <= _arrivalDistance)
        {
            bot.SetDirection(Vector2.Zero);
            bot.TryShoot();
        }
        // Иначе сближаемся
        else
        {
            direction = Vector2.Normalize(direction);
            Vector2 testPosition = bot.Position + direction * bot.CurrentSpeed * (float)context.TotalSeconds;
            Rectangle testBounds = new Rectangle(
                (int)testPosition.X,
                (int)testPosition.Y,
                bot.Bounds.Width,
                bot.Bounds.Height
            );

            if (!_collisionChecker.CheckCollision(testBounds))
            {
                bot.SetDirection(direction);
                bot.Position = testPosition;
            }
        }

        return BTStatus.Running;
    }
} 