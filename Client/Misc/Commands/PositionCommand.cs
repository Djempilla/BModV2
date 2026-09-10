using System.Runtime.Intrinsics.X86;
using BasicTypes;
using BModv2.Patches.Impl;
using BModv2.Patches.Impl.Utils;
using Il2CppSystem;
using UnityEngine;

namespace BModv2.Patches.Misc.Commands;

public class PositionCommand
{
    public static void Execute()
    {
        if (Constants.thePlayer != null)
        {
            Plugin.Log.LogInfo($"Pos: {Constants.thePlayer.GetPlayerMapPoint().x} {Constants.thePlayer.GetPlayerMapPoint().y}");
            if (Camera.main != null)
            {
                Vector2i v = WorldUtils.ConvertWorldPointToMapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                World.BlockType blockType = Constants.getWorld().GetBlockType(v);
                World.BlockType blockTypeBg = Constants.getWorld().GetBlockBackgroundType(v);
                Plugin.Log.LogInfo($"Cursor: {v.x} {v.y}");
                Plugin.Log.LogInfo($"BlockID: {blockType}");
                Plugin.Log.LogInfo($"BackgroundID: {blockTypeBg}");
                // Plugin.Log.LogInfo($"seed: {Constants.getWorld().GetSeedDataAt(v.x, v.y)}");
                // Plugin.Log.LogInfo($"seed null: {Constants.getWorld().GetSeedDataAt(v.x, v.y) == null}");
                // Plugin.Log.LogInfo($"seed growthDurationInSeconds: {Constants.getWorld().GetSeedDataAt(v.x, v.y).growthDurationInSeconds}");
                // Plugin.Log.LogInfo($"seed growthEndTime: {Constants.getWorld().GetSeedDataAt(v.x, v.y).growthEndTime}");
                // Plugin.Log.LogInfo($"IsGrown: {Constants.getWorld().GetSeedDataAt(v.x, v.y).growthEndTime < DateTime.UtcNow}");
            }
            
        }
    }
}