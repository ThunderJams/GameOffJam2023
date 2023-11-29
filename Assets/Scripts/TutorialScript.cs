using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TutorialScript : MonoBehaviour
{
    float tutorialTimer = 0f;
    bool tutorialDisplayed = false;

    // array of bools for each tutorial
    bool[] tutorialActivated = new bool[6];

    string[] tutorialText = new string[6];

    [SerializeField] Image professorSprite;

    [SerializeField] List<GameObject> scales;
    float initialHeight;

    CatBase firstCat;
    [SerializeField] public TutorialScreen tutorialScreen;
    public List<AudioClip> clipsPurfessor;
    private bool canClickOnScreen = false;
    void Awake()
    {
        tutorialText[0] = "This is the cat tree! Cats love to climb on it, and the goal of the game is to keep it balanced! \n \n (click to continue)";
        tutorialText[1] = "A cat has been launched from the cat cannon! Click and drag to move it onto the scale.\nYou can rotate it with mouse wheel or right click. \n \n (click to continue)";
        tutorialText[2] = "Now that a cat has been placed on the scale, it leans towards the heavy side. \n \n (click to continue)";
        tutorialText[3] = "The timer in the top left shows the length of the Round. Each round gets more difficult! \n \n (click to continue)";
        tutorialText[4] = "This is the Scratch-O-Meter! It increases for each cat that falls off-screen. \n Once it reaches it's full capacity, it's Game Over! \n \n (click to continue)";
        tutorialText[5] = "At the start of a round, some cats will poof away ! Good luck ! \n \n (click to continue)";
        initialHeight = scales[0].transform.position.y;
    }

    private void Start()
    {
        if (PlayerPrefs.GetString("tutoDone","false") == "true")
        {
            gameObject.SetActive(false);
        }
        Sequence s = DOTween.Sequence();
        s.Append(professorSprite.transform.DOPunchScale(new Vector3(0.2f, 0.2f), 2, 2).SetEase(Ease.OutBounce)).PrependInterval(Random.Range(0.5f, 3f)).SetLoops(-1).SetUpdate(true);
    }
    // Update is called once per frame
    void Update()
    {
        tutorialTimer += Time.deltaTime;

        

        if (tutorialTimer > 1 && tutorialActivated[0] == false)
        {
            // new line
            StartCoroutine(EnableTutorial(0));
        }


        if (firstCat){
            if (firstCat.activated && tutorialActivated[1] == false)
            {
                // new line
                StartCoroutine(EnableTutorial(1));
            }
        }
        

        if (tutorialActivated[2] == false){
            foreach (GameObject scale in scales)
            {
                if (scale.transform.position.y < initialHeight - 0.2f)
                {
                    StartCoroutine(EnableTutorial(2));
                }
            }
        }
        else{
            if (tutorialTimer > 25 && tutorialActivated[3] == false)
            {
                // new line
                StartCoroutine(EnableTutorial(3));
            }

        }

        if (tutorialTimer > 35 && tutorialActivated[4] == false)
        {
            // new line
            StartCoroutine(EnableTutorial(4));
        }

        if (GameManager.instance.round > 2 && GameManager.instance.roundTimer < GameManager.instance.gameParameters.roundTimer - 5 && tutorialActivated[5] == false)
        {
            // new line
            StartCoroutine(EnableTutorial(5));
            PlayerPrefs.SetString("tutoDone", "true");
        }

        // if click and tutorial is displayed
        if (canClickOnScreen && Input.GetMouseButtonDown(0) && tutorialDisplayed)
        {
            // disable tutorial
            DisableTutorial();
        }

        if (firstCat == null)
        {
            firstCat = FindObjectOfType<CatBase>();
        }
    }

    IEnumerator EnableTutorial(int tutorialNumber)
    {
        canClickOnScreen = false;
        tutorialActivated[tutorialNumber] = true;
        AudioManager.instance.PlaySound(clipsPurfessor[Random.Range(0, clipsPurfessor.Count)].name);
        tutorialDisplayed = true;
        // freeze deltatime
        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0, 0.5f).SetUpdate(true);
        //Time.timeScale = 0;
        tutorialScreen.showSpriteMask(tutorialNumber);
        // set self active
        gameObject.GetComponent<Image>().enabled = true;
        // set text
        gameObject.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = tutorialText[tutorialNumber];

        professorSprite.enabled = true;
        yield return new WaitForSecondsRealtime(1f);
        canClickOnScreen = true;
    }
    void DisableTutorial()
    {
        tutorialDisplayed = false;
        // unfreeze deltatime
        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1, 0.5f).SetUpdate(true);
        // set self inactive
        gameObject.GetComponent<Image>().enabled = false;
        gameObject.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
        tutorialScreen.hideSpriteMask();
        professorSprite.enabled = false;
    }
}
