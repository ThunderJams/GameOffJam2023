using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private GameObject heldCat;
    private bool holding = false;

    private float holdDuration = 0.0f;

    void Update()
    {
        if (GameManager.instance.gameOver)
            return;

        if (heldCat == null)
        {
            holding = false;
        }

        // If holding cat, move cat to mouse position
        if (holding == true)
        {
            Vector3 catToMouseOffset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - heldCat.transform.position;
            Rigidbody2D catBody = heldCat.GetComponent<Rigidbody2D>();
            catBody.velocity = catToMouseOffset * 10;
            //clamp velocity
            if (catBody.velocity.magnitude > 30)
                catBody.velocity = catBody.velocity.normalized * 30;
            catBody.mass = 0.005f;

            holdDuration += Time.deltaTime;

            if (holdDuration > 2)
            {
                if (heldCat.GetComponent<AngryKitty>())
                {
                    DropCat();
                }
            }
        }

        // Drop cat
        if (Input.GetMouseButtonUp(0) && holding == true)
        {
            DropCat();
        }

        // Try pick up cat
        if (Input.GetMouseButtonDown(0) && holding == false) 
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if(hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("Cat") && hit.collider.gameObject.GetComponent<CatBase>().activated == true)
                {
                    heldCat = hit.collider.gameObject;
                    holding = true;
                    heldCat.GetComponent<CatBase>().PickUp();
                
                    holdDuration = 0.0f;
                }
                
            }
        }

        // Rotate cat with mouse wheel
        if (Input.GetAxis("Mouse ScrollWheel") != 0 && holding == true)
        {
            heldCat.transform.Rotate(0, 0, Input.GetAxis("Mouse ScrollWheel") * 60000 * Time.deltaTime);
        }

        //Rotat cat with right click
        if (Input.GetMouseButton(1) && holding == true)
        {
            if (heldCat.GetComponent<CatBase>().CanRotate)
            {
                heldCat.transform.Rotate(0, 0, 300 * Time.deltaTime);
            }
        }
    }

    public void DropCat()
    {
        Rigidbody2D catBody = heldCat.GetComponent<Rigidbody2D>();
        catBody.mass = 0.1f;
        holding = false;
        Vector2 mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        heldCat.GetComponent<Rigidbody2D>().velocity = mouseMovement * 10;
        heldCat = null;

        holdDuration = 0.0f;
    }
}
