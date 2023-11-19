using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class PauseButton : MonoBehaviour
{
    public void onClick()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.PauseGame();
        }
    }

    //Shake the transform
    public void onPointerEnter(BaseEventData data)
    {
        transform.DOShakeRotation(0.5f,new Vector3(0,0,5), 10, 40,true, ShakeRandomnessMode.Harmonic).SetEase(Ease.InBack);
    }

    //Reset to initialState
    public void onPointerExit(BaseEventData data)
    {
        transform.DORotate(Vector3.zero, 0.1f);
    }


}
