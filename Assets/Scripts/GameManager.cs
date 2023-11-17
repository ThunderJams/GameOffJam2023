using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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


    [SerializeField] List<GameObject> cats;
    [SerializeField] GameObject nextCat = null;
    [SerializeField] GameObject catometerBar;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject pauseMenu;

    // text mesh pro
    [SerializeField] TextMeshProUGUI roundText;
    [SerializeField] TextMeshProUGUI scoreText;
    //Timer countdown text
    [SerializeField] TextMeshProUGUI timerText;

    Slider catometerSlider;

    float catometer = 0f;

    float catCooldown = 1f;

    public bool gameOver = false;

    int round = 1;

    bool paused = false;

    float roundTimer = 60f;
    int score = 0;

    
    void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        round = 1;
        catometerSlider = catometerBar.GetComponent<Slider>();
        catometerSlider.value = catometer;

        selectedCats = catTypes.OrderBy(x => Random.value).Take(6).ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        if (roundText != null)
            roundText.text = "Round: " + round.ToString();
        if (scoreText != null)
        {
            //scoreText.text = "Score: " + score.ToString();
            scoreText.text = score.ToString();
        }
        if (timerText != null)
        {
            //update the Timer text with the round timer 
            timerText.text = System.Math.Round(roundTimer).ToString();
        }

        if (catCooldown > 0)
            catCooldown -= (Time.deltaTime * (1 + (1/round)));
        else
        {
            FireCat();
            catCooldown = 5f;
        }

        if (Input.GetButtonDown("Pause")){
            PauseGame();
        }

        if (roundTimer > 0)
            roundTimer -= Time.deltaTime;
        else
            NewRound();
    }

    void FireCat()
    {
        if (nextCat == null)
        {
            nextCat = Instantiate(selectedCats[Random.Range(0, selectedCats.Length)].prefab);
        }
        //GameObject cat = Instantiate(catTypes[Random.Range(0, catTypes.Length)].prefab);
        
        nextCat.GetComponent<Rigidbody2D>().isKinematic = false;
        catCannon.GetComponent<CatCannon>().LoadCat(nextCat);
        cats.Append(nextCat);

        // add to the score for the cat placed
        score += (int)nextCat.GetComponent<CatBase>().scoreValue;

        // generate the next cat to be shown on the cannon
        nextCat = Instantiate(catTypes[Random.Range(0, catTypes.Length)].prefab);
        nextCat.transform.position = catCannon.transform.position;
        // disable physics on the next cat
        nextCat.GetComponent<Rigidbody2D>().isKinematic = true;

        
    }

    void NewRound(){
        round++;
        roundTimer = 60f;

        // select up to 6 new cats
        selectedCats = catTypes.OrderBy(x => Random.value).Take(6).ToArray();

        //score += catsOnPlatform * catMultiplierSum;
        foreach (GameObject cat in cats){
            score += (int)cat.GetComponent<CatBase>().scoreValue;
        }
        //score += catsOnPlatform * catMultiplierSum;

        // set catometer to 0.8 of itself
        catometer = catometer * 0.8f;
    }

    public void SpawnCat(int catType = -1)
    {
        if (catType == -1)
            catType = Random.Range(0, catTypes.Length);
        GameObject cat = Instantiate(catTypes[catType].prefab);
        cat.transform.position = new Vector3(4,2,0);
        cat.GetComponent<CatBase>().Activate();
        cats.Append(cat);
    }

    public void RemoveCat(GameObject cat)
    {
        DamageTower(cat.GetComponent<CatBase>().damage);
        cats.Remove(cat);
    }

    void DamageTower(float damage)
    {
        catometer += damage;
        catometerSlider.value = catometer;
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
        if (paused){
            Time.timeScale = 1;
            paused = false;
        }
        else{
            Time.timeScale = 0;
            paused = true;
        }
        pauseMenu.SetActive(paused);
    }
}
