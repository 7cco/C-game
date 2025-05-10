namespace C__game;

public static class Program
{
    [STAThread]
    private static void Main()
    {
        using var game = new GameCore();
        game.Run();
    }
}