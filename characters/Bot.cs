namespace C__game;

public class Bot: HealthEntity
{
    private Texture2D _texture;
    private Vector2 _position;
    private Vector2 _direction;
    private Vector2 _previousPosition;
    private BTNode _behaviorTree;
    protected readonly Shoot _shoot;
    protected float _shootCooldown;
    protected const float PATROL_SPEED = 200f;
    protected const float COMBAT_SPEED = 100f;
    protected float _currentSpeed = PATROL_SPEED;
    protected const float BOT_MAX_HEALTH = 50f;
    protected const float BOT_DAMAGE = 10f;
    protected float _damageFlashTime = 0f;
    protected const float DAMAGE_FLASH_DURATION = 0.2f;
    protected IRangedWeapon _currentWeapon;

    public Hero Target { get; set; }
    public Vector2 Position { get => _position; set => _position = value; }
    public Vector2 Direction => _direction;
    protected readonly ICollisionChecker _collisionChecker;
    protected readonly GameContext _context;

    public Bot(GameContext context, Vector2 startPosition, ICollisionChecker collisionChecker, BotType botType = BotType.Pistol)
        : base(BOT_MAX_HEALTH)
    {   
        _context = context;
        _collisionChecker = collisionChecker;
        _position = startPosition;
        _previousPosition = startPosition;
        _direction = Vector2.Zero;
        _texture = context.Content.Load<Texture2D>("final gg-2x");

        // Инициализируем оружие в зависимости от типа бота
        _currentWeapon = botType switch
        {
            BotType.Pistol => new Pistol(context.Content.Load<Texture2D>("pistol")),
            BotType.Sniper => new SniperRifle(context.Content.Load<Texture2D>("sniper")),
            BotType.Assault => new AssaultRifle(context.Content.Load<Texture2D>("rifl")),
            _ => new Pistol(context.Content.Load<Texture2D>("pistol"))
        };

        _shoot = new Shoot(_position, collisionChecker, context, new List<Bot> { this }, _currentWeapon.Damage, false);
        _shoot.SetWeapon(_currentWeapon);
        _shoot.UpdateWeaponStats(_currentWeapon.Damage, _currentWeapon.FireRate, _currentWeapon.Range, _currentWeapon.BulletSpeed);
        
        Animations = new AnimationManager();

        Animations.AddAnimation(new Vector2(0, 1), new Animation(_texture, 6, 4, 0.1f, 4));   
        Animations.AddAnimation(new Vector2(-1, 0), new Animation(_texture, 6, 4, 0.1f, 2)); 
        Animations.AddAnimation(new Vector2(1, 0), new Animation(_texture, 6, 4, 0.1f, 1));  
        Animations.AddAnimation(new Vector2(0, -1), new Animation(_texture, 6, 4, 0.1f, 3)); 
        Animations.AddAnimation(new Vector2(-1, 1), new Animation(_texture, 6, 4, 0.1f, 2)); 
        Animations.AddAnimation(new Vector2(-1, -1), new Animation(_texture, 6, 4, 0.1f, 2)); 
        Animations.AddAnimation(new Vector2(1, 1), new Animation(_texture, 6, 4, 0.1f, 1));  
        Animations.AddAnimation(new Vector2(1, -1), new Animation(_texture, 6, 4, 0.1f, 1)); 

        // Настраиваем поведение в зависимости от типа бота
        float combatRange = botType switch
        {
            BotType.Sniper => 600f,  
            BotType.Assault => 300f,  
            _ => 200f                 
        };

        float minDistance = botType switch
        {
            BotType.Sniper => 500f,   
            BotType.Assault => 150f, 
            _ => 100f                 
        };

        _behaviorTree = new Selector(
            new Sequence(
                new IsTargetInRange(_collisionChecker, combatRange),
                new CombatBehavior(_collisionChecker, combatRange, minDistance)
            ),
            new Patrol(_collisionChecker, 200f)
        );
    }

    public void SetDirection(Vector2 direction)
    {
        if (direction != Vector2.Zero)
        {
            _direction = Vector2.Normalize(direction);
        }
        else
        {
            _direction = direction;
        }
    }

    public void Update(GameContext context)
    {
        if (_isDead) return;

        // Обновляем эффект получения урона
        if (_damageFlashTime > 0)
        {
            _damageFlashTime -= context.TotalSeconds;
        }

        _previousPosition = _position;
        
        _behaviorTree.Execute(context, this);

        if (_position != _previousPosition)
        {
            Vector2 movementDirection = Vector2.Normalize(_position - _previousPosition);
            SetDirection(movementDirection);
        }

        if (_shootCooldown > 0)
        {
            _shootCooldown -= context.TotalSeconds;
        }

        Animations.Update(context, _direction);

       
        _shoot.UpdateBullets(context);

        // Обновляем текущее оружие
        _currentWeapon.Position = _position;
        _currentWeapon.Update(context);
    }

    public virtual void Draw(GameContext context)
    {
        // Рисуем текущую анимацию в позиции бота с эффектом получения урона
        Color tint = _isDead ? Color.Red : (_damageFlashTime > 0 ? Color.Red : Color.White);
        Animations.Draw(context, _position, tint);
        _shoot.Draw(context);
        DrawHealthBar(context);

        // Рисуем текущее оружие
        _currentWeapon.Draw(context);
    }

    private void DrawHealthBar(GameContext context)
    {
        const int BAR_WIDTH = 50;
        const int BAR_HEIGHT = 5;
        const int BAR_OFFSET_Y = -10;

        Rectangle backgroundRect = new Rectangle(
            (int)_position.X,
            (int)_position.Y + BAR_OFFSET_Y,
            BAR_WIDTH,
            BAR_HEIGHT
        );
        context.SpriteBatch.Draw(
            context.Content.Load<Texture2D>("white"),
            backgroundRect,
            Color.Red
        );

        Rectangle healthRect = new Rectangle(
            (int)_position.X,
            (int)_position.Y + BAR_OFFSET_Y,
            (int)(BAR_WIDTH * HealthPercentage),
            BAR_HEIGHT
        );
        context.SpriteBatch.Draw(
            context.Content.Load<Texture2D>("white"),
            healthRect,
            Color.Green
        );
    }

    public Rectangle Bounds => new Rectangle(
        (int)_position.X,
        (int)_position.Y,
        56,
        50
    );

    public void RevertPosition()
    {
        _position = _previousPosition;
    }

    public virtual void SetCombatMode(bool isCombat)
    {
        _currentSpeed = isCombat ? COMBAT_SPEED : PATROL_SPEED;
    }

    public float CurrentSpeed => _currentSpeed;

    public virtual void TryShoot()
    {
        UpdateShooting();
    }

    private void UpdateShooting()
    {
        if (_shootCooldown <= 0 && Target != null && !_isDead)
        {
            
            if (_currentWeapon.CurrentAmmo <= 0)
            {
                _currentWeapon.StartReload();
                return;
            }

            Vector2 shootDirection = Target.Position - _position;
            if (shootDirection != Vector2.Zero)
            {
                shootDirection = Vector2.Normalize(shootDirection);
                _shoot.FireAtDirection(_context, shootDirection, _position);
                _shootCooldown = 1.0f / _currentWeapon.FireRate;
            }
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        _damageFlashTime = DAMAGE_FLASH_DURATION;
    }

    protected override void OnDeath()
    {
        // Останавливаем движение и стрельбу при смерти
        _direction = Vector2.Zero;
        
        // Убедимся, что все пули обновляются даже после смерти
        if (_shoot != null && _context != null)
        {
            // Выполним финальное обновление пуль и очистим оставшиеся
            _shoot.UpdateBullets(_context);
            _shoot.ClearBullets();
        }
    }
}