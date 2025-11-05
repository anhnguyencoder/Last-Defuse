using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class PlayerUnitFrame : MonoBehaviour
{
    [Header("Avatar Components")]
    public Image avatarImage;
    public Image avatarBackground;
    public Image avatarMask;
    public Image avatarOverlay;
    
    [Header("Health Bar Components")]
    public Image healthBarBackground;
    public Image healthBarFill;
    public TMP_Text healthText;
    
    [Header("Player Info")]
    public TMP_Text usernameText;
    public TMP_Text levelText;
    
    [Header("Settings")]
    public string playerUsername = "USERNAME";
    [Tooltip("Nếu true, sẽ tự động lấy level từ tên scene. Nếu false, sẽ dùng playerLevel")]
    public bool autoDetectLevel = true;
    public int playerLevel = 60; // Dùng khi autoDetectLevel = false
    
    [Header("Health Bar Animation")]
    [Tooltip("Tốc độ animation của thanh máu (càng cao càng nhanh)")]
    public float healthBarAnimationSpeed = 2f;
    
    private float currentHealth;
    private float maxHealth;
    private float currentFillAmount;
    private Coroutine healthBarAnimationCoroutine;
    
    private void Start()
    {
        // Initialize with player health controller
        if (PlayerHealthController.instance != null)
        {
            maxHealth = PlayerHealthController.instance.maxHealth;
            currentHealth = maxHealth;
        }
        
        // Initialize fill amount
        if (healthBarFill != null)
        {
            currentFillAmount = maxHealth > 0 ? currentHealth / maxHealth : 0f;
            healthBarFill.fillAmount = currentFillAmount;
        }
        
        // Load saved username from PlayerPrefs
        playerUsername = PlayerPrefs.GetString("PlayerUsername", "USERNAME");
        
        // Set initial values
        UpdateUsername();
        UpdateLevelFromScene();
        UpdateHealthBar();
    }
    
    public void UpdateHealth(float current, float max)
    {
        currentHealth = current;
        maxHealth = max;
        UpdateHealthBar();
    }
    
    public void SetAvatar(Sprite avatarSprite)
    {
        if (avatarImage != null && avatarSprite != null)
        {
            avatarImage.sprite = avatarSprite;
        }
    }
    
    public void SetUsername(string username)
    {
        playerUsername = username;
        UpdateUsername();
    }
    
    public void SetLevel(int level)
    {
        playerLevel = level;
        UpdateLevel();
    }
    
    private void UpdateUsername()
    {
        if (usernameText != null)
        {
            usernameText.text = playerUsername;
        }
    }
    
    private void UpdateLevel()
    {
        if (levelText != null)
        {
            levelText.text = playerLevel.ToString();
        }
    }
    
    private void UpdateLevelFromScene()
    {
        if (autoDetectLevel)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            int detectedLevel = ExtractLevelNumber(sceneName);
            
            if (detectedLevel > 0)
            {
                playerLevel = detectedLevel;
            }
        }
        
        UpdateLevel();
    }
    
    private int ExtractLevelNumber(string sceneName)
    {
        // Pattern để tìm số trong tên scene (ví dụ: "Level 1" -> 1, "Level 2" -> 2, "Test 1" -> 1)
        // Tìm số đầu tiên trong tên scene
        Match match = Regex.Match(sceneName, @"\d+");
        
        if (match.Success)
        {
            if (int.TryParse(match.Value, out int levelNumber))
            {
                return levelNumber;
            }
        }
        
        // Xử lý các trường hợp đặc biệt
        if (sceneName.Contains("Boss Arena") || sceneName.Contains("Boss"))
        {
            return 99; // Hoặc giá trị đặc biệt cho boss
        }
        
        // Nếu không tìm thấy số, trả về 0 hoặc giá trị mặc định
        return 0;
    }
    
    private void UpdateHealthBar()
    {
        // Calculate target fill amount
        float targetFillAmount = maxHealth > 0 ? currentHealth / maxHealth : 0f;
        
        // Debug log
        Debug.Log($"UpdateHealthBar: Health={currentHealth}/{maxHealth}, TargetFill={targetFillAmount}, CurrentFill={currentFillAmount}");
        
        // Animate health bar smoothly
        if (healthBarFill != null)
        {
            // Ensure Image Type is Filled
            if (healthBarFill.type != Image.Type.Filled)
            {
                Debug.LogWarning("Health Bar Fill Image Type is not set to Filled! Setting it now...");
                healthBarFill.type = Image.Type.Filled;
                healthBarFill.fillMethod = Image.FillMethod.Horizontal;
                healthBarFill.fillOrigin = 0; // Left
            }
            
            // Stop any existing animation
            if (healthBarAnimationCoroutine != null)
            {
                StopCoroutine(healthBarAnimationCoroutine);
            }
            
            // Start new animation
            healthBarAnimationCoroutine = StartCoroutine(AnimateHealthBar(targetFillAmount));
        }
        else
        {
            Debug.LogError("Health Bar Fill is NULL! Please assign it in Inspector.");
        }
        
        // Update health text immediately (e.g., "200/250")
        if (healthText != null)
        {
            healthText.text = Mathf.RoundToInt(currentHealth) + "/" + Mathf.RoundToInt(maxHealth);
        }
    }
    
    private IEnumerator AnimateHealthBar(float targetFillAmount)
    {
        float startFillAmount = currentFillAmount;
        float elapsedTime = 0f;
        
        Debug.Log($"Starting animation: {startFillAmount} -> {targetFillAmount}");
        
        // Calculate animation duration based on distance
        float distance = Mathf.Abs(targetFillAmount - startFillAmount);
        float duration = distance / healthBarAnimationSpeed;
        
        // Ensure minimum duration for visibility
        if (duration < 0.1f)
        {
            duration = 0.1f;
        }
        
        // If distance is very small, just set it immediately
        if (distance < 0.001f)
        {
            currentFillAmount = targetFillAmount;
            if (healthBarFill != null)
            {
                healthBarFill.fillAmount = currentFillAmount;
            }
            yield break;
        }
        
        // Animate from current to target
        while (elapsedTime < duration)
        {
            // Use unscaled time to work even when game is paused
            elapsedTime += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            
            // Use smooth step for more natural animation
            currentFillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, t);
            
            if (healthBarFill != null)
            {
                healthBarFill.fillAmount = currentFillAmount;
            }
            
            yield return null;
        }
        
        // Ensure we end at exact target value
        currentFillAmount = targetFillAmount;
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = currentFillAmount;
        }
        
        Debug.Log($"Animation complete: {currentFillAmount}");
        healthBarAnimationCoroutine = null;
    }
    
    // Called from PlayerHealthController when health changes
    public void OnHealthChanged(float newHealth)
    {
        Debug.Log($"OnHealthChanged called with: {newHealth}");
        
        if (PlayerHealthController.instance != null)
        {
            UpdateHealth(newHealth, PlayerHealthController.instance.maxHealth);
        }
        else
        {
            Debug.LogWarning("PlayerHealthController.instance is NULL!");
        }
    }
}
