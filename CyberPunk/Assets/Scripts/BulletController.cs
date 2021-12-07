using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public float moveSpeed, lifetime;

    public Rigidbody theRB;

    public GameObject impactEffect;

    public int damage = 1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        theRB.velocity = transform.forward * moveSpeed;

        //destroy object depending on lifetime
        lifetime -= Time.deltaTime;
        if(lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
 
    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyController>().DamageEnemy(damage);
        }

        Destroy(gameObject);
        Instantiate(impactEffect, transform.position + (transform.forward * (-moveSpeed) * Time.deltaTime), transform.rotation);
    }
}
