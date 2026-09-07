using System.Collections.Generic;
using UnityEngine;

public class RoomRuntimeData
{
    // 기존 방 콜라이더 담을 오브젝트
    public GameObject roomObject;


    public Room room;
    public List<GameObject> doors;
    public List<GameObject> monsterPrefabs;
    public RoomTileGrid tileGrid;
    public bool isSpawned = false;
    public bool isLooted = false;
    public bool isCleared = false;
    public int[,] distanceField;
    public List<Monster> spawnedMonsters;
}

