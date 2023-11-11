using UnityEngine;

public class EnemySPECIALSearchnShoot : MonoBehaviour
{
    private GameObject player;

    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float minPlayerDistance;
    [SerializeField] private float timeBetweenBullets;

    private const float MAX_SHOOTING_TIME = 2.5f;
    private const float MIN_SHOOTING_TIME = 1.5f;

    private float timeToShoot;
    private bool ableToShoot;
    private float timerBullets;
    private float timerShoot;
    private Vector2 direction;

    bool firstBulletShot = false;
    bool secondBulletShot = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        timeToShoot = Random.Range(MIN_SHOOTING_TIME, MAX_SHOOTING_TIME);
    }

    // Update is called once per frame
    void Update()
    {
        direction = (player.transform.position - gameObject.transform.position).normalized;
        Search(direction);

        if (ableToShoot) { Shoot(direction); }
        else
        {
            timerShoot += Time.deltaTime;
            if (timerShoot >= timeToShoot)
            {
                ableToShoot = true;
                timerShoot = 0;
                timeToShoot = Random.Range(MIN_SHOOTING_TIME, MAX_SHOOTING_TIME);
            }
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

        float playerToEnemyDistance = Mathf.Sqrt(Mathf.Pow(player.transform.position.x - gameObject.transform.position.x, 2) + Mathf.Pow(player.transform.position.y - gameObject.transform.position.y, 2));

        if (playerToEnemyDistance > minPlayerDistance)
        {
            ableToShoot = false;
        }

    }

    private void Shoot(Vector2 direction)
    {
        Vector2 shootPosition1 = gameObject.transform.GetChild(0).transform.position;
        Vector2 shootPosition2 = gameObject.transform.GetChild(1).transform.position;
        Vector2 shootPosition3 = gameObject.transform.GetChild(2).transform.position;

        Vector2 shootDirection1 = (shootPosition1 - (Vector2)transform.position).normalized;
        Vector2 shootDirection2 = (shootPosition2 - (Vector2)transform.position).normalized;
        Vector2 shootDirection3 = (shootPosition3 - (Vector2)transform.position).normalized;

        timerBullets += Time.deltaTime;
        if ((timerBullets < timeBetweenBullets) && !firstBulletShot)
        {
            SpawnBullet(shootDirection1, shootPosition1);
            firstBulletShot = true;
        }
        else if ((timerBullets >= timeBetweenBullets) && (timerBullets < timeBetweenBullets * 2) && !secondBulletShot)
        {
            SpawnBullet(shootDirection2, shootPosition2);
            secondBulletShot = true;
        }
        else if (timerBullets >= timeBetweenBullets * 2)
        {
            firstBulletShot = false;
            secondBulletShot = false;
            SpawnBullet(shootDirection3, shootPosition3);
            ableToShoot = false;
            timerBullets = 0;
        }

    }

    private void SpawnBullet(Vector2 direction, Vector2 position)
    {

        GameObject actualBullet = Instantiate(bullet);
        actualBullet.transform.position = position;
        actualBullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
        actualBullet.transform.localRotation = gameObject.transform.localRotation;
    }
}
