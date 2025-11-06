using UnityEngine;

public class KillTracker : MonoBehaviour
{
    public static KillTracker instance;

    private int killCount = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Reset kill count khi bắt đầu level mới
        ResetKillCount();
    }

    public void AddKill()
    {
        killCount++;
        
        // Thông báo cho UI cập nhật
        if (UIController.instance != null)
        {
            UIController.instance.UpdateKillCount(killCount);
        }
        
        Debug.Log("Enemy killed! Total kills: " + killCount);
    }

    public int GetKillCount()
    {
        return killCount;
    }

    public void ResetKillCount()
    {
        killCount = 0;
        
        // Cập nhật UI về 0
        if (UIController.instance != null)
        {
            UIController.instance.UpdateKillCount(0);
        }
    }
}

