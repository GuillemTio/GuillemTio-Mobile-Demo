
using UnityEngine;

public class EnemySearchnShoot : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed;

    private const float MAX_SHOOTING_TIME = 2.5f;
    private const float MIN_SHOOTING_TIME = 1.5f;

    private float timeToShoot;
    private Vector2 shootPosition;
    

    private bool ableToShoot;
    private float timer;
    private Vector2 direction;



    void Start()
    {
        timeToShoot = Random.Range(MIN_SHOOTING_TIME, MAX_SHOOTING_TIME);
    }

    // Update is called once per frame
    void Update()
    {
        direction = (player.transform.position - gameObject.transform.position).normalized;
        Search(direction);

        if (ableToShoot) { Shoot(direction); }
        timer += Time.deltaTime;
        if (timer >= timeToShoot)
        {
            ableToShoot = true;
            timer = 0;
            timeToShoot = Random.Range(MIN_SHOOTING_TIME, MAX_SHOOTING_TIME);
        }
    }

    private void Search(Vector2 direction)
    {
        Quaternion desiredRotation;

        float angleToRotate = (Mathf.Atan(direction.y / direction.x) * Mathf.Rad2Deg);

        if (player.transform.position.x < gameObject.transform.position.x)
        {
            angleToRotate += 180;
        }

        desiredRotation = Quaternion.Euler(0f, 0f, angleToRotate - 90);
        gameObject.transform.localRotation = Quaternion.RotateTowards(gameObject.transform.localRotation, desiredRotation, 100 * Time.deltaTime);

    }

    private void Shoot(Vector2 direction)
    {
        shootPosition = gameObject.transform.GetChild(0).transform.position;

        GameObject actualBullet = Instantiate(bullet);
        actualBullet.transform.position = shootPosition;
        actualBullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
        actualBullet.transform.localRotation = gameObject.transform.localRotation;

        ableToShoot = false;
    }
}
