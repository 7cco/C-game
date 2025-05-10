using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace C__game;

public class SniperRifle : Weapon
{
    public SniperRifle(Texture2D texture) : base(
        texture: texture,
        damage: 50f,      // Высокий урон
        fireRate: 1f,     // 1 выстрел в секунду
        range: 800f,      // Дальняя дистанция
        bulletSpeed: 1200f, // Очень быстрые пули
        maxAmmo: 5,       // 5 патронов в магазине
        reloadTime: 2.5f  // 2.5 секунды на перезарядку
    )
    {
    }
} 