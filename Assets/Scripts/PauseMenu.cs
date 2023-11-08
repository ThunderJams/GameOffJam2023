using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartButton(){
        // reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitButton(){
        // load menu scene
        SceneManager.LoadScene("TitleScreen");
    }

    public void SettingsButton(){
        // find the settings manager
        GameObject settingsManager = GameObject.FindGameObjectWithTag("Settings");
        // enable the settings manager
        settingsManager.GetComponent<SettingsManager>().EnableSettings();
    }
}
