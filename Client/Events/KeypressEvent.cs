using BasicTypes;
using BModv2.Client.Impl.Utils;
using BModv2.Patches.Impl;
using BModv2.Patches.Impl.Utils;
using UnityEngine;

namespace BModv2.Events;

public class KeypressEvent
{
    public bool isInAir = false;
    
    public static void OnKeyPress(KeyCode key)
    {
        

        if (key == KeyCode.O)
        {
            DupeTest.Execute();
        }
        return;
        if (key == KeyCode.LeftArrow)
        {
            if(Constants.getWorld().GetBlockType(new Vector2i(Constants.getCurrentPlayerMapPoint().x - 1,
                   Constants.getCurrentPlayerMapPoint().y)) != World.BlockType.None)
                return;
            Constants.getPlayer().transform.position = WorldUtils.ConvertMapPointToWorldPoint(Constants.getCurrentPlayerMapPoint().x - 1, Constants.getCurrentPlayerMapPoint().y);
        }
        else if (key == KeyCode.RightArrow)
        {
            if(Constants.getWorld().GetBlockType(new Vector2i(Constants.getCurrentPlayerMapPoint().x + 1,
                   Constants.getCurrentPlayerMapPoint().y)) != World.BlockType.None)
                return;
            Constants.getPlayer().transform.position = WorldUtils.ConvertMapPointToWorldPoint(Constants.getCurrentPlayerMapPoint().x + 1, Constants.getCurrentPlayerMapPoint().y);
        }
        else if (key == KeyCode.UpArrow)
        {
            if(Constants.getWorld().GetBlockType(new Vector2i(Constants.getCurrentPlayerMapPoint().x,
                   Constants.getCurrentPlayerMapPoint().y + 1)) != World.BlockType.None)
                return;
            Constants.getPlayer().transform.position = WorldUtils.ConvertMapPointToWorldPoint(Constants.getCurrentPlayerMapPoint().x, Constants.getCurrentPlayerMapPoint().y + 1);
        }
        else if (key == KeyCode.DownArrow)
        {
            if(Constants.getWorld().GetBlockType(new Vector2i(Constants.getCurrentPlayerMapPoint().x,
                Constants.getCurrentPlayerMapPoint().y - 1)) != World.BlockType.None)
                return;
            Constants.getPlayer().transform.position = WorldUtils.ConvertMapPointToWorldPoint(Constants.getCurrentPlayerMapPoint().x, Constants.getCurrentPlayerMapPoint().y - 1);
        }
    }
}