using BasicTypes;
using BModv2.Patches.Impl;
using Il2CppSystem.Collections.Generic;
using Kernys.Bson;
using PlayFab;
using UnityEngine;

namespace BModv2.Client.Impl.Utils;

public class PlayerUtils
{
    public static void PlaceBlock(int x, int y, World.BlockType blockId)
    {
        BSONObject bsonobject = new BSONObject();
        bsonobject["ID"] = "SB";
        bsonobject["x"] = x;
        bsonobject["y"] = y;
        bsonobject["BlockType"] = (int)blockId;
        Constants.getPlayer().myPlayerData.RemoveItemFromInventory(blockId, PlayerData.InventoryItemType.Block);
        OutgoingMessages.AddOneMessageToList(bsonobject);
    }
    
    public static void PlaceBackground(int x, int y, World.BlockType blockId)
    {
        BSONObject bsonobject = new BSONObject();
        bsonobject["ID"] = "SBB";
        bsonobject["x"] = x;
        bsonobject["y"] = y;
        bsonobject["BlockType"] = (int)blockId;
        Constants.getPlayer().myPlayerData.RemoveItemFromInventory(blockId, PlayerData.InventoryItemType.BlockBackground);
        OutgoingMessages.AddOneMessageToList(bsonobject);
    }
    
    
    
    public static void PlaceSeed(int x, int y, World.BlockType blockId)
    {
        BSONObject bsonobject = new BSONObject();
        bsonobject["ID"] = "SS";
        bsonobject["x"] = x;
        bsonobject["y"] = y;
        bsonobject["BlockType"] = (int)blockId;
        Constants.getPlayer().myPlayerData.RemoveItemFromInventory(blockId, PlayerData.InventoryItemType.Seed);
        OutgoingMessages.AddOneMessageToList(bsonobject);
    }
    
    public static short GetItemCount(World.BlockType blockType, PlayerData.InventoryItemType itemType)
    {
        PlayerData.InventoryKey[] inv = Constants.getInventory();
        foreach (var item in inv)
        {
            if (item.blockType == blockType && item.itemType == itemType)
                return Constants.getPlayerData().GetCount(item);
        }
        return 0;
    }

    public static void CollectAllNearby(bool ingoreFullStack = false)
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

            if(ingoreFullStack && cd.amount == 999 && ConfigData.GetIsSeed(cd.blockType)) continue;
            
            if (canPick)
                OutgoingMessages.SendCollectCollectableMessage(cd.id);
        }
    }
    
    public static bool TryGetInventoryKey(World.BlockType blockType, PlayerData.InventoryItemType itemType, 
        out PlayerData.InventoryKey key)
    {
        PlayerData.InventoryKey[] inv = Constants.getInventory();
        foreach (var item in inv)
        {
            if (item.blockType == blockType && item.itemType == itemType)
            {
                key = item;
                return true;
            }
        }
        key = default;
        return false;
    }
    
    // @author Charon Client v1.6
    public static bool TryGetPlayerConnectionStatus(out PlayerConnectionStatus status)
    {
        status = 0;
        bool result;
        try
        {
            NetworkClient networkClient = ControllerHelper.networkClient;
            if (networkClient == null)
            {
                result = false;
            }
            else
            {
                status = networkClient.playerConnectionStatus;
                result = true;
            }
        }
        catch
        {
            result = false;
        }
        return result;
    }
    
    public static bool IsPlayerInWorld()
    {
        PlayerConnectionStatus playerConnectionStatus;
        return TryGetPlayerConnectionStatus(out playerConnectionStatus) && playerConnectionStatus == PlayerConnectionStatus.InRoom;
    }
    
    public static bool IsPlayerInMenus()
    {
        PlayerConnectionStatus playerConnectionStatus;
        return TryGetPlayerConnectionStatus(out playerConnectionStatus) && playerConnectionStatus == PlayerConnectionStatus.InMenus;
    }
    
    public static bool IsPlayerJoiningWorld()
    {
        PlayerConnectionStatus playerConnectionStatus;
        return TryGetPlayerConnectionStatus(out playerConnectionStatus) && playerConnectionStatus == PlayerConnectionStatus.JoiningRoom;
    }
    
    public static bool IsPlayerInLimbo()
    {
        PlayerConnectionStatus playerConnectionStatus;
        return TryGetPlayerConnectionStatus(out playerConnectionStatus) && playerConnectionStatus == PlayerConnectionStatus.InLimbo;
    }
    
    public static bool IsPlayerCheckingGameVerion()
    {
        PlayerConnectionStatus playerConnectionStatus;
        return TryGetPlayerConnectionStatus(out playerConnectionStatus) && playerConnectionStatus == PlayerConnectionStatus.CheckingGameVersion;
    }
    
    
    // ==== NIGGA CHARON CLIENT THANKS FOR INSPIRATION <3<3<3 ====

    public static void ResetPlayerAfterTeleport()
    {
        Player player = Constants.getPlayer();
        
        if(player = null) return;
        
        player.deltaMovement = Vector3.zero;
        player.velocity = Vector3.zero;
        if (player.playerCharacterController2D != null)
        {
            player.playerCharacterController2D.velocity = player.velocity;
        }
        
        player.lastFrameVelocityY = 0f;
        player.leftButton = false;
        player.leftButtonDown = false;
        player.leftButtonUp = false;
        player.rightButton = false;
        player.rightButtonDown = false;
        player.rightButtonUp = false;
        player.jumpButton = false;
        player.jumpButtonDown = false;
        player.jumpButtonUp = false;
        player.useButton = false;
        player.useButtonDown = false;
        player.useButtonUp = false;
        player.isJumpLanding = false;
        player.isDoubleJumpFirstJumpDone = false;
        player.isSwimming = false;
        player.deathByColliderInColliderTimeCounter = 0f;
        
        
        Vector3 velocity = player.velocity;
        velocity.y = -player.gravity * Time.deltaTime;
        player.gravity = 0f;
        player.velocity = velocity;
    }
    
}