namespace C__game;

using System.Linq;

public class GameManager: ICollisionChecker
{
    private Hero _hero;
    private Vector2 _previousHeroPosition;
    private ObstacleManager _obstacleManager;
    private List<Bot> _bots;
    private BossManager _bossManager;
    private GameContext _context;
    private int _currentLevel = 1;

    public GameManager(GameContext context)
    {
        _context = context;
        _obstacleManager = new ObstacleManager();
        _bots = new List<Bot>();
        _hero = new Hero(context, this, _bots);

        _bossManager = new BossManager();

        Externalwalls(context);
        InnerWalls(context);
        CreateBots(context);
        CreateBosses(context);
    }
    
    private void CreateBosses(GameContext context)
    {
        _bossManager.ClearBosses();
        
        if (_currentLevel == 2)
        {
            CreateBossesLevel2();
        }
        else
        {
            CreateBossesLevel1();
        }
        
        // Добавляем боссов к общему списку ботов для обнаружения столкновений пуль
        var bosses = _bossManager.GetBossesAsBots();
        _bots.AddRange(bosses);
    }

    private void CreateBossesLevel1()
    {
        var heavyGunner = new Boss(_context, new Vector2(800, 800), this, BossType.Assault);
        var sniper = new Boss(_context, new Vector2(200, 800), this, BossType.Sniper);
        var dualPistols = new Boss(_context, new Vector2(500, 200), this, BossType.Pistol);
        
        heavyGunner.Target = _hero;
        sniper.Target = _hero;
        dualPistols.Target = _hero;
        
        _bossManager.AddBoss(heavyGunner);
        _bossManager.AddBoss(sniper);
        _bossManager.AddBoss(dualPistols);
    }

    private void CreateBossesLevel2()
    {
        var heavyGunner = new Boss(_context, new Vector2(400, 400), this, BossType.Assault);
        
        heavyGunner.Target = _hero;
        
        _bossManager.AddBoss(heavyGunner);
    }

    private void CreateBots(GameContext context)
    {
        _bots.Clear();
        
        if (_currentLevel == 2)
        {
            CreateBotsLevel2(context);
        }
        else
        {
            CreateBotsLevel1(context);
        }
        
        foreach (var bot in _bots)
        {
            bot.Target = _hero;
        }
    }

    private void CreateBotsLevel1(GameContext context)
    {

        _bots.Add(new Bot(context, new Vector2(60, 300), this, BotType.Sniper));    
        _bots.Add(new Bot(context, new Vector2(60, 600), this, BotType.Sniper));    
        _bots.Add(new Bot(context, new Vector2(400, 600), this, BotType.Assault));   
        _bots.Add(new Bot(context, new Vector2(400, 800), this, BotType.Assault));    
        _bots.Add(new Bot(context, new Vector2(500, 350), this, BotType.Pistol));    
        _bots.Add(new Bot(context, new Vector2(900, 400), this, BotType.Pistol));   
    }

    private void CreateBotsLevel2(GameContext context)
    {

        _bots.Add(new Bot(context, new Vector2(200, 200), this, BotType.Sniper));
        _bots.Add(new Bot(context, new Vector2(50, 800), this, BotType.Pistol));
        _bots.Add(new Bot(context, new Vector2(50, 900), this, BotType.Pistol));
       
    }

    public void Init()
    {
        
    }

    public void Update(GameContext context)
    {
        _previousHeroPosition = _hero.Position;

        InputManager.Update();

        _hero.Update(context);

        foreach (var bot in _bots.Where(b => !(b is Boss)))
        {
            bot.Update(context);
        }
        
        _bossManager.Update(context);

        if (_obstacleManager.CheckCollision(_hero.Bounds))
        {
            _hero.SetPosition(_previousHeroPosition);
        }
    }

    public void Draw(GameContext context)
    {
        _hero.Draw(context);
        _obstacleManager.Draw(context);
        
        foreach (var bot in _bots.Where(b => !(b is Boss)))
        {
            bot.Draw(context);
        }
        
        _bossManager.Draw(context);
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
        _obstacleManager.AddObstacle(context, "wall", new Vector2(17, 62), 6, 896);
        _obstacleManager.AddObstacle(context, "wall", new Vector2(1002, 62), 6, 896);
        _obstacleManager.AddObstacle(context, "wall", new Vector2(17, 62), 988, 6);
        _obstacleManager.AddObstacle(context, "wall", new Vector2(17, 955), 718, 6);
        _obstacleManager.AddObstacle(context, "wall", new Vector2(805, 955), 200, 6);
    }
    private void InnerWalls(GameContext context)
    {
        _obstacleManager.AddObstacle(context, "wall", new Vector2(288, 65), 8, 176);
        _obstacleManager.AddObstacle(context, "wall", new Vector2(288, 301), 8, 422);
        _obstacleManager.AddObstacle(context, "wall", new Vector2(288, 812), 8, 146);
        _obstacleManager.AddObstacle(context, "wall", new Vector2(20, 238), 11, 8);
        _obstacleManager.AddObstacle(context, "wall", new Vector2(117, 238), 170, 8);

        _obstacleManager.AddObstacle(context, "wall", new Vector2(292, 305), 34, 8);
        _obstacleManager.AddObstacle(context, "wall", new Vector2(408, 305), 363, 8);
        _obstacleManager.AddObstacle(context, "wall", new Vector2(838, 305), 163, 8);

        _obstacleManager.AddObstacle(context, "wall", new Vector2(452, 309), 8, 230);
        _obstacleManager.AddObstacle(context, "wall", new Vector2(292, 540), 211, 8);
        _obstacleManager.AddObstacle(context, "wall", new Vector2(593, 540), 408, 8);

    }

    private void Walls2(GameContext context)
    {
        _obstacleManager.AddInvisibleObstacle(context, new Vector2(201, 279), 376, 422);
        _obstacleManager.AddInvisibleObstacle(context, new Vector2(679, 289), 354, 432);
        _obstacleManager.AddInvisibleObstacle(context, new Vector2(689, 0), 354, 187);
        _obstacleManager.AddInvisibleObstacle(context, new Vector2(0, 279), 102, 422);
        _obstacleManager.AddInvisibleObstacle(context, new Vector2(201, 0), 347, 187);
        _obstacleManager.AddInvisibleObstacle(context, new Vector2(0, 0), 103, 187);

        _obstacleManager.AddInvisibleObstacle(context, new Vector2(220, 800), 81, 29);
        _obstacleManager.AddInvisibleObstacle(context, new Vector2(434, 801), 81, 29);
        _obstacleManager.AddInvisibleObstacle(context, new Vector2(789, 800), 81, 29);

        _obstacleManager.AddInvisibleObstacle(context, new Vector2(-1, -1), 1025, 0);
        _obstacleManager.AddInvisibleObstacle(context, new Vector2(-1, -1), 0, 1025);
        _obstacleManager.AddInvisibleObstacle(context, new Vector2(1025, 0), 0, 1025);
        _obstacleManager.AddInvisibleObstacle(context, new Vector2(0, 1025), 1025, 0);
    }

    public void RecreateLevels(GameContext context, int level)
    {
        _currentLevel = level;

        _obstacleManager.ClearObstacles();
        

        if (level == 2)
        {
            Walls2(context);
        }
        else
        {
            Externalwalls(context);
            InnerWalls(context);
        }

        CreateBots(context);
        
        CreateBosses(context);
    }

}