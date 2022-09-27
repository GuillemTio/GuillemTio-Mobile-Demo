using UnityEngine;

public class JoyMove : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private RectTransform backJoystick;
    [SerializeField] private RectTransform joystick;
    [SerializeField] private float speed;
    [SerializeField] private float moveLimit;
    [SerializeField] private int windowMargin = 50;
    private Vector2 joystickStart;
    private Vector2 pointA;
    private Vector2 pointB;
    private Vector2 vectorPoints;
    private bool touchStarted = false;
    private bool touchChosen = false;
    private Touch firstTouch;
    private int savedNum = -1;
    private int windowWidth;
    private float angle;
    private float _dt;


    void Start()
    {
        joystickStart = joystick.localPosition;
        windowWidth = Screen.width;
    }

    // Update is called once per frame
    void Update()
    {
        _dt = Time.deltaTime;

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
            touchStarted = false;
            firstTouch.phase = TouchPhase.Began;
            backJoystick.localPosition = joystickStart;
            joystick.localPosition = joystickStart;
            pointA = joystickStart;
            pointB = joystickStart;
        }
        if (touchStarted)
        {
            if (firstTouch.position.x >= (windowWidth / 2) - windowMargin)
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

                if(Mathf.Sqrt((vectorPoints.x* vectorPoints.x) + (vectorPoints.y* vectorPoints.y)) > moveLimit)
                {
                    angle = Mathf.Acos((1 * vectorPoints.x) / Mathf.Sqrt((vectorPoints.x * vectorPoints.x) + (vectorPoints.y * vectorPoints.y)));
                    if (pointB.y< pointA.y)
                    {
                        angle = 2*Mathf.PI-angle;
                    }
                    pointB.x = pointA.x + moveLimit * Mathf.Cos(angle);
                    pointB.y = pointA.y + moveLimit * Mathf.Sin(angle);
                }
                joystick.position = new Vector3(Camera.main.ScreenToWorldPoint(pointB).x, Camera.main.ScreenToWorldPoint(pointB).y, 0);
                _rigidbody2D.velocity += (pointB - pointA) * speed * _dt / 20;

            }
            else if (firstTouch.phase == TouchPhase.Ended)
            {
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
            if (Input.GetTouch(i).position.x < (windowWidth / 2) - windowMargin)
            {
                firstTouch.position = Input.GetTouch(i).position;
                savedNum = i;
                touchChosen = true;
                touchStarted = true;
                i = Input.touchCount;
            }
        }
    }
}
