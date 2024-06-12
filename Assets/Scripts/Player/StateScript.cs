using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateScript : MonoBehaviour
{
    private MovementScript movementScript;
    private AnimatorScript animatorScript;

    private void Awake()
    {
        movementScript = GetComponent<MovementScript>();
        animatorScript = GetComponent<AnimatorScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Boolean IsGrounded() { return movementScript.IsGrounded(); }
}
