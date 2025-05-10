using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace C__game;

public interface IRangedWeapon
{
    float Damage { get; }
    float FireRate { get; }
    float Range { get; }
    float BulletSpeed { get; }
    Texture2D Texture { get; }
    Rectangle Bounds { get; }
    Vector2 Position { get; set; }
    int CurrentAmmo { get; }
    int MaxAmmo { get; }
    bool IsReloading { get; }
    float ReloadProgress { get; }
    void Draw(GameContext context);
    void Update(GameContext context);
    bool StartReload();
    void UpdateLastFireTime(float currentTime);
} 