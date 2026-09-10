using System.Threading.Tasks;
using BasicTypes;
using BModv2.Patches.Impl;
using BModv2.Patches.Impl.Utils;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace BModv2.Client.Misc.Commands;

public class GotoCommand
{
    private static AStarPathfinder _pathfinder;

    public static async Task Execute(int x, int y)
    {
        World world = Constants.getWorld();
        if (world == null)
            return;

        if (_pathfinder == null)
        {
            _pathfinder = new AStarPathfinder(world);
            _pathfinder.WalkableIds.Add(World.BlockType.None);
            _pathfinder.WalkableIds.Add(World.BlockType.Grass);
            _pathfinder.WalkableIds.Add(World.BlockType.EntrancePortal);
            _pathfinder.WalkableIds.Add(World.BlockType.Door);
            _pathfinder.WalkableIds.Add(World.BlockType.Tree);
            _pathfinder.WalkableIds.Add(World.BlockType.Portal);
        }

        if (!_pathfinder.HasCachedWorld)
        {
            _pathfinder.CacheWorld();
        }

        Vector2i playerPos = Constants.getCurrentPlayerMapPoint();
        Vector2i targetPos = new Vector2i(x, y);

        Vector2[] path = await Task.Run(() =>
            _pathfinder.Pathfind(
                new Vector2(playerPos.x, playerPos.y),
                new Vector2(targetPos.x, targetPos.y)
            )
        );

        AutoMove.SetPath(path);
    }

    public static void ForceStop()
    {
        
    }
}