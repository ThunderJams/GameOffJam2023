using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CatalogueGrid : MonoBehaviour
{
    public GameObject catContainerPrefab;
    public  GridLayoutGroup gridLayout;
    public int maxColumns = 4;
    public int maxRows = 4; 

    public List<CatType> cats;
    public List<GameObject> catContainers;

    [SerializeField] TextMeshProUGUI catName;
    [SerializeField] TextMeshProUGUI catDescription;
    [SerializeField] Image catImage;

    public float soundMaxCooldown = 0.3f;
    public float soundCooldown = 0f;
    // Start is called before the first frame update
    void Start()
    {
        catImage.transform.DOShakeRotation(2, new Vector3(0, 0, 4), 2, 1).SetDelay(1).SetLoops(-1);
        catImage.transform.DOLocalJump(new Vector3(0, 0, 0), 30, 1, 3).SetDelay(3).SetLoops(-1).SetEase(Ease.InQuart);
        RefreshGrid();
    }
    public void Update()
    {
        if (soundCooldown>0)
            soundCooldown -= Time.deltaTime;
    }


    public void CreateGrid()
    {
        int currentIndex = 0;
        maxPages = 1;
        foreach ( CatType cat in cats)
        {
            GameObject go = GameObject.Instantiate(catContainerPrefab);
            go.transform.SetParent(gridLayout.transform);
            go.GetComponent<CatContainer>().LoadCat(cat);
            go.GetComponent<CatContainer>().OnShowCat += DisplayCat;
            if (currentIndex >= maxPages * maxColumns * maxRows)
            {
                maxPages += 1;
            }
            if (maxPages > 1)
            {
                go.SetActive(false);
            }
            catContainers.Add(go);
            currentIndex++;
        }
    }
    public void DeleteGrid()
    {
        foreach (GameObject catCon in catContainers)
        {
            Destroy(catCon);
        }
        catContainers.Clear();
        currentPageIndex = 0;
    }
    public void RefreshGrid()
    {
        DisplayPage(currentPageIndex);
    }
    int maxPages;
    int currentPageIndex = 0;

    public void nextPage()
    {
        if(currentPageIndex+1 < maxPages)
        {
            currentPageIndex++;
            DisplayPage(currentPageIndex);
        }
    }
    public void previousPage()
    {
        if(currentPageIndex > 0)
        {
            currentPageIndex--;
            DisplayPage(currentPageIndex);
        }
    }
    public void DisplayPage(int pageIndex)
    {
        int i = 0;
        foreach(GameObject c in catContainers)
        {
            if (i >= (pageIndex) * maxColumns * maxRows && i < (pageIndex + 1 ) * maxColumns * maxRows)
            {
                c.SetActive(true);
                c.GetComponent<CatContainer>().RefreshCat();
            }
            else
            {
                c.SetActive(false);
            }

            i++;
        }
    }

    public void DisplayCat(CatContainer catContainer)
    {
        CatType cat = catContainer.type;

        if (cat.prefab.GetComponent<CatBase>().sprite != null)
            catImage.sprite = cat.prefab.GetComponent<CatBase>().sprite.sprite;
        catName.text = cat.catName;
        catDescription.text = cat.description;
        if (SettingsManager.instance.GetCatSeen(cat.catName) == "true" || SettingsManager.instance.settingsValues.FillCatalogue)
        {
            catImage.color = Color.white;
            if(soundCooldown<= 0)
            {
                AudioManager.instance.PlaySound(cat.prefab.GetComponent<CatBase>().pickedUpSound.name);
                soundCooldown = soundMaxCooldown;
            }
        }
        else
        {
            catImage.color = Color.black;
            catName.text  = System.Text.RegularExpressions.Regex.Replace(catName.text, "[a-zA-Z]", "?");
            catDescription.text = System.Text.RegularExpressions.Regex.Replace(catDescription.text, "[a-zA-Z]", "?");
        }

    }
}
