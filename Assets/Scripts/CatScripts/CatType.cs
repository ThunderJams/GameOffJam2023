using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CatFactory;

[CreateAssetMenu]
public class CatType : ScriptableObject
{
    public string catName;
    public string description;
    public GameObject prefab;
    public CatRarity rarity;
}
