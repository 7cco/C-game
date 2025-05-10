namespace C__game;

public class CameraManager
{
    private readonly Viewport _viewport;
    private Vector2 _position;
    private int _mapWidth;
    private int _mapHeight;
    public static CameraManager Instance { get; private set; }
    public Vector2 Position => _position;
    public CameraManager(Viewport viewport, int mapWidth, int mapHeight)
    {
        _viewport = viewport;
        _position = Vector2.Zero;
        _mapWidth = mapWidth;
        _mapHeight = mapHeight;
        Instance = this; // Initialize the singleton instance
    }

    /// Устанавливает позицию камеры так, чтобы цель находилась в центре экрана.
     public void Follow(Vector2 targetPosition)
    {
        // Центрируем камеру на целевой позиции
        _position.X = targetPosition.X - _viewport.Width / 2f;
        _position.Y = targetPosition.Y - _viewport.Height / 2f;

        // Ограничиваем камеру, чтобы она не выходила за пределы карты
        _position.X = MathHelper.Clamp(_position.X, 0, _mapWidth - _viewport.Width);
        _position.Y = MathHelper.Clamp(_position.Y, 0, _mapHeight - _viewport.Height);
    }


    /// Возвращает матрицу трансформации для отрисовки объектов с учетом положения камеры.
    public Matrix GetViewMatrix()
    {
        return Matrix.CreateTranslation(-_position.X, -_position.Y, 0);
    }
}