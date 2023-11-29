using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NewRoundCats : MonoBehaviour
{
    List<CatType> catQueue = new List<CatType>();
    bool popupActive = false;

    [SerializeField] GameObject popup;
    [SerializeField] TextMeshProUGUI catNameText;
    [SerializeField] TextMeshProUGUI catDescriptionText;
    [SerializeField] Image catSprite;

    // Start is called before the first frame update
    void Start()
    {
        popup.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if ((catQueue.Count > 0) && (!popupActive))
        {
            DisplayCat();
        }
    }

    public void AddCat(CatType cat)
    {
        catQueue.Add(cat);
    }

    private void DisplayCat()
    {
        popupActive = true;
        popup.SetActive(true);

        CatType cat = catQueue[0];
        catQueue.RemoveAt(0);

        catNameText.text = cat.catName;
        catDescriptionText.text = cat.description;
        catSprite.sprite = cat.prefab.GetComponent<CatBase>().sprite.sprite;

        // pause game
        Time.timeScale = 0f;
    }

    public void HideCat()
    {
        popupActive = false;
        popup.SetActive(false);

        // unpause game
        Time.timeScale = 1f;
    }
}
