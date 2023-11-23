using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private bool _paused = false;
    [SerializeField] CanvasGroup PausePanel;
    [SerializeField] Image Background;
    private Color _initialBackgroundColor;
    private Vector3 _initialMenuScale = -Vector3.one;

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
        if (_initialMenuScale == -Vector3.one)
        {
            _initialMenuScale = PausePanel.transform.localScale;
            _initialBackgroundColor = Background.color;
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
            PausePanel.alpha = 0f;
            Time.timeScale = 0;

            lockPauseButton = false;
            //Set the background alpha and content scale at 0 
            Background.color = new Color(Background.color.r, Background.color.g, Background.color.b, 0);
            PausePanel.transform.localScale = Vector3.zero;

            //We set the update to true to work while the timescale is off
            Background.DOFade(_initialBackgroundColor.a, 0.3f).SetUpdate(true);
            gameObject.SetActive(true);
            PausePanel.DOFade(1, 0.1f).SetEase(Ease.OutQuint).SetUpdate(true);
            PausePanel.transform.DOScale(_initialMenuScale, 0.7f).SetEase(Ease.OutBack, 1.2f).SetUpdate(true);
        }
        else
        {
            //Hide the pause menu
            Background.DOFade(0, 0.5f).SetUpdate(true);
            //We wait until the end of the pause animation to resume the game 
            PausePanel.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).SetUpdate(true).onComplete += ()=> 
            {
        
                Time.timeScale = 1;
                lockPauseButton = false;
                gameObject.SetActive(false); 
            };
            PausePanel.DOFade(0, 0.5f).SetEase(Ease.InBack).SetUpdate(true);

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

    public void CatalogueButton()
    {
        CatalogueManager.instance.EnableCatalogue();
    }
}
