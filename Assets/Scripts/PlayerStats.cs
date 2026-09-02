using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;
    private int curHp;

    private void OnEnable()
    {
        curHp = maxHealth;
    }


    public void TakeDamage(int amount)
    {
        curHp -= amount;
        Debug.Log($"플레이어 데미지 받음: {amount}, 남은 체력{curHp}");

        if (curHp <= 0)
        {
            Debug.Log("Die");
        }
    }
}
