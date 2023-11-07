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
        SceneManager.LoadScene("Credits");
    }
    
    public void OnPlayButtonClicked(){
        SceneManager.LoadScene("Game");
    }
    
    public void OnTitleButtonClicked(){
        SceneManager.LoadScene("TitleScreen");
    }
}
