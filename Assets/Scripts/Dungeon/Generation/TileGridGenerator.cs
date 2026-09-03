using System;
using System.Collections.Generic;
using UnityEngine;

public class TileGridGenerator
{
    private readonly int width;
    private readonly int height;
    private readonly int maxAttempts;
    private readonly float wallRatio;
    private readonly float roughRatio;

    public TileGridGenerator(int width, int height, int maxAttempts, float wallRatio, float roughRatio)
    {
        this.width = width;
        this.height = height;
        this.maxAttempts = maxAttempts;
        this.wallRatio = wallRatio;
        this.roughRatio = roughRatio;
    }

    public RoomTileGrid Generate(List<Vector2Int> doorPositions)
    {
        // 랜덤으로 장애물 배치 전 작업(좌표 중 랜덤 값 불러오기)
        // 1. 리스트에 좌표 전부 넣기 (벽 제외)
        List<Vector2Int> innerTiles = new List<Vector2Int>();
        for (int x = 1; x < width -1 ; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                innerTiles.Add(new Vector2Int(x, y));
            }
        }

        int attempts = 0;
        while (attempts < maxAttempts)
        {
            RoomTileGrid roomTile = new RoomTileGrid(width, height);

            // 벽 중에 door 제외
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    bool isBorder = x == 0 || x == width - 1
                        || y == 0 || y == height - 1;

                    if(isBorder)
                    {
                        Vector2Int pos = new Vector2Int(x, y);
                        if(!doorPositions.Contains(pos))
                        {
                            // 문이 아닌 것들만 wall 타입 tile로
                            roomTile.SetTile(pos, TileType.Wall);
                        }
                    }
                }
            }

            // 2. 리스트 랜덤으로 섞기 (Fisher-Yates 셔플)
            for (int i = innerTiles.Count -1 ; i > 0; i--)
            {
                int randomIndex = UnityEngine.Random.Range(0, i+1);

                Vector2Int tmp = innerTiles[randomIndex];
                innerTiles[randomIndex] = innerTiles[i];
                innerTiles[i] = tmp;
            }

            int wallCount = Mathf.CeilToInt(innerTiles.Count * wallRatio);
            int roughCount = Mathf.CeilToInt(innerTiles.Count * roughRatio);

            for (int i = 0; i < wallCount + roughCount; i++)
            {
                if (i < wallCount)
                {
                    roomTile.SetTile(innerTiles[i], TileType.Wall);
                }
                else
                {
                    roomTile.SetTile(innerTiles[i], TileType.Rough);
                }

            }

            // 첫번째 문에서부터 모두 도달할수있다면 (다른 문은 nonWall에 포함되어있기에 )
            if (roomTile.IsAllReachable(doorPositions[0]))
                return roomTile;

            attempts++;
        }


        throw new InvalidOperationException($"maxAttempts({maxAttempts})번 시도했지만 유효한 지형을 생성하지 못했습니다.");
    }


}
