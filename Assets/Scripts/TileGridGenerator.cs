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

    public RoomTileGrid Generate()
    {
        // 랜덤으로 장애물 배치 전 작업(좌표 중 랜덤 값 불러오기)
        // 1. 리스트에 좌표 전부 넣기
        List<Vector2Int> allTiles = new List<Vector2Int>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                allTiles.Add(new Vector2Int(x, y));
            }
        }

        int attempts = 0;
        while (attempts < maxAttempts)
        {

            // 2. 리스트 랜덤으로 섞기 (Fisher-Yates 셔플)
            for (int i = width * height - 1; i > 0; i--)
            {
                int randomIndex = UnityEngine.Random.Range(0, i);

                Vector2Int tmp = allTiles[randomIndex];
                allTiles[randomIndex] = allTiles[i];
                allTiles[i] = tmp;
            }

            // 타일 랜덤 배정
            RoomTileGrid roomTile = new RoomTileGrid(width, height);

            int wallCount = Mathf.CeilToInt(width * height * wallRatio);
            int roughCount = Mathf.CeilToInt(width * height * roughRatio);

            for (int i = 0; i < wallCount + roughCount; i++)
            {
                if (i < wallCount)
                {
                    roomTile.SetTile(allTiles[i], TileType.Wall);
                }
                else
                {
                    roomTile.SetTile(allTiles[i], TileType.Rough);
                }

            }

            // 모두 도달할 수 있다면 
            if (roomTile.IsAllReachable())
                return roomTile;

            attempts++;
        }


        throw new InvalidOperationException($"maxAttempts({maxAttempts})번 시도했지만 유효한 지형을 생성하지 못했습니다.");
    }


}
