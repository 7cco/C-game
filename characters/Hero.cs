namespace C__game;

using System.Collections.Generic;

public class Hero: HealthEntity
{
    private Texture2D _texture;
    private Vector2 _position = new(100, 100);
    private readonly float _speed = 200f;
    private readonly Shoot _shoot;
    private const float HERO_MAX_HEALTH = 100f;
    private const float HERO_DAMAGE = 20f;
    private float _damageFlashTime = 0f;
    private const float DAMAGE_FLASH_DURATION = 0.2f;
    private IRangedWeapon _currentWeapon;
    private readonly Dictionary<Type, IRangedWeapon> _weapons;

    public Vector2 Position => _position;

    public Hero(GameContext context, ICollisionChecker collisionChecker, List<Bot> bots)
        : base(HERO_MAX_HEALTH)
    {
        // Загружаем текстуру через контекст
        _texture = context.Content.Load<Texture2D>("final gg-2x");
        _shoot = new Shoot(_position, collisionChecker, context, bots);
        // Инициализируем менеджер анимаций
        Animations = new AnimationManager();

        // Инициализируем оружие
        _weapons = new Dictionary<Type, IRangedWeapon>
        {
            { typeof(Pistol), new Pistol(context.Content.Load<Texture2D>("pistol")) },
            { typeof(SniperRifle), new SniperRifle(context.Content.Load<Texture2D>("sniper")) },
            { typeof(AssaultRifle), new AssaultRifle(context.Content.Load<Texture2D>("rifl")) }
        };

        // Устанавливаем пистолет как начальное оружие
        _currentWeapon = _weapons[typeof(Pistol)];
        _shoot.SetWeapon(_currentWeapon);
        // Обновляем характеристики стрельбы для начального оружия
        _shoot.UpdateWeaponStats(_currentWeapon.Damage, _currentWeapon.FireRate, _currentWeapon.Range, _currentWeapon.BulletSpeed);

        // Добавляем анимации для всех направлений движения
        Animations.AddAnimation(new Vector2(0, 1), new Animation(_texture, 6, 4, 0.1f, 4));   // Вниз
        Animations.AddAnimation(new Vector2(-1, 0), new Animation(_texture, 6, 4, 0.1f, 2)); // Влево
        Animations.AddAnimation(new Vector2(1, 0), new Animation(_texture, 6, 4, 0.1f, 1));  // Вправо
        Animations.AddAnimation(new Vector2(0, -1), new Animation(_texture, 6, 4, 0.1f, 3)); // Вверх
        Animations.AddAnimation(new Vector2(-1, 1), new Animation(_texture, 6, 4, 0.1f, 2)); // Влево-вниз
        Animations.AddAnimation(new Vector2(-1, -1), new Animation(_texture, 6, 4, 0.1f, 2)); // Влево-вверх
        Animations.AddAnimation(new Vector2(1, 1), new Animation(_texture, 6, 4, 0.1f, 1));  // Вправо-вниз
        Animations.AddAnimation(new Vector2(1, -1), new Animation(_texture, 6, 4, 0.1f, 1)); // Вправо-вверх
    }

    public Rectangle Bounds => new Rectangle(
        (int)_position.X,
        (int)_position.Y,
        56,
        50
    );

    public void SetPosition(Vector2 newPosition)
    {
        _position = newPosition;
    }
    
    public void Update(GameContext context)
    {
        if (_isDead) return;

        // Обновляем эффект получения урона
        if (_damageFlashTime > 0)
        {
            _damageFlashTime -= context.TotalSeconds;
        }

        // Обновляем позицию героя, если он движется
        if (InputManager.Moving)
        {
            _position += Vector2.Normalize(InputManager.Direction) * _speed * context.TotalSeconds;
        }

        // Проверяем смену оружия
        if (InputManager.IsPistolSelected())
        {
            SwitchWeapon<Pistol>();
        }
        else if (InputManager.IsSniperRifleSelected())
        {
            SwitchWeapon<SniperRifle>();
        }
        else if (InputManager.IsAssaultRifleSelected())
        {
            SwitchWeapon<AssaultRifle>();
        }

        // Проверяем перезарядку
        if (InputManager.IsReloading())
        {
            _currentWeapon.StartReload();
        }

        // Обновляем текущую анимацию
        Animations.Update(context, InputManager.Direction);

        // Обновляем логику стрельбы
        _shoot.Update(context, _position, InputManager.Direction);

        // Обновляем текущее оружие
        _currentWeapon.Position = _position;
        _currentWeapon.Update(context);
    }

    public void Draw(GameContext context)
    {
        // Рисуем текущую анимацию в позиции героя с эффектом получения урона
        Color tint = _damageFlashTime > 0 ? Color.Red : Color.White;
        Animations.Draw(context, _position, tint);

        // Рисуем текущее оружие
        _currentWeapon.Draw(context);

        // Рисуем пули
        _shoot.Draw(context);
        DrawHealthBar(context);
    }

    private void DrawHealthBar(GameContext context)
    {
        const int BAR_WIDTH = 50;
        const int BAR_HEIGHT = 5;
        const int BAR_OFFSET_Y = -10;

        // Фон полоски здоровья (красный)
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

        // Текущее здоровье (зеленый)
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

        // Отображаем количество патронов
        string ammoText = $"{_currentWeapon.CurrentAmmo}/{_currentWeapon.MaxAmmo}";
        Vector2 ammoPosition = new Vector2(_position.X, _position.Y + BAR_OFFSET_Y - 15);
        context.SpriteBatch.DrawString(
            context.Content.Load<SpriteFont>("font"),
            ammoText,
            ammoPosition,
            _currentWeapon.IsReloading ? Color.Yellow : Color.White
        );
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        _damageFlashTime = DAMAGE_FLASH_DURATION;
    }

    protected override void OnDeath()
    {
        // Останавливаем движение и стрельбу при смерти
        _position = _position; // Сохраняем последнюю позицию
    }

    public void SwitchWeapon<T>() where T : IRangedWeapon
    {
        if (_weapons.TryGetValue(typeof(T), out var weapon))
        {
            _currentWeapon = weapon;
            _shoot.SetWeapon(_currentWeapon);
            _shoot.UpdateWeaponStats(_currentWeapon.Damage, _currentWeapon.FireRate, _currentWeapon.Range, _currentWeapon.BulletSpeed);
        }
    }
}