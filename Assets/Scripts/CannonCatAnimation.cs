using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CannonCatAnimation : MonoBehaviour
{
    public enum CannonCatMode { CALM,PANIC, NONE }
    public SpriteRenderer spriteRenderer;

    public Sprite spriteCalm;
    public Sprite spritePanic;
    public Vector3 catIdleShakeScale;
    public Vector3 catPanicShakeScale;
    private CannonCatMode _currentmode = CannonCatMode.NONE; 
    public CannonCatMode CurrentMode
    {
        get { return _currentmode; }
        set { if (_currentmode != value) { _currentmode = value; changeMode(_currentmode); } }
          
    }
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        CurrentMode = CannonCatMode.CALM;
    }
    private void Update()
    {
        if (GameManager.instance != null)
        {
            if(GameManager.instance.gameParameters.roundTimer < GameManager.instance.gameParameters.roundTimer/2)
            {
                CurrentMode = CannonCatMode.PANIC;
            }
            else
            {
                CurrentMode = CannonCatMode.CALM;
            }
        }

    }

    void changeMode(CannonCatMode _currentmode)
    {
        if(_currentmode == CannonCatMode.CALM)
        {
            spriteRenderer.sprite = spriteCalm;
            transform.DOPunchScale(catIdleShakeScale,1f,2).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutElastic, 0.2f);
        }
        else if (_currentmode == CannonCatMode.PANIC)
        {
            spriteRenderer.sprite = spritePanic;
            transform.DOPunchScale(catIdleShakeScale, 0.7f, 1).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutElastic, 0.2f);

        }

    }
}
