using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class mainMenuButtonAnimation : MonoBehaviour
{
    private TextMeshProUGUI text;
    private Color initialTextColor;
    public Color clickedTextColor;
    public void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        initialTextColor = text.color;
    }

    //Shake the transform
    public void onPointerEnter(BaseEventData data)
    {
        transform.DOShakeRotation(0.5f, new Vector3(0, 0, 1), 10, 40, true, ShakeRandomnessMode.Harmonic).SetEase(Ease.InBack);
    }

    //Reset to initialState
    public void onPointerExit(BaseEventData data)
    {
        transform.DORotate(Vector3.zero, 0.1f);
    }
    public void onPointerDown(BaseEventData data)
    {
        text.DOColor(clickedTextColor, 0.1f).SetUpdate(true);
    }

    public void onPointerUp(BaseEventData data)
    {
        text.DOColor(initialTextColor, 0.1f).SetUpdate(true);
    }
}
