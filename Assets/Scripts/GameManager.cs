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
    public CatType[] catTypes;

    public CatType[] selectedCats;


    List<GameObject> cats;
    [SerializeField] GameObject nextCat = null;
    [SerializeField] GameObject catometerBar;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] PauseMenu pauseMenu;

    // text mesh pro
    [SerializeField] TextMeshProUGUI roundText;
    [SerializeField] ScoreUpdateText scoreText;
    //Timer countdown text
    [SerializeField] TextMeshProUGUI timerText;

    CatOMeterSlider catometerSlider;

    float catometer = 0f;

    float catCooldown = 1f;

    public bool gameOver = false;

    int round = 1;

    bool paused = false;

    public float roundTimer = 60f;

    int activeBuccaneers = 0;

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
    void Start()
    {
        StartGame();
        cats = new List<GameObject>();
    }

    void StartGame()
    {
        round = 1;

        catometerSlider = catometerBar.GetComponent<CatOMeterSlider>();
        selectedCats = catTypes.OrderBy(x => Random.value).Take(6).ToArray();
    }

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
        if (timerText != null)
        {
            //update the Timer text with the round timer 
            timerText.text = System.Math.Round(roundTimer).ToString();
        }

        if (catCooldown > 0)
        {
            catCooldown -= Time.deltaTime * (1 + (1/round));
        }
        else
        {
            catCooldown = 5f;
            for (int i = 0; i < activeBuccaneers; i++)
                catCooldown *= 0.8f;
            FireCat();
        }

        if (Input.GetButtonDown("Pause")){
            PauseGame();
        }

        if (roundTimer > 0)
            roundTimer -= Time.deltaTime;
        else
            EndRound();
            
    }

    void FireCat()
    {
        if (nextCat == null)
        {
            nextCat = Instantiate(selectedCats[Random.Range(0, selectedCats.Length)].prefab);
        }
        nextCat.SetActive(true);
        //GameObject cat = Instantiate(catTypes[Random.Range(0, catTypes.Length)].prefab);

        nextCat.GetComponent<Rigidbody2D>().isKinematic = false;

        catCannon.GetComponent<CatCannon>().LoadCat(nextCat);
        // add to the score for the cat placed
        score += (int)nextCat.GetComponent<CatBase>().scoreValue;
        cats.Add(nextCat);

        // generate the next cat to be shown on the cannon
        nextCat = Instantiate(catTypes[Random.Range(0, catTypes.Length)].prefab);
        nextCat.transform.position = catCannon.transform.position;
        //For now disable the cat preview renderers 
        nextCat.SetActive(false);

        // disable physics on the next cat
        nextCat.GetComponent<Rigidbody2D>().isKinematic = true;


    }

    public void EndRound()
    {
        OnEndOfRound();
        NewRound();
    }

    void NewRound(){
        round++;
        roundTimer = 30f;

        // select up to 6 new cats
        selectedCats = catTypes.OrderBy(x => Random.value).Take(6).ToArray();

        //score += catsOnPlatform * catMultiplierSum;
        foreach (GameObject cat in cats){
            score += (int)cat.GetComponent<CatBase>().scoreValue;
        }
        //score += catsOnPlatform * catMultiplierSum;

        // set catometer to 0.8 of itself
        catometer = catometer * 0.8f;
        catometerSlider.UpdateValue(catometer);
    }

    public void SpawnCat(int catType = -1)
    {
        if (catType == -1)
            catType = Random.Range(0, catTypes.Length);
        GameObject cat = Instantiate(catTypes[catType].prefab);
        cat.transform.position = new Vector3(4,2,0);
        cat.GetComponent<CatBase>().Activate();
        cats.Add(cat);
    }

    public void RemoveCat(GameObject cat)
    {
        DamageTower(cat.GetComponent<CatBase>().damage);
        cats.Remove(cat);
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
