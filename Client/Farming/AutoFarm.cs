using System.Collections.Generic;
using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.ReadyToRun;
using BasicTypes;
using BModv2.Client.Impl.Utils;
using BModv2.Client.Misc.Commands;
using BModv2.Patches.Impl;
using Il2CppSystem;
using PlayFab.EconomyModels;

namespace BModv2.Client.Farming;


public class AutoFarm
{
    
    // TODO после перезахода чучуть ждать типо IDLE стадию и там кд думаю 120 тиков тип
    // TODO ресет всего стейта пафайндинга??? если вылетели

    private static int ticksPassed = 0;
    public static bool isEnabled = false;
    private static string farmingWorld;
    // 0.3 sec == 20 frames (with 60FPS) and 40 frames (with 30FPS)
    private static int actionDelay = 20;
    private static int idleTimer = 0;
    private static bool isFarmingBackground;
    private static Vector2i farmPosition;
    private static World.BlockType farmableBlockType;
    private static bool NoAvalibleFarmSpotsFound;
    
    // cached data to prevent world scan every 0.3 sec
    private static bool isWorldCached;
    private static List<TileData> worldTilesData= new List<TileData>();
    
    private static AutoFarmStages.Stages currentStage;
    
    public static void onTick()
    {

        if(!isEnabled) return;  
        
        // задержки, ускоряемся в 4 раза если процесс сбора
        if (currentStage == AutoFarmStages.Stages.BREAK_TREE || 
            currentStage == AutoFarmStages.Stages.PLANT_TREE ||
            currentStage == AutoFarmStages.Stages.COLLECT_TREE ||
            currentStage == AutoFarmStages.Stages.FIND_PLATFORM)
        {
            ticksPassed += 5;
        }
        else
        {
            ticksPassed++;
        }
        if(ticksPassed < actionDelay) return;
        ticksPassed = 0;

        // escaping method if we aren't in world rn
        if (HandleReconnect())
            return;
        
        // we stil reconnect itc while moving, only main login paused
        if(AutoMove.IsWalking) return;

        Player player = Constants.getPlayer();
        World world = Constants.getWorld();
        Vector2i playerPos = Constants.getCurrentPlayerMapPoint();

        if (player == null || world == null)
        {
            Plugin.Log.LogInfo("world or player is null. Disabling");
            isEnabled = false;
            return;
        }
        
        if (playerPos.x == 40 && playerPos.y == 30)
        {
            Plugin.Log.LogWarning("[AutoFarm] Player is at spawn point. Stopping to prevent griefing.");
            isEnabled = false;
            return;
        }

        Plugin.Log.LogInfo($"Current stage: {currentStage}");
        switch (currentStage)
        {
            case AutoFarmStages.Stages.IDLE:
                idleTimer++;
                if(idleTimer < 120) return;
                idleTimer = 0;
                currentStage = AutoFarmStages.Stages.PLACE_FARMABLE;
                return;
            case AutoFarmStages.Stages.PLACE_FARMABLE:
                
                // if somehow we got force moved, fixing our position
                if (Constants.getCurrentPlayerMapPoint() != farmPosition)
                {
                    currentStage = AutoFarmStages.Stages.WARP_PORTAL;
                    return;
                }

                PlayerData.InventoryItemType iitype = isFarmingBackground
                    ? PlayerData.InventoryItemType.BlockBackground
                    : PlayerData.InventoryItemType.Block;
                // кончились блоки пока ставим
                if (PlayerUtils.GetItemCount(farmableBlockType, iitype) == 0)
                {
                    currentStage = AutoFarmStages.Stages.BREAK_FARMABLE;
                    return;
                }
                
                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        // skipping block in and under us
                        if (x == 0 && y == -1) continue;
                        if (x == 0 && y == 0) continue;

                        // if tile is empty - placing block
                        if (isTileEmpty(world, playerPos.x + x, playerPos.y + y, isFarmingBackground))
                        {
                            InvalidateWorldCache();
                            if (isFarmingBackground)
                                PlayerUtils.PlaceBackground(playerPos.x + x, playerPos.y + y, farmableBlockType);
                            else
                                PlayerUtils.PlaceBlock(playerPos.x + x, playerPos.y + y, farmableBlockType);
                        }
                    }
                }

                currentStage = AutoFarmStages.Stages.BREAK_FARMABLE;
                return;
            case AutoFarmStages.Stages.BREAK_FARMABLE:
                
                
                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        if (x == 0 && y == -1) continue;
                        if (x == 0 && y == 0) continue;

                        if (!isTileEmpty(world, playerPos.x + x, playerPos.y + y, isFarmingBackground))
                        {
                            InvalidateWorldCache();
                            if (isFarmingBackground)
                                OutgoingMessages.SendHitBlockBackgroundMessage(new Vector2i(playerPos.x + x, playerPos.y + y), DateTime.Now);
                            else
                                OutgoingMessages.SendHitBlockMessage(new Vector2i(playerPos.x + x, playerPos.y + y), DateTime.Now, false);
                            return;
                        }
                    }
                }

                currentStage = AutoFarmStages.Stages.COLLECT_FARMABLE;
                return;
            case AutoFarmStages.Stages.COLLECT_FARMABLE:
                PlayerUtils.CollectAllNearby(true);
                
                // если заполнился стак семок - дроп
                if (PlayerUtils.GetItemCount(farmableBlockType, PlayerData.InventoryItemType.Seed) == 999)
                {
                    PlayerData.InventoryKey invKey;
                    PlayerUtils.TryGetInventoryKey(farmableBlockType, PlayerData.InventoryItemType.Seed, out invKey);
                    player.DropItems(invKey, 999);
                    return;
                }
                
                if (isFarmingBackground)
                {
                    if (PlayerUtils.GetItemCount(farmableBlockType, PlayerData.InventoryItemType.BlockBackground) == 0)
                    {
                        currentStage = AutoFarmStages.Stages.FIND_PLATFORM;
                        return;
                    }  
                }
                else
                {
                    if (PlayerUtils.GetItemCount(farmableBlockType, PlayerData.InventoryItemType.Block) == 0)
                    {
                        currentStage = AutoFarmStages.Stages.FIND_PLATFORM;
                        return;
                    }  
                }
                
                currentStage = AutoFarmStages.Stages.PLACE_FARMABLE;
                return;
            case AutoFarmStages.Stages.FIND_PLATFORM:
                // проверяем кеш мира
                if (!isWorldCached)
                {
                    cacheWorld(world);
                    return;
                }
                
                PlayerData.InventoryItemType iit = isFarmingBackground ? PlayerData.InventoryItemType.BlockBackground : PlayerData.InventoryItemType.Block;
                if (PlayerUtils.GetItemCount(farmableBlockType, iit) == 999)
                {
                    Plugin.Log.LogInfo($"Farmable stack is full {farmableBlockType} going back");
                    currentStage = AutoFarmStages.Stages.WARP_PORTAL;
                    return;
                }

                // out of seed, forcing NoAvalibleFarmSpotsFound
                if (PlayerUtils.GetItemCount(farmableBlockType, PlayerData.InventoryItemType.Seed) == 0)
                {
                    NoAvalibleFarmSpotsFound = true;
                    currentStage = AutoFarmStages.Stages.WARP_PORTAL;
                    return;
                }
                
                // итерация по всем блокам
                foreach (TileData tileData in worldTilesData)
                {
                    // если нашли платформу
                    if (tileData.blockType == World.BlockType.WoodenPlatform)
                    {
                        
                        // валидируем что блок на платформе - блоктип нужный нам либо пустая платформа
                        World.BlockType bt = world.GetBlockType(tileData.x, tileData.y + 1);
                        SeedData seedData = world.GetSeedDataAt(tileData.x, tileData.y + 1);
                        // Plugin.Log.LogInfo($"Found platform isSeed: {seedData != null}, isGrown: ${seedData.growthEndTime < DateTime.UtcNow}, isNone: {bt == World.BlockType.None} isTree {bt == World.BlockType.Tree}");
                        
                        // если нашли платформу с вырасшем деревом
                        if (seedData != null && seedData.growthEndTime < DateTime.UtcNow)
                        {
                            GotoCommand.Execute(tileData.x, tileData.y+1);
                            currentStage = AutoFarmStages.Stages.BREAK_TREE;
                            return;
                        }

                        // если платформа пустая
                        if (bt == World.BlockType.None)
                        {
                            GotoCommand.Execute(tileData.x, tileData.y + 1);
                            currentStage = AutoFarmStages.Stages.PLANT_TREE;
                            return;
                        }
                        // в остальных случаях (платформа не подходит для фарма либо семечко не вырасло) - пропускаем
                    }
                }
                
                // финал - не нашли ни одной платформы под условие
                NoAvalibleFarmSpotsFound = true;
                Plugin.Log.LogInfo("Can't find more platforms. Returning");
                currentStage = AutoFarmStages.Stages.WARP_PORTAL;
                return;
            case AutoFarmStages.Stages.BREAK_TREE:
                // если на позиции куда мы пришли есть семечка - проверяем что она уже выросла тогда ударяем по ней
                if (world.GetSeedDataAt(playerPos.x, playerPos.y) != null && world.GetSeedDataAt(playerPos.x, playerPos.y).growthEndTime < DateTime.UtcNow)
                {
                    OutgoingMessages.SendHitBlockMessage(new Vector2i(playerPos.x, playerPos.y), DateTime.Now);
                    InvalidateWorldCache();
                }

                currentStage = AutoFarmStages.Stages.COLLECT_TREE;
                return;
            case AutoFarmStages.Stages.COLLECT_TREE:
                PlayerUtils.CollectAllNearby();
                currentStage = AutoFarmStages.Stages.PLANT_TREE;
                return;
            case AutoFarmStages.Stages.PLANT_TREE:
                if (PlayerUtils.GetItemCount(farmableBlockType, PlayerData.InventoryItemType.Seed) > 0)
                {
                    PlayerUtils.PlaceSeed(playerPos.x, playerPos.y, farmableBlockType);    
                    InvalidateWorldCache();
                }

                currentStage = AutoFarmStages.Stages.FIND_PLATFORM;
                return;
            case AutoFarmStages.Stages.WARP_PORTAL:
                if (Constants.getCurrentPlayerMapPoint() == farmPosition)
                {
                    // если мы уже обошли все ячейки
                    if (NoAvalibleFarmSpotsFound)
                    {
                        NoAvalibleFarmSpotsFound = false;
                        currentStage = AutoFarmStages.Stages.PLACE_FARMABLE; // небольшая заметка, по идее если так вышло что у нас в этот момент 0 блоков макрос чуть застрянет, этим пока принебрегаем
                        return;
                    }
                    // эта часть выполняется только если остались платформы которые мы не обошли
                    // узнаем что фармим сейчас
                    PlayerData.InventoryItemType dropInvType = isFarmingBackground
                        ? PlayerData.InventoryItemType.BlockBackground
                        : PlayerData.InventoryItemType.Block;

                    // если есть блоки которые надо выкинуть - выкидываем
                    if (PlayerUtils.GetItemCount(farmableBlockType, dropInvType) > 0)
                    {
                        PlayerData.InventoryKey inventoryKey;
                        if (PlayerUtils.TryGetInventoryKey(farmableBlockType, dropInvType,
                                out inventoryKey))
                        {
                            player.DropItems(inventoryKey, PlayerUtils.GetItemCount(farmableBlockType, dropInvType));
                            currentStage = AutoFarmStages.Stages.FIND_PLATFORM;
                        }
                    }
                    else
                    {
                        currentStage = AutoFarmStages.Stages.FIND_PLATFORM;
                    }
                    
                }
                else
                {
                    // сработает если мы не в позиции фарма чтоб её починить.
                    SceneLoader.CheckIfWeCanGoFromWorldToWorld(farmingWorld, "farm", null, false);
                }
                return;
            
        }
    }
    
    
    // удаляет кеш мира, если мы его редактировали
    private static void InvalidateWorldCache()
    {
        isWorldCached = false;
        worldTilesData.Clear();
    }
    
    // x - 60 y - 70
    private static void cacheWorld(World world)
    {
        isWorldCached = true;
        Plugin.Log.LogInfo("Caching world...");

        for (int y = 3; y < 60; y++)
        {
            for (int x = 0; x < 80; x++)
            {
                var pos = new Vector2i(x, y);
                worldTilesData.Add(new TileData(
                    x,
                    y,
                    world.GetBlockType(pos),
                    world.GetBlockBackgroundType(pos)
                ));
            }
        }
    }

    private class TileData
    {
        public int x;
        public int y;
        public World.BlockType blockType;
        public World.BlockType backgroundType;

        public TileData(int x, int y, World.BlockType fg, World.BlockType bg)
        {
            this.x = x;
            this.y = y;
            blockType = fg; // foreground
            backgroundType = bg; // background
        }
    }
    
    
    /*
     * returns if tile (x, y) is empty (both)
     */
    private static bool isTileEmpty(World world, int x, int y, bool background)
    {
        if (background)
            return world.GetBlockBackgroundType(x, y) == World.BlockType.None;
        return world.GetBlockType(x, y) == World.BlockType.None;
    }
    
    /*
     * Called to toggle autofarm
     */
    public static void onToggle(World.BlockType blockType ,bool isBackgroundFarming)
    {
        isEnabled = !isEnabled;
        if (isEnabled)
            farmingWorld = Constants.getWorld().worldName;

        farmableBlockType = blockType;
        isFarmingBackground = isBackgroundFarming;
        // we always expect player started farming while in world so he have valid position
        farmPosition = Constants.getCurrentPlayerMapPoint();
        currentStage = AutoFarmStages.Stages.PLACE_FARMABLE;
        isWorldCached = false;
        NoAvalibleFarmSpotsFound = false;
    }


    
    /*
     * Handles auto reconnect.
     * Returns true if reconnect IS REQUIRED
     */
    private static bool HandleReconnect()
    {

        // we are connected
        if (PlayerUtils.IsPlayerInWorld()) return false;
        
        // we are currently connecting
        if (PlayerUtils.IsPlayerInLimbo() || PlayerUtils.IsPlayerCheckingGameVerion() ||
            PlayerUtils.IsPlayerJoiningWorld())
        {
            Plugin.Log.LogInfo("Player is RECONNECTING. Force stopping AutoMove");
            AutoMove.Stop();
            currentStage = AutoFarmStages.Stages.IDLE;
            return true;
        }

        // if we are in Main menu
        if (PlayerUtils.IsPlayerInMenus())
        {
            // trying to connect exactly at farm point
            Plugin.Log.LogInfo("Player is IN MAIN MENU. Force stopping AutoMove and Reconnecting");
            SceneLoader.GoFromMainMenuToWorld(farmingWorld, "farm");
            currentStage = AutoFarmStages.Stages.IDLE;
            AutoMove.Stop();
        }

        return true;
    }
    
    
}