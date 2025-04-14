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
        AddExampleObstacles(context);
    }

     private void AddExampleObstacles(GameContext context)
    {
        // Добавляем несколько примеров препятствий
        _obstacleManager.AddObstacle(context, "Bricks_01-256x256", new Vector2(300, 400), 50, 50);
        _obstacleManager.AddObstacle(context, "Bricks_01-256x256", new Vector2(600, 200), 50, 50);
        _obstacleManager.AddObstacle(context, "Bricks_01-256x256", new Vector2(800, 600), 50, 50);
    }

    public void Init()
    {
        // Дополнительная инициализация, если требуется
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