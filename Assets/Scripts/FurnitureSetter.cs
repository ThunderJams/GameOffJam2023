using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureSetter : MonoBehaviour
{
    public int minFurniture = 2;
    public int maxFurniture = 5;

    private List<GameObject> m_furniture = new List<GameObject>();

    public bool Debug_Randomize;
    // Start is called before the first frame update
    void Start()
    {
        //Grab all furnitures that can be shuffled
        for (int i = 0; i < transform.childCount; i++)
        {
            m_furniture.Add(transform.GetChild(i).gameObject);
        }

        //We randomize the furniture once at the start of the game
        RandomizeFurniture();
    }

    // Update is called once per frame
    void Update()
    {
        if (Debug_Randomize)
        {
            Debug_Randomize = false;
            RandomizeFurniture();
        }


    }

    /// <summary>
    /// Show or hide the different furniture, based on a minimum and maximum value
    /// Called between the end of the round and at the start of a next round, between cat shuffling
    /// </summary>
    public void RandomizeFurniture()
    {
        //Get a random amount of furniture
        int furnitureCount = Random.Range(minFurniture, maxFurniture);
        //Shuffle the list and take the first ones
        m_furniture.Sort((a, b) => 1 - 2 * Random.Range(0, m_furniture.Count));
        for (int i=0; i < m_furniture.Count; i++)
        {
            m_furniture[i].SetActive(i < furnitureCount);

        }


    }
}
