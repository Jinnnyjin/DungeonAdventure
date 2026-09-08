using UnityEngine;

public class DoorGate : MonoBehaviour
{
    public Room room;
    public RoomEventChannel roomEnterChannel;
    public RoomEventChannel roomClearChannel;

    private BoxCollider2D boxCollider;
    public RoomRuntimeRegistry roomRegistry;


    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;
    }

    private void Start()
    {
        roomEnterChannel.OnEventRaised += OnRoomEntered;
        roomClearChannel.OnEventRaised += OnRoomCleared;
    }

    private void OnDisable()
    {
        roomEnterChannel.OnEventRaised -= OnRoomEntered;
        roomClearChannel.OnEventRaised -= OnRoomCleared;
    }

    private void OnRoomEntered(Room enteredRoom)
    {
        if(enteredRoom == this.room && !roomRegistry.Get(enteredRoom.Id).isCleared)
        {
            boxCollider.enabled = true;
        }
    }

    private void OnRoomCleared(Room clearedRoom)
    {
        if(clearedRoom == this.room)
        {
            boxCollider.enabled = false;
            roomRegistry.Get(clearedRoom.Id).isCleared = true;
        }
    }
}
