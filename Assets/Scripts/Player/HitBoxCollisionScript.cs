using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxCollisionScript : MonoBehaviour
{
    private AttackScript attackScript;
    //private BoxCollider2D collider;

    private void Awake()
    {
        attackScript = GetComponentInParent<AttackScript>();
        //collider = GetComponent<BoxCollider2D>();
        //collider = gameObject.GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject parent = gameObject.transform.parent.gameObject;
        if (collision.gameObject == parent) { return; }
        Debug.Log("hit " + collision.gameObject.name);
        attackScript.OnHitBoxCollide();
        //Debug.Log(collider.gameObject.name);

    }

    private void OnTriggerExit2D(Collider2D collision)
    { 
        attackScript.OnHitBoxExit();
    }
}
