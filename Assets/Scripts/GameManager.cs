using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    [SerializeField] List<GameObject> cats;
    [SerializeField] GameObject nextCat = null;
    [SerializeField] GameObject catometerBar;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject pauseMenu;

    Slider catometerSlider;

    float catometer = 0f;

    float catCooldown = 1f;

    public bool gameOver = false;

    int round = 1;

    bool paused = false;

    
    void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        round = 1;
        catometerSlider = catometerBar.GetComponent<Slider>();
        catometerSlider.value = catometer;
    }

    // Update is called once per frame
    void Update()
    {
        if (catCooldown > 0)
            catCooldown -= Time.deltaTime;
        else
        {
            FireCat();
            catCooldown = 5f;
        }

        if (Input.GetButtonDown("Pause")){
            PauseGame();
        }
    }

    void FireCat()
    {
        if (nextCat == null)
        {
            nextCat = Instantiate(catTypes[Random.Range(0, catTypes.Length)].prefab);
        }
        //GameObject cat = Instantiate(catTypes[Random.Range(0, catTypes.Length)].prefab);
        
        nextCat.GetComponent<Rigidbody2D>().isKinematic = false;
        catCannon.GetComponent<CatCannon>().LoadCat(nextCat);
        cats.Append(nextCat);

        // generate the next cat to be shown on the cannon
        nextCat = Instantiate(catTypes[Random.Range(0, catTypes.Length)].prefab);
        nextCat.transform.position = catCannon.transform.position;
        // disable physics on the next cat
        nextCat.GetComponent<Rigidbody2D>().isKinematic = true;
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
