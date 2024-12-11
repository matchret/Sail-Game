using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Environment Sounds")]
    public AudioClip enviroClip;
    private AudioSource enviroSource;

    [Header("Agent Sounds")]
    public AudioClip coinCollectClip;
    public AudioClip windPushClip;

    [Header("Music")]
    public AudioSource menuMusic;
    public AudioSource gameMusic;

    [Header("Volumes")]
    [Range(0, 1)] public float environmentVolume = 0.5f;
    [Range(0, 1)] public float agentVolume = 1f;
    [Range(0, 1)] public float musicVolume = 0.5f;

    private void Awake()
    {
        // Gestion du singleton avec préservation de la configuration audio existante
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
        }
        else
        {
            // Au lieu de détruire complètement, on copie les configurations
            CopyAudioSettingsFrom(Instance);
            Destroy(gameObject);
        }
    }

    private void CopyAudioSettingsFrom(AudioManager existingManager)
    {
        // Copier les volumes
        environmentVolume = existingManager.environmentVolume;
        agentVolume = existingManager.agentVolume;
        musicVolume = existingManager.musicVolume;

        // Copier les clips
        enviroClip = existingManager.enviroClip;
        coinCollectClip = existingManager.coinCollectClip;
        windPushClip = existingManager.windPushClip;

        // Copier les sources de musique (si nécessaire)
        if (existingManager.menuMusic != null)
            menuMusic = existingManager.menuMusic;
        if (existingManager.gameMusic != null)
            gameMusic = existingManager.gameMusic;
    }

    private void InitializeAudioSources()
    {
        // Initialiser l'environnement audio source
        if (enviroClip != null)
        {
            enviroSource = gameObject.AddComponent<AudioSource>();
            enviroSource.clip = enviroClip;
            enviroSource.volume = environmentVolume;
            enviroSource.loop = true;
            enviroSource.playOnAwake = false;
        }

        // Configuration des sources de musique
        if (menuMusic != null)
        {
            menuMusic.playOnAwake = false;
            menuMusic.loop = true;
            menuMusic.volume = musicVolume;
        }

        if (gameMusic != null)
        {
            gameMusic.playOnAwake = false;
            gameMusic.loop = true;
            gameMusic.volume = musicVolume;
        }
    }

    private void Start()
    {
        // Démarrer la musique de menu
        PlayMenuMusic();
    }

    #region Environment Sounds
    public void PlayEnvironmentSounds()
    {
        if (enviroSource != null && !enviroSource.isPlaying)
        {
            enviroSource.Play();
        }
        else if (enviroSource == null)
        {
            Debug.LogWarning("Environment AudioSource is missing!");
        }
    }

    public void StopEnvironmentSounds()
    {
        if (enviroSource != null && enviroSource.isPlaying)
        {
            enviroSource.Stop();
        }
    }
    #endregion

    #region Agent Sounds
    public void PlayCoinCollectSound(Vector3 position)
    {
        if (coinCollectClip != null)
        {
            AudioSource.PlayClipAtPoint(coinCollectClip, position, agentVolume * 100000000);
        }
        else
        {
            Debug.LogWarning("Coin Collect Clip not loaded!");
        }
    }

    public void PlayWindPushSound(Vector3 position)
    {
        if (windPushClip != null)
        {
            AudioSource.PlayClipAtPoint(windPushClip, position, agentVolume * 100000000);
        }
        else
        {
            Debug.LogWarning("Wind Push Clip not loaded!");
        }
    }
    #endregion

    #region Music Management
    public void PlayMenuMusic()
    {
        StopGameMusic();

        if (menuMusic != null)
        {
            menuMusic.Play();
        }
        else
        {
            Debug.LogWarning("Menu Music AudioSource is missing!");
        }
    }

    public void PlayGameMusic()
    {
        StopMenuMusic();

        if (gameMusic != null)
        {
            gameMusic.Play();
        }
        else
        {
            Debug.LogWarning("Game Music AudioSource is missing!");
        }
    }

    public void StopMenuMusic() => menuMusic?.Stop();
    public void StopGameMusic() => gameMusic?.Stop();
    #endregion
}