using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private PlayerController player;

    [Header("Cài Đặt Di Chuyển")]
    [Tooltip("Tốc độ di chuyển của enemy")]
    public float moveSpeed;

    [Tooltip("Rigidbody component của enemy (tự động tìm nếu để trống)")]
    public Rigidbody theRB;

    [Tooltip("Khoảng cách để enemy bắt đầu đuổi theo player")]
    public float chaseRange = 15f;
    [Tooltip("Khoảng cách tối thiểu để enemy dừng lại và bắn (không tiến gần hơn)")]
    public float stopCloseRange = 4f;

    private float strafeAmount;

    [Header("Cài Đặt Hoạt Hình")]
    [Tooltip("Animator component của enemy")]
    public Animator anim;

    [Header("Cài Đặt Tuần Tra")]
    [Tooltip("Danh sách các điểm tuần tra (patrol points)")]
    public Transform[] patrolPoints;

    [HideInInspector]
    public int currentPatrolPoint;
    [Tooltip("GameObject chứa các điểm tuần tra (sẽ được tách ra khỏi parent)")]
    public Transform pointsHolder;

    [Tooltip("Thời gian chờ tại mỗi điểm tuần tra (giây)")]
    public float pointWaitTime = 3f;
    private float waitCounter;

    private bool isDead;

    [Header("Cài Đặt Máu")]
    [Tooltip("Máu hiện tại của enemy")]
    public float currentHealth = 25f;
    [Tooltip("Máu ban đầu của enemy")]
    public float startingHealth = 25f;

    [Tooltip("Thời gian chờ trước khi enemy biến mất sau khi chết (giây)")]
    public float waitToDisappear = 4f;

    [Header("Cài Đặt Bắn")]
    [Tooltip("Điểm spawn projectile (nơi bắn ra đạn)")]
    public Transform shootPoint;
    [Tooltip("Prefab của projectile để bắn")]
    public EnemyProjectile projectile;
    [Tooltip("Khoảng thời gian giữa các lần bắn (giây)")]
    public float timeBetweenShots;
    private float shotCounter;
    [Tooltip("Sát thương của mỗi viên đạn")]
    public float shotDamage;

    [Header("Cài Đặt Chia Đôi Khi Chết")]
    [Tooltip("Enemy có chia thành 2 enemy nhỏ hơn khi chết không?")]
    public bool splitOnDeath;
    [Tooltip("Kích thước tối thiểu để enemy có thể chia đôi (nhỏ hơn sẽ không chia)")]
    public float minSize = .4f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();

        strafeAmount = Random.Range(-.75f, .75f);

        pointsHolder.SetParent(null);

        waitCounter = Random.Range(.75f, 1.25f) * pointWaitTime;



        shotCounter = timeBetweenShots;

        currentHealth = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead == true)
        {
            waitToDisappear -= Time.deltaTime;

            if(waitToDisappear <= 0)
            {
                transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, Time.deltaTime);

                if(transform.localScale.x == 0)
                {
                    Destroy(gameObject);

                    if (splitOnDeath == false)
                    {
                        Destroy(pointsHolder.gameObject);
                    }
                }
            }


            return;
        }

        float yStore = theRB.linearVelocity.y;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < chaseRange && PlayerController.instance.isDead == false)
        {

            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));

            if (distance > stopCloseRange)
            {
                theRB.linearVelocity = (transform.forward + (transform.right * strafeAmount)) * moveSpeed;

                anim.SetBool("moving", true);
            } else
            {
                theRB.linearVelocity = Vector3.zero;

                anim.SetBool("moving", false);
            }

            shotCounter -= Time.deltaTime;
            if(shotCounter <= 0)
            {
                shootPoint.LookAt(player.theCam.transform.position);

                EnemyProjectile newProjectile = Instantiate(projectile, shootPoint.position, shootPoint.rotation);
                newProjectile.damageAmount = shotDamage;
                newProjectile.transform.localScale = transform.localScale;

                shotCounter = timeBetweenShots;

                anim.SetTrigger("shooting");

                AudioManager.instance.PlaySFX(2);
            }


        } else
        {
            if (patrolPoints.Length > 0)
            {
                if (Vector3.Distance(transform.position, new Vector3(patrolPoints[currentPatrolPoint].position.x, transform.position.y, patrolPoints[currentPatrolPoint].position.z)) < .25f)
                {
                    waitCounter -= Time.deltaTime;

                    theRB.linearVelocity = Vector3.zero;
                    anim.SetBool("moving", false);

                    if (waitCounter <= 0)
                    {

                        currentPatrolPoint++;
                        if (currentPatrolPoint >= patrolPoints.Length)
                        {
                            currentPatrolPoint = 0;
                        }

                        waitCounter = Random.Range(.75f, 1.25f) * pointWaitTime;
                    }
                }
                else
                {
                    transform.LookAt(new Vector3(patrolPoints[currentPatrolPoint].position.x, transform.position.y, patrolPoints[currentPatrolPoint].position.z));

                    theRB.linearVelocity = transform.forward * moveSpeed;

                    anim.SetBool("moving", true);
                }
            }
            else
            {

                theRB.linearVelocity = Vector3.zero;

                anim.SetBool("moving", false);
            }
        }

        theRB.linearVelocity = new Vector3(theRB.linearVelocity.x, yStore, theRB.linearVelocity.z);
    }

    public void TakeDamage(float damageToTake)
    {
        //Debug.Log("Enemy Hit");

        //Destroy(gameObject);

        currentHealth -= damageToTake;

        if (currentHealth <= 0)
        {
            if(splitOnDeath == true)
            {
                if (transform.localScale.x > minSize)
                {
                    startingHealth *= .75f;

                    currentHealth = startingHealth;
                    shotDamage *= .75f;

                    moveSpeed *= .75f;

                    GameObject clone1 = Instantiate(gameObject, transform.position + (transform.right * .5f * transform.localScale.x), Quaternion.identity);
                    GameObject clone2 = Instantiate(gameObject, transform.position + (-transform.right * .5f * transform.localScale.x), Quaternion.identity);

                    clone1.transform.localScale = transform.localScale * .75f;
                    clone2.transform.localScale = transform.localScale * .75f;
                }
            }


            anim.SetTrigger("die");

            isDead = true;

            theRB.linearVelocity = Vector3.zero;
            theRB.isKinematic = true;

            GetComponent<Collider>().enabled = false;

            // Tăng số enemy bị tiêu diệt
            if (KillTracker.instance != null)
            {
                KillTracker.instance.AddKill();
            }
        }
    }
}
