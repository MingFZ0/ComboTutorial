using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Physics2D;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        //Fields
        private Rigidbody2D rb2d;
        private PlayerControlsInput playerControlsInput;
        private bool dashHeld;

        private ActionScript actionScript;
        private PlayerAttack playerAttack;

        //Attributes

        public bool IsDashing;

        // jumpuing variables
        private bool airborne;
        private bool jumpHeld;
        private float jumpForce;
        private float currentJumpDisplacement;

        private Movement dashBuffer;
        [SerializeField] private float dashBufferMemory;
        private float dashBufferTime;

        [SerializeField] private float speed;
        [SerializeField] private float jumpHeight;
        [SerializeField] private float dashSpeed;
        [SerializeField] private Rigidbody2D floor;

        private void Awake()
        {
            actionScript = GetComponent<ActionScript>();
            playerAttack = GetComponent<PlayerAttack>();

            rb2d = gameObject.GetComponent<Rigidbody2D>();
            playerAttack = gameObject.GetComponent<PlayerAttack>();

            playerControlsInput = new PlayerControlsInput();
            playerControlsInput.Player.Enable();
          
        }

        private void Update()
        {
            if (jumpHeld) 
            if (!jumpHeld && playerControlsInput.Player.Up.WasPressedThisFrame() && !airborne)
            if (dashHeld) PrepareDash();
            if (dashHeld == false && playerControlsInput.Player.Move.WasPressedThisFrame() && playerAttack.IsAttacking == false) StoreDashBuffer();


            if (IsDashing) return;
            //Check for Attacking
            if (playerAttack.IsAttacking)
            {
                rb2d.velocity = new Vector2(0, 0);
                return;
            }

            //Check for Movement
            if (playerControlsInput.Player.Move.IsPressed()) Moving();
            else
            {
                rb2d.velocity = new Vector2(0, 0);
                actionScript.Action(Movement.Player_Idle.ToString());
            }

        }


        private void Moving()
        {
            Vector2 movement = playerControlsInput.Player.Move.ReadValue<Vector2>();
            if (IsDashing) return;
            if (movement.x != 0)
            {
                if (movement.x > 0) { 
                    actionScript.Action(Movement.Player_WalkForward.ToString());
                    dashBuffer = Movement.Player_WalkForward;
                }
                else if (movement.y != 0)
                {
                    //placeholder for now
                    actionScript.Action(Movement.Player_Idle.ToString());
                }
                else {
                    actionScript.Action(Movement.Player_WalkBackward.ToString());
                    dashBuffer = Movement.Player_WalkBackward;
                }
            }

            rb2d.velocity = new Vector2(movement.x * speed, rb2d.velocity.y);
        }

        /// <summary>
        /// Storing Buffer for Dash && Start BufferTimer if the player moves
        /// </summary>
        /// <returns></returns>
        private void StoreDashBuffer()
        {
            Vector2 movement = playerControlsInput.Player.Move.ReadValue<Vector2>();

            if (movement.x > 0) dashBuffer = Movement.Player_WalkForward;
            if (movement.x < 0) dashBuffer = Movement.Player_WalkBackward;
            else if (movement.x == 0)
            {
                dashBuffer = Movement.Player_Idle;
                return;
            }

            dashBufferTime = dashBufferMemory;
            dashHeld = true;
        }
        
        private void PrepareDash()
        {
            if (playerControlsInput.Player.Move.WasPressedThisFrame() && IsDashing == false && dashBufferTime >= 0)
            {
                Vector2 movement = playerControlsInput.Player.Move.ReadValue<Vector2>();
                if (movement.x > 0 && dashBuffer.ToString() == Movement.Player_WalkForward.ToString()) Dashing(Dash.Player_DashForward);
                else if (movement.x < 0 && dashBuffer.ToString() == Movement.Player_WalkBackward.ToString()) Dashing(Dash.Player_DashBackward);
                dashHeld = false;
                //Debug.Log("Dash Ready!");
                return;
            }

            if (dashBufferTime < 0)
            {
                dashHeld = false;
                dashBufferTime = dashBufferMemory;
            }
            else dashBufferTime -= Time.deltaTime;
            return;
        }

        private void Dashing(Dash dash)
        {
            if (actionScript.Action(dash.ToString()))
            {
                //Debug.Log(dash.ToString());
                dashBuffer = Movement.Player_Idle;
                IsDashing = true;
            } else return;
            if (dash == Dash.Player_DashForward) rb2d.velocity = new Vector2(dashSpeed, 0);
            else if (dash == Dash.Player_DashBackward) rb2d.velocity = new Vector2(-dashSpeed, 0);
        }

        public void CutSpeed()
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x / 2, rb2d.velocity.y);
        }

        /// <summary>
        /// checks to see if the jump key has been pressed long enough for a full height jump
        /// </summary>
        /// <returns></returns>
        private bool isFullJump()
        {
            return false;
        }
    }
}