using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCreditsButtonClicked(){
        ScreenTransition.instance.ChangeScene("Credits");
    }
    
    public void OnPlayButtonClicked()
    {
        ScreenTransition.instance.ChangeScene("Game");
    }
    
    public void OnTitleButtonClicked()
    {
        ScreenTransition.instance.ChangeScene("TitleScreen");
    }

    public void OnSettingsButtonClicked(){
        // find the settings manager
        GameObject settingsManager = GameObject.FindGameObjectWithTag("Settings");
        // enable the settings manager
        settingsManager.GetComponent<SettingsManager>().EnableSettings();
    }

    public void OnCatalogueButtonClicked()
    {
        CatalogueManager.instance.EnableCatalogue();
            
    }
    public void OnQuitButtonClicked(){
        Application.Quit();
    }
}
