using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;

    public SettingsTweaker settingsValues;
    String activeScene;
    public float textScaleMultiplier = 1f;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
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


    public string GetCatSeen(string catName)
    {
        return PlayerPrefs.GetString(catName, "false");
    }

    public void SetCatSeen(string catName, string seen)
    {
        PlayerPrefs.SetString(catName, seen);

    }
}
