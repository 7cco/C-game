namespace C__game;

public class Shoot
{   
    private IRangedWeapon _weapon;
    private List<Bullet> _bullets;
    private float _shootCooldown;
    private float _currentShootCooldownTime;
    private Vector2 _position;
    private readonly Vector2 _characterCenter = new(28, 25); // Половина размера модели (56x50)
    private readonly IBulletCollisionChecker _collisionChecker;
    private readonly GameContext _context;
    private readonly List<Bot> _bots;
    private float _damage;
    private float _bulletSpeed;
    private float _bulletRange;
    private readonly bool _isPlayerShoot;
    private GameTime _lastGameTime;

    public Shoot(Vector2 position, ICollisionChecker baseCollisionChecker, GameContext context, List<Bot> bots, float damage = 10f, bool isPlayerShoot = true)
    {   
        _context = context;         
        _bullets = new List<Bullet>();
        _shootCooldown = 0f;
        _currentShootCooldownTime = 0.2f;
        _position = position;
        _collisionChecker = new BulletCollisionChecker(baseCollisionChecker);
        _bots = bots;
        _damage = damage;
        _bulletSpeed = 800f;
        _bulletRange = 300f;
        _isPlayerShoot = isPlayerShoot;
        _lastGameTime = new GameTime();
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
        UpdateShootCooldown(context.TotalSeconds);
        UpdateBullets(context);

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
    }

    public void UpdateBullets(GameContext context)
    {
        UpdateShootCooldown(context.TotalSeconds);
        _lastGameTime = new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(context.TotalSeconds));

        for (int i = _bullets.Count - 1; i >= 0; i--)
        {
            _bullets[i].Update(_lastGameTime);
            if (!_bullets[i].IsActive)
            {
                _bullets.RemoveAt(i);
            }
        }
    }

    private void UpdateShootCooldown(float deltaTime)
    {
        if (_shootCooldown > 0)
        {
            _shootCooldown -= deltaTime;
        }
    }

    public void Fire(GameContext context)
    {
        if (_weapon == null || _weapon.CurrentAmmo <= 0) return;
        
        var mouseState = Mouse.GetState();
        var mousePosition = new Vector2(mouseState.X, mouseState.Y);
        var shootPosition = _position + _characterCenter;
        var cameraOffset = CameraManager.Instance.Position;
        var worldMousePosition = mousePosition + cameraOffset;
        var shootDirection = worldMousePosition - shootPosition;
        
        shootDirection = shootDirection != Vector2.Zero 
            ? Vector2.Normalize(shootDirection) 
            : Vector2.UnitX;

        _bullets.Add(new Bullet(context, shootPosition, shootDirection, _collisionChecker, _bots, _damage, _isPlayerShoot, _bulletSpeed, _bulletRange));
        _weapon.UpdateLastFireTime(context.TotalSeconds);
    }

    public void FireAtDirection(GameContext context, Vector2 direction, Vector2 shootPosition)
    {
        if (_weapon == null || _weapon.CurrentAmmo <= 0) return;

        shootPosition += _characterCenter;
        direction = direction != Vector2.Zero 
            ? Vector2.Normalize(direction) 
            : Vector2.UnitX;

        _bullets.Add(new Bullet(context, shootPosition, direction, _collisionChecker, _bots, _damage, _isPlayerShoot, _bulletSpeed, _bulletRange));
        _weapon.UpdateLastFireTime(context.TotalSeconds);
    }

    public void Draw(GameContext context)
    {
        foreach (var bullet in _bullets)
        {
            bullet.Draw(context);
        }
    }

    public void ClearBullets()
    {
        _bullets.Clear();
    }
}
