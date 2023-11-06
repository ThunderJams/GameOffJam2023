using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CatBase : MonoBehaviour
{
    bool aboveScreen = false;

    public float damage = 10f;

    [SerializeField] GameObject indicator;

    void Update()
    {
        if (aboveScreen == false && transform.position.y > 5.5f)
        {
            aboveScreen = true;
        }
        else if (aboveScreen == true && transform.position.y < 5.5f)
        {
            Activate();
        }

        if (aboveScreen == true)
        {
            indicator.SetActive(true);
            indicator.transform.position = new Vector3(transform.position.x, 4.5f, 0);
            indicator.transform.rotation = Quaternion.Euler(0, 0, 0);
            float distanceFromScreen = (transform.position.y - 5.5f) * 0.05f;
            indicator.transform.localScale = new Vector3(1 - distanceFromScreen, 1 - distanceFromScreen, 1);
        }
        else
        {
            indicator.SetActive(false);
        }

        if (transform.position.y < -5.5f)
            FallOffScreen();
    }

    public void Activate()
    {
        aboveScreen = false;
        GetComponent<SpriteRenderer>().sortingOrder = 10;
        GetComponent<Rigidbody2D>().excludeLayers = 8;
    }

    void FallOffScreen()
    {
        GameManager.instance.RemoveCat(gameObject);
        Destroy(gameObject);
    }
}
