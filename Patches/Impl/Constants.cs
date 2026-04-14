using BasicTypes;

namespace BModv2.Patches.Impl;

public static class Constants
{
    // HUGE TODO REPLACE WITH GETTERS ESPECIALLY FOR WORLD
    public static Player thePlayer =  ControllerHelper.worldController.player;
    public static World theWorld = ControllerHelper.worldController.world;
    public static Vector2i currentPlayerMapPoint = ControllerHelper.freeSpaceController.currentPlayerMapPoint;

    public static World getWorld()
    {
        return ControllerHelper.worldController.world;
    }
}