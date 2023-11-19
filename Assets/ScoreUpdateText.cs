using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUpdateText : MonoBehaviour
{
    private TextMeshProUGUI text;

    public int currentValue;
    public int goal;
    
    /// <summary>
    /// Modify this to increase or decrease the duration of the tween on the text
    /// </summary>
    [Range(-5,5)] public float durationLogModifier;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = "0";

    }

    public void Update()
    {
        text.text = currentValue.ToString();
    }

    public void UpdateText(int value)
    {
        int diff = value - goal;
        
        float duration = durationLogModifier - Mathf.Log(Mathf.Abs(diff));
        goal = value;
        DOTween.To(() => currentValue, x => currentValue = x, goal, duration).SetEase(Ease.OutQuint);
    }
}
