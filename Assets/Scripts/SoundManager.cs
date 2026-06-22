using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource musicSource;

    [Header("Sound Effects")]
    public AudioClip pickupSound;
    public AudioClip hitSound;
    public AudioClip shieldBlockSound;
    public AudioClip bossSpawnSound;
    public AudioClip bossDefeatedSound;
    public AudioClip spikeAttackSound;
    public AudioClip levelCompleteSound;
    public AudioClip gameOverSound;

    [Header("Music")]
    public AudioClip backgroundMusic;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (sfxSource == null)
        {
            Debug.LogWarning("SoundManager: SFX AudioSource is not assigned.");
        }

        if (musicSource == null)
        {
            Debug.LogWarning("SoundManager: Music AudioSource is not assigned.");
        }
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusic == null || musicSource == null)
        {
            return;
        }

        if (musicSource.clip == backgroundMusic && musicSource.isPlaying)
        {
            return;
        }

        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.volume = 0.35f;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null)
        {
            return;
        }

        sfxSource.PlayOneShot(clip);
    }

    public void PlayPickup()
    {
        PlaySFX(pickupSound);
    }

    public void PlayHit()
    {
        PlaySFX(hitSound);
    }

    public void PlayShieldBlock()
    {
        PlaySFX(shieldBlockSound);
    }

    public void PlayBossSpawn()
    {
        PlaySFX(bossSpawnSound);
    }

    public void PlayBossDefeated()
    {
        PlaySFX(bossDefeatedSound);
    }

    public void PlaySpikeAttack()
    {
        PlaySFX(spikeAttackSound);
    }

    public void PlayLevelComplete()
    {
        PlaySFX(levelCompleteSound);
    }

    public void PlayGameOver()
    {
        PlaySFX(gameOverSound);
    }
}