using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour, IDamageable
{
    private enum MonsterState { Idle, Chase, Attack }

    [SerializeField] private float deathAnimationDuration = 1f;
    [SerializeField] private MonsterData monsterData;
    public RoomRuntimeData runtimeData;
    public DungeonCoordinateConverter coordinateConverter;
    public RoomEventChannel roomClearChannel;
    public MonsterSpawner spawner;
    public GameObject sourcePrefab;
    public Transform playerTransform;
    public OnMonsterKilledChannel onMonsterKilledChannel;

    private Rigidbody2D rb;
    private int curHp;
    private bool isDead;
    private float lastAttackTime;
    private MonsterState curState;
    private Animator monsterAnimator;
    private SpriteRenderer spriteRenderer;
    private HitFlashEffect hitFlashEffect;

    private void OnEnable()
    {
        curHp = monsterData.Health;
        rb = GetComponent<Rigidbody2D>();
        monsterAnimator = GetComponent<Animator>();
        monsterAnimator.SetFloat("ChasingSpeed", monsterData.MoveSpeed);

        isDead = false;
        monsterAnimator.ResetTrigger("Died");
        monsterAnimator.Play("Idle", 0, 0f);

        spriteRenderer = GetComponent<SpriteRenderer>();
        hitFlashEffect = GetComponent<HitFlashEffect>();
        hitFlashEffect.ResetColor();

        curState = MonsterState.Idle;

    }

    private void UpdateFacing()
    {
        float dx = playerTransform.position.x - transform.position.x;
        if (dx > 0) spriteRenderer.flipX = false;
        else if (dx < 0) spriteRenderer.flipX = true;
    }


    private void FixedUpdate()
    {
        if (isDead) return;

        // 상태 판단
        float distance = Vector3.Distance(playerTransform.position, rb.position);
        if (distance > monsterData.DetectionRange)
        {
            curState = MonsterState.Idle;
        }
        else if (distance > monsterData.AttackBehavior.AttackRange)
        {
            curState = MonsterState.Chase;
        }
        else { curState = MonsterState.Attack; }

        // 상태 별 행동 분기

        switch (curState)
        {
            case MonsterState.Idle:
                rb.linearVelocity = Vector2.zero;
                monsterAnimator.SetBool("Move", false);
                return;

            case MonsterState.Attack:
                rb.linearVelocity = Vector2.zero;
                monsterAnimator.SetBool("Move", false);
                UpdateFacing();
                TryAttack();
                return;

            case MonsterState.Chase:
                monsterAnimator.SetBool("Move", true);
                UpdateFacing();
                Chase();
                return;
        }
    }

    private void TryAttack()
    {
        if (Time.time - lastAttackTime >= monsterData.AttackBehavior.Cooldown)
        {
            monsterData.AttackBehavior.Attack(transform, playerTransform);
            monsterAnimator.SetTrigger("EnemyAttack");
            lastAttackTime = Time.time;
        }
    }

    private void Chase()
    {
        if (runtimeData == null || runtimeData.distanceField == null) return;

        Vector2Int localPos = coordinateConverter.GetLocalPos(runtimeData.room, transform.position);
        bool selfInBounds = localPos.x >= 0 && localPos.y >= 0
        && localPos.x < runtimeData.tileGrid.Width && localPos.y < runtimeData.tileGrid.Height;
        if (!selfInBounds) return;

        Vector2Int bestDir = Vector2Int.zero;
        int bestDist = runtimeData.distanceField[localPos.x, localPos.y];

        foreach (Vector2Int dir in GridDirections.Direction)
        {
            Vector2Int nextPos = localPos + dir;

            bool inBounds = nextPos.x >= 0 && nextPos.y >= 0
            && nextPos.x < runtimeData.tileGrid.Width && nextPos.y < runtimeData.tileGrid.Height;

            if (!inBounds) continue;

            int dist = runtimeData.distanceField[nextPos.x, nextPos.y];
            if (dist < bestDist)
            {
                bestDist = dist;
                bestDir = dir;
            }
        }

        float speedMultiplier = runtimeData.tileGrid.GetTile(localPos) == TileType.Rough
                                ? RoomTileGrid.ROUGH_SPEED_MULTIPLIER : 1f;

        Vector2 velocity = new Vector2(bestDir.x, bestDir.y).normalized * monsterData.MoveSpeed * speedMultiplier;

        rb.linearVelocity = velocity;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        hitFlashEffect.Flash();

        curHp -= amount;

        if (curHp <= 0)
        {
            isDead = true;
            rb.linearVelocity = Vector2.zero;
            monsterAnimator.SetTrigger("Died");
        }

        if (curHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // 미리 저장
        MonsterData data = monsterData;
        Vector3 pos = transform.position;

        // 방의 spawnedMonsters에서 자신 제거
        runtimeData.spawnedMonsters.Remove(this);

        // 보스를 처치했다면 남은 몬스터와 상관없이 즉시 클리어, 그 외엔 전멸 시 클리어
        if (data.IsBoss || runtimeData.spawnedMonsters.Count == 0)
        {
            roomClearChannel.Raise(runtimeData.room);
        }

        StartCoroutine(ReleaseAfterDeathAnim(data, pos));
    }

    private IEnumerator ReleaseAfterDeathAnim(MonsterData data, Vector3 pos)
    {
        yield return new WaitForSeconds(deathAnimationDuration);

        // 오브젝트 풀에 반납 (SetActive(false) + spawner.ReleaseMonster)
        spawner.ReleaseMonster(sourcePrefab, this);

        // 이벤트 발행
        onMonsterKilledChannel.Raise(new MonsterDeathInfo { MonsterData = data, Position = pos });
    }
}
