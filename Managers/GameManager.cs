namespace C__game;

public class GameManager
{
    private Hero _hero;
    private Vector2 _previousHeroPosition;
    private ObstacleManager _obstacleManager;

    public GameManager(GameContext context)
    {
        // Инициализируем героя, передавая контекст
        _hero = new Hero(context);
        _obstacleManager = new ObstacleManager();
        Externalwalls(context);
        InnerWalls(context);
    }

    private void Externalwalls(GameContext context)
    {
        _obstacleManager.AddObstacle(context, "Bricks_01-256x256", new Vector2(17, 62), 3, 896);
        _obstacleManager.AddObstacle(context, "Bricks_01-256x256", new Vector2(1002, 62), 3, 896);
        _obstacleManager.AddObstacle(context, "Bricks_01-256x256", new Vector2(17, 62), 988, 3);
        _obstacleManager.AddObstacle(context, "Bricks_01-256x256", new Vector2(17, 955), 718, 3);
        _obstacleManager.AddObstacle(context, "Bricks_01-256x256", new Vector2(805, 955), 200, 3);
    }
    private void InnerWalls(GameContext context)
    {
        _obstacleManager.AddObstacle(context, "Bricks_01-256x256", new Vector2(288, 65), 4, 176);
        _obstacleManager.AddObstacle(context, "Bricks_01-256x256", new Vector2(288, 301), 4, 422);
        _obstacleManager.AddObstacle(context, "Bricks_01-256x256", new Vector2(288, 812), 4, 146);
        _obstacleManager.AddObstacle(context, "Bricks_01-256x256", new Vector2(20, 238), 11, 4);
        _obstacleManager.AddObstacle(context, "Bricks_01-256x256", new Vector2(117, 238), 170, 4);

        _obstacleManager.AddObstacle(context, "Bricks_01-256x256", new Vector2(292, 305), 34, 4);
        _obstacleManager.AddObstacle(context, "Bricks_01-256x256", new Vector2(408, 305), 363, 4);
        _obstacleManager.AddObstacle(context, "Bricks_01-256x256", new Vector2(838, 305), 163, 4);

        _obstacleManager.AddObstacle(context, "Bricks_01-256x256", new Vector2(452, 309), 4, 230);
        _obstacleManager.AddObstacle(context, "Bricks_01-256x256", new Vector2(292, 540), 211, 4);
        _obstacleManager.AddObstacle(context, "Bricks_01-256x256", new Vector2(593, 540), 408, 4);

    }

    public void Init()
    {
        
    }

    public void Update(GameContext context)
    {
        _previousHeroPosition = _hero.Position;
        // Обновляем InputManager
        InputManager.Update();
        // Обновляем логику героя
        _hero.Update(context);

        if (_obstacleManager.CheckCollision(_hero.Bounds))
        {
            // Реакция на столкновение (например, остановка движения)
            _hero.SetPosition(_previousHeroPosition);
        }

    }

    public void Draw(GameContext context)
    {
        // Рисуем героя через контекст
        _hero.Draw(context);
        _obstacleManager.Draw(context);
    }
    public Vector2 HeroPosition => _hero.Position;
}