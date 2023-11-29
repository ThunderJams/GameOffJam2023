using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostCat : MonoBehaviour
{
    public AudioClip clipFade;
    void Start()
    {
        GameManager.OnEndOfRound += EndOfRound;
    }

    void EndOfRound()
    {
        AudioManager.instance.PlaySound(clipFade.name, 0.6f, Random.Range(0.9f, 1.1f));
        GameManager.instance.RemoveCat(gameObject);
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        GameManager.OnEndOfRound -= EndOfRound;
    }
}
