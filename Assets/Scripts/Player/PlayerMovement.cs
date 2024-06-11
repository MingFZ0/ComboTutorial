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
        [SerializeField] private float jumpForce;

        public bool isJumping;

        private Rigidbody2D rb2d;
        private ActionScript actionScript;

        private List<Move> movementMoves = new();
        private Move jumpMove;
        private Move crouchMove;
        
        private void Awake()
        {
            actionScript = GetComponent<ActionScript>();
            rb2d = gameObject.GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            List<Move> moves = actionScript.MovesetPriorityMap[0].Moves;
            Debug.Log(moves.Count);
            foreach (Move move in moves)
            {
                if (move.DirectionalInput.action.name == "Up") { jumpMove = move; }
                else if (move.DirectionalInput.action.name == "Down") { crouchMove = move; }
                else { movementMoves.Add(move); }
            }
        }

        private void Update()
        {
            if (actionScript.MovesetPriorityMap[0].LevelInput.action.IsPressed())
            {
                Vector2 movement = actionScript.MovesetPriorityMap[0].LevelInput.action.ReadValue<Vector2>();
                Debug.Log(movement);
                List<Move> moves = actionScript.MovesetPriorityMap[0].Moves;

                //Jumping
                if (movement.y > 0 && isJumping == false)
                {
                    isJumping = true;
                    actionScript.Action(jumpMove.ToString());
                    MoveCharacter(movement, 1, jumpForce);
                    rb2d.velocity = movement * jumpForce;
                }
                else if (movement.y == 0)
                {
                    //Moving
                    foreach (Move move in movementMoves)
                    {
                        if (move.DirectionalInput.action.IsPressed())
                        {
                            actionScript.Action(move.ToString());
                            Debug.Log(movement);
                            MoveCharacter(movement, walkSpeed);
                            //MoveCharacter(movement, walkSpeed);
                            //rb2d.AddForce(movement * walkSpeed, ForceMode2D.Impulse);
                            //MoveCharacter(movement, walkSpeed);
                            return;
                        }
                    }
                } 
                else
                {
                    actionScript.Action(crouchMove.ToString());
                }

            }
        }

        public void MoveCharacter(Vector2 movement, float xForce, float yForce = 1)
        {
            Vector3 newLocation = new Vector3(movement.x * xForce * Time.deltaTime, movement.y * yForce * Time.deltaTime, transform.position.z);
            Debug.Log(transform.position + " to " + newLocation);
            transform.Translate(newLocation);
        }

        public void CutSpeed()
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x / 2, rb2d.velocity.y);
        }

    }
}