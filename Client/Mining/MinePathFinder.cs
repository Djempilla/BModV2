using BasicTypes;
using BModv2.Patches.Impl;

namespace BModv2.Mining;

using System.Collections.Generic;

public static class MinePathFinder
{
    public class PNode
    {
        public int x, y;
        public PNode parent;
        public PNode(int x, int y, PNode parent = null)
        {
            this.x = x;
            this.y = y;
            this.parent = parent;
        }
    }

    static readonly int[] dx = { -1, 1, 0, 0 };
    static readonly int[] dy = {  0, 0, -1, 1 };

    // Неломаемые блоки — через них идти нельзя никак
    static readonly HashSet<int> UnbreakableBlocks = new() { 3988, 3990, 3993 };

    static bool IsWalkable(int x, int y)
    {
        var world = Constants.getWorld();
        if (!world.IsMapPointInWorld(new Vector2i(x, y))) return false;
        var block = world.GetBlockType(new Vector2i(x, y));
        return !ConfigData.doesBlockHaveCollider[(int)block];
    }

    static bool IsBreakable(int x, int y)
    {
        var world = Constants.getWorld();
        if (!world.IsMapPointInWorld(new Vector2i(x, y))) return false;
        var block = world.GetBlockType(new Vector2i(x, y));
        int id = (int)block;

        // Неломаемые — пропускаем
        if (UnbreakableBlocks.Contains(id)) return false;

        // Ломаемо если есть коллайдер (т.е. это твёрдый блок, но не стена)
        return ConfigData.doesBlockHaveCollider[(int)block];
    }

    public static List<PNode> FindPathToExit(Vector2i from, bool walkThroughBlocks = true)
    {
        var world = Constants.getWorld();

        var target = world.GetFirstMapPointOfBlockType(
            World.BlockType.PortalMineExit,
            World.LayerType.Block
        );

        if (target == null)
        {
            Plugin.Log?.LogWarning("[PathFinder] PortalMineExit not found on map");
            return new List<PNode>();
        }

        Plugin.Log?.LogInfo($"[PathFinder] Searching: ({from.x},{from.y}) → exit ({target.x},{target.y})");

        var queue   = new Queue<PNode>();
        var visited = new bool[world.worldSizeX, world.worldSizeY];

        var start = new PNode(from.x, from.y);
        queue.Enqueue(start);
        visited[from.x, from.y] = true;

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (current.x == target.x && current.y == target.y)
            {
                var path = ReconstructPath(current, start);
                Plugin.Log?.LogInfo($"[PathFinder] Path found: {path.Count} steps");
                return path;
            }

            for (int i = 0; i < 4; i++)
            {
                int nx = current.x + dx[i];
                int ny = current.y + dy[i];

                if (nx < 0 || nx >= world.worldSizeX) continue;
                if (ny < 0 || ny >= world.worldSizeY) continue;
                if (visited[nx, ny]) continue;

                // Можно пройти если: свободно ИЛИ можно сломать (если разрешено)
                bool canStep = IsWalkable(nx, ny)
                            || (walkThroughBlocks && IsBreakable(nx, ny));

                if (!canStep) continue;

                visited[nx, ny] = true;
                queue.Enqueue(new PNode(nx, ny, current));
            }
        }

        Plugin.Log?.LogWarning($"[PathFinder] No path found from ({from.x},{from.y}) to ({target.x},{target.y})");
        return new List<PNode>();
    }

    static List<PNode> ReconstructPath(PNode end, PNode start)
    {
        var path = new List<PNode>();
        for (var node = end; node != start; node = node.parent)
            path.Add(node);
        path.Add(start);
        path.Reverse();
        return path;
    }
}