using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Projectile : MonoBehaviour
{
    public GameObject SourcePrefab;
    public Vector2 MoveDir;
    public float MoveSpeed;
    public float Damage;
    public Transform Attacker;
    [SerializeField] private float maintainTime;
    [SerializeField] private Color monsterProjectileColor = Color.red;

    public static readonly List<Projectile> Active = new List<Projectile>();

    private float spawnedTime;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isReturned;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    private void OnEnable()
    {
        spawnedTime = Time.time;
        rb = GetComponent<Rigidbody2D>();
        isReturned = false;
        Active.Add(this);
    }

    // 몬스터/플레이어 여부에 따라 레이어가 정해진 몬스터 투사체는 구분되는 색으로 표시
    public void ApplyColorByLayer()
    {
        bool isMonsterProjectile = gameObject.layer == LayerMask.NameToLayer("MonsterProjectile");
        spriteRenderer.color = isMonsterProjectile ? monsterProjectileColor : originalColor;
    }

    private void OnDisable()          
    {
        Active.Remove(this);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = MoveDir * MoveSpeed;

        if(Time.time - spawnedTime >= maintainTime)
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        if (isReturned) return;
        isReturned = true;

        ObjectPoolManager.Instance.Release<Projectile>(SourcePrefab, this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log($"투사체가 부딪힌 대상: {collision.gameObject.name}");

        IDamageable damageable = collision.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage((int)Damage);
        }

        ReturnToPool();
    }
}
