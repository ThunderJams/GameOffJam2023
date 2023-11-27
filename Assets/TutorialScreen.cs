using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScreen : MonoBehaviour
{
    public List<GameObject> spriteMasks;
    public SpriteRenderer tutorialBackground;
    private Color targetColor;

    private GameObject currentActiveSpriteMask;
    // Start is called before the first frame update
    void Start()
    {
        targetColor = tutorialBackground.color;
        tutorialBackground.color = new Color(0,0,0,0);
        tutorialBackground.gameObject.SetActive(true);
    }

    public void showSpriteMask(int spriteMaskNumber)
    {
        tutorialBackground.DOColor(targetColor, 0.5f).SetUpdate(true);
        if(spriteMaskNumber < spriteMasks.Count)
        {
            currentActiveSpriteMask = spriteMasks[spriteMaskNumber];
            currentActiveSpriteMask.SetActive(true);
        }
    }
    public void hideSpriteMask()
    {
        tutorialBackground.DOColor(new Color(0, 0, 0, 0), 0.5f).SetUpdate(true).onComplete += () => {
            currentActiveSpriteMask.SetActive(false);
            currentActiveSpriteMask = null;
        };
    }
}
