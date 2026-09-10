using System;
using System.Collections.Generic;
using UnityEngine;

public enum TileType { Normal, Wall, Rough }

public class RoomTileGrid
{
    private TileType[,] tiles;
    public int Width => tiles.GetLength(0);
    public int Height => tiles.GetLength(1);

    private const int NORMAL_COST = 1;
    private const int ROUGH_COST = 3;
    public const float ROUGH_SPEED_MULTIPLIER = 0.5f;


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
        
        // 1) "이론적으로 도달해야하는 칸" 개수 정하기 => Wall이 아닌 칸 (Normal + Rough 타일)
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                if (GetTile(pos) != TileType.Wall) nonWallCount++;
            }
        }

        // 시작점부터 BFS 돌리기, WALL 제외 모두 닿을 수 있는지

        // 2) BFS 준비
        // 이미 가본 곳 = visited         가야할 곳 = position
        HashSet<Vector2Int> visited = new HashSet<Vector2Int> ();
        Queue<Vector2Int> position = new Queue<Vector2Int>();

        // 3) 시작점 (문 좌표)을 방문 처리 후 대기열
        visited.Add(startPos);
        position.Enqueue(startPos);

        // 4) 대기열이 전부 빌때까지  => 더이상 가 볼 곳이 없음
        while (position.Count > 0)
        {

            // 5) 대기열에서 꺼냄 => FIFO라 가까운칸 먼저
            Vector2Int curPos = position.Dequeue();
            
            // 6) 현재 칸 기준 4방향 모두 확인
            foreach (Vector2Int dir in GridDirections.Direction)
            {
                Vector2Int nextPos = curPos + dir;
                
                // 7) 이웃하는 칸이 갈수 있는 곳인지 확인
                // 조건 : 그리드 범위 안, wall이 아님, 아직 안가봄
                bool condition = nextPos.x < Width && nextPos.x >= 0 && nextPos.y < Height && nextPos.y >= 0
                    && GetTile(nextPos)!= TileType.Wall
                    && !visited.Contains(nextPos);

                // 8) 해당 조건에 통과 => 방문 처리 및 대기열에 추가
                if(condition)
                {
                    visited.Add(nextPos);
                    position.Enqueue(nextPos);
                }
            }
        }

        // 9) 1에서 구한 칸 수와 방문한 칸수가 같다면 고립 구역 없음 판정
        return visited.Count == nonWallCount;
    }

    public Vector2Int FindNearestNormalTile(Vector2Int startPos)
    {
        if (IsSpawnable(startPos)) return startPos;

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

                if (IsSpawnable(nextPos))
                {
                    return nextPos;
                }

                visited.Add(nextPos);
                queue.Enqueue(nextPos);
            }

        }

        throw new InvalidOperationException("방 안에 Normal 타일이 존재하지 않습니다.");
    }

    // 재계산마다 새로 할당하지 않고 재사용하는 버퍼 (방 하나당 한 번만 할당됨)
    private int[,] distancesBuffer;
    private bool[,] visitedBuffer;
    private readonly MinHeap<Vector2Int> heapBuffer = new MinHeap<Vector2Int>();

    // Dijkstra 알고리즘
    public int[,] ComputeDistanceField(Vector2Int fromPos)
    {
        if (distancesBuffer == null)
        {
            distancesBuffer = new int[Width, Height];
            visitedBuffer = new bool[Width, Height];
        }

        // 거리 초기화 -> 무한대로
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                distancesBuffer[x, y] = int.MaxValue;
            }
        }
        Array.Clear(visitedBuffer, 0, visitedBuffer.Length);
        heapBuffer.Clear();

        distancesBuffer[fromPos.x, fromPos.y] = 0;
        heapBuffer.Enqueue(fromPos, 0);

        // 힙이 빌때까지
        while (heapBuffer.Count > 0)
        {
            Vector2Int curPos = heapBuffer.Dequeue();

            // 이미 방문한 노드인지, 아니라면 이제 방문 체크
            if (visitedBuffer[curPos.x, curPos.y]) continue;
            visitedBuffer[curPos.x, curPos.y] = true;

            // 4방향으로 이동
            foreach(var dir in GridDirections.Direction)
            {
                Vector2Int nextPos = curPos + dir;

                bool gridCondition = nextPos.x >= 0 && nextPos.y >= 0 && nextPos.x < Width && nextPos.y < Height;

                // 범위 이내, Wall이 아니면
                if(gridCondition && GetTile(nextPos) != TileType.Wall)
                {
                    int moveCost = GetTile(nextPos) == TileType.Normal ? NORMAL_COST : ROUGH_COST;

                    // new거리 = 현 타일 + 이동비용
                    int newDist = distancesBuffer[curPos.x, curPos.y] + moveCost;
                    if (newDist < distancesBuffer[nextPos.x, nextPos.y])
                    {
                        distancesBuffer[nextPos.x, nextPos.y] = newDist;
                        heapBuffer.Enqueue(nextPos, newDist);
                    }

                }
            }
        }

        return distancesBuffer;
    }

    public bool IsSpawnable(Vector2Int pos)
    {
        return GetTile(pos) == TileType.Normal;
    }
}
