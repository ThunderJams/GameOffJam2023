using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class NewRoundCats : MonoBehaviour
{
    List<CatType> catQueue = new List<CatType>();
    bool popupActive = false;

    [SerializeField] GameObject popup;
    [SerializeField] TextMeshProUGUI catNameText;
    [SerializeField] TextMeshProUGUI catDescriptionText;
    [SerializeField] Image catSprite;

    public CanvasGroup canvas;
    public Image Background;
    private Color _initialBackgroundColor;
    private Vector3 _initialMenuScale = -Vector3.one;
    // Start is called before the first frame update
    void Start()
    {
        popup.SetActive(false);

        catSprite.transform.DOShakeRotation(2, new Vector3(0, 0, 4), 2, 1).SetDelay(1).SetLoops(-1).SetUpdate(true);
    }

    // Update is called once per frame
    void Update()
    {
        if ((catQueue.Count > 0) && (!popupActive) && !GameManager.instance.gameOver)
        {
            DisplayCat();
        }
    }

    public void AddCat(CatType cat, string seen)
    {
        if(seen == "false")
        {
            SettingsManager.instance.SetCatSeen(cat.catName, "true");
        }
        catQueue.Add(cat);

    }

    private void DisplayCat()
    {

        CatType cat = catQueue[0];
        catQueue.RemoveAt(0);

        catNameText.text = cat.catName;
        catDescriptionText.text = cat.description;
        catSprite.sprite = cat.prefab.GetComponent<CatBase>().sprite.sprite;

        // pause game
        Time.timeScale = 0f;
        SetNewRound(true);
    }

    public void SetNewRound(bool paused)
    {
        if (_initialMenuScale == -Vector3.one)
        {
            _initialMenuScale = canvas.transform.localScale;
            _initialBackgroundColor = Background.color;
        }
        AnimateNewRound(paused);
    }

    public void AnimateNewRound(bool paused)
    {

        if (paused)
        {

            popupActive = true;
            popup.SetActive(true);
            canvas.alpha = 0f;
            //Set the background alpha and content scale at 0 
            Background.color = new Color(Background.color.r, Background.color.g, Background.color.b, 0);
            canvas.transform.localScale = Vector3.zero;
            AudioManager.instance.PlaySound("Musical_Jingle_Low",0.7f);
            //We set the update to true to work while the timescale is off
            Background.DOFade(_initialBackgroundColor.a, 0.3f).SetUpdate(true);
            canvas.DOFade(1, 0.1f).SetEase(Ease.OutQuint).SetUpdate(true);
            canvas.transform.DOScale(_initialMenuScale, 0.7f).SetEase(Ease.OutBack, 1.2f).SetUpdate(true);
        }
        else
        {
            //Hide the pause menu
            Background.DOFade(0, 0.5f).SetUpdate(true);
            //We wait until the end of the pause animation to resume the game 
            canvas.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).SetUpdate(true).onComplete += () =>
            {

                Time.timeScale = 1; 
                popupActive = false;
                popup.SetActive(false);

            };
            canvas.DOFade(0, 0.5f).SetEase(Ease.InBack).SetUpdate(true);

        }
    }

    public void HideCat()
    {
        SetNewRound(false);
    }
}
