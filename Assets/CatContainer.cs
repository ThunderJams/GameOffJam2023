using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CatContainer : MonoBehaviour
{

    public delegate void ShowCat(CatContainer c);
    public event ShowCat OnShowCat;

    public CatType type;

    [SerializeField] Image catSprite;
    [SerializeField] TextMeshProUGUI lockedText;

    public void LoadCat(CatType t)
    {
        type = t;
        if (type.prefab.GetComponent<CatBase>().sprite != null)
            catSprite.sprite = type.prefab.GetComponent<CatBase>().sprite?.sprite;
        else
            print("Cat is null");
        RefreshCat();
    }
    public void RefreshCat()
    {
        if (type.seen)
        {
            catSprite.gameObject.SetActive(true);
            lockedText.gameObject.SetActive(false);

        }
        else
        {
            catSprite.gameObject.SetActive(false);
            lockedText.gameObject.SetActive(true);

        }
    }

    public void DisplayCat()
    {
        OnShowCat(this);
    }
}
