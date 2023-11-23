using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CatBase : MonoBehaviour
{
    bool aboveScreen = false;
    [HideInInspector] public bool activated = false;
    public float damage = 10f;

    [SerializeField] GameObject indicator;

    [HideInInspector] public float gravity;

    [HideInInspector] public bool onDish;

    Rigidbody2D rb;

    public float scoreValue = 10;



    public AudioClip pickedUpSound;
    public AudioClip fallOffSound;
    public SpriteRenderer sprite;
    Tween catPickedUpTween;

    public bool CanRotate = true;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gravity = rb.gravityScale;
        changeScale(GameManager.instance.gameParameters.catScaleMultiplier);
        rb.mass += GameManager.instance.gameParameters.catMassAddition;
    }

    void Update()
    {
        if (!onDish)
        {
            // Will check stuff
            
        }

        if (aboveScreen == false && transform.position.y > 5.5f)
        {
            aboveScreen = true;
        }
        else if (aboveScreen == true && transform.position.y < 5.5f)
        {
            Activate();
        }

        if (aboveScreen == true && rb.velocity.y < 0)
        {
            indicator.SetActive(true);
            indicator.transform.position = new Vector3(transform.position.x, 4.5f, 0);
            indicator.transform.rotation = Quaternion.Euler(0, 0, 0);
            float distanceFromScreen = (transform.position.y - 5.5f) * 0.1f;
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
            foreach(SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
        sr.sortingOrder = 10;
        rb.excludeLayers = 8;
        activated = true;
    }

    void FallOffScreen()
    {
        AudioManager.instance?.PlaySound(fallOffSound.name.ToString());
        GameManager.instance.FallOffScreen(gameObject);
        Destroy(gameObject);
    }

    void changeScale(float multiplier)
    {
        transform.localScale *= multiplier;
    }
    private void OnDestroy()
    {
        GameManager.instance?.RemoveCat(gameObject);
    }

    public void PickUp()
    {
        AudioManager.instance?.PlaySound(pickedUpSound.name.ToString(),1,Random.Range(0.90f,1.1f));
        if (catPickedUpTween == null)
        {
            catPickedUpTween = sprite.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f, 1).SetEase(Ease.InOutElastic, 0.2f);
            catPickedUpTween.onComplete += () => { catPickedUpTween = null; };

        }
    }

}
