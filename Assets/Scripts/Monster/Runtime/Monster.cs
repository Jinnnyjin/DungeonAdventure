using UnityEngine;

public class Monster : MonoBehaviour, IDamageable
{
    private enum MonsterState { Idle, Chase, Attack }

    [SerializeField] private MonsterData monsterData;
    public RoomRuntimeData runtimeData;
    public DungeonRenderer dungeonRenderer;
    public MonsterSpawner spawner;
    public GameObject sourcePrefab;
    public Transform playerTransform;

    private Rigidbody2D rb;
    private int curHp;
    private float lastAttackTime;
    private MonsterState curState;

    private void OnEnable()
    {
        curHp = monsterData.Health;
        rb = GetComponent<Rigidbody2D>();
        curState = MonsterState.Idle;
    }


    private void FixedUpdate()
    {
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
                return;

            case MonsterState.Attack:
                rb.linearVelocity = Vector2.zero;
                TryAttack();
                return;

            case MonsterState.Chase:
                Chase();
                return;
        }
    }

    private void TryAttack()
    {
        if (Time.time - lastAttackTime >= monsterData.AttackBehavior.Cooldown)
        {
            monsterData.AttackBehavior.Attack(transform, playerTransform);
            lastAttackTime = Time.time;
        }
    }

    private void Chase()
    {
        if (runtimeData == null || runtimeData.distanceField == null) return;

        Vector2Int localPos = dungeonRenderer.GetLocalPos(runtimeData.room, transform.position);
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

        Vector2 velocity = new Vector2(bestDir.x, bestDir.y).normalized * monsterData.MoveSpeed;
        rb.linearVelocity = velocity;
    }

    public void TakeDamage(int amount)
    {
        curHp -= amount;
        Debug.Log($"플레이어 -> 몬스터 공격, 남은 HP: {curHp}");

        if (curHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // 방의 spawnedMonsters에서 자신 제거
        runtimeData.spawnedMonsters.Remove(this);

        // 제거 후 리스트가 비었으면 → 방 클리어 이벤트 발행
        if (runtimeData.spawnedMonsters.Count == 0)
        {
            dungeonRenderer.roomClearChannel.Raise(runtimeData.room);
        }

        // 오브젝트 풀에 반납 (SetActive(false) + spawner.ReleaseMonster)
        spawner.ReleaseMonster(sourcePrefab, this);
    }
}
