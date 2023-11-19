using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostCat : MonoBehaviour
{
    void Start()
    {
        GameManager.OnEndOfRound += EndOfRound;
    }

    void EndOfRound()
    {
        GameManager.instance.RemoveCat(gameObject);
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        GameManager.OnEndOfRound -= EndOfRound;
    }
}
