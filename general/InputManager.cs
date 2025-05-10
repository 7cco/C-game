namespace C__game;

public static class InputManager
{
    private static Vector2 _direction;
    public static Vector2 Direction => _direction;
    public static bool Moving => _direction != Vector2.Zero;

    public static void Update()
    {
        _direction = Vector2.Zero;
        var keyboardState = Keyboard.GetState();

        if (keyboardState.GetPressedKeyCount() > 0)
        {
            if (keyboardState.IsKeyDown(Keys.A)) _direction.X--;
            if (keyboardState.IsKeyDown(Keys.D)) _direction.X++;
            if (keyboardState.IsKeyDown(Keys.W)) _direction.Y--;
            if (keyboardState.IsKeyDown(Keys.S)) _direction.Y++;
        }
    }

    public static bool IsShooting()
    {
        return Keyboard.GetState().IsKeyDown(Keys.Space) || 
               Mouse.GetState().LeftButton == ButtonState.Pressed;
    }

    public static bool IsReloading()
    {
        return Keyboard.GetState().IsKeyDown(Keys.R);
    }

    public static bool IsPistolSelected()
    {
        return Keyboard.GetState().IsKeyDown(Keys.D1);
    }

    public static bool IsSniperRifleSelected()
    {
        return Keyboard.GetState().IsKeyDown(Keys.D2);
    }

    public static bool IsAssaultRifleSelected()
    {
        return Keyboard.GetState().IsKeyDown(Keys.D3);
    }
}