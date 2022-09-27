using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    private float currentX;
    private float currentY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentX = Mathf.Clamp(player.transform.position.x, minX, maxX);
        currentY = Mathf.Clamp(player.transform.position.y, minY, maxY);

        gameObject.transform.position =Vector3.Lerp(gameObject.transform.position, new Vector3(currentX, currentY, -10),0.05f);
    }
}
