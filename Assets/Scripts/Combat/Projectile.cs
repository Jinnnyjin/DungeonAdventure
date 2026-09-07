using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject SourcePrefab;
    public Vector2 MoveDir;
    public float MoveSpeed;
    public float Damage;
    public Transform Attacker;
    [SerializeField] private float maintainTime;

    private float spawnedTime;
    private Rigidbody2D rb;
    private bool isReturned;


    private void OnEnable()
    {
        spawnedTime = Time.time;
        rb = GetComponent<Rigidbody2D>();
        isReturned = false;
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
