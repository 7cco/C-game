namespace C__game;

public class LevelSelection
{
    private readonly GameContext _context;
    private Texture2D _backgroundTexture;
    private SpriteFont _font;
    private int _selectedLevel = 0; // Индекс выбранного уровня
    private readonly string[] _levels = { "Level 1", "Level 2", "Level 3" };
    private KeyboardState previousKeyboardState;
    private bool isMenuOpen = false; // Новая переменная для отслеживания состояния меню

    public LevelSelection(GameContext context)
    {
        _context = context;
    }

    public void LoadContent()
    {
        _backgroundTexture = _context.Content.Load<Texture2D>("backfon");
        _font = _context.Content.Load<SpriteFont>("font");
    }

    public void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();

        // Проверяем, открыто ли меню
        if (!isMenuOpen)
        {
            // Если меню не открыто, проверяем нажатие клавиши для его открытия
            if (keyboardState.IsKeyDown(Keys.Enter) && !previousKeyboardState.IsKeyDown(Keys.Enter))
            {
                isMenuOpen = true; // Открываем меню
                _selectedLevel = 0; // Сбрасываем выбор уровня на первый
            }
        }
        else
        {
            // Если меню открыто, обрабатываем выбор уровня
            if (keyboardState.IsKeyDown(Keys.Left) && !previousKeyboardState.IsKeyDown(Keys.Left))
            {
                _selectedLevel = (_selectedLevel - 1 + _levels.Length) % _levels.Length;
            }
            else if (keyboardState.IsKeyDown(Keys.Right) && !previousKeyboardState.IsKeyDown(Keys.Right))
            {
                _selectedLevel = (_selectedLevel + 1) % _levels.Length;
            }

            if (keyboardState.IsKeyDown(Keys.Enter) && !previousKeyboardState.IsKeyDown(Keys.Enter))
            {
                LoadSelectedLevel(); // Загружаем выбранный уровень
                isMenuOpen = false; // Закрываем меню после загрузки уровня
            }
        }

        previousKeyboardState = keyboardState;
    }

    public void Draw(GameContext context)
    {
        context.SpriteBatch.Begin();
        context.SpriteBatch.Draw(_backgroundTexture, Vector2.Zero, Color.White);

        for (int i = 0; i < _levels.Length; i++)
        {
            var levelName = _levels[i];
            var position = new Vector2(300 + i * 200, 400);
            var color = i == _selectedLevel ? Color.Yellow : Color.White;
            context.SpriteBatch.DrawString(_font, levelName, position, color);
        }

        context.SpriteBatch.End();
    }

    private void LoadSelectedLevel()
    {
        // Загрузка выбранного уровня
        GameCore.Instance.LoadLevel(_selectedLevel + 1);
    }
}