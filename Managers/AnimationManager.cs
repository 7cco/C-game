namespace C__game;

public class AnimationManager
{
    private readonly Dictionary<object, Animation> _anims = [];
    private object _lastKey;

    public void AddAnimation(object key, Animation animation)
    {
        _anims.Add(key, animation);
        _lastKey ??= key; // Инициализируем _lastKey, если он еще не установлен
    }

    public void Update(GameContext context, object key)
    {
        if (_anims.TryGetValue(key, out Animation value))
        {
            value.Start();
            value.Update(context); // Обновляем анимацию через контекст
            _lastKey = key;
        }
        else
        {
            if (_lastKey != null && _anims.ContainsKey(_lastKey))
            {
                _anims[_lastKey].Stop();
                _anims[_lastKey].Reset();
            }
        }
    }

    public void Draw(GameContext context, Vector2 position)
    {
        if (_lastKey != null && _anims.ContainsKey(_lastKey))
        {
            _anims[_lastKey].Draw(context, position); // Рисуем анимацию через контекст
        }
    }
}