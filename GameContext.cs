namespace C__game;

public class GameContext
{
    public ContentManager Content { get; }
    public SpriteBatch SpriteBatch { get; }
    public GraphicsDevice GraphicsDevice { get; }
    public float TotalSeconds { get; private set; }

    public GameContext(ContentManager content, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
    {
        Content = content;
        SpriteBatch = spriteBatch;
        GraphicsDevice = graphicsDevice;
    }

    public void Update(GameTime gameTime)
    {
        TotalSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
}