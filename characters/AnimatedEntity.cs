namespace C__game;
public abstract class AnimatedEntity
{
    protected Texture2D Texture;
    protected Vector2 Position;
    protected Vector2 Direction;
    protected AnimationManager Animations;

    public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, 56, 50);

    public virtual void Update(GameContext context) => 
        Animations.Update(context, Direction);

    public virtual void Draw(GameContext context) => 
        Animations.Draw(context, Position);
}