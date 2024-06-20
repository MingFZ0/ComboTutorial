using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxCollisionScript : MonoBehaviour
{
    private AttackScript attackScript;

    private void Awake()
    {
        attackScript = GetComponentInParent<AttackScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FixedJoint2D joint = collision.GetComponent<FixedJoint2D>();

        GameObject parent = GetComponent<FixedJoint2D>().connectedBody.gameObject;
        GameObject collisionParent = joint.connectedBody.gameObject;
        //Debug.Log("this parent: " + parent.name + " | Collision Parent: " + collisionParent.name);
        if (collisionParent.gameObject == parent) { return; }
        Debug.Log("hit " + collisionParent.name);
        attackScript.OnHitBoxCollide();

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (attackScript.HitBoxCollided == false) { return; }
        FixedJoint2D joint = collision.GetComponent<FixedJoint2D>();
        GameObject collisionParent = joint.connectedBody.gameObject;

        Debug.Log("hit end " + collisionParent.name);
        attackScript.OnHitBoxExit();
    }
}
