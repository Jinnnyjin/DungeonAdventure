using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public RoomEventChannel roomEventChannel;
    public Room EnteringRoom;
    public MonsterSpawner spawner;
    public DungeonRenderer dungeonRenderer;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            roomEventChannel.Raise(EnteringRoom);
            Debug.Log($"방 입장: {EnteringRoom.Id}");

            RoomRuntimeData runtimeData = dungeonRenderer.GetRoomRuntimeData(EnteringRoom.Id);
            if (!runtimeData.isSpawned)
            {
                foreach(GameObject prefab in runtimeData.monsterPrefabs)
                {
                    spawner.SpawnMonster(prefab);
                }
                runtimeData.isSpawned = true;
            }

            
        }
    }
}
