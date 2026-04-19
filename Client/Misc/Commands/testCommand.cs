
using BasicTypes;
using BModv2.Patches.Impl;
using BModv2.Patches.Impl.Utils;
using Il2CppSystem.Collections.Generic;
using Kernys.Bson;

namespace BModv2.Patches.Misc.Commands;

public class testCommand
{
    //  so nigga badass shit
    public static void Execute()
    {
        ControllerHelper.rootUI.RemoveWorldLighting();

        // Vector2i firstMapPointOfBlockType = Constants.getWorld().GetFirstMapPointOfBlockType(World.BlockType.PortalMineExit, World.LayerType.Block);
        // BSONObject bson = new BSONObject();
        // bson["ID"] = "GMExit";
        // bson["x"] = firstMapPointOfBlockType.x;
        // bson["y"] = firstMapPointOfBlockType.y;
        // OutgoingMessages.AddOneMessageToList(bson);
        
        return;
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
            Plugin.Log.LogInfo($"Point: {cd.mapPoint.x} {cd.mapPoint.y} collectable: {a}");
            if (a)
            {
                OutgoingMessages.SendCollectCollectableMessage(cd.id);
            }
        }
    }
    
}