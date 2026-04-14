using BasicTypes;
using BModv2.Patches.Impl;
using BModv2.Patches.Impl.Utils;
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
                Plugin.Log.LogInfo($"Cursor: {v.x} {v.y}");
            }
            
        }
    }
}