using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsPanel : MonoBehaviour
{
    public GameObject settingsPanel;
    
    // Volume sliders
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    
    // Volume toggle buttons
    public Button musicVolumeButton;
    public Button sfxVolumeButton;
    
    // Mute overlay images (diagonal slash) - assign these in Unity Inspector
    public Image musicMuteOverlay;
    public Image sfxMuteOverlay;
    
    // Text labels
    public TMP_Text musicVolumeText;
    public TMP_Text sfxVolumeText;
    
    // Username settings
    public TMP_Text usernameLabel; // Label "User name:"
    public TMP_InputField usernameInputField; // Input field để nhập username
    public Button usernameDoneButton; // Nút Done để lưu username
    
    public Button closeButton;
    
    private float musicVolume = 1f;
    private float sfxVolume = 1f;
    private string currentUsername = "USERNAME";
    private string savedUsername = "USERNAME";
    
    private void Start()
    {
        // Load saved volumes from PlayerPrefs
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        
        // Load saved username from PlayerPrefs
        savedUsername = PlayerPrefs.GetString("PlayerUsername", "USERNAME");
        currentUsername = savedUsername;
        
        // Initialize username input field
        InitializeUsernameField();
        
        // Set slider values and listeners
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
        
        // Set button listeners
        if (musicVolumeButton != null)
        {
            musicVolumeButton.onClick.AddListener(ToggleMusicVolume);
        }
        
        if (sfxVolumeButton != null)
        {
            sfxVolumeButton.onClick.AddListener(ToggleSFXVolume);
        }
        
        // Done button listener
        if (usernameDoneButton != null)
        {
            usernameDoneButton.onClick.AddListener(SaveUsername);
        }
        
        // Update button visuals and text
        UpdateButtonVisuals();
        UpdateVolumeTexts();
        
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
    
    private void InitializeUsernameField()
    {
        // Set username label text
        if (usernameLabel != null)
        {
            usernameLabel.text = "User name:";
        }
        
        // Initialize input field
        if (usernameInputField != null)
        {
            usernameInputField.text = savedUsername;
            usernameInputField.onValueChanged.AddListener(OnUsernameChanged);
        }
        
        // Hide Done button initially
        if (usernameDoneButton != null)
        {
            usernameDoneButton.gameObject.SetActive(false);
        }
    }
    
    private void OnUsernameChanged(string newValue)
    {
        currentUsername = newValue;
        
        // Show Done button if username changed from saved value
        if (usernameDoneButton != null)
        {
            bool hasChanged = currentUsername != savedUsername && !string.IsNullOrEmpty(currentUsername);
            usernameDoneButton.gameObject.SetActive(hasChanged);
        }
    }
    
    private void SaveUsername()
    {
        // Validate username (not empty)
        if (string.IsNullOrEmpty(currentUsername) || string.IsNullOrWhiteSpace(currentUsername))
        {
            currentUsername = savedUsername; // Revert to saved value
            if (usernameInputField != null)
            {
                usernameInputField.text = savedUsername;
            }
            return;
        }
        
        // Save to PlayerPrefs
        savedUsername = currentUsername;
        PlayerPrefs.SetString("PlayerUsername", savedUsername);
        PlayerPrefs.Save();
        
        // Update PlayerUnitFrame if exists
        if (UIController.instance != null && UIController.instance.playerUnitFrame != null)
        {
            UIController.instance.playerUnitFrame.SetUsername(savedUsername);
        }
        
        // Hide Done button
        if (usernameDoneButton != null)
        {
            usernameDoneButton.gameObject.SetActive(false);
        }
    }
    
    public void OpenSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
            
            // Reset username field to saved value when opening settings
            if (usernameInputField != null)
            {
                currentUsername = savedUsername;
                usernameInputField.text = savedUsername;
            }
            
            // Hide Done button when opening
            if (usernameDoneButton != null)
            {
                usernameDoneButton.gameObject.SetActive(false);
            }
        }
    }
    
    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }
    
    private void OnMusicVolumeChanged(float value)
    {
        musicVolume = value;
        UpdateAudioVolumes();
        UpdateButtonVisuals();
        UpdateMusicVolumeText();
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.Save();
    }
    
    private void OnSFXVolumeChanged(float value)
    {
        sfxVolume = value;
        UpdateAudioVolumes();
        UpdateButtonVisuals();
        UpdateSFXVolumeText();
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
    }
    
    private void ToggleMusicVolume()
    {
        // Toggle between 0 and previous value (or 1 if was 0)
        if (musicVolume > 0f)
        {
            // Save current volume before muting
            if (musicVolumeSlider != null)
            {
                musicVolumeSlider.value = 0f;
            }
            else
            {
                musicVolume = 0f;
                OnMusicVolumeChanged(0f);
            }
        }
        else
        {
            // Restore to 1f (or previous value if you want to save it)
            if (musicVolumeSlider != null)
            {
                musicVolumeSlider.value = 1f;
            }
            else
            {
                musicVolume = 1f;
                OnMusicVolumeChanged(1f);
            }
        }
    }
    
    private void ToggleSFXVolume()
    {
        // Toggle between 0 and previous value (or 1 if was 0)
        if (sfxVolume > 0f)
        {
            // Save current volume before muting
            if (sfxVolumeSlider != null)
            {
                sfxVolumeSlider.value = 0f;
            }
            else
            {
                sfxVolume = 0f;
                OnSFXVolumeChanged(0f);
            }
        }
        else
        {
            // Restore to 1f (or previous value if you want to save it)
            if (sfxVolumeSlider != null)
            {
                sfxVolumeSlider.value = 1f;
            }
            else
            {
                sfxVolume = 1f;
                OnSFXVolumeChanged(1f);
            }
        }
    }
    
    private void UpdateButtonVisuals()
    {
        // Show/hide mute overlay for music button
        if (musicMuteOverlay != null)
        {
            musicMuteOverlay.gameObject.SetActive(musicVolume <= 0f);
        }
        
        // Show/hide mute overlay for SFX button
        if (sfxMuteOverlay != null)
        {
            sfxMuteOverlay.gameObject.SetActive(sfxVolume <= 0f);
        }
    }
    
    private void UpdateVolumeTexts()
    {
        UpdateMusicVolumeText();
        UpdateSFXVolumeText();
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
            // Keep master volume at 1f (removed from UI)
            AudioManager.instance.SetMasterVolume(1f);
            AudioManager.instance.SetMusicVolume(musicVolume);
            AudioManager.instance.SetSFXVolume(sfxVolume);
        }
    }
}

