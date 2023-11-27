using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCat : MonoBehaviour
{
    private bool transformed = false;
    public GameObject spriteHidden;
    public GameObject spriteTransformed;
    public AudioClip soundCatTransformed;
    public AudioClip soundCatTransformation;
    public CatBase catbase;
    void Start()
    {
        GameManager.OnEndOfRound += EndOfRound;
        spriteHidden.SetActive(true);
        spriteTransformed.SetActive(false);
        catbase.GetComponent<PolygonCollider2D>().points = spriteHidden.GetComponent<PolygonCollider2D>().points;
        catbase.GetComponent<PolygonCollider2D>().offset = spriteHidden.transform.localPosition;
    }

    void EndOfRound()
    {
        Sequence s = DOTween.Sequence();
        s.Append(spriteHidden.transform.DOShakePosition(0.5f, new Vector3(0.05f, 0, 0), 10, 90, false, true, ShakeRandomnessMode.Harmonic)).SetEase(Ease.InOutElastic);
        s.Append(spriteHidden.transform.DOScale( new Vector3(1, 1.3f, 1), 1f)).SetEase(Ease.InQuart);
        s.onComplete += () => { TransformCat(); };

        GameManager.OnEndOfRound -= EndOfRound;
    }

    public void TransformCat()
    {
        AudioManager.instance.PlaySound(soundCatTransformation.name);
        spriteHidden.SetActive(false);
        spriteTransformed.SetActive(true);
        catbase.GetComponent<PolygonCollider2D>().points = spriteTransformed.GetComponent<PolygonCollider2D>().points;
        catbase.GetComponent<PolygonCollider2D>().offset = spriteTransformed.transform.localPosition;
        catbase.sprite.sprite = spriteTransformed.GetComponent<SpriteRenderer>().sprite;
        catbase.pickedUpSound = soundCatTransformed;
        spriteHidden.transform.DOScale(new Vector3(1, 1, 1), 1f);

    }
    void OnDestroy()
    {
        GameManager.OnEndOfRound -= EndOfRound;
    }
}
