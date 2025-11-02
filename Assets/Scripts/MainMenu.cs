using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string levelSelectionScene;

    private void Start()
    {
        AudioManager.instance.PlayTitleMusic();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(levelSelectionScene);
    }

    public void QuitGame()
    {
        Application.Quit();

        Debug.Log("Quitting Game");
    }
}
