using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarWorld : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private VoidEventChannel onPlayerStatChangedChannel;
    [SerializeField] private Image fillImage;

    private void Start()
    {
        onPlayerStatChangedChannel.OnEventRaised += RefreshBar;
        RefreshBar();
    }

    private void OnDisable()
    {
        onPlayerStatChangedChannel.OnEventRaised -= RefreshBar;
    }

    private void RefreshBar()
    {
        fillImage.fillAmount = playerStats.CurHp / playerStats.MaxHealth;
    }
}
