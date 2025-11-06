using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    private void Awake()
    {
        instance = this;
    }

    [Header("UI Text - Đạn")]
    [Tooltip("Text hiển thị số đạn hiện tại trong băng")]
    public TMP_Text ammoText;
    [Tooltip("Text hiển thị số đạn còn lại trong kho")]
    public TMP_Text remainingAmmoText;

    [Header("UI Text - Timer")]
    [Tooltip("Text hiển thị thời gian còn lại")]
    public TMP_Text timerText;
    [Tooltip("Icon hiển thị cạnh timer text")]
    public Image timerIcon;

    [Header("Các Màn Hình")]
    [Tooltip("GameObject màn hình hiển thị khi player chết")]
    public GameObject deathScreen;

    [Header("Cài Đặt Scene")]
    [Tooltip("Tên scene main menu")]
    public string mainMenuScene;

    [Tooltip("Tên scene level selection")]
    public string levelSelectionScene;

    [Header("Cài Đặt Pause")]
    [Tooltip("GameObject màn hình pause")]
    public GameObject pauseScreen;

    [Tooltip("Input action để pause/unpause game")]
    public InputActionReference pauseAction;

    [Header("Tham Chiếu Component")]
    [Tooltip("SettingsPanel component")]
    public SettingsPanel settingsPanel;
    
    [Tooltip("PlayerUnitFrame component (hiển thị máu, avatar, username)")]
    public PlayerUnitFrame playerUnitFrame;

    [Tooltip("EnemyKillDisplay component (hiển thị số enemy đã tiêu diệt)")]
    public EnemyKillDisplay enemyKillDisplay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Load avatar ngay khi khởi động
        LoadSavedAvatar();
    }
    
    // Load avatar đã lưu từ PlayerPrefs
    private void LoadSavedAvatar()
    {
        if (playerUnitFrame == null)
        {
            return;
        }
        
        // Nếu có settingsPanel, lấy avatar sprite từ đó
        if (settingsPanel != null)
        {
            Sprite savedAvatarSprite = settingsPanel.GetSavedAvatarSprite();
            if (savedAvatarSprite != null)
            {
                playerUnitFrame.SetAvatar(savedAvatarSprite);
                return;
            }
        }
        
        // Nếu settingsPanel chưa khởi tạo xong, thử load lại sau một frame
        if (settingsPanel != null)
        {
            StartCoroutine(LoadAvatarDelayed());
        }
    }
    
    // Coroutine để load avatar sau khi SettingsPanel đã khởi tạo xong
    private System.Collections.IEnumerator LoadAvatarDelayed()
    {
        yield return null; // Đợi một frame để SettingsPanel.Start() chạy xong
        
        if (settingsPanel != null && playerUnitFrame != null)
        {
            Sprite savedAvatarSprite = settingsPanel.GetSavedAvatarSprite();
            if (savedAvatarSprite != null)
            {
                playerUnitFrame.SetAvatar(savedAvatarSprite);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(pauseAction.action.WasPressedThisFrame())
        {
            PauseUnpause();
        }
    }

    public void UpdateAmmoText(int currentAmmo, int remainingAmmo)
    {
        ammoText.text = currentAmmo.ToString();

        remainingAmmoText.text = "/" + remainingAmmo.ToString();
    }

    public void UpdateTimerText(float timeRemaining)
    {
        if (timerText != null)
        {
            // Đảm bảo không bao giờ hiển thị số âm - nếu <= 0 thì hiển thị 00:00
            if (timeRemaining <= 0f)
            {
                timerText.text = "00:00";
                timerText.color = Color.red;
                
                // Đổi màu icon cùng với text
                if (timerIcon != null)
                {
                    timerIcon.color = Color.red;
                }
                return;
            }
            
            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);
            // Bỏ chữ "Time:" - chỉ hiển thị thời gian
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            
            // Đổi màu khi thời gian sắp hết (dưới 10 giây)
            Color textColor;
            if (timeRemaining <= 10f)
            {
                textColor = Color.red;
            }
            else if (timeRemaining <= 30f)
            {
                textColor = Color.yellow;
            }
            else
            {
                textColor = Color.white;
            }
            
            timerText.color = textColor;
            
            // Đổi màu icon cùng với text
            if (timerIcon != null)
            {
                timerIcon.color = textColor;
            }
        }
    }

    public void ShowDeathScreen()
    {
        if(deathScreen != null)
        {
            deathScreen.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.None;
    }

    public void RestartLevel()
    {
        //Debug.Log("Restarting");

        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);

        Time.timeScale = 1f;
    }

    public void GoToLevelSelection()
    {
        SceneManager.LoadScene(levelSelectionScene);

        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseUnpause()
    {
        if(pauseScreen.activeSelf == false)
        {
            pauseScreen.SetActive(true);

            Cursor.lockState = CursorLockMode.None;

            Time.timeScale = 0;

        } else
        {
            pauseScreen.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;

            Time.timeScale = 1f;
        }
        
        // Close settings panel if open
        if (settingsPanel != null && settingsPanel.settingsPanel != null && settingsPanel.settingsPanel.activeSelf)
        {
            settingsPanel.CloseSettings();
        }
    }

    public void OpenSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.OpenSettings();
        }
    }

    public void UpdateKillCount(int killCount)
    {
        if (enemyKillDisplay != null)
        {
            enemyKillDisplay.UpdateKillCount(killCount);
        }
    }
}
