using System.Collections.Generic;
using UnityEngine;

public class RoomRuntimeData
{
    public Room room;
    public List<GameObject> doors;
    public List<GameObject> monsterPrefabs;
    public RoomTileGrid tileGrid;
    public bool isSpawned = false;
    public bool isCleared = false;
}

