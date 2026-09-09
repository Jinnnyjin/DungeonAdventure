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

    // 정적 필드 초기화 시점에는 LayerMask.NameToLayer 되지않아 최초 접근 시점까지 지연 계산
    private static int playerProjectileLayer = -1;
    private static int monsterProjectileLayer = -1;

    public static int PlayerProjectileLayer
    {
        get
        {
            if (playerProjectileLayer == -1) playerProjectileLayer = LayerMask.NameToLayer("PlayerProjectile");
            return playerProjectileLayer;
        }
    }

    public static int MonsterProjectileLayer
    {
        get
        {
            if (monsterProjectileLayer == -1) monsterProjectileLayer = LayerMask.NameToLayer("MonsterProjectile");
            return monsterProjectileLayer;
        }
    }

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
        bool isMonsterProjectile = gameObject.layer == MonsterProjectileLayer;
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
        IDamageable damageable = collision.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage((int)Damage);
        }

        ReturnToPool();
    }
}
