using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    [SerializeField] private float baseMaxHealth = 100;
    [SerializeField] private float baseAttack;
    [SerializeField] private float baseDefense;
    [SerializeField] private float baseMoveSpeed;

    private float maxHealth;
    private float curHp;
    private float attack;
    private float defense;
    private float moveSpeed;

    private Inventory inventory;
    [SerializeField] private VoidEventChannel onItemEquippedChannel;
    [SerializeField] private VoidEventChannel onItemUnequippedChannel;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
    }

    private void OnEnable()
    {
        onItemEquippedChannel.OnEventRaised += RecalculateStats;
        onItemUnequippedChannel.OnEventRaised += RecalculateStats;

        RecalculateStats();
        curHp = maxHealth;

        Debug.Log($"[초기화 완료] 체력: {curHp}/{maxHealth}");
    }

    private void OnDisable()
    {
        onItemEquippedChannel.OnEventRaised -= RecalculateStats;
        onItemUnequippedChannel.OnEventRaised -= RecalculateStats;
    }


    public void RecalculateStats()
    {
        // 체력
        float newMaxHp = baseMaxHealth + inventory.SumModifiers(StatType.Health);

        curHp = Mathf.Min(curHp, newMaxHp);
        maxHealth = newMaxHp;

        // 공격력
        attack = baseAttack + inventory.SumModifiers(StatType.Attack);

        // 방어력
        defense = baseDefense + inventory.SumModifiers(StatType.Defense);

        // 이동속도
        moveSpeed = baseMoveSpeed * ( 1 + inventory.SumModifiers(StatType.Speed) ) ;

        Debug.Log($"체력: {curHp}/{maxHealth}, 공격력: {attack}, 방어력: {defense}, 이동속도: {moveSpeed}");
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
