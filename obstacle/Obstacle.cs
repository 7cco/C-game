namespace C__game;

public class Obstacle
{
    public Texture2D Texture { get; private set; }
    public Rectangle Bounds;
    

    public Obstacle(GameContext context, string textureName, Vector2 pos, int width, int height)
    {
        // Загружаем текстуру препятствия
        Texture = context.Content.Load<Texture2D>(textureName);

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
        // Рисуем препятствие
        context.SpriteBatch.Draw(
            Texture,
            Bounds,
            Color.White
        );
    }
}