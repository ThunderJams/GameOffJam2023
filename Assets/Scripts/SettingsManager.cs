using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    String activeScene;
    
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        // set first child to inactive
        transform.GetChild(0).gameObject.SetActive(false);
        SceneManager.LoadScene("TitleScreen");
    }

    // Update is called once per frame
    void Update()
    {
        // if scene changes
        if (activeScene != SceneManager.GetActiveScene().name){
            DisableSettings();
            activeScene = SceneManager.GetActiveScene().name;
        }

    }

    public void EnableSettings(){
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void DisableSettings(){
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
