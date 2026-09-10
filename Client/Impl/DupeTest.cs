using BasicTypes;

namespace BModv2.Patches.Impl;

public class DupeTest
{
    public static void Execute()
    {
        // 59 40
        
        PlayerData.InventoryKey[] inv = Constants.getInventory();
        
        // OutgoingMessages.SendTakeItemsFromBankMessage(new Vector2i(59, 40), new PlayerData.InventoryKey(World.BlockType.ExtraDropFeather), int.MaxValue, 0);

        foreach (PlayerData.InventoryKey invKey in inv)
        {
            if (invKey.blockType == World.BlockType.FamiliarBunny2A)
            {
                OutgoingMessages.SendTryToJoinRandomMessage();
            }
            
        }
        
    }
}