using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace C__game;

public class AssaultRifle : Weapon
{
    public AssaultRifle(Texture2D texture) : base(
        texture: texture,
        damage: 15f,      // Средний урон
        fireRate: 10f,    // 10 выстрелов в секунду
        range: 400f,      // Средне-дальняя дистанция
        bulletSpeed: 1000f, // Быстрые пули
        maxAmmo: 30,      // 30 патронов в магазине
        reloadTime: 2.0f  // 2 секунды на перезарядку
    )
    {
    }
} 