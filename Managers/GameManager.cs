namespace C__game;

public class GameManager
{
    private Hero _hero;
    private Vector2 _previousHeroPosition;

    public GameManager(GameContext context)
    {
        // Инициализируем героя, передавая контекст
        _hero = new Hero(context);

    }

    public void Init()
    {
        // Дополнительная инициализация, если требуется
    }

    public void Update(GameContext context)
    {
        _previousHeroPosition = _hero.Position;
        // Обновляем InputManager
        InputManager.Update();



        // Обновляем логику героя
        _hero.Update(context);

    }

    public void Draw(GameContext context)
    {
        // Рисуем героя через контекст
        _hero.Draw(context);
    }
    public Vector2 HeroPosition => _hero.Position;
}