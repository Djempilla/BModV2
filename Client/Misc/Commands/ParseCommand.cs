using System.Collections.Generic;
using BModv2.Client.Impl.Utils;
using BModv2.Mining;
using BModv2.Patches.Impl;
using BModv2.Patches.Impl.Utils;
using UnityEngine;

namespace BModv2.Patches.Misc.Commands;

public class ParseCommand
{

    public static void Execute()
    {
        World world = Constants.getWorld();
        if (world == null)
        {
            Plugin.Log?.LogInfo("World is null");
            return;
        }

        
        var playerPos = Constants.getCurrentPlayerMapPoint();
        var path = MinePathFinder.FindPathToExit(playerPos);
        
        var points = new List<Vector3>();
        
        foreach (var node in path)
        {
            var worldPos = WorldUtils.ConvertMapPointToWorldPoint(node.x, node.y);
            worldPos.z = 0f;
            points.Add(worldPos);
        }
        
        RenderUtils.RenderPath(points);
        
        
    }
    
}