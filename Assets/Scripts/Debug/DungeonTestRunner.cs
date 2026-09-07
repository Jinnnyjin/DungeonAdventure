using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class DungeonTestRunner : MonoBehaviour
{
    [Header("던전 그리드")]
    [SerializeField] private DungeonRenderer dungeonRenderer;

    [Header("이벤트")]
    public RoomEventChannel roomEnteredChannel;

    private Room currentRoom;

    private void OnEnable()
    {
        roomEnteredChannel.OnEventRaised += OnRoomEntered;
    }

    private void OnDisable()
    {
        roomEnteredChannel.OnEventRaised -= OnRoomEntered;
    }

    private void OnRoomEntered(Room room)
    {
        currentRoom = room;
        Debug.Log(currentRoom);
    }

    private void Update()
    {
        if(Keyboard.current.fKey.wasPressedThisFrame)
        {
            RoomRuntimeData runtimeData = dungeonRenderer.GetRoomRuntimeData(currentRoom.Id);

            // 순회 중에 리스트에서 제거되니 역순으로 순회하거나 복사본을 만들어야 함
            List<Monster> monstersCopy = new List<Monster>(runtimeData.spawnedMonsters);
            foreach (Monster monster in monstersCopy)
            {
                monster.TakeDamage(9999);
            }
        }

        if (Keyboard.current.gKey.wasPressedThisFrame)
        {
            RoomTileGrid tileGrid = dungeonRenderer.GetRoomRuntimeData(currentRoom.Id).tileGrid;
            Vector2Int testPos = new Vector2Int(tileGrid.Width / 2, tileGrid.Height / 2);
            int[,] distances = tileGrid.ComputeDistanceField(testPos);

            for (int y = tileGrid.Height - 1; y >= 0; y--)
            {
                string row = "";
                for (int x = 0; x < tileGrid.Width; x++)
                {
                    int d = distances[x, y];
                    row += (d == int.MaxValue ? "#" : d.ToString()) + "\t";
                }
                Debug.Log(row);
            }
        }
    }
}
