using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {

            instance = this;

            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    [Header("Cài Đặt Nhạc Nền")]
    [Tooltip("AudioSource cho nhạc title/main menu")]
    public AudioSource titleMusic;
    [Tooltip("AudioSource cho nhạc level bình thường")]
    public AudioSource levelMusic;
    [Tooltip("AudioSource cho nhạc boss")]
    public AudioSource bossMusic;
    [Tooltip("AudioSource cho nhạc thắng")]
    public AudioSource winMusic;

    [Header("Cài Đặt SFX")]
    [Tooltip("Mảng các AudioSource cho sound effects")]
    public AudioSource[] sfx;

    public void StopMusic()
    {
        titleMusic.Stop();
        levelMusic.Stop();
        bossMusic.Stop();
        winMusic.Stop();
    }

    public void PlayTitleMusic()
    {
        StopMusic();

        titleMusic.Play();
    }

    public void PlayLevelMusic()
    {
        StopMusic();

        levelMusic.Play();
    }

    public void PlayBossMusic()
    {
        StopMusic();

        bossMusic.Play();
    }

    public void PlayWinMusic()
    {
        StopMusic();

        winMusic.Play();
    }

    public void PlaySFX(int sfxToPlay)
    {
        sfx[sfxToPlay].Stop();

        sfx[sfxToPlay].Play();
    }

    private float masterVolume = 1f;
    private float musicVolume = 1f;
    private float sfxVolume = 1f;

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        UpdateAudioVolumes();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        UpdateMusicVolumes();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        UpdateSFXVolumes();
    }

    private void UpdateAudioVolumes()
    {
        UpdateMusicVolumes();
        UpdateSFXVolumes();
    }

    private void UpdateMusicVolumes()
    {
        if (titleMusic != null)
            titleMusic.volume = masterVolume * musicVolume;
        if (levelMusic != null)
            levelMusic.volume = masterVolume * musicVolume;
        if (bossMusic != null)
            bossMusic.volume = masterVolume * musicVolume;
        if (winMusic != null)
            winMusic.volume = masterVolume * musicVolume;
    }

    private void UpdateSFXVolumes()
    {
        if (sfx != null)
        {
            for (int i = 0; i < sfx.Length; i++)
            {
                if (sfx[i] != null)
                {
                    sfx[i].volume = masterVolume * sfxVolume;
                }
            }
        }
    }
}
