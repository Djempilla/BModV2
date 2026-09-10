namespace BModv2.Client.Misc.Commands;

public class WorldCommand
{
    public static void Execute(string worldName)
    {
        SceneLoader.CheckIfWeCanGoFromWorldToWorld(worldName, "", null);
    }
}