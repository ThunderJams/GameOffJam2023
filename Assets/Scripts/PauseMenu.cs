using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private bool _paused = false;
    [SerializeField] GameObject contents;
    [SerializeField] Image background;
    private Color initialBackgroundColor;
    private Vector3 initialMenuScale = -Vector3.one;

    public bool lockPauseButton = false;
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Check wether or not the player Pause button input can be used
    /// </summary>
    /// <returns></returns>
    public bool CanPause()
    {
        return !lockPauseButton;
    }

    /// <summary>
    /// Enable or disable the pause menu screen
    /// Set some initialisation values the first time the menu is opened
    /// </summary>
    /// <param name="paused"></param>
    public void SetPaused(bool paused)
    {
        if (initialMenuScale == -Vector3.one)
        {
            initialMenuScale = contents.transform.localScale;
            initialBackgroundColor = background.color;
        }
        AnimatePauseMenu(paused);
    }

    /// <summary>
    /// Fade in or out the menu
    /// </summary>
    /// <param name="paused"></param>
    public void AnimatePauseMenu(bool paused)
    {
        //We prevent the user from pressing the pause button too fast
        lockPauseButton = true;
        if (paused)
        {
            Time.timeScale = 0;

            lockPauseButton = false;
            //Set the background alpha and content scale at 0 
            background.color = new Color(background.color.r, background.color.g, background.color.b, 0);
            contents.transform.localScale = Vector3.zero;

            //We set the update to true to work while the timescale is off
            background.DOFade(initialBackgroundColor.a, 0.3f).SetUpdate(true);
            gameObject.SetActive(true);
            contents.transform.DOScale(initialMenuScale, 0.7f).SetEase(Ease.OutBack, 1.2f).SetUpdate(true);
        }
        else
        {
            //Hide the pause menu
            background.DOFade(0, 0.5f).SetUpdate(true);
            //We wait until the end of the pause animation to resume the game 
            contents.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).SetUpdate(true).onComplete += ()=> 
            {
        
                Time.timeScale = 1;
                lockPauseButton = false;
                gameObject.SetActive(false); 
            };

        }
    }

    public void RestartButton(){
        Time.timeScale = 1;
        // reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitButton(){
        Time.timeScale = 1;
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
