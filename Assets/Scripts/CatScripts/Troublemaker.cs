using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troublemaker : MonoBehaviour
{
    private float timer = 4.0f;

    void Update()
    {
        if (!GetComponent<CatBase>().activated)
            return;
        
        // Every 3-5 seconds move a little
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = Random.Range(3.0f, 5.0f);
            GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized * 8);
        }
    }
}
