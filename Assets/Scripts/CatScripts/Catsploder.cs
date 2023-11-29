using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catsploder : MonoBehaviour
{
    float explodeRadius = 2.5f;
    float explodeStrength = 10.0f;

    public GameObject spriteCrisp;
    public GameObject spriteNormal;

    public bool exploded = false;
    public CatBase catbase;

    public AudioClip explosionSound;
    public GameObject explosionBang;
    void Start()
    {
        GameManager.OnEndOfRound += EndOfRound;
        catbase.GetComponent<PolygonCollider2D>().points = spriteNormal.GetComponent<PolygonCollider2D>().points;
        catbase.GetComponent<PolygonCollider2D>().offset = spriteNormal.transform.localPosition;
    }

    void EndOfRound()
    {
        if (!GetComponent<CatBase>().activated)
            return;
        if (!exploded)
        {
            exploded = true;
        }
        else
        {
            return;
        }

        spriteNormal.transform.DOScale(1.2f, 0.7f).SetEase(Ease.InCubic).onComplete += () => { Explode(); };
       
    }

    void Explode()
    {
        AudioManager.instance.PlaySound(explosionSound.name,1f, 0.8f);
        explosionBang.transform.localScale = Vector3.one;
        explosionBang.transform.parent = null;
        // Do a circle raycast around the cat
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, explodeRadius, Vector2.zero);
        foreach (RaycastHit2D hit in hits)
        {
            // Check if raycast hit another cat
            if (hit.collider.gameObject.GetComponent<CatBase>() != null && hit.collider.gameObject.GetComponent<CatBase>() != GetComponent<CatBase>())
            {
                // Push away from the other cat
                Vector2 direction = hit.collider.gameObject.transform.position - transform.position;
                float strength = 1 - (direction.magnitude / explodeRadius);
                hit.collider.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * strength * explodeStrength);
            }
        }
        explosionBang.transform.DOScale(0, 3).SetEase(Ease.InQuint).onComplete += () => { Destroy(explosionBang); };
        spriteCrisp.SetActive(true);
        spriteNormal.SetActive(false);
        catbase.GetComponent<PolygonCollider2D>().points = spriteCrisp.GetComponent<PolygonCollider2D>().points;
        catbase.GetComponent<PolygonCollider2D>().offset = spriteCrisp.transform.localPosition;
        catbase.sprite.sprite = spriteCrisp.GetComponent<SpriteRenderer>().sprite;

    }

    void OnDestroy()
    {
        GameManager.OnEndOfRound -= EndOfRound;
    }
}
