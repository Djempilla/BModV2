using BasicTypes;
using BModv2.Farming;
using BModv2.Patches.Impl;
using BModv2.Patches.Impl.Utils;

namespace BModv2.Patches;

public class BreakerCommand
{
    public static void Execute()
    {

        Vector2i v = WorldUtils.getMousePosition();

        World.BlockType bt = Constants.getWorld().GetBlockType(v);
        
        AutoBreak.isEnabled = !AutoBreak.isEnabled;

        if(bt != 0) 
            AutoBreak.blockToFarm = (int)bt;
        Plugin.Log?.LogInfo("AutoBreak enabled: " + AutoBreak.isEnabled);
    }
}