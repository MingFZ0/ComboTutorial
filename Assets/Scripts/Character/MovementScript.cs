using System;
using Player;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{

    public class MovementScript : MonoBehaviour
    {
        //Fields
        [SerializeField] private LayerMask jumpLayerMask;
        [SerializeField] private float dashBufferMemory;
        [SerializeField] private float walkSpeed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float groundDetectionBoxHeight;

        public bool isJumping;

        private BoxCollider2D boxCollider;
        private Rigidbody2D rb2d;
        private ActionScript actionScript;

        private List<Move> movementMoves = new();
        private Move jumpMove;
        private Move crouchMove;

        private void Awake()
        {
            actionScript = GetComponent<ActionScript>();
            rb2d = gameObject.GetComponent<Rigidbody2D>();
            boxCollider = gameObject.GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            List<Move> moves = actionScript.MovesetPriorityMap[0].Moves;
            foreach (Move move in moves)
            {
                if (move.DirectionalInput.action.name == "Up") { jumpMove = move; }
                else if (move.DirectionalInput.action.name == "Down") {  crouchMove = move; }
                else { movementMoves.Add(move); }
            }
        }

        private void Update()
        {
            if (actionScript.MovesetPriorityMap[0].LevelInput.action.IsPressed())
            {
                Vector2 movement = actionScript.MovesetPriorityMap[0].LevelInput.action.ReadValue<Vector2>();
                //Debug.Log(movement);
                List<Move> moves = actionScript.MovesetPriorityMap[0].Moves;

                //Jumping
                if (movement.y > 0 && IsGrounded() == jumpMove.Grounded)
                {
                    if (actionScript.Action(jumpMove.AnimationClip))
                    {
                        rb2d.velocity = movement * jumpForce;
                    }
                }
                else if (movement.y == 0)
                {
                    //Moving
                    foreach (Move move in movementMoves)
                    {
                        if (move.DirectionalInput.action.IsPressed() && IsGrounded() == move.Grounded)
                        {
                            if (actionScript.Action(move.AnimationClip))
                            {
                                MoveCharacter(movement, walkSpeed);
                            }
                            return;
                        }
                    }
                }
                else if (movement.y < 0 && IsGrounded() == crouchMove.Grounded)
                {
                    actionScript.Action(crouchMove.AnimationClip);
                }

            }
        }

        public void MoveCharacter(Vector2 movement, float xForce, float yForce = 1)
        {
            Vector3 newLocation = new Vector3(movement.x * xForce * Time.deltaTime, movement.y * yForce * Time.deltaTime, transform.position.z);
            transform.Translate(newLocation);
        }

        public bool IsGrounded()
        {
            float groundDetectionBoxHeight = 0.10f;
            RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, groundDetectionBoxHeight, jumpLayerMask);
            Color rayColor = Color.red;
            if (raycastHit.collider != null) { rayColor = Color.green; }
            Debug.DrawRay(boxCollider.bounds.center + new Vector3(boxCollider.bounds.extents.x, 0), Vector2.down * (boxCollider.bounds.extents.y + groundDetectionBoxHeight), rayColor);
            Debug.DrawRay(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x, 0), Vector2.down * (boxCollider.bounds.extents.y + groundDetectionBoxHeight), rayColor);
            Debug.DrawRay(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x, boxCollider.bounds.extents.y + groundDetectionBoxHeight), Vector2.right * (boxCollider.bounds.extents.x * 2f), rayColor);
            //Debug.Log(raycastHit.collider);
            return raycastHit.collider != null;
        }

        public void CutSpeed()
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x / 2, rb2d.velocity.y);
        }

    }
}