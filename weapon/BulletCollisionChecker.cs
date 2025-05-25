namespace C__game;

public class BulletCollisionChecker : IBulletCollisionChecker
{
    private readonly ICollisionChecker _baseCollisionChecker;
    private readonly float _maxStepSize;
    private readonly int _bulletSize;
    private const int DEFAULT_BULLET_SIZE = 4;
    private const float DEFAULT_MAX_STEP_SIZE = 10f;

    public BulletCollisionChecker(
        ICollisionChecker baseCollisionChecker, 
        float maxStepSize = DEFAULT_MAX_STEP_SIZE,
        int bulletSize = DEFAULT_BULLET_SIZE)
    {
        _baseCollisionChecker = baseCollisionChecker;
        _maxStepSize = maxStepSize;
        _bulletSize = bulletSize;
    }

    public bool CheckCollision(Rectangle bounds)
    {
        return _baseCollisionChecker.CheckCollision(bounds);
    }

    public bool CheckCollisionAlongPath(Vector2 start, Vector2 end, float stepSize)
    {
        return GetCollisionPoint(start, end, stepSize) != null;
    }

    public Vector2? GetCollisionPoint(Vector2 start, Vector2 end, float stepSize)
    {
        Vector2 direction = end - start;
        float distance = direction.Length();
        direction.Normalize();

        float actualStepSize = Math.Min(stepSize, _maxStepSize);
        int steps = (int)(distance / actualStepSize);
        int halfBulletSize = _bulletSize / 2;

        for (int i = 0; i <= steps; i++)
        {
            Vector2 currentPosition = start + direction * (actualStepSize * i);
            Rectangle bounds = new Rectangle(
                (int)currentPosition.X - halfBulletSize,
                (int)currentPosition.Y - halfBulletSize,
                _bulletSize,
                _bulletSize
            );

            if (_baseCollisionChecker.CheckCollision(bounds))
            {
                return currentPosition;
            }
        }

        return null;
    }
} 