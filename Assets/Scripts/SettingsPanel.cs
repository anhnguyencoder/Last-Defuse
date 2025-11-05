using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

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
    
    // Avatar selection settings
    [System.Serializable]
    public class AvatarOption
    {
        public Button avatarButton; // Button để click chọn avatar (chứa toàn bộ avatar structure)
        public Sprite avatarSprite; // Sprite của avatar
        public Image checkmark; // Dấu tích màu xanh (hiển thị khi được chọn)
        
        // Cached components (tự động tìm, không cần gán)
        [System.NonSerialized]
        public Image avatarImage; // Image hiển thị avatar sprite (tự động tìm)
    }
    
    public TMP_Text avatarLabel; // Label "Avatar:" hoặc "Select Avatar:"
    public List<AvatarOption> avatarOptions = new List<AvatarOption>(); // Danh sách các avatar options
    
    public Button closeButton;
    
    private float musicVolume = 1f;
    private float sfxVolume = 1f;
    private string currentUsername = "USERNAME";
    private string savedUsername = "USERNAME";
    private int selectedAvatarIndex = 0; // Index của avatar đã chọn (0-4)
    
    private void Start()
    {
        // Load saved volumes from PlayerPrefs
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        
        // Load saved username from PlayerPrefs
        savedUsername = PlayerPrefs.GetString("PlayerUsername", "USERNAME");
        currentUsername = savedUsername;
        
        // Load saved avatar index from PlayerPrefs
        selectedAvatarIndex = PlayerPrefs.GetInt("SelectedAvatarIndex", 0);
        
        // Initialize username input field
        InitializeUsernameField();
        
        // Initialize avatar selection
        InitializeAvatarSelection();
        
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
    
    private void InitializeAvatarSelection()
    {
        // Set avatar label text
        if (avatarLabel != null)
        {
            avatarLabel.text = "Avatar:";
        }
        
        // Setup button listeners và initialize avatars
        for (int i = 0; i < avatarOptions.Count; i++)
        {
            int index = i; // Capture index for lambda
            AvatarOption option = avatarOptions[i];
            
            // Auto-find Avatar Image component trong children của button
            if (option.avatarButton != null)
            {
                // Tìm Avatar Image trong children (có thể nằm trong Mask Container)
                option.avatarImage = FindAvatarImageInChildren(option.avatarButton.transform);
                
                // Setup button click listener
                option.avatarButton.onClick.AddListener(() => SelectAvatar(index));
            }
            
            // Set avatar sprite
            if (option.avatarImage != null && option.avatarSprite != null)
            {
                option.avatarImage.sprite = option.avatarSprite;
            }
            
            // Hide checkmark initially
            if (option.checkmark != null)
            {
                option.checkmark.gameObject.SetActive(false);
            }
        }
        
        // Show selected avatar
        UpdateAvatarSelection();
        
        // Apply saved avatar to PlayerUnitFrame
        ApplySelectedAvatar();
    }
    
    private Image FindAvatarImageInChildren(Transform parent)
    {
        // Tìm Image component có tên chứa "Avatar" và "Image" (không phải Background, Mask, Overlay)
        foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
        {
            Image img = child.GetComponent<Image>();
            if (img != null)
            {
                string name = child.name.ToLower();
                // Tìm Image có tên chứa "avatar" và "image" nhưng không phải background, mask, overlay
                if (name.Contains("avatar") && name.Contains("image") && 
                    !name.Contains("background") && !name.Contains("mask") && !name.Contains("overlay"))
                {
                    return img;
                }
            }
        }
        
        // Nếu không tìm thấy, tìm Image đầu tiên trong children (ngoại trừ checkmark)
        foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
        {
            Image img = child.GetComponent<Image>();
            if (img != null && !img.name.ToLower().Contains("checkmark"))
            {
                // Kiểm tra xem có phải là Image trong Mask Container không
                if (child.parent != null && child.parent.GetComponent<Mask>() != null)
                {
                    return img;
                }
            }
        }
        
        return null;
    }
    
    private void SelectAvatar(int index)
    {
        if (index >= 0 && index < avatarOptions.Count)
        {
            selectedAvatarIndex = index;
            
            // Save to PlayerPrefs
            PlayerPrefs.SetInt("SelectedAvatarIndex", selectedAvatarIndex);
            PlayerPrefs.Save();
            
            // Update UI
            UpdateAvatarSelection();
            
            // Update PlayerUnitFrame
            ApplySelectedAvatar();
        }
    }
    
    private void UpdateAvatarSelection()
    {
        // Update checkmarks for all avatars
        for (int i = 0; i < avatarOptions.Count; i++)
        {
            AvatarOption option = avatarOptions[i];
            
            if (option.checkmark != null)
            {
                // Show checkmark only for selected avatar
                option.checkmark.gameObject.SetActive(i == selectedAvatarIndex);
            }
        }
    }
    
    private void ApplySelectedAvatar()
    {
        if (selectedAvatarIndex >= 0 && selectedAvatarIndex < avatarOptions.Count)
        {
            AvatarOption selectedOption = avatarOptions[selectedAvatarIndex];
            
            // Update PlayerUnitFrame if exists
            if (UIController.instance != null && UIController.instance.playerUnitFrame != null)
            {
                if (selectedOption.avatarSprite != null)
                {
                    UIController.instance.playerUnitFrame.SetAvatar(selectedOption.avatarSprite);
                }
            }
        }
    }
    
    // Public method để lấy avatar sprite đã lưu (để UIController có thể gọi)
    public Sprite GetSavedAvatarSprite()
    {
        // Load saved avatar index từ PlayerPrefs
        int savedIndex = PlayerPrefs.GetInt("SelectedAvatarIndex", 0);
        
        if (savedIndex >= 0 && savedIndex < avatarOptions.Count)
        {
            AvatarOption selectedOption = avatarOptions[savedIndex];
            if (selectedOption != null && selectedOption.avatarSprite != null)
            {
                return selectedOption.avatarSprite;
            }
        }
        
        return null;
    }
    
    // Public method để apply avatar đã lưu (để UIController có thể gọi)
    public void ApplySavedAvatar()
    {
        ApplySelectedAvatar();
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
            
            // Update avatar selection display
            UpdateAvatarSelection();
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

