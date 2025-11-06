using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExitProgressBar : MonoBehaviour
{
    public static ExitProgressBar instance;

    [Header("UI References")]
    [Tooltip("Image component với Image Type = Filled, Fill Method = Radial 360")]
    public Image progressCircle;
    [Tooltip("Text hiển thị phần trăm ở giữa circle")]
    public TMP_Text percentageText;

    [Header("Settings")]
    [Tooltip("Thời gian cần để fill đầy (giây)")]
    public float fillDuration = 3f;
    [Tooltip("Có hiển thị phần trăm không?")]
    public bool showPercentage = true;

    private float currentProgress = 0f;
    private bool isFilling = false;

    private void Awake()
    {
        // Đặt instance để LevelExit có thể tìm thấy
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Có nhiều hơn một ExitProgressBar trong scene! Chỉ có một sẽ được sử dụng.");
        }
    }

    private void Start()
    {
        // Ẩn progress bar ban đầu
        if (progressCircle != null)
        {
            progressCircle.fillAmount = 0f;
            progressCircle.gameObject.SetActive(false);
        }

        if (percentageText != null)
        {
            percentageText.text = "0%";
            percentageText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (isFilling)
        {
            // Tăng progress
            currentProgress += Time.deltaTime / fillDuration;
            
            // Giới hạn progress trong khoảng 0-1
            currentProgress = Mathf.Clamp01(currentProgress);

            // Cập nhật UI
            UpdateProgressUI();

            // Kiểm tra nếu đã đầy
            if (currentProgress >= 1f)
            {
                OnProgressComplete();
            }
        }
    }

    public void StartProgress()
    {
        isFilling = true;
        
        // Hiển thị progress bar
        if (progressCircle != null)
        {
            progressCircle.gameObject.SetActive(true);
        }

        if (percentageText != null && showPercentage)
        {
            percentageText.gameObject.SetActive(true);
        }
    }

    public void StopProgress()
    {
        isFilling = false;
        
        // Reset progress khi dừng (player ra khỏi range)
        currentProgress = 0f;
        UpdateProgressUI();
        
        // Ẩn progress bar
        if (progressCircle != null)
        {
            progressCircle.gameObject.SetActive(false);
        }

        if (percentageText != null)
        {
            percentageText.gameObject.SetActive(false);
        }
    }

    public void ResetProgress()
    {
        currentProgress = 0f;
        isFilling = false;
        UpdateProgressUI();
        StopProgress();
    }

    private void UpdateProgressUI()
    {
        // Cập nhật fill amount của circle
        if (progressCircle != null)
        {
            progressCircle.fillAmount = currentProgress;
        }

        // Cập nhật text phần trăm
        if (percentageText != null && showPercentage)
        {
            int percentage = Mathf.RoundToInt(currentProgress * 100f);
            percentageText.text = percentage + "%";
        }
    }

    private void OnProgressComplete()
    {
        // Sự kiện khi progress đầy
        isFilling = false;
        
        // Thông báo cho LevelExit
        if (LevelExit.instance != null)
        {
            LevelExit.instance.OnProgressComplete();
        }
    }

    public float GetProgress()
    {
        return currentProgress;
    }

    public bool IsComplete()
    {
        return currentProgress >= 1f;
    }
}

