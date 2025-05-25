namespace C__game;

public class Obstacle
{
    public Texture2D Texture { get; private set; }
    public Rectangle Bounds;
    private bool _isVisible;

    public Obstacle(GameContext context, string textureName, Vector2 pos, int width, int height, bool isVisible = true)
    {
        // Загружаем текстуру препятствия только если оно видимое
        if (isVisible)
        {
            Texture = context.Content.Load<Texture2D>(textureName);
        }
        _isVisible = isVisible;

        // Устанавливаем границы препятствия
        Bounds = new Rectangle(
            (int)pos.X,
            (int)pos.Y,
            width,
            height
        );
    }

    public void Draw(GameContext context)
    {
        // Рисуем препятствие только если оно видимое
        if (_isVisible && Texture != null)
        {
            context.SpriteBatch.Draw(
                Texture,
                Bounds,
                Color.White
            );
        }
    }
}