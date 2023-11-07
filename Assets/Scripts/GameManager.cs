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
    [SerializeField] GameObject[] catTypes;
    [SerializeField] List<GameObject> cats;
    [SerializeField] GameObject catometerBar;
    [SerializeField] GameObject gameOverText;

    Slider catometerSlider;

    float catometer = 0f;

    float catCooldown = 1f;

    public bool gameOver = false;

    int round = 1;
    
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
        if (gameOver)
        {
            if (Input.anyKeyDown)
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }
            
        if (catCooldown > 0)
            catCooldown -= Time.deltaTime;
        else
        {
            FireCat();
            catCooldown = 5f;
        }
    }

    void FireCat()
    {
        GameObject cat = Instantiate(catTypes[Random.Range(0, catTypes.Length)]);
        catCannon.GetComponent<CatCannon>().LoadCat(cat);
        cats.Append(cat);
    }

    public void SpawnCat()
    {
        GameObject cat = Instantiate(catTypes[Random.Range(0, catTypes.Length)]);
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
        gameOverText.SetActive(true);
    }
}
