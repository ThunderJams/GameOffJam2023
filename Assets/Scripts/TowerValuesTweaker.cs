using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerValuesTweaker : MonoBehaviour
{
    public Rigidbody2D DishL;
    public Rigidbody2D DishR;
    public Rigidbody2D Beam;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitWhile(()=> { return GameManager.instance == null; });
        DishL.drag = GameManager.instance.gameParameters.dishLinearDrag;
        DishL.mass = GameManager.instance.gameParameters.dishMass;
        DishR.drag = GameManager.instance.gameParameters.dishLinearDrag;
        DishR.mass = GameManager.instance.gameParameters.dishMass;
        Beam.angularDrag = GameManager.instance.gameParameters.towerBeamDrag;
        Beam.mass = GameManager.instance.gameParameters.towerBeamMass;
    }

}
