using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonCat : MonoBehaviour
{
    public bool isRed;

    float repelRadius = 3f;
    float repelForce = 0.05f;
    public Sprite calmSprite;
    public Sprite angrySprite;
    private bool angry = false;
    void Update()
    {
        if (!GetComponent<CatBase>().activated)
            return;
        angry = false;
        // Do a circle raycast around the cat
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, repelRadius, Vector2.zero);
        foreach (RaycastHit2D hit in hits)
        {
            // Check if raycast hit another demon cat
            if (hit.collider.gameObject.GetComponent<DemonCat>() != null)
            {
                // Check if the other demon cat is red
                if (hit.collider.gameObject.GetComponent<DemonCat>().isRed != isRed)
                {
                    // Repel self from the other demon cat
                    Vector2 direction = hit.collider.gameObject.transform.position - transform.position;
                    float strength = 1 - (direction.magnitude / repelRadius);
                    GetComponent<Rigidbody2D>().AddForce(-direction * strength * repelForce);
                    angry = true;
                }
            }
        }
        if (angry)
        {
            GetComponent<CatBase>().sprite.sprite = angrySprite;
        }
        else
        {
            GetComponent<CatBase>().sprite.sprite = calmSprite;
        }
    }
}
