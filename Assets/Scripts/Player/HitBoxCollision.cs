using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxCollision : MonoBehaviour
{
    private ActionScript actionScript;
    private BoxCollider2D collider;

    private void Awake()
    {
        actionScript = GetComponentInParent<ActionScript>();
        collider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != GetComponentInParent<Collider2D>().gameObject)
        {
            Debug.Log("hit");
            actionScript.OnHitBoxCollide();
        }
        Debug.Log(collider.gameObject.name);
        
    }


    private void OnTriggerExit2D(Collider2D collision)
    { 
        actionScript.OnHitBoxExit();
    }
}
