namespace C__game;

public class Shoot
{   
    private IRangedWeapon _weapon;
    private List<Bullet> _bullets;
    private float _shootCooldown;
    private float _currentShootCooldownTime;
    private Vector2 _position;
    private readonly Vector2 _characterCenter = new(28, 25); // Половина размера модели (56x50)
    private readonly ICollisionChecker _collisionChecker;
    private readonly GameContext _context;
    private readonly List<Bot> _bots;
    private float _damage;
    private float _bulletSpeed;
    private float _bulletRange;
    private readonly bool _isPlayerShoot;

    public Shoot(Vector2 position, ICollisionChecker collisionChecker, GameContext context, List<Bot> bots, float damage = 10f, bool isPlayerShoot = true)
    {   
        _context = context;         
        _bullets = new List<Bullet>();
        _shootCooldown = 0f;
        _currentShootCooldownTime = 0.2f;
        _position = position;
        _collisionChecker = collisionChecker;
        _bots = bots;
        _damage = damage;
        _bulletSpeed = 800f;
        _bulletRange = 300f;
        _isPlayerShoot = isPlayerShoot;
    }

    public void SetWeapon(IRangedWeapon weapon)
    {
        _weapon = weapon;
    }

    public void UpdateWeaponStats(float damage, float fireRate, float range, float bulletSpeed)
    {
        _damage = damage;
        _currentShootCooldownTime = 1.0f / fireRate;
        _bulletRange = range;
        _bulletSpeed = bulletSpeed;
    }

    public void Update(GameContext context, Vector2 position, Vector2 direction)
    {
        _position = position;

        // Обновляем кулдаун стрельбы
        if (_shootCooldown > 0)
        {
            _shootCooldown -= context.TotalSeconds;
        }

        // Обработка стрельбы
        if (_isPlayerShoot && InputManager.IsShooting() && _shootCooldown <= 0)
        {
            if (_weapon != null && _weapon.CurrentAmmo > 0)
            {
                Fire(context);
                _shootCooldown = _currentShootCooldownTime;
            }
            else if (_weapon != null && !_weapon.IsReloading)
            {
                _weapon.StartReload();
            }
        }

        // Обновляем пули
        for (int i = _bullets.Count - 1; i >= 0; i--)
        {
            _bullets[i].Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(context.TotalSeconds)));
            if (!_bullets[i].IsActive)
            {
                _bullets.RemoveAt(i);
            }
        }
    }

    public void UpdateBullets(GameContext context)
    {
        // Обновляем кулдаун стрельбы
        if (_shootCooldown > 0)
        {
            _shootCooldown -= context.TotalSeconds;
        }

        // Обновляем пули
        for (int i = _bullets.Count - 1; i >= 0; i--)
        {
            _bullets[i].Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(context.TotalSeconds)));
            if (!_bullets[i].IsActive)
            {
                _bullets.RemoveAt(i);
            }
        }
    }

    public void Fire(GameContext context)
    {
        if (_weapon == null || _weapon.CurrentAmmo <= 0) return;
        
        MouseState mouseState = Mouse.GetState();
        Vector2 mousePosition = new(mouseState.X, mouseState.Y);
        
        // Получаем позицию центра персонажа
        Vector2 shootPosition = _position + _characterCenter;
        
        // Получаем смещение камеры
        Vector2 cameraOffset = CameraManager.Instance.Position;
        
        // Преобразуем позицию мыши с учетом смещения камеры
        Vector2 worldMousePosition = mousePosition + cameraOffset;
        
        // Вычисляем направление от центра персонажа к позиции мыши в мировых координатах
        Vector2 shootDirection = worldMousePosition - shootPosition;
        
        // Нормализуем вектор направления
        if (shootDirection != Vector2.Zero)
        {
            shootDirection = Vector2.Normalize(shootDirection);
        }
        else
        {
            // Если курсор находится в центре персонажа, стреляем вправо
            shootDirection = new Vector2(1, 0);
        }

        _bullets.Add(new Bullet(context, shootPosition, shootDirection, _collisionChecker, _bots, _damage, _isPlayerShoot, _bulletSpeed, _bulletRange));
        _weapon.UpdateLastFireTime(context.TotalSeconds);
    }

    public void FireAtDirection(GameContext context, Vector2 direction, Vector2 shootPosition)
    {
        if (_weapon == null || _weapon.CurrentAmmo <= 0) return;

        // Получаем позицию центра персонажа
        shootPosition += _characterCenter; // Центр персонажа
        if (direction != Vector2.Zero)
        {
            direction = Vector2.Normalize(direction);
        }
        else
        {
            direction = new Vector2(1, 0); // Стреляем вправо, если направление нулевое
        }
        _bullets.Add(new Bullet(context, shootPosition, direction, _collisionChecker, _bots, _damage, _isPlayerShoot, _bulletSpeed, _bulletRange));
        _weapon.UpdateLastFireTime(context.TotalSeconds);
    }

    public void Draw(GameContext context)
    {
        // Рисуем пули
        foreach (var bullet in _bullets)
        {
            bullet.Draw(context);
        }
    }
}
