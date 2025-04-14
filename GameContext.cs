namespace C__game;

public class GameContext
{
    public ContentManager Content { get; }
    public SpriteBatch SpriteBatch { get; }
    public float TotalSeconds { get; private set; }

    public GameContext(ContentManager content, SpriteBatch spriteBatch)
    {
        Content = content;
        SpriteBatch = spriteBatch;
    }

    public void Update(GameTime gameTime)
    {
        TotalSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
}