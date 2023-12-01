using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class petTheCats : MonoBehaviour
{
    Vector3 initialScale;
    // Start is called before the first frame update
    void Start()
    {
        initialScale = transform.localScale;  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void PetTheCat()
    {
        transform.localScale = initialScale;
        transform.DOScale(new Vector3(1, initialScale.y - 0.1f, 1), 0.1f).SetEase(Ease.OutQuint).onComplete += () =>
         {
             transform.DOScale(new Vector3(1, 1, 1), 0.1f).SetEase(Ease.OutQuint) ;
         };
    }
}
