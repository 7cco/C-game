namespace C__game;
public interface ICollisionChecker
{
    bool CheckCollision(Rectangle bounds);
    bool HasLineOfSight(Vector2 start, Vector2 end);
}