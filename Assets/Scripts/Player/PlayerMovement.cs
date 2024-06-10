using System;
using Player;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{

    public class PlayerMovement : MonoBehaviour
    {
        //Fields
        [SerializeField] private float dashBufferMemory;
        [SerializeField] private float walkSpeed;

        private Rigidbody2D rb2d;
        private ActionScript actionScript;
        
        private void Awake()
        {
            actionScript = GetComponent<ActionScript>();
            rb2d = gameObject.GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (actionScript.MovesetPriorityMap[0].LevelInput.action.IsPressed())
            {
                Vector2 movement = actionScript.MovesetPriorityMap[0].LevelInput.action.ReadValue<Vector2>();
                Debug.Log(movement);
                List<Move> moves = actionScript.MovesetPriorityMap[0].Moves;
                foreach (Move move in moves)
                {
                    if (move.DirectionalInput.action.IsPressed())
                    {
                        actionScript.Action(move.ToString());
                        MoveCharacter(movement, walkSpeed);
                        return;
                    }
                }
                //if (movement.y < 0) actionScript.Action(movementMoveset.Moves[2].ToString());
            }
        }

        public void MoveCharacter(Vector2 movement, float force)
        {
            rb2d.velocity = movement * force;
        }

        public void CutSpeed()
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x / 2, rb2d.velocity.y);
        }

    }
}