namespace C__game;

public interface IBulletCollisionChecker
{
    bool CheckCollision(Rectangle bounds);
    bool CheckCollisionAlongPath(Vector2 start, Vector2 end, float stepSize);
    Vector2? GetCollisionPoint(Vector2 start, Vector2 end, float stepSize);
} 