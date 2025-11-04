using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    public string winScene;
    public string nextLevel;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            // Dừng timer khi player thắng
            if (CountdownTimer.instance != null)
            {
                CountdownTimer.instance.StopTimer();
            }

            // Save current level info and next level before loading win scene
            string currentLevel = SceneManager.GetActiveScene().name;
            LevelManager.SetCurrentLevel(currentLevel);
            LevelManager.SetNextLevel(nextLevel);

            // Unlock next level when player completes current level
            if (!string.IsNullOrEmpty(nextLevel))
            {
                LevelManager.UnlockLevel(nextLevel);
                Debug.Log("Unlocked level: " + nextLevel + " after completing: " + currentLevel);
            }

            // Load win scene when player completes the level
            SceneManager.LoadScene(winScene);

            AudioManager.instance.PlaySFX(3);
        }
    }
}
