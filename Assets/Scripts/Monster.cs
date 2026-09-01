using UnityEngine;

public class Monster : MonoBehaviour, IDamageable
{
    [SerializeField] private MonsterData monsterData;
    private int curHp;

    private void OnEnable()
    {
        curHp = monsterData.Health;
    }

    public void TakeDamage(int amount)
    {
        curHp -= amount;

        if(curHp <= 0)
        {
            Debug.Log("Die");
        }
    }
}
