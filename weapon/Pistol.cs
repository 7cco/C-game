using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace C__game;

public class Pistol : Weapon
{
    public Pistol(Texture2D texture) : base(
        texture: texture,
        damage: 10f,      // Средний урон
        fireRate: 5f,     // 5 выстрелов в секунду
        range: 300f,      // Средняя дальность
        bulletSpeed: 800f, // Быстрые пули
        maxAmmo: 12,      // 12 патронов в магазине
        reloadTime: 1.0f  // 1 секунда на перезарядку
    )
    {
    }
} 