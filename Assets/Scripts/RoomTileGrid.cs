using System;
using System.Collections.Generic;
using UnityEngine;

public enum TileType { Normal, Wall, Rough}

public class RoomTileGrid
{
    private TileType[,] tiles;
    public int Width => tiles.GetLength(0);
    public int Height => tiles.GetLength(1);



    public RoomTileGrid(int width, int height)
    {
        tiles = new TileType[width, height];
    }


    public TileType GetTile(Vector2Int pos)
    {
        return tiles[pos.x, pos.y];
    }


    public void SetTile(Vector2Int pos, TileType tile)
    {
        tiles[pos.x, pos.y] = tile;
    }


    public bool IsAllReachable(Vector2Int startPos)
    {
        int nonWallCount = 0; 
        
        // Wall개수 구하기
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                if (GetTile(pos) != TileType.Wall) nonWallCount++;
            }
        }

        // 시작점부터 BFS 돌리기, WALL 제외 모두 닿을 수 있는지
        HashSet<Vector2Int> visited = new HashSet<Vector2Int> ();
        Queue<Vector2Int> position = new Queue<Vector2Int>();

        visited.Add(startPos);
        position.Enqueue(startPos);

        while (position.Count > 0)
        {
            Vector2Int curPos = position.Dequeue();
            
            foreach (Vector2Int dir in GridDirections.Direction)
            {
                Vector2Int nextPos = curPos + dir;
                // 조건 : 그리드 범위 안, wall이 아님, 아직 안가봄 
                bool condition = nextPos.x < Width && nextPos.x >= 0 && nextPos.y < Height && nextPos.y >= 0
                    && GetTile(nextPos)!= TileType.Wall
                    && !visited.Contains(nextPos);

                if(condition)
                {
                    visited.Add(nextPos);
                    position.Enqueue(nextPos);
                }
            }
        }

        return visited.Count == nonWallCount;
    }

    public Vector2Int FindNearestNormalTile(Vector2Int startPos)
    {
        if (GetTile(startPos) == TileType.Normal) return startPos;

        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();

        visited.Add(startPos);
        queue.Enqueue(startPos);

        while (queue.Count > 0)
        {
            Vector2Int curPos = queue.Dequeue();

            foreach (Vector2Int dir in GridDirections.Direction)
            {
                Vector2Int nextPos = curPos + dir;

                bool inBounds = nextPos.x < Width && nextPos.x >= 0 && nextPos.y < Height && nextPos.y >= 0;
                if (!inBounds) continue;
                if (visited.Contains(nextPos)) continue;

                if (GetTile(nextPos) == TileType.Normal)
                {
                    return nextPos;
                }

                visited.Add(nextPos);
                queue.Enqueue(nextPos);
            }

        }

        throw new InvalidOperationException("방 안에 Normal 타일이 존재하지 않습니다.");
    }
}
