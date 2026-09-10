using System;
using System.Collections.Generic;
using System.Numerics;
using BasicTypes;
using BModv2;
using BModv2.Patches.Impl;

public class AStarPathfinder
{
    private readonly World world;

    public int MinX { get; set; } = 0;
    public int MaxX { get; set; } = 79;
    public int MinY { get; set; } = 3;
    public int MaxY { get; set; } = 59;

    public HashSet<World.BlockType> WalkableIds { get; } = new HashSet<World.BlockType>();

    public bool HasCachedWorld { get; private set; }

    private World.BlockType[,] cachedBlocks;

    public AStarPathfinder(World world)
    {
        this.world = world;
        Plugin.Log.LogInfo("[AStarPathfinder] Created pathfinder instance");
    }

    public void CacheWorld()
    {
        int width = MaxX - MinX + 1;
        int height = MaxY - MinY + 1;

        Plugin.Log.LogInfo($"[AStarPathfinder] Caching world area X:{MinX}-{MaxX}, Y:{MinY}-{MaxY}, Size:{width}x{height}");

        cachedBlocks = new World.BlockType[width, height];

        for (int y = MinY; y <= MaxY; y++)
        {
            for (int x = MinX; x <= MaxX; x++)
            {
                cachedBlocks[x - MinX, y - MinY] = world.GetBlockType(new Vector2i(x, y));
            }
        }

        HasCachedWorld = true;
        Plugin.Log.LogInfo("[AStarPathfinder] World cache built");
    }

    public void InvalidateCache()
    {
        cachedBlocks = null;
        HasCachedWorld = false;
        Plugin.Log.LogInfo("[AStarPathfinder] World cache invalidated");
    }

    public World.BlockType GetCachedBlock(int x, int y)
    {
        if (!HasCachedWorld)
            throw new InvalidOperationException("World cache is not built.");

        if (!IsInside(x, y))
            throw new ArgumentOutOfRangeException();

        return cachedBlocks[x - MinX, y - MinY];
    }

    public void SetCachedBlock(int x, int y, World.BlockType blockType)
    {
        if (!HasCachedWorld)
            throw new InvalidOperationException("World cache is not built.");

        if (!IsInside(x, y))
            throw new ArgumentOutOfRangeException();

        cachedBlocks[x - MinX, y - MinY] = blockType;
        Plugin.Log.LogInfo($"[AStarPathfinder] Cached block updated at ({x},{y}) -> {blockType}");
    }

    public void SetCachedBlocks(IEnumerable<(int x, int y, World.BlockType blockType)> changes)
    {
        if (!HasCachedWorld)
            throw new InvalidOperationException("World cache is not built.");

        int applied = 0;
        int skipped = 0;

        foreach (var change in changes)
        {
            if (!IsInside(change.x, change.y))
            {
                skipped++;
                continue;
            }

            cachedBlocks[change.x - MinX, change.y - MinY] = change.blockType;
            applied++;
        }

        Plugin.Log.LogInfo($"[AStarPathfinder] Batch cache update applied={applied}, skipped={skipped}");
    }

    public Vector2[] Pathfind(Vector2 from, Vector2 to)
    {
        if (!HasCachedWorld)
            throw new InvalidOperationException("Call CacheWorld() before Pathfind().");

        int startX = (int)Math.Round(from.X);
        int startY = (int)Math.Round(from.Y);
        int goalX = (int)Math.Round(to.X);
        int goalY = (int)Math.Round(to.Y);

        Plugin.Log.LogInfo($"[AStarPathfinder] Pathfind requested from ({startX},{startY}) to ({goalX},{goalY})");

        if (!IsInside(startX, startY) || !IsInside(goalX, goalY))
        {
            Plugin.Log.LogWarning("[AStarPathfinder] Pathfind aborted: start or goal is outside cache bounds");
            return Array.Empty<Vector2>();
        }

        if (!IsWalkable(startX, startY))
        {
            Plugin.Log.LogWarning($"[AStarPathfinder] Pathfind aborted: start point ({startX},{startY},{world.GetBlockType(startX, startY)}) is not walkable");
            return Array.Empty<Vector2>();
        }

        if (!IsWalkable(goalX, goalY))
        {
            Plugin.Log.LogWarning($"[AStarPathfinder] Pathfind aborted: goal point ({goalX},{goalY},{world.GetBlockType(goalX, goalY)}) is not walkable");
            return Array.Empty<Vector2>();
        }

        var openQueue = new PriorityQueue<Node, int>();
        var allNodes = new Dictionary<(int x, int y), Node>();
        var closed = new HashSet<(int x, int y)>();

        var startNode = new Node(startX, startY)
        {
            G = 0,
            H = Heuristic(startX, startY, goalX, goalY),
            Parent = null
        };

        allNodes[(startX, startY)] = startNode;
        openQueue.Enqueue(startNode, startNode.F);

        while (openQueue.Count > 0)
        {
            Node current = openQueue.Dequeue();

            if (closed.Contains((current.X, current.Y)))
                continue;

            if (current.X == goalX && current.Y == goalY)
            {
                Vector2[] path = BuildPath(current);
                Plugin.Log.LogInfo($"[AStarPathfinder] Path found. Length={path.Length}");
                return path;
            }

            closed.Add((current.X, current.Y));

            foreach (var (nx, ny) in GetNeighbors(current.X, current.Y))
            {
                if (!IsInside(nx, ny))
                    continue;

                if (!IsWalkable(nx, ny))
                    continue;

                if (closed.Contains((nx, ny)))
                    continue;

                int tentativeG = current.G + 1;

                if (!allNodes.TryGetValue((nx, ny), out Node neighbor))
                {
                    neighbor = new Node(nx, ny);
                    allNodes[(nx, ny)] = neighbor;
                }

                if (neighbor.Parent == null && !(nx == startX && ny == startY))
                {
                    neighbor.G = tentativeG;
                    neighbor.H = Heuristic(nx, ny, goalX, goalY);
                    neighbor.Parent = current;
                    openQueue.Enqueue(neighbor, neighbor.F);
                }
                else if (tentativeG < neighbor.G)
                {
                    neighbor.G = tentativeG;
                    neighbor.Parent = current;
                    openQueue.Enqueue(neighbor, neighbor.F);
                }
            }
        }

        Plugin.Log.LogWarning("[AStarPathfinder] Path not found");
        return Array.Empty<Vector2>();
    }

    private bool IsInside(int x, int y)
    {
        return x >= MinX && x <= MaxX && y >= MinY && y <= MaxY;
    }

    private bool IsWalkable(int x, int y)
    {
        World.BlockType blockType = cachedBlocks[x - MinX, y - MinY];
        
        if (!WalkableIds.Contains(blockType))
            return false;
        
        return true;
    }

    private IEnumerable<(int x, int y)> GetNeighbors(int x, int y)
    {
        yield return (x + 1, y);
        yield return (x - 1, y);
        yield return (x, y + 1);
        yield return (x, y - 1);
    }

    private int Heuristic(int x1, int y1, int x2, int y2)
    {
        return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
    }

    private Vector2[] BuildPath(Node endNode)
    {
        var result = new List<Vector2>();
        Node current = endNode;

        while (current != null)
        {
            result.Add(new Vector2(current.X, current.Y));
            current = current.Parent;
        }

        result.Reverse();
        return result.ToArray();
    }

    private class Node
    {
        public int X;
        public int Y;
        public int G = int.MaxValue;
        public int H;
        public int F => G + H;
        public Node Parent;

        public Node(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}