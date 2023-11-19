using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CatCannon : MonoBehaviour
{
    [SerializeField] GameObject cannon;
    private HingeJoint2D cannonHinge;
    [HideInInspector] public GameObject cat;
    [SerializeField] GameObject tip;
    private float fuse;
    private float loadAnimTime;

    private float cannonLimit;

    void Start()
    {
        cannonHinge = cannon.GetComponent<HingeJoint2D>();
        float motorStartSpeed = Random.Range(30, 40);
        if (Random.Range(0, 2) == 0)
            motorStartSpeed *= -1;
        SetMotorSpeed(motorStartSpeed);
        cannonLimit = cannonHinge.limits.max - 0.1f;
    }

    void Update()
    {
        if (GameManager.instance.gameOver)
            return;

        if (DebugMenu.instance.cannonMove)
        {
            if ((cannon.transform.rotation.eulerAngles.z < 180 && cannon.transform.rotation.eulerAngles.z >= cannonLimit && Mathf.Sign(cannonHinge.motor.motorSpeed) == -1) || 
            (cannon.transform.rotation.eulerAngles.z > 180 && cannon.transform.rotation.eulerAngles.z <= 360 - cannonLimit && Mathf.Sign(cannonHinge.motor.motorSpeed) == 1))
            SetMotorSpeed(Random.Range(20, 80) * -Mathf.Sign(cannonHinge.motor.motorSpeed));
        }

        if (cat == null)
            return;

        loadAnimTime -= Time.deltaTime;
        if (loadAnimTime < 0)
            loadAnimTime = 0;

        cat.transform.position = Vector3.Lerp(tip.transform.position, transform.position, loadAnimTime);
        cat.transform.rotation = tip.transform.rotation;

        if (fuse > 0)
            fuse -= Time.deltaTime;
        else
            FireCat();
    }

    public void LoadCat(GameObject loadCat)
    {
        cat = loadCat;
        cat.transform.position = Vector3.Lerp(tip.transform.position, transform.position, 0.5f);
        cat.transform.rotation = tip.transform.rotation;
        foreach(SpriteRenderer sr in cat.GetComponentsInChildren<SpriteRenderer>())
            sr.sortingOrder = 0;
        cat.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        cat.GetComponent<Rigidbody2D>().angularVelocity = 0;
        cat.GetComponent<Rigidbody2D>().gravityScale = 0;

        fuse = GameManager.instance.gameParameters.cannonFuseBaseSpeed / GameManager.instance.FinalTimeIncrement;
        loadAnimTime = 0.5f * GameManager.instance.FinalTimeIncrement;
    }

    public void FireCat()
    {
        Debug.Log("Firing cat");

        cat.GetComponent<Rigidbody2D>().gravityScale = cat.GetComponent<CatBase>().gravity * GameManager.instance.gameParameters.globalCatGravityMultiplier;
        cat.transform.position = tip.transform.position;
        cat.GetComponent<Rigidbody2D>().velocity = tip.transform.up * Random.Range(GameManager.instance.gameParameters.minCannonPropellingStrength, GameManager.instance.gameParameters.maxCannonPropellingStrength);
        cat = null;
    }

    void SetMotorSpeed(float speed)
    {
        JointMotor2D motor = cannonHinge.motor;
        motor.motorSpeed = speed;
        cannonHinge.motor = motor;
    }
}
