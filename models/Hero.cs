namespace C__game;

public class Hero
{
    private Texture2D _texture;
    private Vector2 _position = new(100, 100);
    private readonly float _speed = 200f;
    private readonly AnimationManager _anims;
    public Vector2 Position => _position;

    public Hero(GameContext context)
    {
        // Загружаем текстуру через контекст
        _texture = context.Content.Load<Texture2D>("final gg-2x");

        // Инициализируем менеджер анимаций
        _anims = new AnimationManager();

        // Добавляем анимации для всех направлений движения
        _anims.AddAnimation(new Vector2(0, 1), new Animation(_texture, 6, 4, 0.1f, 4));   // Вниз
        _anims.AddAnimation(new Vector2(-1, 0), new Animation(_texture, 6, 4, 0.1f, 2)); // Влево
        _anims.AddAnimation(new Vector2(1, 0), new Animation(_texture, 6, 4, 0.1f, 1));  // Вправо
        _anims.AddAnimation(new Vector2(0, -1), new Animation(_texture, 6, 4, 0.1f, 3)); // Вверх
        _anims.AddAnimation(new Vector2(-1, 1), new Animation(_texture, 6, 4, 0.1f, 2)); // Влево-вниз
        _anims.AddAnimation(new Vector2(-1, -1), new Animation(_texture, 6, 4, 0.1f, 2)); // Влево-вверх
        _anims.AddAnimation(new Vector2(1, 1), new Animation(_texture, 6, 4, 0.1f, 1));  // Вправо-вниз
        _anims.AddAnimation(new Vector2(1, -1), new Animation(_texture, 6, 4, 0.1f, 1)); // Вправо-вверх
    }

    public Rectangle Bounds => new Rectangle(
        (int)_position.X,
        (int)_position.Y,
        56,
        50
    );

    public void SetPosition(Vector2 newPosition)
    {
        _position = newPosition;
    }









    public void Update(GameContext context)
    {
        // Обновляем позицию героя, если он движется
        if (InputManager.Moving)
        {
            _position += Vector2.Normalize(InputManager.Direction) * _speed * context.TotalSeconds;
        }

        // Обновляем текущую анимацию
        _anims.Update(context, InputManager.Direction);
    }

    public void Draw(GameContext context)
    {
    // Рисуем текущую анимацию в позиции героя
        _anims.Draw(context, _position);
    }
}