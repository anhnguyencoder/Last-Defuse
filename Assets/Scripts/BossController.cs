using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Cài Đặt Spawn Đạn")]
    [Tooltip("Danh sách các điểm spawn đạn")]
    public Transform[] ammoPoints;

    [Tooltip("Prefab của ammo pickup để spawn")]
    public GameObject ammoPickup;

    [Tooltip("Thời gian giữa các lần spawn đạn (giây)")]
    public float ammoSpawnTime;
    private float ammoCounter;

    [Header("Cài Đặt Kiểm Tra")]
    [Tooltip("Khoảng thời gian kiểm tra xem boss đã bị đánh bại chưa (giây)")]
    public float checkInterval;
    private float checkCounter;

    [Header("Cài Đặt Exit")]
    [Tooltip("GameObject level exit (sẽ được kích hoạt khi boss chết)")]
    public GameObject levelExit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ammoCounter = ammoSpawnTime;

        checkCounter = checkInterval;

        AudioManager.instance.PlayBossMusic();
    }

    // Update is called once per frame
    void Update()
    {
        checkCounter -= Time.deltaTime;

        if (checkCounter <= 0)
        {
            checkCounter = checkInterval;

            if(FindFirstObjectByType<EnemyController>() == null)
            {
                Debug.Log("Boss Is Defeated");

                gameObject.SetActive(false);

                levelExit.SetActive(true);

                AudioManager.instance.PlayLevelMusic();
            }
        }


        ammoCounter -= Time.deltaTime;
        if(ammoCounter <= 0)
        {
            ammoCounter = ammoSpawnTime;

            Instantiate(ammoPickup, ammoPoints[Random.Range(0, ammoPoints.Length)].position, Quaternion.identity);
        }
    }
}
