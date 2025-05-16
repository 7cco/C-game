namespace C__game;

public class MainMenu
{
    private readonly GameContext _context;
    private Texture2D _backgroundTexture;
    private SpriteFont _font;
    private int _selectedOption = 0; // Индекс выбранной опции (0 - "Start", 1 - "Level Select", 2 - "Exit")
    private readonly string[] _menuOptions = { "Start Game", "Select Level", "Exit" };
    private KeyboardState previousKeyboardState;

    public MainMenu(GameContext context)
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

        if (keyboardState.IsKeyDown(Keys.Down) && !previousKeyboardState.IsKeyDown(Keys.Down))
        {
            _selectedOption = (_selectedOption + 1) % _menuOptions.Length;
        }
        else if (keyboardState.IsKeyDown(Keys.Up) && !previousKeyboardState.IsKeyDown(Keys.Up))
        {
            _selectedOption = (_selectedOption - 1 + _menuOptions.Length) % _menuOptions.Length;
        }

        if (keyboardState.IsKeyDown(Keys.Enter) && !previousKeyboardState.IsKeyDown(Keys.Enter))
        {
            switch (_selectedOption)
            {
                case 0:
                    StartGame();
                    break;
                case 1:
                    SelectLevel();
                    break;
                case 2:
                    ExitGame();
                    break;
            }
        }

        previousKeyboardState = keyboardState;
    }

    public void Draw(GameContext context)
    {
        context.SpriteBatch.Begin();
        context.SpriteBatch.Draw(_backgroundTexture, Vector2.Zero, Color.White);

        for (int i = 0; i < _menuOptions.Length; i++)
        {
            var option = _menuOptions[i];
            var position = new Vector2(400, 300 + i * 50);
            var color = i == _selectedOption ? Color.Yellow : Color.White;
            context.SpriteBatch.DrawString(_font, option, position, color);
        }

        context.SpriteBatch.End();
    }

    private void StartGame()
    {
        // Переход к игре (например, загрузка первого уровня)
        GameCore.Instance.StartGame();
    }

    private void SelectLevel()
    {
        // Переход к экрану выбора уровня
        GameCore.Instance.ShowLevelSelection();
    }

    private void ExitGame()
    {
        // Выход из игры
        GameCore.Instance.Exit();
    }
}