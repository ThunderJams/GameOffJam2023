using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingAnimation : MonoBehaviour
{

    public float shakeRandomness;
    public float shakeDuration;
    public float shakeStrength;
    public int shakeVibrato;
    public float minDelay;
    public float maxDelay;
    // Start is called before the first frame update
    void Start()
    {

        Sequence s = DOTween.Sequence();
        s.SetDelay(Random.Range(minDelay,maxDelay));
        s.Append(transform.DOShakeRotation(shakeDuration,new Vector3(0,0, shakeStrength), shakeVibrato, shakeRandomness,true,ShakeRandomnessMode.Harmonic).SetEase(Ease.OutBack,1.2f));
        s.AppendInterval(Random.Range(minDelay, maxDelay));
        s.SetLoops(-1, LoopType.Yoyo);
    }

}
