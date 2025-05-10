namespace C__game;

public abstract class Weapon : IRangedWeapon
{
    protected Texture2D texture;
    protected Vector2 position;
    protected Rectangle bounds;
    protected float damage;
    protected float fireRate;
    protected float range;
    protected float bulletSpeed;
    protected float lastFireTime;
    protected int maxAmmo;
    protected int currentAmmo;
    protected float reloadTime;
    protected float currentReloadTime;
    protected bool isReloading;

    public float Damage => damage;
    public float FireRate => fireRate;
    public float Range => range;
    public float BulletSpeed => bulletSpeed;
    public Texture2D Texture => texture;
    public Rectangle Bounds => bounds;
    public int CurrentAmmo => currentAmmo;
    public int MaxAmmo => maxAmmo;
    public bool IsReloading => isReloading;
    public float ReloadProgress => 1f - (currentReloadTime / reloadTime);

    public Vector2 Position 
    { 
        get => position;
        set
        {
            position = value;
            bounds = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }
    }

    protected Weapon(Texture2D texture, float damage, float fireRate, float range, float bulletSpeed, int maxAmmo, float reloadTime)
    {
        this.texture = texture;
        this.damage = damage;
        this.fireRate = fireRate;
        this.range = range;
        this.bulletSpeed = bulletSpeed;
        this.maxAmmo = maxAmmo;
        this.currentAmmo = maxAmmo;
        this.reloadTime = reloadTime;
        this.currentReloadTime = 0f;
        this.isReloading = false;
        this.bounds = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
    }

    public virtual void Draw(GameContext context)
    {
        context.SpriteBatch.Draw(texture, position, Color.White);
    }

    public virtual void Update(GameContext context)
    {
        if (isReloading)
        {
            currentReloadTime -= context.TotalSeconds;
            if (currentReloadTime <= 0)
            {
                FinishReload();
            }
        }
    }

    public bool CanFire(float currentTime)
    {
        return !isReloading && 
               currentAmmo > 0 && 
               currentTime - lastFireTime >= 1.0f / fireRate;
    }

    public void UpdateLastFireTime(float currentTime)
    {
        lastFireTime = currentTime;
        if (currentAmmo > 0)
        {
            currentAmmo--;
        }
    }

    public bool StartReload()
    {
        if (!isReloading && currentAmmo < maxAmmo)
        {
            isReloading = true;
            currentReloadTime = reloadTime;
            return true;
        }
        return false;
    }

    protected void FinishReload()
    {
        currentAmmo = maxAmmo;
        isReloading = false;
        currentReloadTime = 0f;
    }
} 