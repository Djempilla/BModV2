using BasicTypes;
using BModv2.Farming;
using BModv2.Mining;
using BModv2.Patches.Impl;
using BModv2.Patches.Impl.Utils;

namespace BModv2.Patches;

public class BreakerCommand
{
    public static void Execute(bool breakBG)
    {

        Vector2i v = WorldUtils.getMousePosition();

        World.BlockType bt;
        if (breakBG)
        {
            bt = Constants.getWorld().GetBlockBackgroundType(v);
        }
        else
        {
            bt = Constants.getWorld().GetBlockType(v);
        }
        
        LEGACY_AutoBreak.breakBg = breakBG;
        LEGACY_AutoBreak.isEnabled = !LEGACY_AutoBreak.isEnabled;
        LEGACY_AutoBreak.farmWorld = Constants.getWorld().worldName;

        if(bt != 0) 
            LEGACY_AutoBreak.blockToFarm = (int)bt;
        Plugin.Log?.LogInfo("AutoBreak enabled: " + LEGACY_AutoBreak.isEnabled);
    }
}