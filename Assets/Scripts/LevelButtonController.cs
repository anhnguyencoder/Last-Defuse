using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LevelButtonData
{
    public string levelSceneName;
    public Button button;
    public GameObject lockOverlay;
}

public class LevelButtonController : MonoBehaviour
{
    public LevelButtonData[] levelButtons;

    private void Start()
    {
        UpdateLevelButtons();
    }

    public void UpdateLevelButtons()
    {
        foreach (var levelButton in levelButtons)
        {
            if (levelButton.button != null && levelButton.lockOverlay != null)
            {
                bool isUnlocked = LevelManager.IsLevelUnlocked(levelButton.levelSceneName);
                
                // Enable/disable button based on unlock status
                levelButton.button.interactable = isUnlocked;
                
                // Show/hide lock overlay based on unlock status
                levelButton.lockOverlay.SetActive(!isUnlocked);
            }
        }
    }

    // Method để load level (được gọi từ button)
    public void LoadLevel(string levelSceneName)
    {
        // Kiểm tra level đã unlock chưa
        if (LevelManager.IsLevelUnlocked(levelSceneName))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(levelSceneName);
            AudioManager.instance.PlayLevelMusic();
        }
        else
        {
            Debug.Log("Level " + levelSceneName + " chưa được mở khóa!");
            // Có thể thêm sound effect hoặc message ở đây
        }
    }
}
