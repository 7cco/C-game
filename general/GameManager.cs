namespace C__game;

public class GameManager: ICollisionChecker
{
    private Hero _hero;
    private Vector2 _previousHeroPosition;
    private ObstacleManager _obstacleManager;
    private List<Bot> _bots;
    private GameContext _context;
    public GameManager(GameContext context)
    {
        _context = context;
        _obstacleManager = new ObstacleManager();
        _bots = new List<Bot>();
        _hero = new Hero(context, this, _bots);
        Externalwalls(context);
        InnerWalls(context);
        CreateBots(context);
    }

    private void CreateBots(GameContext context)
    {
        // Создаем ботов разных типов в разных позициях
        _bots.Add(new Bot(context, new Vector2(500, 500), this, BotType.Pistol));    // Бот с пистолетом
        _bots.Add(new Bot(context, new Vector2(700, 320), this, BotType.Sniper));    // Снайпер
        _bots.Add(new Bot(context, new Vector2(300, 700), this, BotType.Assault));   // Штурмовик
        _bots.Add(new Bot(context, new Vector2(500, 500), this, BotType.Pistol));    // Бот с пистолетом
        _bots.Add(new Bot(context, new Vector2(700, 320), this, BotType.Sniper));    // Снайпер
        _bots.Add(new Bot(context, new Vector2(300, 700), this, BotType.Assault));   // Штурмовикы
        
        // Устанавливаем героя как цель для всех ботов
        foreach (var bot in _bots)
        {
            bot.Target = _hero;
        }
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

        // Обновляем ботов
        foreach (var bot in _bots)
        {
            bot.Update(context);
        }

        // Проверяем столкновения героя с препятствиями
        if (_obstacleManager.CheckCollision(_hero.Bounds))
        {
            _hero.SetPosition(_previousHeroPosition);
        }
    }

    public void Draw(GameContext context)
    {
        // Рисуем героя через контекст
        _hero.Draw(context);
        _obstacleManager.Draw(context);
        
        // Рисуем ботов
        foreach (var bot in _bots)
        {
            bot.Draw(context);
        }
    }
    public Vector2 HeroPosition => _hero.Position;

    public bool CheckCollision(Rectangle bounds) => 
        _obstacleManager.CheckCollision(bounds);

    public bool HasLineOfSight(Vector2 start, Vector2 end)
    {
        // Проверяем каждые 10 пикселей на пути от бота к игроку
        float step = 5f;
        Vector2 direction = end - start;
        float distance = direction.Length();
        direction.Normalize();

        for (float t = 0; t < distance; t += step)
        {
            Vector2 checkPoint = start + direction * t;
            Rectangle checkBounds = new Rectangle(
                (int)checkPoint.X,
                (int)checkPoint.Y,
                20,  // размер проверяемой области
                20
            );

            if (_obstacleManager.CheckCollision(checkBounds))
            {
                return false; // Есть препятствие на пути
            }
        }

        return true; // Прямая видимость есть
    }

      private void Externalwalls(GameContext context)
    {
        _obstacleManager.AddObstacle(context, "wall", new Vector2(17, 62), 3, 896);
        _obstacleManager.AddObstacle(context, "wall", new Vector2(1002, 62), 3, 896);
        _obstacleManager.AddObstacle(context, "wall", new Vector2(17, 62), 988, 3);
        _obstacleManager.AddObstacle(context, "wall", new Vector2(17, 955), 718, 3);
        _obstacleManager.AddObstacle(context, "wall", new Vector2(805, 955), 200, 3);
    }
    private void InnerWalls(GameContext context)
    {
        _obstacleManager.AddObstacle(context, "wall", new Vector2(288, 65), 4, 176);
        _obstacleManager.AddObstacle(context, "wall", new Vector2(288, 301), 4, 422);
        _obstacleManager.AddObstacle(context, "wall", new Vector2(288, 812), 4, 146);
        _obstacleManager.AddObstacle(context, "wall", new Vector2(20, 238), 11, 4);
        _obstacleManager.AddObstacle(context, "wall", new Vector2(117, 238), 170, 4);

        _obstacleManager.AddObstacle(context, "wall", new Vector2(292, 305), 34, 4);
        _obstacleManager.AddObstacle(context, "wall", new Vector2(408, 305), 363, 4);
        _obstacleManager.AddObstacle(context, "wall", new Vector2(838, 305), 163, 4);

        _obstacleManager.AddObstacle(context, "wall", new Vector2(452, 309), 4, 230);
        _obstacleManager.AddObstacle(context, "wall", new Vector2(292, 540), 211, 4);
        _obstacleManager.AddObstacle(context, "wall", new Vector2(593, 540), 408, 4);

    }

}