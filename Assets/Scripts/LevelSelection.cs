using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    [Header("Cài Đặt Scene")]
    [Tooltip("Tên scene main menu để quay lại")]
    public string mainMenuScene;

    [Header("Tham Chiếu Component")]
    [Tooltip("LevelButtonController component để quản lý các nút level")]
    public LevelButtonController levelButtonController;

    private void Start()
    {
        AudioManager.instance.PlayTitleMusic();
        
        // Đảm bảo Level 1 luôn được unlock
        if (!LevelManager.IsLevelUnlocked("Level 1"))
        {
            LevelManager.UnlockLevel("Level 1");
        }
        
        // Update level buttons khi vào scene
        if (levelButtonController != null)
        {
            levelButtonController.UpdateLevelButtons();
        }
    }

    public void LoadLevel(string levelSceneName)
    {
        // Kiểm tra unlock trước khi load
        if (LevelManager.IsLevelUnlocked(levelSceneName))
        {
            SceneManager.LoadScene(levelSceneName);
            AudioManager.instance.PlayLevelMusic();
        }
        else
        {
            Debug.Log("Level " + levelSceneName + " chưa được mở khóa!");
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}
