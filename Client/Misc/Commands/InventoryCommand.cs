using BasicTypes;
using BModv2.Client.Impl.Utils;
using BModv2.Patches.Impl;
using Il2CppSystem.Collections.Generic;

namespace BModv2.Client.Misc.Commands;

public class InventoryCommand
{
    public static void Execute()
    {
        PlayerData.InventoryKey[] inv = Constants.getInventory();
        
        foreach (var item in inv)
        {
            Plugin.Log?.LogInfo($"BlockType: {item.blockType}");
            Plugin.Log?.LogInfo($"ItemType: {item.itemType}");
        }
        
        World world = Constants.getWorld();
        
        int collectablesCount = world.GetCollectablesCount();
        Plugin.Log.LogInfo($"Collectables in world: {collectablesCount}");
        
        List<BlockPositionData> blockPositions = new List<BlockPositionData>();
        
        for (int y = 0; y < 80; y++)
        {
            for (int x = 3; x < 60; x++)
            {
                World.BlockType bt = world.GetBlockType(new Vector2i(x, y));
                World.BlockType bg = world.GetBlockBackgroundType(new Vector2i(x, y));
                blockPositions.Add(new BlockPositionData(x, y, bt, bg));
            }
        }
        
        // collectables data
        
        // List<Collectable> collectables = ControllerHelper.worldController.currentCollectables;
        // List<CollectableData> data = new List<CollectableData>();
        //
        // foreach (Collectable collectable in collectables)
        // {
        //     data.Add(collectable.collectableData);
        // }
        //
        //
        // foreach (CollectableData cd  in data)
        // {
        //     bool a = ConfigData.CanPlayerPickCollectableFromMapPoint(Constants.getWorld(), cd.mapPoint, true,
        //         Constants.getPlayerData());
        //     Plugin.Log.LogInfo($"point: {cd.mapPoint.x} {cd.mapPoint.y}");
        //     Plugin.Log.LogInfo($"isCollectable: {a}");
        //     Plugin.Log.LogInfo($"count: {cd.amount}");
        //     Plugin.Log.LogInfo($"isGem: {cd.isGem}");
        //     Plugin.Log.LogInfo($"blockType: {cd.blockType}");
        // }
        //
    }
}

public class BlockPositionData
{
    public int x;
    public int y;
    World.BlockType blockType;
    World.BlockType blockBackgroundType;

    public BlockPositionData(int x, int y, World.BlockType blockType, World.BlockType blockBackgroundType)
    {
        this.x = x;
        this.y = y;
        this.blockType = blockType;
        this.blockBackgroundType = blockBackgroundType;
    }
}