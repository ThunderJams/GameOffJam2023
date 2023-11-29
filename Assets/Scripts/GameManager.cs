using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public delegate void EndOfRound();
    public static event EndOfRound OnEndOfRound;

    /// <summary>
    /// Data about the different game parameters
    /// </summary>
    public GameParameters gameParameters;
    public CatFactory catFactory;
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

    [SerializeField] GameObject catCannon;
    public List<CatType> discoveredCats;


    List<GameObject> cats = new List<GameObject>();
    [SerializeField] GameObject nextCat = null;
    [SerializeField] GameObject catometerBar;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] PauseMenu pauseMenu;
    [SerializeField] NewRoundCats newRoundCats;

    // text mesh pro
    [SerializeField] TextMeshProUGUI roundText;
    [SerializeField] ScoreUpdateText scoreText;
    //Timer countdown text
    [SerializeField] Image timerImage;

    CatOMeterSlider catometerSlider;

    float catometer = 0f;

    float catCooldown = 1f;

    public bool gameOver = false;

    public int round = 1;

    bool paused = false;

    public float roundTimer;

    int activeBuccaneers = 0;

    public PlayerManager player;
    public void ChangeBuccaneer(int change)
    {
        activeBuccaneers += change;
    }

    private int _score;
    public int score
    {
        get { return _score; }
        set { _score = value; UpdateScore(); }
    }

    //Placeholder for now, the game goes faster the morerounds and the lower the catometer is 
    //formulas are roughly :
    //BaseTimeIncrement =  1 + floor(RoundTimer/15) * ((roundsSurvived+1) * incrementRate)
    //FinalTimeIncrement = BaseTimeIncrement* lerp(1+catOMeterVariance,1-catOMeterVariance, catOMeterValue)

    public float BaseTimeIncrement = 1;
    public float FinalTimeIncrement = 1;
    void Start()
    {
        catFactory = GetComponent<CatFactory>();
        StartGame();
        
        roundTimer = gameParameters.roundTimer;
    }


    void StartGame()
    {
        round = 1;

        catometerSlider = catometerBar.GetComponent<CatOMeterSlider>();
        List<CatType> newCats =  catFactory.SetNewRound(1);

        foreach (CatType cats in newCats)
        {
            SettingsManager.instance.SetCatSeen(cats.catName, "true");
        }
    }
    private bool inBetweenRounds = false;
    private bool ticking = false;
    // Update is called once per frame
    void Update()
    {
        if (roundText != null)
            roundText.text = "Round: " + round.ToString();
        //if (scoreText != null)
        //{
        //    //scoreText.text = "Score: " + score.ToString();
        //    scoreText.UpdateText(score);
        //}
        if (timerImage != null)
        {
            //update the Timer text with the round timer 
            timerImage.fillAmount = roundTimer / gameParameters.roundTimer;
        }

        if (catCooldown > 0)
        {
            catCooldown -= Time.deltaTime * (1 + (1/round));
        }
        else if (roundTimer > 5)
        {
            catCooldown = gameParameters.baseCatCooldown / FinalTimeIncrement;
            if(activeBuccaneers> 0)
            {
                catCooldown *= 0.8f;
            }
            if (catCooldown < 0.5f)
                catCooldown = 0.5f;
            FireCat();
        }
        if (roundTimer < 5 && ! ticking)
        {
            ticking = true;
            AudioManager.instance?.PlaySound("5_second_Clock");
        }

        if (Input.GetButtonDown("Pause")){
            PauseGame();
        }

        if (roundTimer > 0)
            roundTimer -= Time.deltaTime;
        else if(!inBetweenRounds)
            StartCoroutine(EndRound());

        ComputeTimeIncrement();
    }

    public void ComputeTimeIncrement()
    {
        BaseTimeIncrement = 1 + (gameParameters.desiredRoundMaxDifficulty /(roundTimer+1)) * ((round+1) * gameParameters.incrementRate);
        FinalTimeIncrement = BaseTimeIncrement * Mathf.Lerp(1 + gameParameters.catOMeterVariance, 1 - gameParameters.catOMeterVariance, catometerSlider.getCatOMeterRatio()) * gameParameters.finalTimeMultiplier;
    }

    void FireCat()
    {
        if (nextCat == null)
        {
            nextCat = Instantiate(catFactory.SpawnNewCat().prefab);
        }
        nextCat.SetActive(true);
        //GameObject cat = Instantiate(catTypes[Random.Range(0, catTypes.Length)].prefab);

        nextCat.GetComponent<Rigidbody2D>().isKinematic = false;

        catCannon.GetComponent<CatCannon>().LoadCat(nextCat, gameParameters.cannonFuseBaseSpeed / FinalTimeIncrement);
        // add to the score for the cat placed
        score += (int)(nextCat.GetComponent<CatBase>().scoreValue * gameParameters.baseCatDroppedScoreMultiplier);
        cats.Add(nextCat);

        // generate the next cat to be shown on the cannon
        nextCat = Instantiate(catFactory.SpawnNewCat().prefab);
        nextCat.SetActive(false);
        nextCat.transform.position = catCannon.transform.position;
        // disable physics on the next cat
        nextCat.GetComponent<Rigidbody2D>().isKinematic = true;


    }

    public IEnumerator EndRound()
    {
        if (gameOver)
        {
            yield break;
        }
        inBetweenRounds = true;
        if (OnEndOfRound != null)
        {
            OnEndOfRound();
        }
        AudioManager.instance?.PlaySound("dRUM_SHOT", 1, 1);
        yield return new WaitForSeconds(gameParameters.inBetweenRoundTime);
        gameParameters.roundTimer = Mathf.Min(gameParameters.roundTimer+1, gameParameters.maxRoundTimer);
        NewRound();
        inBetweenRounds = false;
    }
    private IEnumerator DestroyCats(int catsToDestroy, int delay)
    {
        yield return new WaitForSeconds(delay);
        if (cats.Count < 5)
        {
            yield break;
        }

        for(int i=0; i< catsToDestroy; i++)
        {
            int randIndex = Random.Range(0, cats.Count - 1);
            GameObject catToDestroy = cats[randIndex];
            cats.RemoveAt(randIndex);
            Destroy(catToDestroy);
            AudioManager.instance.PlaySound("pOPPING_nOISE", 1, 1);
            yield return new WaitForSeconds(0.2f);
        }

    }
    void NewRound(){
        player.roundsSurvived = round;
        round++;
        roundTimer = gameParameters.roundTimer;

        List<CatType> newCats = catFactory.SetNewRound(round);


        //score += catsOnPlatform * catMultiplierSum;
        foreach (GameObject cat in cats){
            score += (int)(cat.GetComponent<CatBase>().scoreValue * gameParameters.baseCatDroppedScoreMultiplier);
        }
        foreach (CatType cats in newCats)
        {
            newRoundCats.AddCat(cats, SettingsManager.instance.GetCatSeen(cats.catName));
            SettingsManager.instance.SetCatSeen(cats.catName, "true");
        }
        //score += catsOnPlatform * catMultiplierSum;

        // set catometer to 0.8 of itself
        catometer = catometer * gameParameters.catOMeterDecreaseValue;
        catometerSlider.UpdateValue(catometer);


        StartCoroutine(DestroyCats(Mathf.Min((int)(round*0.7f),10),3));
        ticking = false;
    }

    public void SpawnCat(int catType = -1)
    {
        if (catType == -1)
            catType = Random.Range(0,catFactory.catTypes.Count);
        GameObject cat = Instantiate(catFactory.catTypes[catType].prefab);
        cat.transform.position = new Vector3(4,2,0);
        cat.GetComponent<CatBase>().Activate();
        cats.Add(cat);
    }

    public void RemoveCat(GameObject cat)
    {
        cats.Remove(cat);
    }
    public void FallOffScreen(GameObject cat)
    {
        if (!gameOver)
        {
            player.catLost += 1;
        }
        DamageTower(cat.GetComponent<CatBase>().damage);
    }

    void DamageTower(float damage)
    {
        catometer += damage;
        catometerSlider.UpdateValue(catometer);
        if (catometer >= 100)
            GameOver();
    }

    void GameOver()
    {
        gameOver = true;
        gameOverScreen.SetActive(true);
        gameOverScreen.GetComponent<GameOver>().fillEndGameValues();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseGame(){
        //Prevent the player from spamming the pause button
        if (pauseMenu.CanPause()){
            paused = !paused;
            //enable the pause menu screen
            pauseMenu.SetPaused(paused);
        }
    }

    /// <summary>
    /// Send an update request for the score
    /// </summary>
    private void UpdateScore()
    {
        if (scoreText != null)
        {
            scoreText.UpdateText(score);
        }
    }

}
