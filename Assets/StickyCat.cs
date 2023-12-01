using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyCat : MonoBehaviour
{

    private List<FixedJoint2D> joints = new List<FixedJoint2D>();
    private List<int> connectedInstances = new List<int>();
    public int maxConnections = 3;
    public int currentConnections = 0;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentConnections <maxConnections &&  collision.collider.tag == "Cat")
        {
            GameObject go = collision.collider.gameObject;
            if (!connectedInstances.Contains(go.GetInstanceID()))
            {
                //Stick the cat together
                FixedJoint2D joint = gameObject.AddComponent(typeof(FixedJoint2D)) as FixedJoint2D;
                joint.connectedBody = go.GetComponent<Rigidbody2D>();
                go.GetComponent<CatBase>().CanRotate = false;
                GetComponent<CatBase>().CanRotate = false;
                connectedInstances.Add(go.GetInstanceID());
                joints.Add(joint);
                currentConnections += 1;
            }
        }
    }
}
