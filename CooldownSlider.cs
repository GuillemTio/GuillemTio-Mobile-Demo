using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownSlider : MonoBehaviour
{
    [SerializeField] private Slider cooldownSlider;
    [SerializeField] private JoyAttack joyAttack;
    [SerializeField] private RectTransform sliderTransform;
    [SerializeField] private Transform playerTransform;

    [SerializeField] private float offsetY;

    private float counter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        counter = joyAttack.CooldownCounter;
        if (counter> joyAttack.MAXCOOLDOWNTIME || counter == 0)
        {
            cooldownSlider.value = 1;
        }
        else
        {
            cooldownSlider.value = counter;
        }
        SetPosition();
    }

    private void SetPosition()
    {
        sliderTransform.transform.position = new Vector2(playerTransform.transform.position.x, playerTransform.transform.position.y + offsetY);
    }
}
