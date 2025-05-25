namespace C__game;

public class ObstacleManager
{
    private readonly List<Obstacle> _obstacles = [];

    public void AddObstacle(GameContext context, string textureName, Vector2 position, int width, int height)
    {
        _obstacles.Add(new Obstacle(context, textureName, position, width, height));
    }

    public void AddInvisibleObstacle(GameContext context, Vector2 position, int width, int height)
    {
        _obstacles.Add(new Obstacle(context, "wall", position, width, height, false));
    }

    public void ClearObstacles()
    {
        _obstacles.Clear();
    }

    public void Draw(GameContext context)
    {
        foreach (var obstacle in _obstacles)
        {
            obstacle.Draw(context);
        }
    }

    public bool CheckCollision(Rectangle playerBounds)
    {
        // Проверяем коллизию игрока с препятствиями
        foreach (var obstacle in _obstacles)
        {
            if (playerBounds.Intersects(obstacle.Bounds))
            {
                return true; // Коллизия обнаружена
            }
        }
        return false; // Коллизии нет
    }
}