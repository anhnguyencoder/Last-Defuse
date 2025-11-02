using UnityEngine;

public static class LevelManager
{
    private static string currentLevel;
    private static string nextLevel;

    private const string FIRST_LEVEL = "Level 1"; // Level đầu tiên luôn được unlock

    public static void SetCurrentLevel(string levelName)
    {
        currentLevel = levelName;
        PlayerPrefs.SetString("CurrentLevel", levelName);
    }

    public static string GetCurrentLevel()
    {
        if (string.IsNullOrEmpty(currentLevel))
        {
            currentLevel = PlayerPrefs.GetString("CurrentLevel", "");
        }
        return currentLevel;
    }

    public static void SetNextLevel(string levelName)
    {
        nextLevel = levelName;
        PlayerPrefs.SetString("NextLevel", levelName);
    }

    public static string GetNextLevel()
    {
        if (string.IsNullOrEmpty(nextLevel))
        {
            nextLevel = PlayerPrefs.GetString("NextLevel", "");
        }
        return nextLevel;
    }

    public static bool HasNextLevel()
    {
        string next = GetNextLevel();
        return !string.IsNullOrEmpty(next);
    }

    // Unlock level khi hoàn thành level trước đó
    public static void UnlockLevel(string levelName)
    {
        if (!string.IsNullOrEmpty(levelName))
        {
            PlayerPrefs.SetInt("Unlocked_" + levelName, 1);
            PlayerPrefs.Save();
            Debug.Log("Unlocked level: " + levelName);
        }
    }

    // Kiểm tra level đã được unlock chưa
    public static bool IsLevelUnlocked(string levelName)
    {
        // Level đầu tiên luôn được unlock
        if (levelName == FIRST_LEVEL)
        {
            return true;
        }

        return PlayerPrefs.GetInt("Unlocked_" + levelName, 0) == 1;
    }

    // Reset tất cả unlock (dùng để test)
    public static void ResetAllUnlocks()
    {
        // Không reset level đầu tiên
        PlayerPrefs.DeleteKey("Unlocked_Level 2");
        PlayerPrefs.DeleteKey("Unlocked_Boss Arena");
        PlayerPrefs.Save();
    }

    // Unlock tất cả level (cheat mode)
    public static void UnlockAllLevels()
    {
        UnlockLevel("Level 1");
        UnlockLevel("Level 2");
        UnlockLevel("Boss Arena");
    }
}
