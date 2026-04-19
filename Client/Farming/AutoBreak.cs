using System.Collections.Generic;
using BasicTypes;
using BModv2.Client.Impl.Utils;
using BModv2.Patches;
using BModv2.Patches.Impl;
using Il2CppSystem;
using UnityEngine;

namespace BModv2.Farming;

public class AutoBreak
{
    public static bool isEnabled = false;
    // wooden platform by default
    public static int blockToFarm = 16;
    
    private static int ticksPassed = 0;
    private static float nextPlacementTime = 0f;

    private enum Stage
    {
        Breaking,
        Collecting,
        Placing
    }

    private static Stage currentStage = Stage.Breaking;

    public static void onTick()
    {
        if (!isEnabled) return;

        Vector2i myPos = Constants.getCurrentPlayerMapPoint();
        
        if (myPos.x == 40 && myPos.y == 30)
        {
            isEnabled = false;
            return;
        }
        
        if (currentStage == Stage.Placing)
        {
            ticksPassed += 5;
        }
        else
            ++ticksPassed;
        
        if (ticksPassed < 20) return;
        ticksPassed = 0;

        World world = Constants.getWorld();
        if (world == null) return;
        

        switch (currentStage)
        {
            case Stage.Breaking:
                if (TryBreakOneBlock(world, myPos))
                    return;

                currentStage = Stage.Collecting;
                return;

            case Stage.Collecting:
                CollectAllNearby();
                currentStage = Stage.Placing;
                return;

            case Stage.Placing:
                if (!HasEnoughPlatforms())
                {
                    isEnabled = false;
                    Plugin.Log?.LogInfo("Out of blocks. Disabling");
                    return;
                }

                if (TryPlaceOneBlock(world, myPos))
                    return;

                currentStage = Stage.Breaking;
                return;
        }
    }

    private static bool TryBreakOneBlock(World world, Vector2i myPos)
    {
        for (int w = -2; w < 3; w++)
        {
            for (int h = -2; h < 3; h++)
            {
                if (w == 0 && h == 0) continue;
                if (w == 0 && h == -1) continue;

                Vector2i breakPos = new Vector2i(myPos.x + w, myPos.y + h);
                World.BlockType blockType = world.GetBlockType(breakPos);

                if (blockType == World.BlockType.None) continue;

                OutgoingMessages.SendHitBlockMessage(breakPos, DateTime.Now, false);
                return true;
            }
        }

        return false;
    }

    private static void CollectAllNearby()
    {
        Il2CppSystem.Collections.Generic.List<Collectable> collectables = ControllerHelper.worldController.currentCollectables;
        List<CollectableData> data = new List<CollectableData>();

        foreach (Collectable collectable in collectables)
            data.Add(collectable.collectableData);

        foreach (CollectableData cd in data)
        {
            bool canPick = ConfigData.CanPlayerPickCollectableFromMapPoint(
                Constants.getWorld(),
                cd.mapPoint,
                true,
                Constants.getPlayerData()
            );

            if (canPick)
                OutgoingMessages.SendCollectCollectableMessage(cd.id);
        }
    }

    private static bool TryPlaceOneBlock(World world, Vector2i myPos)
    {
        if (Time.time < nextPlacementTime)
            return true;

        for (int w = -1; w <= 1; w++)
        {
            for (int h = -1; h <= 1; h++)
            {
                if (w == 0 && h == 0) continue;
                if (w == 0 && h == -1) continue;

                Vector2i placePos = new Vector2i(myPos.x + w, myPos.y + h);
                World.BlockType blockType = world.GetBlockType(placePos);

                if (blockType != World.BlockType.None) continue;

                PlayerUtils.PlaceBlock(placePos.x, placePos.y, (World.BlockType)blockToFarm);
                nextPlacementTime = Time.time + 0.1f;
                return true;
            }
        }

        return false;
    }

    private static bool HasEnoughPlatforms()
    {
        PlayerData.InventoryKey[] inv = Constants.getInventory();

        foreach (PlayerData.InventoryKey invKey in inv)
        {
            if ((int)invKey.blockType != blockToFarm) continue;
            var a = Constants.getPlayerData().GetCount(invKey);
            Plugin.Log?.LogInfo($"Bot has {a} platforms");
            return a >= 8;

        }

        return false;
    }
}