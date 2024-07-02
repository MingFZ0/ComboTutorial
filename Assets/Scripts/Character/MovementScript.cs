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
        #region Walk
        [Header("Walk Attributes")]
        [SerializeField] private float walkSpeed;
        [Space]
        #endregion

        //Jump
        #region Jump
        [Header("Jump Attributes")]
        [SerializeField] private LayerMask jumpLayerMask;
        [SerializeField] private float groundDetectionBoxHeight;
        private float _jumpTime;
        public bool isJumping;
        private Vector2 jumpingMotion;

        private WaitForSeconds waitForAFrame = new WaitForSeconds(0.0133f);
        private Coroutine _dashInputCoroutine;
        [Space]
        #endregion

        //Dash
        #region Dash
        [Header("Dash Attributes")]
        [SerializeField] private float dashBufferMemory;
        private Move _dashBufferedMove;
        private float _dashBufferMemory;
        private float _dashForce;
        private float _dashDirecton;

        private float _dashTime;
        private float _dashTotalHorizontalTime;
        private Move _dashMove;
        private bool isDashing;
        #endregion

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
            if (isDashing) { DashPhysicsExecute(); }
        }

        private void Update()
        {

            if (_dashBufferMemory > 0 && !isJumping)
            {
                if (_dashBufferedMove.DirectionalInput.action.WasPressedThisFrame())
                {
                    foreach (Move dashMove in actionScript.MovesetPriorityMap[1].Moves)
                    {
                        if (dashMove.DirectionalInput.name == _dashBufferedMove.DirectionalInput.name && actionScript.Action(dashMove.AnimationClip))
                        {
                            _dashMove = dashMove;
                            _dashTotalHorizontalTime = _dashMove.MovementAcceration.HorizontalAccerationCurve[_dashMove.MovementAcceration.HorizontalAccerationCurve.length - 1].time;
                            isDashing = true;
                        }
                    }
                }
            }

            if (!isJumping && !isDashing && actionScript.MovesetPriorityMap[0].LevelInput.action.IsPressed())
            {
                Vector2 movement = actionScript.MovesetPriorityMap[0].LevelInput.action.ReadValue<Vector2>();
                List<Move> moves = actionScript.MovesetPriorityMap[0].Moves;

                //if (movement.y > 0 && IsGrounded() == jumpMove.Grounded && !isJumping) { ButtonJumpingMovement(movement); }
                if (movement.y > 0 && !isJumping)
                {
                    ButtonJumpingMovement(movement);
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
                Debug.Log("Jumping");
                isJumping = true;
                Vector3 newPos = new Vector3(movement.x, movement.y, transform.position.z);
                _jumpTime = 0f;
                jumpingMotion = newPos;

                transform.Translate(new Vector2(jumpingMotion.x * jumpMove.MovementAcceration.HorizontalAccerationCurve.Evaluate(0), jumpingMotion.y * jumpMove.MovementAcceration.VerticalAccerationCurve.Evaluate(0)) * Time.deltaTime);
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
                        if (_dashInputCoroutine == null && move.DirectionalInput.action.WasPressedThisFrame()) { 
                            _dashInputCoroutine = StartCoroutine(PrepareDash(move));
                            _dashDirecton = movement.x;
                        }
                        else if (move.DirectionalInput.action.WasPressedThisFrame()) { 
                            StopCoroutine(_dashInputCoroutine);
                            _dashInputCoroutine = StartCoroutine(PrepareDash(move));
                            _dashDirecton = movement.x;
                        }
                        MoveCharacter(movement, walkSpeed);
                    }
                    return;
                }
            }
        }

        public void JumpPhysicExecute()
        {

            _jumpTime += Time.deltaTime;
            float currentVerticalJumpForce;
            float currentHorizontalJumpForce;
            AnimationCurve verticalJumpForce = jumpMove.MovementAcceration.VerticalAccerationCurve;
            AnimationCurve horizontalJumpForce = jumpMove.MovementAcceration.HorizontalAccerationCurve; ;
            if (_jumpTime >= verticalJumpForce[verticalJumpForce.length - 1].time) { currentVerticalJumpForce = verticalJumpForce[verticalJumpForce.length - 1].value; }
            else { currentVerticalJumpForce = verticalJumpForce.Evaluate(_jumpTime); }

            if (_jumpTime >= horizontalJumpForce[horizontalJumpForce.length - 1].time) { currentHorizontalJumpForce = horizontalJumpForce[horizontalJumpForce.length - 1].value; }
            else { currentHorizontalJumpForce = horizontalJumpForce.Evaluate(_jumpTime); }
            transform.Translate(new Vector3(jumpingMotion.x * currentHorizontalJumpForce, jumpingMotion.y * currentVerticalJumpForce) * Time.deltaTime );
            if (IsGrounded() && isJumping && currentVerticalJumpForce < 0) 
            {
                _jumpTime = 0f;
                RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, groundDetectionBoxHeight, jumpLayerMask);
                Vector2 surface = Physics2D.ClosestPoint(transform.position, raycastHit.collider) + Vector2.up * 2;
                transform.position = new Vector2(transform.position.x, surface.y + groundDetectionBoxHeight);
                isJumping = false; 
            }
        }

        public void DashPhysicsExecute()
        {
            if (_dashTime > _dashTotalHorizontalTime) { return; }
            _dashTime += Time.deltaTime;

            float dashForce = _dashMove.MovementAcceration.HorizontalAccerationCurve.Evaluate(_dashTime);
              
            transform.Translate(new Vector2(dashForce * _dashDirecton * Time.deltaTime, 0));
        }

        public void MoveCharacter(Vector2 movement, float xForce, float yForce = 1)
        {
            Vector3 newLocation = new Vector3(movement.x * xForce * Time.deltaTime, movement.y * yForce * Time.deltaTime, transform.position.z);
            transform.Translate(newLocation);
        }

        public void MoveCharacter(float xForce, float yForce)
        {
            transform.Translate(new Vector2(xForce * Time.deltaTime, yForce * Time.deltaTime));
        }

        public bool IsGroundedWithJumping()
        {
            if (isJumping) { return false; }
            else { return IsGrounded(); }
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

        public void SetJumpingTrue() { this.isJumping = true; }
        public void SetJumpingFalse() 
        {  
            this.isJumping = false;
            this.jumpingMotion = Vector2.zero;
            //this._jumpForce = this.jumpForce;
        }

        private IEnumerator PrepareDash(Move move)
        {
            //Debug.Log("Moving " + move.AnimationClip.name);
            _dashBufferMemory = dashBufferMemory;
            _dashBufferedMove = move;

            yield return waitForAFrame;

            while (_dashBufferMemory > 0)
            {
                _dashBufferMemory--;
                yield return waitForAFrame;
            }

            isDashing = false;
            _dashForce = 0;
            _dashTime = 0;
            yield break;
        }
    
        
    }
}