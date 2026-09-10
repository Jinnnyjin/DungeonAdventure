# DungeonAdventure


<img width="1264" height="708" alt="image" src="https://github.com/user-attachments/assets/6e1d46d7-e46e-47a8-bb26-51d29cc74996" />

랜덤으로 생성되는 던전을 탐험하고 몬스터를 처치하는 2D 탑다운 로그라이크

---

## 1. 프로젝트 개요

| 항목 | 내용 |
|---|---|
| 이름 | Dungeon Adventure |
| 장르 | 2D 탑다운 로그라이크 |
| 개발 기간 | 2026.08.27~2026.09.10 |
| 개발 인원 | 1인 |
| 엔진 | Unity 6000.3.6f1 |

**기획 의도**
- 던전 구조, 방 내부 지형, 몬스터 배치가 매 실행마다 랜덤하게 달라져 재플레이할 때마다 다른 경험을 주는 것을 목표
- 절차적 생성(procedural generation)을 구현하여 재플레이하더라도 단조롭지 않게 느껴지도록 설계

---

## 2. 조작 방법

| 키 | 동작 |
|---|---|
| `W` `A` `S` `D` | 이동 |
| `J` | 공격 |
| `K` | 아이템 줍기 |
| `I` | 인벤토리 열기/닫기 |
| 마우스 좌클릭 | 장비 장착/해제 |
| 마우스 우클릭 | 아이템 버리기 |

---

## 3. 핵심 플레이 흐름

```
Title (직업 선택: 전사 / 궁수)
   ↓
Play (던전 생성 → 방 탐험 → 전투 → 보스방)
   ↓
Result (승리 / 패배)
   ↓
재시작 또는 타이틀로 복귀
```
- 직업 선택
<img width="1383" height="777" alt="image" src="https://github.com/user-attachments/assets/96c5bf2a-3a97-42a5-bf30-1eda0f3284be" />

- 던전
<img width="1367" height="766" alt="image" src="https://github.com/user-attachments/assets/3da37100-bc17-426a-8795-abb019d9c465" />

- 결과 
<img width="1383" height="781" alt="image" src="https://github.com/user-attachments/assets/8ffc3f8b-b824-45da-a6ee-db0f39cf70d5" />

---

## 4. 주요 기능

**직업 & 성장**
- 전사 / 궁수 중 선택, 직업마다 시작 무기·공격 모션이 다름
- 무기·방어구·악세사리 장착 시 스탯(체력/공격력/방어력/이동속도)이 즉시 재계산됨

**던전**
- 실행할 때마다 방 배치와 방 내부 지형이 랜덤 생성 (재시작해도 항상 다른 던전)
- 미탐험 방은 어둡게 가려져 있다가 처음 들어가면 밝아짐
- 거친 지형을 밟으면 이동속도가 느려짐

**전투 & 몬스터**
- 근접/원거리 몬스터가 존재하고, 감지 범위 안에 들어오면 플레이어를 추적·공격
- 플레이어도 장착한 무기에 따라 근접 공격 또는 투사체 공격
- 방에 있는 몬스터를 전부 처치해야 문이 열림 (보스방/보물방은 던전마다 자동 배정)

**아이템**
- 몬스터 처치·보물방에서 가중치 기반 확률로 아이템 드롭
- 인벤토리에서 장착/해제, 우클릭으로 버리기 가능


**인벤토리 및 장비UI**
<img width="1131" height="658" alt="image" src="https://github.com/user-attachments/assets/b8266d82-7ec6-4a17-b51f-094899310974" />


---

## 5. 프로젝트 구조

```
Assets/
├─ Scripts/
│  ├─ GameFlow/        # 씬 전환, 던전 한 판의 시작~종료 사이클
│  ├─ Dungeon/
│  │  ├─ Generation/   # 절차적 생성 (그래프 생성, 타일 배치, BFS 검증)
│  │  └─ Runtime/      # 생성 결과를 씬에 반영 (타일맵, 방 트리거, 문)
│  ├─ Monster/
│  │  ├─ Data/         # 몬스터 스탯 정의 (ScriptableObject)
│  │  └─ Runtime/      # 몬스터 AI, 스폰
│  ├─ Item/            # 인벤토리, 아이템 드롭
│  ├─ Combat/          # 공격 방식 (ScriptableObject 기반)
│  ├─ Player/          # 플레이어 조작·스탯·UI
│  ├─ Events/          # ScriptableObject 이벤트 채널
│  ├─ Core/            # 공용 매니저 (오브젝트 풀링 등)
│  ├─ Common/          # 공용 유틸/인터페이스
│  └─ Debug/           # 개발용 디버그 도구
├─ Datas/              # 몬스터/아이템/공격 ScriptableObject 에셋
├─ Scenes/
│  ├─ Title Scene.unity
│  └─ Stage Scene.unity
└─ Prefabs/
```

### 던전 생성 흐름
<img width="1839" height="1385" alt="image" src="https://github.com/user-attachments/assets/13b26b5c-8848-4dda-93a7-7019519e135c" />

---

## 6. 핵심 로직 설명

### 6-1. 절차적 던전 생성

방 그래프는 랜덤 워크로 생성합니다. 시작 방에서 4방향 중 하나로 한 칸씩 이동하며, 이미 있는 칸이면 연결만 추가하고 없으면 새 방을 만듭니다.
<img width="1583" height="1347" alt="image" src="https://github.com/user-attachments/assets/7f1f7e8a-8d0d-492e-95dc-75a4644affe9" />

방 내부 타일(벽/거친 지형)은 랜덤 배치 후, 문에서 BFS로 모든 칸에 도달 가능한지 검증하고 실패하면 재시도합니다.

### 6-2. 몬스터 추적 (다익스트라 거리장)

플레이어 위치에서 방 전체로 다익스트라를 한 번 계산해 거리장을 만들고, 몬스터들은 그 결과표에서 자기 위치의 값만 조회해 가장 가까운 방향으로 이동합니다.

```csharp
// RoomTileGrid.cs - 거리장 계산
int moveCost = GetTile(nextPos) == TileType.Normal ? NORMAL_COST : ROUGH_COST;
int newDist = distancesBuffer[curPos.x, curPos.y] + moveCost;
if (newDist < distancesBuffer[nextPos.x, nextPos.y])
{
    distancesBuffer[nextPos.x, nextPos.y] = newDist;
    heapBuffer.Enqueue(nextPos, newDist);
}
```

### 6-3. SO 이벤트 채널

모듈 간 직접 참조 대신 ScriptableObject 기반 이벤트 채널로 연결합니다.

<img width="1828" height="701" alt="image" src="https://github.com/user-attachments/assets/d7a18bd7-136c-4b7f-a381-ceb51bb10b4b" />


---

## 7. 적용한 심화 기술과 선택 이유

| 기술 | 무엇을 했나 | 왜 선택했나 |
|---|---|---|
| 절차적 생성 (랜덤 워크 + BFS 검증) | 방 그래프를 랜덤 워크로 만들고, 타일 배치 후 BFS로 도달 가능성 검증 | 매 실행마다 다른 던전 구조를 만들어 재플레이성을 확보하되, 막힌 방이 나오지 않도록 검증 단계를 뒀음 |
| 다익스트라 거리장 (우선순위 큐) | 거친 지형에 이동 비용(가중치)을 둬서, 단순 BFS 대신 `MinHeap` 기반 다익스트라로 최단 비용 경로 계산 | 지형마다 이동 비용이 달라 단순 홉 수 계산(BFS)으로는 정확한 최단 경로를 구할 수 없었음. 플레이어 위치에서 역산해 한 번만 계산하므로 몬스터 수가 늘어도 계산 비용은 그대로 |
| ScriptableObject 이벤트 채널 | 방 진입/클리어, 아이템 획득, 몬스터 처치 등을 SO 기반 이벤트로 발행/구독 | 모듈 간 직접 참조(싱글턴, 강한 결합) 대신 이벤트로 연결해, 리스너를 추가/제거해도 발행하는 쪽 코드를 건드릴 필요가 없도록 설계 |

---

## 8. 성능 개선 내역

**대상**: 몬스터 추적용 다익스트라 거리장 계산 (`RoomTileGrid.ComputeDistanceField`)

**문제 상황**: 매 `FixedUpdate`마다 조건 없이 거리장을 재계산했고, 재계산할 때마다 배열 2개와 힙(MinHeap)을 매번 새로 할당하고 있었습니다.

### 개선 ① 재계산 조건 추가 (빈도 최적화)

쫓아오는 몬스터가 없거나 플레이어가 같은 타일에 머물러 있으면 결과가 동일하므로 재계산을 건너뛰도록 수정했습니다.

| 상태 | 재계산 빈도 (5초당, 실측) |
|---|---|
| 조건 없음 | ~250회 (항상 이론상 최대치) |
| 조건 적용 + 정지 상태 | **0회** |
| 조건 적용 + 이동 중 | ~12회 |

### 개선 ② 버퍼 재사용 (할당 최적화)

조건을 적용한 이후에도, 실제로 재계산이 일어날 때는 여전히 배열/힙을 매번 새로 만들고 있었습니다. 이를 방(`RoomTileGrid`)당 버퍼를 한 번만 만들어 재사용하도록 변경했습니다.

| 상태 | 재계산 1회당 GC 할당 (Unity Profiler 실측, 25×25 방 기준) |
|---|---|
| 버퍼 재사용 전 | 4.8KB |
| 버퍼 재사용 후 | **0B** |

**평균 FPS**: 현재 방 크기·프레임 여유(240대 FPS)에서는 눈에 띄는 변화가 없었습니다. 이는 최적화 대상 자체가 초당 몇 KB, 0.1ms 수준으로 작아 전체 프레임 예산에 비해 미미했기 때문이며, 방 크기가 커지거나 추적 몬스터 수가 늘어날수록 효과가 누적되는 구조적 개선으로 보고 있습니다.

**측정 방법**: 재계산 빈도는 코드에 카운터를 두고 5초마다 콘솔에 로그를 출력해 측정했고, GC 할당량은 Unity Profiler의 CPU Usage → Hierarchy 뷰에서 해당 함수가 호출된 프레임을 찾아 GC Alloc 컬럼을 직접 확인했습니다.

<img width="1784" height="242" alt="image" src="https://github.com/user-attachments/assets/2a65113c-462c-44e2-8242-6666e39390cb" />


---

## 9. 개발 중 문제와 해결 과정

**몬스터 사망 애니메이션 알파값 문제**: 사망 애니메이션 클립이 `SpriteRenderer.color`(알파)를 직접 애니메이트해서, 상태를 Idle로 되돌려도 Animator가 매 프레임 알파 0을 다시 덮어써 몬스터가 계속 투명하게 보이는 문제가 있었습니다. 애니메이션 클립 자체를 수정하는 근본적인 해결은 하지 못했고, `LateUpdate`에서 매 프레임 강제로 원래 색을 재적용하는 방식으로 우회했습니다.

```csharp
// HitFlashEffect.cs
private void LateUpdate()
{
    if (timer > 0f)
    {
        timer -= Time.deltaTime;
        spriteRenderer.color = timer > 0f ? flashColor : originalColor;
        return;
    }
    spriteRenderer.color = originalColor;
}
```

---

## 10. 개선하고 싶은 점

- **던전 생성을 MST(최소 신장 트리) 기반으로 변경**: 현재는 랜덤 워크로 연결성이 형성되는데, 다익스트라와 유사한 우선순위 큐 기반 알고리즘(Prim's MST)으로 바꾸면 최소 비용으로 연결하면서 루프(순환 경로) 추가 여부를 직접 제어할 수 있음
- **설정값을 ScriptableObject로 통합**: 던전 생성 파라미터(씨드, 방 개수, 타일 크기 등)가 `DungeonRunManager`, `DungeonRenderer` 등 여러 컴포넌트에 흩어져 있음. `MonsterData`/`ItemData`처럼 하나의 설정 에셋으로 모으고 싶음
- **몬스터 AI/상태 다양화**: 현재 근접/원거리 2종, Idle/Chase/Attack 3개 상태로 단순함. 패턴이나 특수 공격을 추가하고 싶음
- **방 크기·모양 다양화**: 현재 모든 방이 동일한 크기. 보스방/보물방처럼 특별한 방은 크기도 다르게 생성되도록 확장하고 싶음
