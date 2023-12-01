using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public static class IEnumerableExtensions
{
    public static System.Random rand = new System.Random();
    public static T RandomElementByWeight<T>(this IEnumerable<T> sequence, Func<T, float> weightSelector)
    {
        float totalWeight = sequence.Sum(weightSelector);
        // The weight we are after...
        float itemWeightIndex = (float)rand.NextDouble() * totalWeight;
        float currentWeightIndex = 0;

        foreach (var item in from weightedItem in sequence select new { Value = weightedItem, Weight = weightSelector(weightedItem) })
        {
            currentWeightIndex += item.Weight;

            // If we've hit or passed the weight we are after for this item then it's the one we want....
            if (currentWeightIndex >= itemWeightIndex)
                return item.Value;

        }

        return default(T);

    }

}

public class CatFactory : MonoBehaviour
{
    public int catAmount;
    public List<CatType> catTypes;

    public enum CatRarity {COMMON,RARE,EPIC,FANCY }

    private Dictionary<CatRarity, float> spawnRates = new Dictionary<CatRarity, float>()
    {
        {CatRarity.COMMON,0f },
        {CatRarity.RARE,1f},
        {CatRarity.EPIC,0.5f},
        {CatRarity.FANCY,0.1f}
    };

    private CatType nextCatToSpawn;
    public List<CatType> selectedCats;
    public int differentCatsToSpawn = 0;
    public bool fancySpawnedThisRound = false;
    public int currentRound = 1;
    public void Start()
    {
        SortCats();
    }

    //Cats sorted by rarity
    public Dictionary<CatRarity, List<CatType>> cats = new Dictionary<CatRarity, List<CatType>>();

    public List<CatType> SetNewRound(int roundNumber)
    {
        currentRound = roundNumber;
        List<CatType> newCatsThisRound = new List<CatType>();
        int catAmount = roundNumber + GameManager.instance.gameParameters.startingCatAmount; 
        fancySpawnedThisRound = false;
        if (catAmount > GameManager.instance.gameParameters.maximumCatAmount)
        {
            differentCatsToSpawn = GameManager.instance.gameParameters.maximumCatAmount;
        }
        else
        {
            for(int i=0; i< catAmount - differentCatsToSpawn; i++)
            {
                CatType c = getNewCat();
                if (c != null)
                {
                    nextCatToSpawn = c;
                    selectedCats.Add(c);
                    newCatsThisRound.Add(c);
                }
            }
        }

        differentCatsToSpawn = catAmount;
        return newCatsThisRound;
    }
    public CatType getNewCat()
    {
        CatType c = null;
        int i = 0;
        while (c == null)
        {
            i++;
            CatRarity rarity = spawnRates.RandomElementByWeight(e => e.Value).Key;
            //Only common cats first round
            if (currentRound == 1)
            {
                rarity = CatRarity.COMMON;
            }
            c = pickCat(rarity);
            if (c == null)
            {
                spawnRates[rarity] = 0;
            }else if (rarity == CatRarity.FANCY && c != null)
            {
                spawnRates[rarity] *= 0.5f;
            }

            if (i == 100)
            {
                break;
            }
        }



        return c;
    }
    public CatType pickCat(CatRarity rarity)
    {
        CatType c = null;
        System.Random rnd = new System.Random();
        cats[rarity] = cats[rarity].OrderBy(x => rnd.Next()).ToList();
        foreach (CatType cat in cats[rarity])
        {
            if (!selectedCats.Contains(cat))
            {
                c = cat;
                break;
            }
        }
        return c;
    }


    public void SortCats()
    {
        foreach(CatType cat in catTypes)
        {
            if(!cats.Keys.Contains(cat.rarity))
            {
                cats[cat.rarity] = new List<CatType>();
            }
            cats[cat.rarity].Add(cat);
        }
    }
    public CatType SpawnNewCat()
    {
        CatType c = selectedCats[UnityEngine.Random.Range(0, selectedCats.Count)];
        if (SettingsManager.instance.settingsValues.OnlyCommonCats)
        {
            c = pickCat(CatRarity.COMMON);
            return  c;
        }
        if (nextCatToSpawn != null)
        {
            c = nextCatToSpawn;
            nextCatToSpawn = null;
        }
        
        while(c.rarity == CatRarity.FANCY && fancySpawnedThisRound)
        {
            c = selectedCats[UnityEngine.Random.Range(0, selectedCats.Count)];
        }

        if (c.rarity == CatRarity.FANCY && !fancySpawnedThisRound)
        {
            fancySpawnedThisRound = true;
        }

        return c;
    }
}
