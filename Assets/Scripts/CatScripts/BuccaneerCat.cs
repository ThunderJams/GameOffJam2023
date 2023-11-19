using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuccaneerCat : MonoBehaviour
{
    void Start()
    {
        GameManager.instance.ChangeBuccaneer(1);
    }

    void OnDestroy()
    {
        GameManager.instance.ChangeBuccaneer(-1);
    }
}
