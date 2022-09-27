using UnityEngine;

public class JoyAttack : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject player;
    [SerializeField] private RectTransform backJoystick;
    [SerializeField] private RectTransform joystick;
    [SerializeField] private float atkSpeed;
    [SerializeField] private float moveLimit;
    [SerializeField] private int windowMargin = 50;

    private Vector2 joystickStart;
    private Vector2 pointA;
    private Vector2 pointB;
    private Vector2 vectorPoints;

    private bool touchStarted = false;
    private bool touchChosen = false;
    private bool readyToAttack = false;

    private Touch firstTouch;

    private int savedNum = -1;
    private int windowWidth;

    private float angle;

    private Rigidbody2D _rigidbody2D;

    private bool isCounting;
    public const float MAX_COOLDOWN_TIME = 1.0f;
    public float MAXCOOLDOWNTIME
    {
        get => MAX_COOLDOWN_TIME;
    }

    public float cooldownCounter;
    public float CooldownCounter
    {
        get => cooldownCounter;
        private set => cooldownCounter = value;
    }

    /*public float cooldownCounter
    {
        get { return cooldownCounter; }
        private set { cooldownCounter = value; }
    }*/

    void Start()
    {
        joystickStart = joystick.localPosition;
        windowWidth = Screen.width;

        _rigidbody2D = player.GetComponent<Rigidbody2D>();

        cooldownCounter = 0;
        isCounting = false;
    }

    // Update is called once per frame
    void Update()
    {
        CooldownChecker();
        if (Input.touchCount > 0)
        {
            if (!touchChosen)
            {
                ChooseTouch();
            }
            else
            {
                while (savedNum + 1 > Input.touchCount)
                {
                    savedNum--;
                }
                firstTouch.position = Input.GetTouch(savedNum).position;
                touchStarted = true;
            }

        }
        else
        {

            if (readyToAttack) { Attack(true); }

            readyToAttack = false;
            touchStarted = false;

            firstTouch.phase = TouchPhase.Began;

            backJoystick.localPosition = joystickStart;
            joystick.localPosition = joystickStart;
            pointA = joystickStart;
            pointB = joystickStart;
        }
        if (touchStarted)
        {
            if (firstTouch.position.x <= (windowWidth / 2) + windowMargin)
            {
                firstTouch.phase = TouchPhase.Ended;
            }
            if (firstTouch.phase == TouchPhase.Began)
            {
                backJoystick.position = new Vector3(Camera.main.ScreenToWorldPoint(firstTouch.position).x, Camera.main.ScreenToWorldPoint(firstTouch.position).y, 0);
                joystick.position = new Vector3(Camera.main.ScreenToWorldPoint(firstTouch.position).x, Camera.main.ScreenToWorldPoint(firstTouch.position).y, 0);
                pointA = firstTouch.position;
                pointB = firstTouch.position;
                firstTouch.phase = TouchPhase.Moved;
            }
            else if (firstTouch.phase == TouchPhase.Moved)
            {
                pointB = firstTouch.position;
                vectorPoints = pointB - pointA;

                angle = Mathf.Acos(vectorPoints.x / Mathf.Sqrt((vectorPoints.x * vectorPoints.x) + (vectorPoints.y * vectorPoints.y))); //angle for shield and joystick
                if (pointB.y < pointA.y)
                {
                    angle = 2 * Mathf.PI - angle;
                }

                if (Mathf.Sqrt((vectorPoints.x * vectorPoints.x) + (vectorPoints.y * vectorPoints.y)) > moveLimit)
                {
                    pointB.x = pointA.x + moveLimit * Mathf.Cos(angle);
                    pointB.y = pointA.y + moveLimit * Mathf.Sin(angle);
                }
                joystick.position = new Vector3(Camera.main.ScreenToWorldPoint(pointB).x, Camera.main.ScreenToWorldPoint(pointB).y, 0);

                RotateShield(angle);
                Attack(false);
                readyToAttack = true;

            }
            else if (firstTouch.phase == TouchPhase.Ended)
            {
                if (readyToAttack) { Attack(true); }

                readyToAttack = false;
                touchStarted = false;
                touchChosen = false;

                backJoystick.localPosition = joystickStart;
                joystick.localPosition = joystickStart;
                pointA = joystickStart;
                pointB = joystickStart;

                firstTouch.phase = TouchPhase.Began;
            }
        }
    }

    private void ChooseTouch()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).position.x > (windowWidth / 2) + windowMargin)
            {
                firstTouch.position = Input.GetTouch(i).position;
                savedNum = i;
                touchChosen = true;
                touchStarted = true;
                i = Input.touchCount;
            }
        }
    }

    private void Attack(bool gonnaAttack)
    {


        if (gonnaAttack)
        {
            _rigidbody2D.drag = 4;
            if (!isCounting)
            {
                _rigidbody2D.velocity = (pointA - pointB) * atkSpeed / 10;
                isCounting = true;
            }
        }
        else
        {
            _rigidbody2D.drag = 25;
        }

    }

    private void RotateShield(float shieldAngle)
    {
        Quaternion desiredRotation;
        shieldAngle = (shieldAngle * Mathf.Rad2Deg) - 180;
        //player.transform.right = -vectorPoints;
        //player.transform.rotation = new Quaternion(0,0,shieldAngle,0);
        //Debug.Log(shieldAngle);
        if (!float.IsNaN(shieldAngle))
        {
            desiredRotation = Quaternion.Euler(0f, 0f, shieldAngle - 90);
            player.transform.localRotation = Quaternion.RotateTowards(player.transform.localRotation, desiredRotation, 1000 * Time.deltaTime);

        }
    }

    private void CooldownChecker()
    {
        if (isCounting)
        {

            if (cooldownCounter >= MAX_COOLDOWN_TIME)
            {
                isCounting = false;
                cooldownCounter = 0;

            }
            else
            {
                cooldownCounter += Time.deltaTime;

            }
        }
    }
}
