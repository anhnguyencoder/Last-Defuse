using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("Cài Đặt Di Chuyển")]
    [Tooltip("Tốc độ di chuyển của projectile")]
    public float moveSpeed = 10f;

    [Header("Cài Đặt Sát Thương")]
    [Tooltip("Sát thương của projectile")]
    public float damageAmount = 15f;

    [Header("Component")]
    [Tooltip("Rigidbody component của projectile")]
    public Rigidbody theRB;

    [Header("Hiệu Ứng")]
    [Tooltip("Hiệu ứng khi va chạm với môi trường")]
    public GameObject impactEffect;
    [Tooltip("Hiệu ứng khi va chạm với player")]
    public GameObject damageEffect;

    [Header("Cài Đặt Thời Gian")]
    [Tooltip("Thời gian tồn tại của projectile trước khi tự hủy (giây)")]
    public float lifetime = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        theRB.linearVelocity = transform.forward * moveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Instantiate(damageEffect, transform.position, Quaternion.identity);

            //Debug.Log("Damaging Player for " + damageAmount);

            PlayerHealthController.instance.TakeDamage(damageAmount);

        } else
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
