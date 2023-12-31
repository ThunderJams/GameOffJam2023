using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Script that do effects when the player moves, click and interact with objects
/// </summary>
public class ClickManager : MonoBehaviour
{
    public Texture2D cursorTexture;
    public Texture2D clickedCursorTexture;

    public Vector2 cursorHotspot;

    public static ClickManager instance;
    // Start is called before the first frame update

    public ParticleSystem clickParticleSystemBase;

    public List<ParticleSystem> clickParticleSystems = new List<ParticleSystem>();
    
    public int currentClickParticleIndex;
    public int maxClickParticleAtOnce = 10;
    void Start()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < maxClickParticleAtOnce; i++)
        {
            ParticleSystem particle = GameObject.Instantiate(clickParticleSystemBase);
            particle.transform.parent = transform;
            particle.transform.name = "PooledParticleSystem " + i; 
            clickParticleSystems.Add(particle);

        }
        Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.ForceSoftware);
    }

    // Update is called once per frame
    void Update()
    {
        //Todo : do this for other key schemes as well
        if (Input.GetMouseButtonDown(0))
        {
            //play particle system once 
            var particleToUse = clickParticleSystems[currentClickParticleIndex];
            particleToUse.Clear();
            Vector3 screenToWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            particleToUse.transform.position =new Vector3(screenToWorld.x,screenToWorld.y,0);
            particleToUse.Play();
            currentClickParticleIndex = (currentClickParticleIndex + 1) % clickParticleSystems.Count;
            Cursor.SetCursor(clickedCursorTexture, cursorHotspot, CursorMode.ForceSoftware);
        }
        if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.ForceSoftware);
        }

        // PET THE CAT
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("CatMenu"))
                {
                    hit.collider.gameObject.GetComponent<petTheCats>().PetTheCat();
                }

            }
        }
    }
}
