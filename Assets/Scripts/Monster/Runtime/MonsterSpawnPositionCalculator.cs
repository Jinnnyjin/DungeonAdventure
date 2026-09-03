using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnPositionCalculator
{
    public List<Vector2Int> CalculateSpawnPositions(RoomTileGrid tileGrid, int count, int minDistance)
    {
        // 노멀 좌표 모두 리스트에 넣기
        List<Vector2Int> normalTiles = new List<Vector2Int>();

        for (int x = 0; x < tileGrid.Width; x++)
        {
            for (int y = 0; y < tileGrid.Height; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                if (tileGrid.IsSpawnable(pos))
                {
                    normalTiles.Add(pos);
                }
            }
        }

        // Fisher-Yates 셔플
        for (int i = normalTiles.Count - 1; i > 0; i --)
        {
            int randomIndex = Random.Range(0, i+1);

            Vector2Int tmp = normalTiles[randomIndex];
            normalTiles[randomIndex] = normalTiles[i];
            normalTiles[i] = tmp;
        }

        // 타 몬스터와 거리 확인 (최소 거리 유지)
        List<Vector2Int> selected = new List<Vector2Int>();

        foreach (Vector2Int pos in normalTiles)
        {
            if (selected.Count >= count) break;

            bool isPass = true;

            // 이미 선택된 좌표랑 거리 확인
            foreach (Vector2Int picked in selected)
            {
                int distance = Mathf.Abs(pos.x - picked.x) + Mathf.Abs(pos.y - picked.y);

                // 거리가 최소보다 작으면
                if( distance < minDistance)
                {
                    isPass = false;
                    break;
                }
            }
            // 조건이 괜찮다면 selected에 추가
            if(isPass)
            {
                selected.Add(pos);
            }
        }

        // 혹시나 Count만큼 좌표를 모으지 못했다면
        if (count == selected.Count) return selected;

        foreach (Vector2Int pos in normalTiles)
        {
            if (selected.Contains(pos)) continue;

            selected.Add(pos);

            if (count == selected.Count) break;
        }

        return selected;
    }
}
