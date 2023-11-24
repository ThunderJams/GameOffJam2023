using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatalogueManager : MonoBehaviour
{
    public static CatalogueManager instance;
    public CatalogueGrid grid;
    public float textScaleMultiplier = 1f;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        transform.GetChild(0).gameObject.SetActive(false);

    }
    public void EnableCatalogue()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        grid.DeleteGrid();
        grid.CreateGrid();

    }

    public void DisableCatalogue()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
