using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
    [SerializeField] private ParticleSystem shieldParticles;

    private GameObject player;
    private PlayerLife pLife;
    private Collider2D playerShield;
    private Collider2D playerHit;

    private Collider2D bulletCol;

    private bool hit;

    void Start()
    {
        player = GameObject.Find("Player");
        playerShield = player.transform.GetChild(0).GetComponent<PolygonCollider2D>();
        playerHit = player.transform.GetChild(1).GetComponent<CircleCollider2D>();
        pLife = player.GetComponent<PlayerLife>();

        bulletCol = gameObject.GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hit && !bulletCol.bounds.Intersects(playerShield.bounds))
        {
            pLife.CheckCol(gameObject.name);
            Destroy(gameObject);
            hit = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.collider == playerShield)
        {
            shieldParticles = Instantiate(shieldParticles);
            shieldParticles.transform.position = gameObject.transform.position;
            shieldParticles.transform.localRotation = gameObject.transform.localRotation;
            Destroy(gameObject);
        }
        else if (collision.collider == playerHit && !bulletCol.bounds.Intersects(playerShield.bounds))
        {
            hit = true; //doublecheck
        }
        else if (collision.collider.tag != "Enemy")
        {
            Destroy(gameObject);
        }
    }


    void OnBecameInvisible() 
    {
        Destroy(gameObject);
    }
}
