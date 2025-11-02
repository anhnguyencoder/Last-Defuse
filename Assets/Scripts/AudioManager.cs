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

    public AudioSource titleMusic, levelMusic, bossMusic, winMusic;

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
