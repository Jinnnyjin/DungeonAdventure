using System.Collections.Generic;
using UnityEngine;

public class RoomDecorationPlacer : MonoBehaviour
{
    private readonly List<GameObject> decorationPrefabs;
    private readonly float density;
    private readonly int minCount;
    private readonly int maxCount;
    private readonly int minDistance;

    public RoomDecorationPlacer(List<GameObject> decorationPrefabs, float desity,  int minCount, int maxCount, int minDistance)
    {
        this.decorationPrefabs = decorationPrefabs;
        this.density = desity;
        this.minCount = minCount;
        this.maxCount = maxCount;
        this.minDistance = minDistance;
    }

    public List<(Vector2Int pos, GameObject prefab)> GetDecorations(RoomTileGrid tileGrid)
    {
        int area = tileGrid.Width * tileGrid.Height;
        int count = Mathf.Clamp(Mathf.RoundToInt(area * density), minCount, maxCount);

        RoomPositionSelector selector = new RoomPositionSelector();
        List<Vector2Int> pos = selector.CalculateSpawnPositions(tileGrid, count, minDistance);

        List<(Vector2Int pos, GameObject prefab)> result = new List<(Vector2Int, GameObject)>();

        for (int i = 0; i < count; i++)
        {
            GameObject prefab = decorationPrefabs[Random.Range(0, decorationPrefabs.Count)];
            result.Add((pos[i], prefab));
        }

        return result;
    }
}
