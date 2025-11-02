using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private PlayerController player;

    public float moveSpeed;

    public Rigidbody theRB;

    public float chaseRange = 15f, stopCloseRange = 4f;

    private float strafeAmount;

    public Animator anim;

    public Transform[] patrolPoints;

    [HideInInspector]
    public int currentPatrolPoint;
    public Transform pointsHolder;

    public float pointWaitTime = 3f;
    private float waitCounter;

    private bool isDead;

    public float currentHealth = 25f;
    public float startingHealth = 25f;

    public float waitToDisappear = 4f;

    public Transform shootPoint;
    public EnemyProjectile projectile;
    public float timeBetweenShots;
    private float shotCounter;
    public float shotDamage;

    public bool splitOnDeath;
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
        }
    }
}
