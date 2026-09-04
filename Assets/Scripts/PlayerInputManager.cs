using UnityEngine;

public class PlayerInputHub : MonoBehaviour
{
    public static PlayerInputHub Instance { get; private set; }
    public PlayerInputActions Actions = new PlayerInputActions();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Actions.Enable();
        }
    }
}
