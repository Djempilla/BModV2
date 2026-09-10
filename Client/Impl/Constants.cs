using BasicTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace BModv2.Patches.Impl;

public static class Constants
{
    
    // Modules toggles constants
    public static bool IsFreecamEnabled = false;
    public static bool IsKeyflyEnabled = false;
    public static bool IsFlyEnabled = false;

    public static bool spamWorldLock = false;
    
    // HUGE TODO REPLACE WITH GETTERS ESPECIALLY FOR WORLD
    public static Player thePlayer =  ControllerHelper.worldController.player;
    public static World theWorld = ControllerHelper.worldController.world;
    public static Vector2i currentPlayerMapPoint = ControllerHelper.freeSpaceController.currentPlayerMapPoint;

    public static World getWorld()
    {
        return ControllerHelper.worldController.world;
    }
    
    public static Player getPlayer()
    {
        return ControllerHelper.worldController.player;
    }

    public static Vector2i getCurrentPlayerMapPoint()
    {
        return ControllerHelper.freeSpaceController.currentPlayerMapPoint;
    }

    public static PlayerData getPlayerData()
    {
        return ControllerHelper.worldController.player.myPlayerData;
    }
    
    
    // QOL Getters

    public static PlayerData.InventoryKey[] getInventory()
    {
        return getPlayerData().GetInventoryAsOrderedByInventoryItemType();
    }
    
    public static int getPlayerX()
    {
        return getCurrentPlayerMapPoint().x;
    }
    
    public static int getPlayerY()
    {
        return getCurrentPlayerMapPoint().y;
    }
}