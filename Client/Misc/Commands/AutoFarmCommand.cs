using System.Threading.Tasks;
using BasicTypes;
using BModv2.Client.Farming;
using BModv2.Patches.Impl;
using BModv2.Patches.Impl.Utils;
using UnityEngine;

namespace BModv2.Client.Misc.Commands
{
    public class AutoFarmCommand
    {
        public static void Execute(bool farmBg)
        {

            World world = Constants.getWorld();
            if (world == null)
            {
                Plugin.Log?.LogInfo("[AutoFarm] You must be in a world to use this command.");
                return;
            }

            Vector2i cursorPos = WorldUtils.ConvertWorldPointToMapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            World.BlockType blockToFarm = world.GetBlockType(cursorPos.x, cursorPos.y);

            if (blockToFarm == World.BlockType.None)
            {
                Plugin.Log?.LogInfo("[AutoFarm] Please aim at a block/tree to farm.");
                return;
            }

            AutoFarm.onToggle(blockToFarm, farmBg);
        }

    }
}