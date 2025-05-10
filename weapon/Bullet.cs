namespace C__game;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

public class Bullet
{   
    private readonly Texture2D _texture;
    private Vector2 _position;
    private Vector2 _direction;
    private readonly float _speed;
    private bool _isActive = true;
    private readonly ICollisionChecker _collisionChecker;
    private readonly GameContext _context;
    private readonly List<Bot> _bots;
    private readonly float _damage;
    private readonly bool _isPlayerBullet;
    private readonly float _range;
    private Vector2 _startPosition;
    private float _distanceTraveled;

    public Bullet(GameContext context, Vector2 startPosition, Vector2 direction, ICollisionChecker collisionChecker, List<Bot> bots, float damage = 10f, bool isPlayerBullet = true, float speed = 800f, float range = 300f)
    {
        _texture = context.Content.Load<Texture2D>("bullet");
        _position = startPosition;
        _startPosition = startPosition;
        _direction = Vector2.Normalize(direction);
        _collisionChecker = collisionChecker;
        _context = context;
        _bots = bots;
        _damage = damage;
        _isPlayerBullet = isPlayerBullet;
        _speed = speed;
        _range = range;
        _distanceTraveled = 0f;
    }

    public void Update(GameTime gameTime)
    {
        if (!_isActive) return;
        
        float moveDistance = _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        _position += _direction * moveDistance;
        _distanceTraveled += moveDistance;

        // Проверяем, не превышена ли дальность стрельбы
        if (_distanceTraveled > _range)
        {
            _isActive = false;
            return;
        }

        if (_isPlayerBullet)
        {
            foreach (var bot in _bots.ToList())
            {
                if (Bounds.Intersects(bot.Bounds))
                {
                    bot.TakeDamage(_damage);
                    _isActive = false;
                    break;
                }
            }
        }
        else
        {
            // Проверяем попадание в игрока
            var hero = _bots.FirstOrDefault()?.Target;
            if (hero != null && !hero.IsDead && Bounds.Intersects(hero.Bounds))
            {
                hero.TakeDamage(_damage);
                _isActive = false;
            }
        }

        if (_position.X < 0 || _position.X > 2000 || _position.Y < 0 || _position.Y > 2000)
        {
            _isActive = false;
        }

        if (_collisionChecker.CheckCollision(Bounds))
        {
            _isActive = false;
        }
    }

    public void Draw(GameContext context)
    {
        if (!_isActive) return;
        
        context.SpriteBatch.Draw(
            _texture,
            _position,
            null,
            Color.White,
            0f,
            new Vector2(_texture.Width / 2, _texture.Height / 2),
            1f,
            SpriteEffects.None,
            0f
        );
    }

    public bool IsActive => _isActive;
    public Vector2 Position => _position;
    public Rectangle Bounds => new Rectangle(
        (int)_position.X - _texture.Width / 2,
        (int)_position.Y - _texture.Height / 2,
        _texture.Width,
        _texture.Height
    );
} 