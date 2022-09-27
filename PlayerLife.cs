using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private Slider playerLifeSlider;

    float lifeCounter;

    // Start is called before the first frame update
    void Start()
    {
        lifeCounter = 100;
        playerLifeSlider.value = lifeCounter;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void GotHit(int typeOfDamage)
    {
        switch (typeOfDamage)
        {
            case 0:
                lifeCounter -= 30;
                break;

            case 1:
                lifeCounter -= 50;
                break;
        }
        playerLifeSlider.value = lifeCounter;

        if (lifeCounter <= 0)
        {
            Destroy(gameObject);
            Destroy(GameObject.Find("Joysticks"));
        }
    }

    public void CheckCol(string gameObjectName)
    {

        if (gameObjectName == "CommonBullet(Clone)")
        {
            GotHit(0);
        } 
    }
}
