using BasicTypes;
using BModv2.Client.Impl.Utils;
using BModv2.Patches.Impl;

namespace BModv2.Patches;

public class PlaceCommand
{
    public static void Execute(World.BlockType blockToPlace)
    {
        for (int w = -2; w < 3; w++)
        {
            for (int h = -2; h < 3; h++)
            {
                Vector2i myPos = Constants.getCurrentPlayerMapPoint();
                Vector2i placePos =  new Vector2i(myPos.x + w, myPos.y + h);

                World.BlockType blockType = Constants.getWorld().GetBlockType(placePos);
                
                // only placing on air
                if(blockType != 0) continue;
                
                PlayerUtils.PlaceBlock(placePos.x, placePos.y, blockToPlace);
            }
        }
    }
}