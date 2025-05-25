namespace C__game;

using System.Linq;

public class BossManager
{
    private readonly List<Boss> _bosses;

    public BossManager()
    {
        _bosses = new List<Boss>();
    }

    public void ClearBosses()
    {
        _bosses.Clear();
    }

    public void AddBoss(Boss boss)
    {
        _bosses.Add(boss);
    }
    
    public void Update(GameContext context)
    {
        // Обновляем всех боссов
        foreach (var boss in _bosses)
        {
            boss.Update(context);
        }
    }
    
    public void Draw(GameContext context)
    {
        // Рисуем всех боссов
        foreach (var boss in _bosses)
        {
            boss.Draw(context);
        }
    }
    
    // Проверяем, все ли боссы побеждены
    public bool AllBossesDefeated()
    {
        return _bosses.Count > 0 && _bosses.All(boss => boss.IsDead);
    }
    
    // Получаем список всех боссов для обнаружения столкновений
    public List<Bot> GetBossesAsBots()
    {
        return _bosses.Cast<Bot>().ToList();
    }
} 