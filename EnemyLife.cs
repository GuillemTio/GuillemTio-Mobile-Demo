using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLife : MonoBehaviour
{

    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject objParticles;

    private Collider2D playerShieldCol;
    
    void Start()
    {
        playerShieldCol = shield.GetComponent<PolygonCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider== playerShieldCol)
        {
            Instantiate(objParticles).transform.position = gameObject.transform.position;
            Destroy(gameObject);
        }
    }


    void Update()
    {
        
    }
}
