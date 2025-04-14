namespace C__game;

public class Map
{
    
    private Texture2D _texture;
    private Vector2 _position;
    public int MapWidth => _texture.Width;  // Ширина карты
    public int MapHeight => _texture.Height; // Высота карты


    public Map(GameContext context, string textureName)
    {
        _texture = context.Content.Load<Texture2D>(textureName);
    }

    public void Update(Vector2 cameraMovement)
    {
        // Обновляем позицию карты на основе движения камеры
        _position += cameraMovement;
    }

    public void Draw(GameContext context)
    {
        context.SpriteBatch.Draw(
        _texture,
        position: _position,
        color: Color.White);
    }


}