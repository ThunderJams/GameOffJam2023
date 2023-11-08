using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugMenu : MonoBehaviour
{
    public static DebugMenu instance;

    public bool cannonMove = true;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    [SerializeField] GameObject debugMenu;
    [SerializeField] TMP_Dropdown typeDropdown;

    bool visible;

    void Start()
    {
        visible = debugMenu.activeSelf;

        CatType[] catTypes = GameManager.instance.catTypes;
        foreach (CatType catType in catTypes)
        {
            typeDropdown.options.Add(new TMP_Dropdown.OptionData(catType.catName));
        }
    }

    void Update()
    {
        //When L is pressed, toggle debug menu
        if (Input.GetKeyDown(KeyCode.L))
        {
            visible = !visible;
            debugMenu.SetActive(visible);
        }
    }

    public void ToggleCannon()
    {
        cannonMove = !cannonMove;
    }

    public void SpawnCat()
    {
        GameManager.instance.SpawnCat(typeDropdown.value - 1);
    }
}
