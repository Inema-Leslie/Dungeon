using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Level Completion")]
    [SerializeField] private AudioClip levelCompleteClip;

    [Header("Enemy Sounds")]
    [SerializeField] private AudioClip enemyDefeatedClip;

    private float musicVolume = 0.8f;
    private float sfxVolume = 0.8f;

   private void Awake()
{
    if (Instance != null && Instance != this)
    {
        Destroy(gameObject);
        return;
    }
    Instance = this;
    DontDestroyOnLoad(gameObject);

    if (musicSource == null)
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
    }
    if (sfxSource == null)
    {
        sfxSource = gameObject.AddComponent<AudioSource>();
    }

    LoadVolumeFromSave();
    
}
    private void OnEnable()
    {
        GameEvents.OnItemCollected += HandleItemCollected;
        GameEvents.OnDoorOpened += HandleDoorOpened;
        GameEvents.OnEnemyDefeated += HandleEnemyDefeated;
        GameEvents.OnLevelCompleted += HandleLevelCompleted;
    }

    private void OnDisable()
    {
        GameEvents.OnItemCollected -= HandleItemCollected;
        GameEvents.OnDoorOpened -= HandleDoorOpened;
        GameEvents.OnEnemyDefeated -= HandleEnemyDefeated;
        GameEvents.OnLevelCompleted -= HandleLevelCompleted;
    }

   

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource.clip == clip) return;

        musicSource.clip = clip;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    public void PlayCombatSound(AudioClip clip) => PlaySFX(clip);
    public void PlayUISound(AudioClip clip) => PlaySFX(clip);
    public void PlayEnemySound(AudioClip clip) => PlaySFX(clip);

    private void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        musicSource.volume = musicVolume;
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
    }

    private void LoadVolumeFromSave()
    {
        SaveData data = SaveManager.Instance?.LoadGame();
        if (data != null)
        {
            musicVolume = data.musicVolume;
            sfxVolume = data.sfxVolume;
        }
    }

    

    private void HandleItemCollected(string itemId) => PlayUISound(pickupClip);
    private void HandleDoorOpened() => PlayCombatSound(doorOpenClip);
    private void HandleEnemyDefeated(string enemyName) => PlayEnemySound(enemyDefeatedClip);
    private void HandleLevelCompleted(int levelIndex) => PlaySFX(levelCompleteClip);

    [Header("Misc SFX (referenced by event hooks above)")]
    [SerializeField] private AudioClip pickupClip;
    [SerializeField] private AudioClip doorOpenClip;
}