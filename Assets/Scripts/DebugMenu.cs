using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    bool visible;

    void Start()
    {
        visible = debugMenu.activeSelf;   
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
        GameManager.instance.SpawnCat();
    }
}
