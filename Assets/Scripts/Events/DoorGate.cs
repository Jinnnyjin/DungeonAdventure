using UnityEngine;

public class DoorGate : MonoBehaviour
{
    public Room room;
    public RoomEventChannel roomEnterChannel;
    public RoomEventChannel roomClearChannel;

    private BoxCollider2D boxCollider;


    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
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
        if(enteredRoom == this.room)
        {
            boxCollider.enabled = true;
        }
    }

    private void OnRoomCleared(Room clearedRoom)
    {
        if(clearedRoom == this.room)
        {
            boxCollider.enabled = false;
        }
    }
}
