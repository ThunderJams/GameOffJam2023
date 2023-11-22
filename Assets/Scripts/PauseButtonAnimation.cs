using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseButtonAnimation : MonoBehaviour
{
    private TextMeshProUGUI text;

    public float scaleFactor = 1.2f;
    public Vector3 initialTextScale;
    private Color initialTextColor;
    public Color clickedTextColor = Color.green;
    public void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        initialTextScale = text.transform.localScale;
        initialTextColor = text.color;
    }

    public void onPointerEnter(BaseEventData data)
    {
        text.transform.DOScale(initialTextScale * scaleFactor, 0.2f).SetEase(Ease.OutCubic).SetUpdate(true);
        AudioManager.instance.PlaySound("Click", 0.5f, 1.2f);
    }

    public void onPointerExit(BaseEventData data)
    {
        text.transform.DOScale(initialTextScale, 0.2f).SetEase(Ease.InCubic).SetUpdate(true);
    }

    public void onPointerDown(BaseEventData data)
    {
        text.DOColor(clickedTextColor, 0.1f).SetUpdate(true);
        AudioManager.instance.PlaySound("Click", 1f, .6f);
    }

    public void onPointerUp(BaseEventData data)
    {
        text.DOColor(initialTextColor, 0.1f).SetUpdate(true);
    }
}
