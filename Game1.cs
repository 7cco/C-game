namespace C__game;

public class Game1 : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private GameManager _gameManager;
    private GameContext _context;
    private Map _background;
    private CameraManager _camera;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
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
        // Создаем контекст игры
        _context = new GameContext(Content, _spriteBatch);

        _background = new Map(_context, "genmap");
        _camera = new CameraManager(GraphicsDevice.Viewport, _background.MapWidth, _background.MapHeight);
        
        // Инициализируем менеджер игры
        _gameManager = new GameManager(_context);
        _gameManager.Init();
    }
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
            Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }
        
        // Обновляем контекст игры
        _context.Update(gameTime);
        // Обновляем логику игры
        _gameManager.Update(_context);
        _camera.Follow(_gameManager.HeroPosition);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {

        GraphicsDevice.Clear(Color.Beige);

        _spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix());

        _background.Draw(_context);
        // Рисуем игру через менеджер
        _gameManager.Draw(_context);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

}