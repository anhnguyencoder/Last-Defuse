using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsPanel : MonoBehaviour
{
    public GameObject settingsPanel;
    
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    
    public TMP_Text masterVolumeText;
    public TMP_Text musicVolumeText;
    public TMP_Text sfxVolumeText;
    
    public Button closeButton;
    
    private float masterVolume = 1f;
    private float musicVolume = 1f;
    private float sfxVolume = 1f;
    
    private void Start()
    {
        // Load saved volumes from PlayerPrefs
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        
        // Set slider values
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.value = masterVolume;
            masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        }
        
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = musicVolume;
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }
        
        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.value = sfxVolume;
            sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        }
        
        // Update audio manager
        UpdateAudioVolumes();
        
        // Close button listener
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseSettings);
        }
        
        // Hide panel by default
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }
    
    public void OpenSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
    }
    
    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }
    
    private void OnMasterVolumeChanged(float value)
    {
        masterVolume = value;
        UpdateMasterVolumeText();
        UpdateAudioVolumes();
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.Save();
    }
    
    private void OnMusicVolumeChanged(float value)
    {
        musicVolume = value;
        UpdateMusicVolumeText();
        UpdateAudioVolumes();
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.Save();
    }
    
    private void OnSFXVolumeChanged(float value)
    {
        sfxVolume = value;
        UpdateSFXVolumeText();
        UpdateAudioVolumes();
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
    }
    
    private void UpdateMasterVolumeText()
    {
        if (masterVolumeText != null)
        {
            masterVolumeText.text = "Master Volume: " + Mathf.RoundToInt(masterVolume * 100) + "%";
        }
    }
    
    private void UpdateMusicVolumeText()
    {
        if (musicVolumeText != null)
        {
            musicVolumeText.text = "Music Volume: " + Mathf.RoundToInt(musicVolume * 100) + "%";
        }
    }
    
    private void UpdateSFXVolumeText()
    {
        if (sfxVolumeText != null)
        {
            sfxVolumeText.text = "SFX Volume: " + Mathf.RoundToInt(sfxVolume * 100) + "%";
        }
    }
    
    private void UpdateAudioVolumes()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetMasterVolume(masterVolume);
            AudioManager.instance.SetMusicVolume(musicVolume);
            AudioManager.instance.SetSFXVolume(sfxVolume);
        }
    }
}

