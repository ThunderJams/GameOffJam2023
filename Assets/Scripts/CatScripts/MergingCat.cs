using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergingCat : MonoBehaviour
{
    private bool merged = false;
    void Update()
    {
        if (!GetComponent<CatBase>().activated)
            return;

        if (merged)
        {
            // If merged, do nothing
            return;
        }

        // Do a circle raycast around the cat
        RaycastHit2D[] eatHits = Physics2D.CircleCastAll(transform.position, 0.5f, Vector2.zero);
        foreach (RaycastHit2D hit in eatHits)
        {
            // Check if raycast hit another cat
            if (hit.collider.gameObject.GetComponent<CatBase>() != null && hit.collider.gameObject.GetComponent<CatBase>() != GetComponent<CatBase>() && hit.collider.gameObject.GetComponent<CatBase>().activated)
            {
                // Merge
                Destroy(hit.collider.gameObject);
                merged = true;
            }
        }
    }
}
