using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidCat : MonoBehaviour
{
    public float gravityRadius = 3f;
    public float gravityPull = 0.05f;
    public int catsToEat = 4;

    public AudioClip suckNoise;
    void Update()
    {
        if (!GetComponent<CatBase>().activated)
            return;

        // Do a circle raycast around the cat
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, gravityRadius, Vector2.zero);
        foreach (RaycastHit2D hit in hits)
        {
            // Check if raycast hit another cat
            if (hit.collider.gameObject.GetComponent<CatBase>() != null && hit.collider.gameObject.GetComponent<CatBase>() != GetComponent<CatBase>())
            {
                // Pull cats in
                Vector2 direction = hit.collider.gameObject.transform.position - transform.position;
                float strength = 1 - (direction.magnitude / gravityRadius);
                hit.collider.gameObject.GetComponent<Rigidbody2D>().AddForce(-direction * strength * gravityPull);
            }
        }

        // Do a circle raycast around the cat
        RaycastHit2D[] eatHits = Physics2D.CircleCastAll(transform.position, 0.5f, Vector2.zero);
        foreach (RaycastHit2D hit in eatHits)
        {
            // Check if raycast hit another cat
            if (hit.collider.gameObject.GetComponent<CatBase>() != null && hit.collider.gameObject.GetComponent<CatBase>() != GetComponent<CatBase>() && hit.collider.gameObject.GetComponent<CatBase>().activated)
            {
                // Eat 
                Destroy(hit.collider.gameObject);
                catsToEat--;
                if (catsToEat <= 0)
                {
                    Destroy(gameObject);
                }
                AudioManager.instance.PlaySound(suckNoise.name,1,Random.Range(0.9f,1.1f));
            }
        }
    }
}
