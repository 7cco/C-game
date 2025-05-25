namespace C__game;

public class Boss : Bot
{
    private const float BOSS_MAX_HEALTH = 200f;
    private const float BOSS_SPEED = 50f; // Медленнее обычных ботов

    public Boss(GameContext context, Vector2 startPosition, ICollisionChecker collisionChecker, BossType bossType = BossType.Assault)
        : base(context, startPosition, collisionChecker, ConvertBossType(bossType))
    {
        // Увеличиваем здоровье босса
        _maxHealth = BOSS_MAX_HEALTH;
        _currentHealth = BOSS_MAX_HEALTH;
        
        // Устанавливаем замедленную скорость
        SetCombatMode(true); // Всегда в боевом режиме
        _currentSpeed = BOSS_SPEED;
    }
    
    // Метод для конвертирования типа босса в тип обычного бота (чтобы использовать существующее оружие)
    private static BotType ConvertBossType(BossType bossType)
    {
        return bossType switch
        {
            BossType.Assault => BotType.Assault,
            BossType.Sniper => BotType.Sniper,
            BossType.Pistol => BotType.Pistol,
            _ => BotType.Assault
        };
    }
    
    public override void TakeDamage(float damage)
    {
        // Боссы получают немного меньше урона
        base.TakeDamage(damage * 0.8f);
    }
    
    // Используем базовую реализацию стрельбы для боссов
    public override void TryShoot()
    {
        base.TryShoot();
    }
    
    // Переопределяем метод рисования для добавления имени босса над полоской здоровья
    public override void Draw(GameContext context)
    {
        base.Draw(context);
        
        // Отображаем имя босса
        string bossName = "BOSS";
        Vector2 textSize = context.Content.Load<SpriteFont>("font").MeasureString(bossName);
        Vector2 textPosition = new Vector2(
            Position.X + 28 - textSize.X / 2, // Центрирование текста (28 - половина ширины персонажа)
            Position.Y - 30 // Поднимаем текст над полоской здоровья
        );
        
        context.SpriteBatch.DrawString(
            context.Content.Load<SpriteFont>("font"),
            bossName,
            textPosition,
            Color.Red
        );
    }
}

