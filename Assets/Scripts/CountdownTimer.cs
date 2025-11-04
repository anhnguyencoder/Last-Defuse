using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    public static CountdownTimer instance;

    [Header("Timer Settings")]
    public float timeLimit = 60f; // Thời gian đếm ngược (giây) - có thể set khác nhau cho mỗi level
    private float currentTime;
    private bool isTimerRunning = false;
    private bool hasExpired = false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Bắt đầu timer khi level bắt đầu
        StartTimer();
    }

    void Update()
    {
        if (isTimerRunning && !hasExpired)
        {
            // Không chạy timer khi game bị pause
            if (Time.timeScale == 0f)
            {
                return;
            }

            // Kiểm tra nếu player đã chết hoặc đã thắng thì dừng timer
            if (PlayerController.instance != null && PlayerController.instance.isDead)
            {
                StopTimer();
                return;
            }

            // Kiểm tra nếu player đã thắng (level exit đã được trigger)
            // Timer sẽ tự động dừng khi player thắng

            currentTime -= Time.deltaTime;

            // Cập nhật UI
            if (UIController.instance != null)
            {
                UIController.instance.UpdateTimerText(currentTime);
            }

            // Kiểm tra nếu hết thời gian
            if (currentTime <= 0f)
            {
                OnTimerExpired();
            }
        }
    }

    public void StartTimer()
    {
        currentTime = timeLimit;
        isTimerRunning = true;
        hasExpired = false;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    public void ResetTimer()
    {
        currentTime = timeLimit;
        hasExpired = false;
    }

    private void OnTimerExpired()
    {
        if (hasExpired) return; // Tránh trigger nhiều lần

        hasExpired = true;
        isTimerRunning = false;
        currentTime = 0f;

        // Kiểm tra nếu player chưa chết và chưa thắng
        if (PlayerController.instance != null && !PlayerController.instance.isDead)
        {
            // Phát tiếng nổ (explosion sound)
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlaySFX(6); // SFX index 6 là player_dead sound
            }

            // Đặt player là dead
            PlayerController.instance.isDead = true;

            // Hiện dead popup
            if (UIController.instance != null)
            {
                UIController.instance.ShowDeathScreen();
            }

            Debug.Log("Time expired! Player failed to complete level in time.");
        }
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    public bool IsExpired()
    {
        return hasExpired;
    }
}

