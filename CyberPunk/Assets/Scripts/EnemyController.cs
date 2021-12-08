using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public int currentHealth = 5;
    public float distanceToChase = 10f, distanceToLose = 15f, distanceToStop = 2f;
    private bool chasing;

    private Vector3 targetPoint, startPoint; //start point is the inital point of the enemy

    public NavMeshAgent agent;

    public float keepChasingTime = 5f;
    private float chaseCounter;

    public GameObject bullet;
    public Transform firePoint;

    public float fireRate, waitBetweenShots, timeToShoot = 1f;
    private float fireCount, shotWaitCounter, shootTimeCounter;

    public Animator anim;

    public void DamageEnemy(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        startPoint = transform.position;

        shootTimeCounter = timeToShoot;
        shotWaitCounter = waitBetweenShots; 
    }

    // Update is called once per frame
    void Update()
    {
        targetPoint = PlayerController.instance.transform.position;
        targetPoint.y = transform.position.y;
        if (!chasing)
        {
            if(Vector3.Distance(transform.position, targetPoint) < distanceToChase)
            {
                chasing = true;

                shootTimeCounter = timeToShoot;
                shotWaitCounter = waitBetweenShots;
            }

            if(chaseCounter > 0)
            { 
                chaseCounter -= Time.deltaTime;

                if (chaseCounter <= 0)
                {
                    agent.destination = startPoint;
                }  
            }

            if(agent.remainingDistance < .25f)
            {
                anim.SetBool("isMoving", false);
            }
            else
            {
                anim.SetBool("isMoving", true);
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, targetPoint) > distanceToStop)
            {
                agent.destination = targetPoint;
            }
            else
            {
                agent.destination = transform.position;
            }

            if(Vector3.Distance(transform.position, targetPoint) > distanceToLose)
            {
                chasing = false;

                chaseCounter = keepChasingTime;
            }

            if (shotWaitCounter > 0)
            {

                shotWaitCounter -= Time.deltaTime;

                if(shotWaitCounter <= 0)
                {
                    shootTimeCounter = timeToShoot;
                }

                anim.SetBool("isMoving", true);

            }
            else 
            {
                shootTimeCounter -= Time.deltaTime;
                if (shootTimeCounter > 0)
                {
                    fireCount -= Time.deltaTime;
 

                    if (fireCount <= 0)
                    {
                        fireCount = fireRate;

                        firePoint.LookAt(PlayerController.instance.transform.position + new Vector3(0f,1.2f,0f));

                        //check the angle to the player
                        Vector3 targetDirection = PlayerController.instance.transform.position - transform.position;
                        float angle = Vector3.SignedAngle(targetDirection, transform.forward, Vector3.up);

                        if(Mathf.Abs(angle) < 30f)
                        {
                            Instantiate(bullet, firePoint.position, firePoint.rotation);

                            anim.SetTrigger("fireShot");
                        }
                        else
                        {
                            shotWaitCounter = waitBetweenShots;
                        }

                        
                    }


                    agent.destination = transform.position;  
                }
                else
                {
                    shotWaitCounter = waitBetweenShots;
                }

                anim.SetBool("isMoving", false);
            }
        }

        
    }

}
