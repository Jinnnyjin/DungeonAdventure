using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private const string MasterVolumeParam = "MasterVolume";
    private const string BGMVolumeParam = "BGMVolume";
    private const string SFXVolumeParam = "SFXVolume";

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip bgmClip;

    [Header("이벤트 채널")]
    [SerializeField] private VoidEventChannel onPlayerAttackChannel;
    [SerializeField] private VoidEventChannel onPlayerHitChannel;
    [SerializeField] private VoidEventChannel onPlayerDeadChannel;
    [SerializeField] private VoidEventChannel onGameWinChannel;
    [SerializeField] private VoidEventChannel onDungeonStartChannel;
    [SerializeField] private MonsterEventChannel onMonsterAttackChannel;
    [SerializeField] private MonsterEventChannel onMonsterHitChannel;
    [SerializeField] private MonsterDeathInfoEventChannel onMonsterKilledChannel;

    [Header("공용 사운드 클립")]
    [SerializeField] private AudioClip playerAttackClip;
    [SerializeField] private AudioClip playerHitClip;
    [SerializeField] private AudioClip gameOverClip;
    [SerializeField] private AudioClip victoryClip;
    [SerializeField] private AudioClip dungeonStartClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (bgmClip != null)
        {
            PlayBGM(bgmClip);
        }
    }

    private void OnEnable()
    {
        if (onPlayerAttackChannel != null) onPlayerAttackChannel.OnEventRaised += HandlePlayerAttack;
        if (onPlayerHitChannel != null) onPlayerHitChannel.OnEventRaised += HandlePlayerHit;
        if (onPlayerDeadChannel != null) onPlayerDeadChannel.OnEventRaised += HandleGameOver;
        if (onGameWinChannel != null) onGameWinChannel.OnEventRaised += HandleGameWin;
        if (onDungeonStartChannel != null) onDungeonStartChannel.OnEventRaised += HandleDungeonStart;
        if (onMonsterAttackChannel != null) onMonsterAttackChannel.OnEventRaised += HandleMonsterAttack;
        if (onMonsterHitChannel != null) onMonsterHitChannel.OnEventRaised += HandleMonsterHit;
        if (onMonsterKilledChannel != null) onMonsterKilledChannel.OnEventRaised += HandleMonsterKilled;
    }

    private void OnDisable()
    {
        if (onPlayerAttackChannel != null) onPlayerAttackChannel.OnEventRaised -= HandlePlayerAttack;
        if (onPlayerHitChannel != null) onPlayerHitChannel.OnEventRaised -= HandlePlayerHit;
        if (onPlayerDeadChannel != null) onPlayerDeadChannel.OnEventRaised -= HandleGameOver;
        if (onGameWinChannel != null) onGameWinChannel.OnEventRaised -= HandleGameWin;
        if (onDungeonStartChannel != null) onDungeonStartChannel.OnEventRaised -= HandleDungeonStart;
        if (onMonsterAttackChannel != null) onMonsterAttackChannel.OnEventRaised -= HandleMonsterAttack;
        if (onMonsterHitChannel != null) onMonsterHitChannel.OnEventRaised -= HandleMonsterHit;
        if (onMonsterKilledChannel != null) onMonsterKilledChannel.OnEventRaised -= HandleMonsterKilled;
    }

    private void HandlePlayerAttack() => PlaySFX(playerAttackClip);
    private void HandlePlayerHit() => PlaySFX(playerHitClip);
    private void HandleGameOver() => PlaySFX(gameOverClip);
    private void HandleGameWin() => PlaySFX(victoryClip);
    private void HandleDungeonStart() => PlaySFX(dungeonStartClip);
    private void HandleMonsterAttack(MonsterData data) => PlaySFX(data.AttackClip);
    private void HandleMonsterHit(MonsterData data) => PlaySFX(data.HitClip);
    private void HandleMonsterKilled(MonsterDeathInfo info) => PlaySFX(info.MonsterData.DeathClip);

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    public void PlayBGM(AudioClip clip)
    {
        if (clip == null || bgmSource.clip == clip) return;

        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void SetMasterVolume(float linearVolume) => SetMixerVolume(MasterVolumeParam, linearVolume);
    public void SetBGMVolume(float linearVolume) => SetMixerVolume(BGMVolumeParam, linearVolume);
    public void SetSFXVolume(float linearVolume) => SetMixerVolume(SFXVolumeParam, linearVolume);

    private void SetMixerVolume(string param, float linearVolume)
    {
        // 슬라이더(0~1)를 데시벨로 변환. 0은 -80dB(무음) 처리
        float dB = linearVolume <= 0.0001f ? -80f : Mathf.Log10(linearVolume) * 20f;
        audioMixer.SetFloat(param, dB);
    }
}
