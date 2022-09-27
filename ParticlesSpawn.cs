using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesSpawn : MonoBehaviour
{
    private ParticleSystem particles;
    void Start()
    {
        particles = gameObject.GetComponent<ParticleSystem>();
        particles.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (particles.isStopped)
        {
            Destroy(gameObject);
        }
    }
}
