using UnityEngine;

public class PlayerActionManager : MonoBehaviour
{
    public static PlayerActionManager Instance { get; private set; }
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
