using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int currentHealth = 5;
    public float moveSpeed = 8;
    public float distanceToChase = 10f, distanceToLose = 15f;
    private bool chasing;

    private Vector3 targetPoint;

    public Rigidbody theRB;

    // Start is called before the first frame update
    void Start()
    {
        
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
            }
        }
        else
        {

            if(Vector3.Distance(transform.position, targetPoint) > distanceToLose)
            {
                chasing = false;
            }

            transform.LookAt(targetPoint);

            theRB.velocity = transform.forward * moveSpeed;
        }

        
    }

    public void DamageEnemy(int damage)
    {
        currentHealth-=damage;

        if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
