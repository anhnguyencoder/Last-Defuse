using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyKillDisplay : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text killCountText;
    public Image enemyIcon;
    public Image frameImage;

    [Header("Settings")]
    public string killCountFormat = "{0}"; // Format cho text, ví dụ: "Kills: {0}" hoặc chỉ "{0}"

    private void Start()
    {
        // Đảm bảo UI hiển thị đúng giá trị ban đầu
        UpdateKillCount(0);
    }

    public void UpdateKillCount(int killCount)
    {
        if (killCountText != null)
        {
            killCountText.text = string.Format(killCountFormat, killCount);
        }
    }

    public void SetEnemyIcon(Sprite icon)
    {
        if (enemyIcon != null && icon != null)
        {
            enemyIcon.sprite = icon;
        }
    }

    public void SetFrameImage(Sprite frame)
    {
        if (frameImage != null && frame != null)
        {
            frameImage.sprite = frame;
        }
    }
}

