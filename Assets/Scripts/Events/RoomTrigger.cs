using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public RoomEventChannel roomEventChannel;
    public Room EnteringRoom;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            roomEventChannel.Raise(EnteringRoom);
            Debug.Log($"방 입장: {EnteringRoom.Id}");
        }
    }
}
