using BasicTypes;
using BModv2.Patches.Impl;
using BModv2.Patches.Impl.Utils;
using UnityEngine;

namespace BModv2.Patches.Misc.Commands;

public class WarpCommand
{

    public static void Execute(bool toSpawn)
    {
        if (Camera.main != null)
        {
            if (toSpawn)
            {
                Constants.thePlayer.WarpPlayer(40, 30);
                Plugin.Log.LogInfo($"Warped at spawn");
                return;
            }

            Constants.getPlayer().doMoveAnimationFlag = true;
            Constants.getPlayer().DoSitAnimation();
            Vector2i v = WorldUtils.ConvertWorldPointToMapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Constants.thePlayer.WarpPlayer(v.x, v.y);
            Plugin.Log.LogInfo($"Warped at {v.x} {v.y}");
        }
    
        
    }
    
    
}