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
        private BoxCollider2D boxCollider;
        private Rigidbody2D rb2d;
        private ActionScript actionScript;

        private List<Move> movementMoves = new();
        private Move jumpMove;
        private Move crouchMove;


        //Walk
        [Header("Walk Attributes")]
        [SerializeField] private float walkSpeed;

        [Space]

        //Jump
        [Header("Jump Attributes")]
        [SerializeField] private LayerMask jumpLayerMask;
        [SerializeField] private float groundDetectionBoxHeight;
        [SerializeField] private Vector2 jumpForce;
        [SerializeField] private float fallForce;
        [Range(0, 1)] [SerializeField] private float risingHorizontalMovementMultiplier;
        [Range(0, 1)] [SerializeField] private float fallingHorizontalMovementMultiplier;
        [Range(0, 1)] [SerializeField] private float risingVerticalMovementMultiplier;
        [SerializeField] private float fallingVerticalMovementMultiplier;

        private float _jumpForceVertical;
        private float _jumpForceHorizontal;
        public bool isJumping;
        private Vector2 jumpingMotion;

        [Space]

        //Dash
        [Header("Dash Attributes")]

        [SerializeField] private float dashBufferMemory;
        
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

        private void FixedUpdate()
        {
            if (isJumping)
            {
                JumpPhysicExecute();
            }
        }

        private void Update()
        {

            if (!isJumping && actionScript.MovesetPriorityMap[0].LevelInput.action.IsPressed())
            {
                Vector2 movement = actionScript.MovesetPriorityMap[0].LevelInput.action.ReadValue<Vector2>();
                List<Move> moves = actionScript.MovesetPriorityMap[0].Moves;

                //if (movement.y > 0 && IsGrounded() == jumpMove.Grounded && !isJumping) { ButtonJumpingMovement(movement); }
                if (movement.y > 0 && !isJumping)
                {
                    isJumping = true;
                    Vector3 newPos = new Vector3(movement.x, movement.y, transform.position.z);
                    _jumpForceHorizontal = jumpForce.x * 0.5f;
                    _jumpForceVertical = jumpForce.y;
                    jumpingMotion = newPos;

                    transform.Translate(new Vector2(jumpingMotion.x * _jumpForceHorizontal, jumpingMotion.y * _jumpForceVertical) * Time.deltaTime);
                }
                if (movement.y == 0) { ButtonHorizontalMovement(movement); }
                else if (movement.y < 0 && !isJumping && IsGrounded() == crouchMove.Grounded) {actionScript.Action(crouchMove.AnimationClip);}

            }

            //if (IsGrounded() == false && rb2d.velocity.y < 0) { rb2d.velocity -= Vector2.down * (Physics2D.gravity.y * fallForce) * Time.deltaTime; }
        }

        private void ButtonJumpingMovement(Vector2 movement) 
        { 
            if (actionScript.Action(jumpMove.AnimationClip)) 
            {
                isJumping = true;
                jumpingMotion = movement;
                Debug.Log("Jumped " + actionScript.CurrentAction + " " + actionScript.playerAnimator.CurrentAnimation);
                //rb2d.velocity = jumpingMotion * _jumpForce;
            } 
        }

        private void ButtonHorizontalMovement(Vector2 movement)
        {
            foreach (Move move in movementMoves)
            {
                if (move.DirectionalInput.action.IsPressed() && !isJumping && IsGroundedWithJumping() == move.Grounded)
                {
                    if (actionScript.Action(move.AnimationClip))
                    {
                        Debug.Log("Moving " + move.AnimationClip.name);
                        MoveCharacter(movement, walkSpeed);
                    }
                    return;
                }
            }
        }

        public void JumpPhysicExecute()
        {
            if (_jumpForceVertical < 1)
            {
                _jumpForceVertical = Math.Abs(_jumpForceVertical) * -1;
                _jumpForceVertical *= fallingVerticalMovementMultiplier;
                _jumpForceHorizontal *= fallingHorizontalMovementMultiplier;
            }
            else { 
                _jumpForceVertical *= risingVerticalMovementMultiplier;

                if (_jumpForceVertical < 1) { _jumpForceVertical = -1f; }
                _jumpForceHorizontal *= risingHorizontalMovementMultiplier;
            }


            transform.Translate(new Vector3(jumpingMotion.x * _jumpForceHorizontal, jumpingMotion.y * _jumpForceVertical) * Time.deltaTime );
            if (IsGrounded() && isJumping && _jumpForceVertical < 0) 
            {
                _jumpForceVertical = 0;
                _jumpForceHorizontal = 0;
                RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, groundDetectionBoxHeight, jumpLayerMask);
                Vector2 surface = Physics2D.ClosestPoint(transform.position, raycastHit.collider) + Vector2.up * 2;
                transform.position = new Vector2(transform.position.x, surface.y + groundDetectionBoxHeight);
                isJumping = false; 
            }
            //jumpingMotion += new Vector2(jumpingMotion.x, jumpingMotion.y + Physics2D.gravity.y * fallForce * Time.deltaTime);
            //transform.Translate(jumpingMotion *  Time.deltaTime);
            //rb2d.velocity += new Vector2 (jumpingMotion.x*(Physics2D.gravity.y * fallForce * Time.deltaTime), jumpingMotion.y*(Physics2D.gravity.y * fallForce * Time.deltaTime));
        }

        public void MoveCharacter(Vector2 movement, float xForce, float yForce = 1)
        {
            Vector3 newLocation = new Vector3(movement.x * xForce * Time.deltaTime, movement.y * yForce * Time.deltaTime, transform.position.z);
            transform.Translate(newLocation);
        }

        public bool IsGroundedWithJumping()
        {
            return isJumping || IsGrounded();
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

        public void CutSpeed() {rb2d.velocity = new Vector2(rb2d.velocity.x / 2, rb2d.velocity.y);}

        public void SetJumpingTrue() { this.isJumping = true; }
        public void SetJumpingFalse() 
        {  
            this.isJumping = false;
            this.jumpingMotion = Vector2.zero;
            //this._jumpForce = this.jumpForce;
        }
    }
}