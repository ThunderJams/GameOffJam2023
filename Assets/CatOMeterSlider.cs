using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatOMeterSlider : MonoBehaviour
{
    private Slider slider;

    public float idleMaxAngle;
    public float idleDuration;

    public float valueChangedMaxAngle;
    public float valueChangedDuration;
    public float valueChangedStrength;
    public int valueChangedVibrato;

    private RectTransform handleRect;

    private Tween loopTween;
    private Sequence valueChangedSequence;

    private float chaosRatio = 0f;

    public void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = 0;
        handleRect = slider.handleRect;
        handleRect.transform.eulerAngles = new Vector3(0, 0, -idleMaxAngle);
        handleAnimationLoop();

    }

    private void handleAnimationLoop()
    {
        chaosRatio = slider.value / 100;
        if (loopTween != null)
        {
            loopTween.Kill();
        }
        loopTween = handleRect.DORotate(handleRect.eulerAngles + new Vector3(0, 0, idleMaxAngle + idleMaxAngle  * chaosRatio) *2, idleDuration *(1.5f-chaosRatio)).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutElastic,0.2f);
        loopTween.onUpdate += () => {};
        loopTween.Play();
    }


    public void UpdateValue(float newValue)
    {
        DOTween.To(() => slider.value, x => slider.value = x, newValue, 0.5f).SetEase(Ease.OutCubic);

        loopTween.Kill();
        //Shake the lil cat head
        Vector3 initialAngle = handleRect.transform.eulerAngles;
        if (valueChangedSequence != null)
        {
            valueChangedSequence.Kill();
        }
        valueChangedSequence = DOTween.Sequence();

        valueChangedSequence.Append(handleRect.DOShakeRotation(valueChangedDuration, new Vector3(0, 0, valueChangedStrength), valueChangedVibrato, valueChangedMaxAngle, true, ShakeRandomnessMode.Harmonic).SetEase(Ease.OutElastic));
        valueChangedSequence.Append(handleRect.DORotate(new Vector3(0, 0, -idleMaxAngle*2), 0.5f)).onComplete += ()=> { handleAnimationLoop(); };

    }
}
