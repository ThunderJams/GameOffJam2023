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
    // Start is called before the first frame update
    void Start()
    { 

        CreateGrid();
        RefreshGrid();
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

    [SerializeField] TextMeshProUGUI catName;
    [SerializeField] TextMeshProUGUI catDescription;
    [SerializeField] Image catImage;
    public void DisplayCat(CatContainer catContainer)
    {
        CatType cat = catContainer.type;

        if (cat.prefab.GetComponent<CatBase>().sprite != null)
            catImage.sprite = cat.prefab.GetComponent<CatBase>().sprite.sprite;
        catName.text = cat.catName;
        print(cat.name);
        catDescription.text = cat.description;
        if (cat.seen)
        {
            catImage.color = Color.white;
        }
        else
        {
            catImage.color = Color.black;
            catName.text  = System.Text.RegularExpressions.Regex.Replace(catName.text, "[a-zA-Z]", "?");
            catDescription.text = System.Text.RegularExpressions.Regex.Replace(catDescription.text, "[a-zA-Z]", "?");
        }

    }
}
