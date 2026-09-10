namespace BModv2.Client.Misc.Commands;

public class ReloadCommand
{
    public static void Execute()
    {
        SceneLoader.ReloadGame();
    }
}