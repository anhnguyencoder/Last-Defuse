using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    public string winScene;
    public string nextLevel;

    [Header("Warning Sound Settings")]
    [Tooltip("AudioClip để phát khi còn 10 giây")]
    public AudioClip warningSound;
    [Tooltip("Khoảng cách tối thiểu để phát âm thanh tối đa")]
    public float minDistance = 5f;
    [Tooltip("Khoảng cách tối đa để nghe được âm thanh")]
    public float maxDistance = 50f;
    [Tooltip("Volume tối đa của âm thanh (0-1)")]
    public float maxVolume = 1f;
    [Tooltip("Lặp lại âm thanh khi còn 10 giây?")]
    public bool loopWarningSound = false;

    private AudioSource audioSource;
    private bool isWarningActive = false;
    private float warningThreshold = 10f; // Phát khi còn 10 giây

    private void Start()
    {
        // Tạo AudioSource nếu chưa có
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Cấu hình AudioSource cho 3D sound
        audioSource.clip = warningSound;
        audioSource.spatialBlend = 1f; // 3D sound (1 = full 3D, 0 = 2D)
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;
        audioSource.volume = maxVolume;
        audioSource.playOnAwake = false;
        audioSource.loop = loopWarningSound;
    }

    private void Update()
    {
        if (CountdownTimer.instance == null)
            return;

        float currentTime = CountdownTimer.instance.GetCurrentTime();
        bool shouldPlayWarning = currentTime <= warningThreshold && currentTime > 0;

        if (shouldPlayWarning && !isWarningActive)
        {
            // Bắt đầu phát âm thanh cảnh báo
            PlayWarningSound();
            isWarningActive = true;
        }
        else if (!shouldPlayWarning && isWarningActive)
        {
            // Dừng âm thanh khi hết thời gian hoặc timer đã dừng
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            isWarningActive = false;
        }

        // Cập nhật volume dựa trên khoảng cách từ player (liên tục)
        if (audioSource != null && audioSource.isPlaying)
        {
            UpdateVolumeBasedOnDistance();
        }
    }

    private void PlayWarningSound()
    {
        if (audioSource != null && warningSound != null)
        {
            // Cập nhật volume ban đầu dựa trên khoảng cách
            UpdateVolumeBasedOnDistance();
            audioSource.Play();
        }
    }

    private void UpdateVolumeBasedOnDistance()
    {
        if (PlayerController.instance == null)
            return;

        // Tính khoảng cách từ player đến LevelExit
        float distance = Vector3.Distance(transform.position, PlayerController.instance.transform.position);

        // Tính volume dựa trên khoảng cách
        float volume = 0f;
        if (distance <= minDistance)
        {
            // Ở gần: volume tối đa
            volume = maxVolume;
        }
        else if (distance >= maxDistance)
        {
            // Ở xa: volume = 0
            volume = 0f;
        }
        else
        {
            // Tính volume theo khoảng cách (linear interpolation)
            float normalizedDistance = (distance - minDistance) / (maxDistance - minDistance);
            volume = maxVolume * (1f - normalizedDistance);
        }

        // Áp dụng volume với master và SFX volume từ AudioManager
        if (AudioManager.instance != null)
        {
            // Volume sẽ được AudioManager tự động điều chỉnh theo master/SFX volume
            audioSource.volume = volume;
        }
        else
        {
            audioSource.volume = volume;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            // Dừng timer khi player thắng
            if (CountdownTimer.instance != null)
            {
                CountdownTimer.instance.StopTimer();
            }

            // Dừng warning sound nếu đang phát
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
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
