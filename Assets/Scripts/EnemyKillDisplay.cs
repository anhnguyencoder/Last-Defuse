using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyKillDisplay : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Text hiển thị số enemy đã tiêu diệt")]
    public TMP_Text killCountText;
    [Tooltip("Image icon của enemy")]
    public Image enemyIcon;
    [Tooltip("Image frame/khung")]
    public Image frameImage;

    [Header("Settings")]
    [Tooltip("Format cho text hiển thị số kill (ví dụ: 'Kills: {0}' hoặc chỉ '{0}')")]
    public string killCountFormat = "{0}";

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

