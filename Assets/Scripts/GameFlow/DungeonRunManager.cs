using System;
using UnityEngine;

public class DungeonRunManager : MonoBehaviour
{
    [Header("던전 그리드")]
    [SerializeField] private int minRooms;
    [SerializeField] private int maxRooms;
    [SerializeField] private int maxAttempts;
    [SerializeField] private bool useFixedSeed;
    [SerializeField] private int seed;
    [SerializeField] private DungeonRenderer dungeonRenderer;

    [Header("몬스터 스폰")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject meleePrefab;
    [SerializeField] private GameObject rangedPrefab;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private RoomRatio normalRoomRatio;
    [SerializeField] private RoomRatio bossRoomRatio;

    [Header("이벤트 채널")]
    [SerializeField] private RoomEventChannel roomEnteredChannel;

    [Header("플레이어")]
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private ItemData warriorStartWeapon;
    [SerializeField] private ItemData archerStartWeapon;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private RuntimeAnimatorController warriorController;
    [SerializeField] private RuntimeAnimatorController archerController;



    private void Start()
    {
        RunDungeon();
    }


    [ContextMenu("던전 재생성 테스트")]
    public void RunDungeon()
    {
        // 던전 생성기
        DungeonGenerator generator = new DungeonGenerator(minRooms, maxRooms, maxAttempts, useFixedSeed, seed);

        // 그래프 생성
        DungeonGraph graph = generator.Generate();

        // 방 할당
        DungeonTypeAssigner typeAssigner = new DungeonTypeAssigner();
        typeAssigner.AssignBossRoom(graph);
        typeAssigner.AssignTreasureRoom(graph);

        // 던전 그리기
        dungeonRenderer.RenderDungeon(graph);

        // 방마다 몬스터 할당
        MonsterSpawnAssigner spawnAssigner = new MonsterSpawnAssigner(meleePrefab, rangedPrefab, bossPrefab, normalRoomRatio, bossRoomRatio);
        spawnAssigner.AssignMonsters(graph, dungeonRenderer);

        // 시작 방
        Room startRoom = null;
        foreach (var rooms in graph.AllRooms)
        {
            if (rooms.Type == RoomType.Start)
            {
                startRoom = rooms;
                break;
            }
        }

        // 시작 하는 장소
        Vector3 startPoint = dungeonRenderer.GetPlayerSpawnWorldPos(startRoom);
        player.transform.position = startPoint;

        // 스타팅 무기 장착
        ItemData startWeapon = GameSession.SelectedJob == JobType.Warrior ? warriorStartWeapon : archerStartWeapon;
        playerInventory.EquipStartingWeapon(startWeapon);

        // 직업에 맞게 컨트롤러 설정
        playerAnimator.runtimeAnimatorController = GameSession.SelectedJob == JobType.Warrior ? warriorController : archerController;

        roomEnteredChannel.Raise(startRoom);

    }
}
