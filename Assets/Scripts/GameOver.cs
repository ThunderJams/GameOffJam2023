using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    public TextMeshProUGUI roundSurvivedText;
    public TextMeshProUGUI catPlacedText;
    public TextMeshProUGUI catDiscoveredText;
    public TextMeshProUGUI scoredPointText;
    public TextMeshProUGUI highestScoreText;
    public TextMeshProUGUI catLostText;

    public PlayerManager player;

    // Start is called before the first frame update
    void Start()
    {
        
    }



    public void fillEndGameValues()
    {
        PlayerPrefs.SetInt("HighScore", Mathf.Max(GameManager.instance.score, PlayerPrefs.GetInt("HighScore", GameManager.instance.score)));
        roundSurvivedText.text = String.Format("You survived {0} rounds !",player.roundsSurvived.ToString());
        catPlacedText.text = player.cannonShot.ToString();
        catDiscoveredText.text = GetUnlockedCats().ToString() +"/"+ GameManager.instance.catFactory.catTypes.Count.ToString();
        scoredPointText.text =  String.Format("You scored {0} points !", GameManager.instance.score.ToString());
        catLostText.text = player.catLost.ToString();
        highestScoreText.text = String.Format ("Highest Score : {0}", Mathf.Max(GameManager.instance.score, PlayerPrefs.GetInt("HighScore")).ToString());
    }


    int GetUnlockedCats()
    {
        int catCount = 0;
        foreach (CatType cat in GameManager.instance.catFactory.catTypes)
        {
            if (PlayerPrefs.GetString(cat.catName,"false") == "true")
            {
                catCount += 1;
            }
        }
        return catCount;
    }

    public void onRetryButton()
    {
        ScreenTransition.instance.ChangeScene(SceneManager.GetActiveScene().name);

    }
    public void onHomeButton()
    {
        ScreenTransition.instance.ChangeScene("TitleScreen");

    }
}
