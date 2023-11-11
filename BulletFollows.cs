using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFollows : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D _rb;
    [SerializeField] private float smoothTime;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        //float rotateAmount = Vector3.Cross(direction, transform.up).z;
        //Debug.Log("amount de rotar:" + rotateAmount);
        //_rb.angularVelocity = rotateAmount * rotateSpeed;
        //Debug.Log("angu velo:" + _rb.angularVelocity);
        Vector2 currentVel = Vector2.one;
        _rb.velocity = Vector2.SmoothDamp(_rb.velocity, direction * 10f, ref currentVel, smoothTime);
        

        Quaternion desiredRotation;

        float angleToRotate = (Mathf.Atan(direction.y / direction.x) * Mathf.Rad2Deg);

        if (player.transform.position.x < gameObject.transform.position.x)
        {
            angleToRotate += 180;
        }

        desiredRotation = Quaternion.Euler(0f, 0f, angleToRotate - 90);
        gameObject.transform.localRotation = Quaternion.RotateTowards(gameObject.transform.localRotation, desiredRotation, 150 * Time.deltaTime);
    }
}
