using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] GameObject healthBar;
    [SerializeField] GameObject gameOverText;

    Slider healthSlider;

    float towerHealth = 100f;

    float catCooldown = 1f;

    bool gameOver = false;
    
    void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        healthSlider = healthBar.GetComponent<Slider>();
        healthSlider.value = towerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
            return;

        if (catCooldown > 0)
            catCooldown -= Time.deltaTime;
        else
        {
            FireCat();
            catCooldown = 3f;
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
        towerHealth -= damage;
        healthSlider.value = towerHealth;
        if (towerHealth <= 0)
            GameOver();
    }

    void GameOver()
    {
        gameOver = true;
        gameOverText.SetActive(true);
    }
}
