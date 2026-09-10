
using BasicTypes;
using BModv2.Client.Render;
using BModv2.Patches.Impl;
using BModv2.Patches.Impl.Utils;
using Il2CppSystem.Collections.Generic;
using Kernys.Bson;
using PlayFab;
using UnityEngine;

namespace BModv2.Patches.Misc.Commands;

public class testCommand
{
    //  so nigga badass shit
    public static void Execute()
    {
        // ControllerHelper.rootUI.RemoveWorldLighting();


        if (Constants.getWorld() != null && Constants.getPlayer() != null)
        {
            Plugin.Log.LogInfo($"BlockType: {Constants.getWorld().lockWorldDataHelper.GetBlockType().ToString()}");
            Plugin.Log.LogInfo($"Owner: {Constants.getWorld().lockWorldDataHelper.GetPlayerWhoOwnsLockName()}");
            Plugin.Log.LogInfo($"{Constants.getWorld().lockWorldDataHelper.GetPlayerWhoOwnsLockId()}");
            Plugin.Log.LogInfo($"{Constants.getWorld().lockWorldDataHelper.GetLastActivatedTime()}");
        } 
        

        
        // Plugin.Log.LogInfo($"Current device id: {PlayFabSettings.DeviceUniqueIdentifier}");

        // Vector2i firstMapPointOfBlockType = Constants.getWorld().GetFirstMapPointOfBlockType(World.BlockType.PortalMineExit, World.LayerType.Block);
        // BSONObject bson = new BSONObject();
        // bson["ID"] = "GMExit";
        // bson["x"] = firstMapPointOfBlockType.x;
        // bson["y"] = firstMapPointOfBlockType.y;
        // OutgoingMessages.AddOneMessageToList(bson);
        
        List<Collectable> collectables = ControllerHelper.worldController.currentCollectables;
        List<CollectableData> data = new List<CollectableData>();

        foreach (Collectable collectable in collectables)
        {
            data.Add(collectable.collectableData);
        }
        
        
        foreach (CollectableData cd  in data)
        {
            bool a = ConfigData.CanPlayerPickCollectableFromMapPoint(Constants.getWorld(), cd.mapPoint, true,
                Constants.getPlayerData());
            Plugin.Log.LogInfo($"Point: {cd.mapPoint.x} {cd.mapPoint.y} collectable: {cd.amount}");
            if (a)
            {
                OutgoingMessages.SendCollectCollectableMessage(cd.id);
            }
        }
    }
    
}

