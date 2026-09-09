using UnityEngine;

// 피격 시 스프라이트를 지정한 색으로 잠깐 바꿨다가 원래 색으로 되돌리는 이펙트
[RequireComponent(typeof(SpriteRenderer))]
public class HitFlashEffect : MonoBehaviour
{
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float duration = 0.15f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private float timer;

    private void Awake()
    {
        EnsureInitialized();
    }

    // 다른 컴포넌트의 OnEnable이 이 컴포넌트의 Awake보다 먼저 실행될 수 있으므로
    // (같은 오브젝트 내 컴포넌트 순서에 따라 Awake/OnEnable이 컴포넌트 단위로 번갈아 호출됨)
    // 외부에서 호출되는 public 메서드는 항상 초기화를 보장하고 시작한다.
    private void EnsureInitialized()
    {
        if (spriteRenderer != null) return;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    // 오브젝트 풀에서 재사용될 때 호출: 이전 상태(예: 사망 애니메이션이 남긴 알파 0)를 리셋.
    // Animator를 Idle로 되돌리는 코드 "다음"에 명시적으로 호출해야 함 -
    // 순서가 반대면 Animator가 Death 클립의 마지막 프레임(alpha 0)을 다시 덮어씀.
    public void ResetColor()
    {
        EnsureInitialized();
        timer = 0f;
        spriteRenderer.color = originalColor;
    }

    public void Flash()
    {
        EnsureInitialized();
        timer = duration;
        spriteRenderer.color = flashColor;
    }

    // 몬스터 사망 애니메이션 클립이 SpriteRenderer.color를 직접 애니메이트하기 때문에,
    // Idle 등으로 스테이트를 되돌려도 Animator가 매 프레임 alpha를 다시 덮어쓸 수 있다.
    // 그래서 한 번만 리셋하는 게 아니라 매 프레임 원래 색을 강제로 재적용해야 한다.
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
}
