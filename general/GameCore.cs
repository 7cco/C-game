namespace C__game;

public enum GameState
{
    MainMenu,
    LevelSelection,
    Playing
}

public class GameCore : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private GameState _currentGameState = GameState.MainMenu;
    private MainMenu _mainMenu;
    private LevelSelection _levelSelection;
    private GameManager _gameManager;
    private GameContext _context;
    private Map _background;
    private CameraManager _camera;

    public static GameCore Instance { get; private set; }

    public GameCore()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Instance = this;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = 1024;
        _graphics.PreferredBackBufferHeight = 768;
        _graphics.ApplyChanges();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _context = new GameContext(Content, _spriteBatch);

        _mainMenu = new MainMenu(_context);
        _mainMenu.LoadContent();

        _levelSelection = new LevelSelection(_context);
        _levelSelection.LoadContent();

    }

    protected override void Update(GameTime gameTime)
{
    _context.Update(gameTime);

    var keyboardState = Keyboard.GetState();

    // Возврат в главное меню из игрового процесса
    if (_currentGameState == GameState.Playing && keyboardState.IsKeyDown(Keys.Escape))
    {
        _currentGameState = GameState.MainMenu;
    }

    switch (_currentGameState)
    {
        case GameState.MainMenu:
            _mainMenu.Update(gameTime);
            break;
        case GameState.LevelSelection:
            _levelSelection.Update(gameTime);
            break;
        case GameState.Playing:
            UpdateGame(gameTime);
            break;
    }

    base.Update(gameTime);
}

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Beige);

        switch (_currentGameState)
        {
            case GameState.MainMenu:
                _mainMenu.Draw(_context);
                break;
            case GameState.LevelSelection:
                _levelSelection.Draw(_context);
                break;
            case GameState.Playing:
                DrawGame(gameTime);
                break;
        }

        base.Draw(gameTime);
    }

    public void StartGame()
    {
        _currentGameState = GameState.Playing;
    }

    public void ShowLevelSelection()
    {
        _currentGameState = GameState.LevelSelection;
    }

    public void LoadLevel(int level)
    {
        // Логика загрузки уровня
        _background = new Map(_context, $"level_{level}");
        _camera = new CameraManager(GraphicsDevice.Viewport, _background.MapWidth, _background.MapHeight);
        _gameManager = new GameManager(_context);
        _gameManager.Init();
        _currentGameState = GameState.Playing;
    }

    private void UpdateGame(GameTime gameTime)
    {
        // Обновление игрового процесса
        _gameManager.Update(_context);
        _camera.Follow(_gameManager.HeroPosition);
    }

    private void DrawGame(GameTime gameTime)
    {
        _spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix());
        _background.Draw(_context);
        _gameManager.Draw(_context);
        _spriteBatch.End();
    }
}