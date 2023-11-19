using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCursor : MonoBehaviour
{
    ParticleSystem ps;
    void Start()
    {
        ps = GetComponent<ParticleSystem>();

    }

    public float maxJumpDistance = 3;
    void Update()
    {
        // Get the position of the mouse cursor in screen space
        Vector3 mousePos = Input.mousePosition;

        // Convert the screen space position of the cursor to world space (2D)
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, -Camera.main.transform.position.z));

        
        // Calculate the direction from the particle system to the cursor position
        Vector2 direction = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);

        // Calculate the angle in radians and convert it to degrees
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg ;


        // Set the updated rotation for the particles
        ParticleSystem.MainModule mainModule = ps.main;
        mainModule.startRotation = angle * Mathf.Deg2Rad;

        if (Vector3.Distance(ps.transform.position, mousePos)> maxJumpDistance)
        {
            ps.Stop();
            ps.transform.position = mousePos;
            ps.Play();
        }
        else
        {

            ps.transform.position = mousePos;

        }
    }
}
