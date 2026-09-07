using UnityEngine;

public class PlayerActionManager : MonoBehaviour
{
    public static PlayerActionManager Instance { get; private set; }
    public PlayerInputActions Actions;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Actions = new PlayerInputActions();
            Actions.Enable();
        }
    }
}
