using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    private void Awake()
    {
        instance = this;
    }

    public TMP_Text ammoText, remainingAmmoText;

    public TMP_Text healthText;

    public TMP_Text timerText;

    public GameObject deathScreen;

    public string mainMenuScene;

    public string levelSelectionScene;

    public GameObject pauseScreen;

    public InputActionReference pauseAction;

    public SettingsPanel settingsPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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

    public void UpdateHealthText(float currentHealth)
    {
        healthText.text = "Health: " + Mathf.RoundToInt(currentHealth);
    }

    public void UpdateTimerText(float timeRemaining)
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);
            timerText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
            
            // Đổi màu khi thời gian sắp hết (dưới 10 giây)
            if (timeRemaining <= 10f)
            {
                timerText.color = Color.red;
            }
            else if (timeRemaining <= 30f)
            {
                timerText.color = Color.yellow;
            }
            else
            {
                timerText.color = Color.white;
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
}
