using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catsploder : MonoBehaviour
{
    float explodeRadius = 2f;
    float explodeStrength = 100.0f;

    void Start()
    {
        GameManager.OnEndOfRound += EndOfRound;
    }

    void EndOfRound()
    {
        if (!GetComponent<CatBase>().activated)
            return;

        // Do a circle raycast around the cat
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, explodeRadius, Vector2.zero);
        foreach (RaycastHit2D hit in hits)
        {
            // Check if raycast hit another cat
            if (hit.collider.gameObject.GetComponent<CatBase>() != null  && hit.collider.gameObject.GetComponent<CatBase>() != GetComponent<CatBase>())
            {
                // Push away from the other cat
                Vector2 direction = hit.collider.gameObject.transform.position - transform.position;
                float strength = 1 - (direction.magnitude / explodeRadius);
                hit.collider.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * strength * explodeStrength);
            }
        }
    }

    void OnDestroy()
    {
        GameManager.OnEndOfRound -= EndOfRound;
    }
}
