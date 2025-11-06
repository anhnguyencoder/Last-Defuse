using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [Header("Cài Đặt Hồi Máu")]
    [Tooltip("Lượng máu được hồi khi nhặt item này")]
    public float healAmount = 25f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerHealthController.instance.Heal(healAmount);

            Destroy(gameObject);

            AudioManager.instance.PlaySFX(5);
        }
    }
}
