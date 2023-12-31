using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatKing : MonoBehaviour
{
    public float gravityRadius = 3f;
    public float gravityPull = 0.01f;

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
    }
}
