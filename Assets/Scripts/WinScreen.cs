using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    public string mainMenuScene;

    public string levelSelectionScene;

    public UnityEngine.UI.Button nextLevelButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;

        AudioManager.instance.PlayWinMusic();

        // Show/hide next level button based on whether there's a next level
        if (nextLevelButton != null)
        {
            nextLevelButton.gameObject.SetActive(LevelManager.HasNextLevel());
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    public void GoToLevelSelection()
    {
        SceneManager.LoadScene(levelSelectionScene);
    }

    public void LoadNextLevel()
    {
        string nextLevel = LevelManager.GetNextLevel();
        
        if (!string.IsNullOrEmpty(nextLevel))
        {
            SceneManager.LoadScene(nextLevel);
            AudioManager.instance.PlayLevelMusic();
        }
        else
        {
            // If no next level, go to level selection
            GoToLevelSelection();
        }
    }
}
