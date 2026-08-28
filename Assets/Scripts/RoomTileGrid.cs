using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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


    public bool IsAllReachable()
    {
        Vector2Int? startPos = null;
        int nonWallCount = 0; 
        
        // Wall제외 아무 칸 1곳 뽑기 + Wall개수 구하기
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                if(GetTile(pos) != TileType.Wall)
                {
                    nonWallCount++;
                    if (startPos == null)
                    {
                        startPos = pos;
                    }
                }
            }
        }

        if (startPos == null)
        {
            throw new InvalidOperationException("Wall이 아닌 타일이 하나도 없습니다.");
        }

        // 시작점부터 BFS 돌리기, WALL 제외 모두 닿을 수 있는지
        HashSet<Vector2Int> visited = new HashSet<Vector2Int> ();
        Queue<Vector2Int> position = new Queue<Vector2Int>();

        visited.Add(startPos.Value);
        position.Enqueue(startPos.Value);

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
}
