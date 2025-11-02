using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    private void Awake()
    {
        instance = this;
    }

    public float maxHealth = 100f;
    private float currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;

        UIController.instance.UpdateHealthText(currentHealth);
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if(currentHealth <= 0)
        {
            currentHealth = 0;

            //Debug.Log("Player is dead!");

            PlayerController.instance.isDead = true;

            UIController.instance.ShowDeathScreen();

            AudioManager.instance.PlaySFX(6);
        } else
        {
            AudioManager.instance.PlaySFX(7);
        }

        UIController.instance.UpdateHealthText(currentHealth);
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UIController.instance.UpdateHealthText(currentHealth);
    }
}
