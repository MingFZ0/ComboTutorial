using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        //Fields
        private Rigidbody2D rb2d;
        private PlayerControlsInput playerControlsInput;
        private bool held;

        private PlayerScript playerScript;
        private PlayerAnimator playerAnimator;
        private PlayerAttack playerAttack;

        //Attributes

        public bool IsDashing;

        private Movement dashBuffer;
        [SerializeField] private float dashBufferMemory;
        private float dashBufferTime;

        [SerializeField] private float speed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float dashSpeed;

        private void Awake()
        {
            playerScript = GetComponent<PlayerScript>();
            playerAnimator = GetComponent<PlayerAnimator>();
            playerAttack = GetComponent<PlayerAttack>();

            rb2d = gameObject.GetComponent<Rigidbody2D>();
            playerAttack = gameObject.GetComponent<PlayerAttack>();

            playerControlsInput = new PlayerControlsInput();
            playerControlsInput.Player.Enable();
          
        }

        private void Update()
        {
           
            if (IsDashing) { return; }
            if (held) PrepareDash();
            if (held == false && playerControlsInput.Player.Move.WasPressedThisFrame() && IsDashing == false) StoreDashBuffer();
            
            

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
                playerScript.Action(Movement.Player_Idle.ToString());
            }

        }


        private void Moving()
        {
            Vector2 movement = playerControlsInput.Player.Move.ReadValue<Vector2>();
            if (IsDashing) return;
            if (movement.x != 0)
            {
                if (movement.x > 0) { 
                    playerScript.Action(Movement.Player_WalkForward.ToString());
                    dashBuffer = Movement.Player_WalkForward;
                }
                else {
                    playerScript.Action(Movement.Player_WalkBackward.ToString());
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
            held = true;
        }
        
        private void PrepareDash()
        {
            if (playerControlsInput.Player.Move.WasPressedThisFrame() && IsDashing == false && dashBufferTime >= 0)
            {
                Vector2 movement = playerControlsInput.Player.Move.ReadValue<Vector2>();
                if (movement.x > 0 && dashBuffer.ToString() == Movement.Player_WalkForward.ToString()) Dashing(Dash.Player_DashForward);
                else if (movement.x < 0 && dashBuffer.ToString() == Movement.Player_WalkBackward.ToString()) Dashing(Dash.Player_DashBackward);
                //Debug.Log("Dash Ready!");
                return;
            }

            if (dashBufferTime < 0)
            {
                held = false;
                dashBufferTime = dashBufferMemory;
            }
            else dashBufferTime -= Time.deltaTime;
            return;
        }

        private void Dashing(Dash dash)
        {
            if (playerScript.Action(dash.ToString()))
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

        public void FinishedDashing()
        {
            //Debug.Log("Dash Ended");
            IsDashing = false;
            playerScript.Action(Movement.Player_Idle.ToString());
        }
    }
}