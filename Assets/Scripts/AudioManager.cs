using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    private bool sfxEnabled = true;

    [Header("Music")]
    public AudioClip backgroundMusic;
    public AudioClip boostMusic;

    [Header("SFX")]
    public AudioClip uiClick;
    public AudioClip jump;
    public AudioClip slide;
    public AudioClip swipe;
    public AudioClip orbPickup;
    public AudioClip gameOver;
    public AudioClip glassBreak;   // ?? ADD THIS

    private float normalMusicVolume = 0.5f;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        PlayBackgroundMusic();
    }

    

   
    // ---------------- MUSIC ----------------

    public void PlayBackgroundMusic()
    {
        musicSource.clip = backgroundMusic;
        musicSource.volume = normalMusicVolume;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayBoostMusic()
    {
        if (boostMusic == null) return;

        musicSource.clip = boostMusic;
        musicSource.volume = normalMusicVolume;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    // ---------------- SFX ----------------

    public void PlaySFX(AudioClip clip)
    {
        if (!sfxEnabled) return;
        if (clip == null) return;

        sfxSource.PlayOneShot(clip);
    }
    public void SetSFXEnabled(bool enabled)
    {
        sfxEnabled = enabled;
    }
    //public void PlaySFX(AudioClip clip)
    //{
    //    if (clip == null) return;

    //    sfxSource.PlayOneShot(clip);
    //}

    // ---------------- DUCKING ----------------

    public IEnumerator DuckMusic(float duckVolume, float duration)
    {
        float startVolume = musicSource.volume;
        musicSource.volume = duckVolume;

        yield return new WaitForSeconds(duration);

        musicSource.volume = startVolume;
    }

    public void DuckForOrb()
    {
        StartCoroutine(DuckMusic(0.2f, 0.4f));
    }
}
