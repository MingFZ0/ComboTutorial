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
        private Vector2 previousMovement;
        private bool held;

        private PlayerAnimator playerAnimator;
        private PlayerAttack playerAttack;

        //Attributes

        public bool IsDashing;

        private Movement dashBuffer;
        [SerializeField] private float dashBufferMemory;
        private float dashBufferTime;
        private float dashTimer;

        [SerializeField] private float speed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float dashSpeed;
        [SerializeField] private float dashDuration;

        private void Awake()
        {
            playerAnimator = GetComponent<PlayerAnimator>();
            playerAttack = GetComponent<PlayerAttack>();

            rb2d = gameObject.GetComponent<Rigidbody2D>();
            playerAttack = gameObject.GetComponent<PlayerAttack>();

            playerControlsInput = new PlayerControlsInput();
            playerControlsInput.Player.Enable();
            playerControlsInput.Player.Move.performed += Moving => held = true;
            playerControlsInput.Player.Move.canceled += _ => held = false;
          
        }

        private void Update()
        {
            if (IsDashing)
            {
                dashTimer -= Time.deltaTime;
                if (dashTimer < 0) 
                { 
                    IsDashing = false;
                    rb2d.velocity = new Vector2(0, rb2d.velocity.y);
                }
            }
            else if (IsDashing == false)
            {
                if (IsDashing == false && dashBufferTime == 0)
                {
                    dashBufferTime = dashBufferMemory;
                }
                else if (playerControlsInput.Player.Right.WasPerformedThisFrame() && dashBuffer == Movement.Player_WalkForward && dashBufferTime > 0 && IsDashing == false) Dashing(Dash.Player_DashForward);
                else if (playerControlsInput.Player.Left.WasPerformedThisFrame() && dashBuffer == Movement.Player_WalkBackward && dashBufferTime > 0 && IsDashing == false) Dashing(Dash.Player_DashBackward);

                if (dashBufferTime > 0) { dashBufferTime -= Time.deltaTime; }
                else { dashBuffer = Movement.Player_Idle; }


                if (playerAttack.IsAttacking)
                {
                    rb2d.velocity = new Vector2(0, 0);
                    return;
                }
                if (held) Moving(previousMovement);
                else
                {
                    previousMovement = new Vector2(0, 0);
                    playerAnimator.Moving("Idle");
                }

                if (dashBufferTime >= 0) dashBufferTime -= Time.deltaTime;
                else dashBuffer = Movement.Player_Idle;
            }

        }

        public void Moving(InputAction.CallbackContext context)
        {
            if (IsDashing) return;
            Vector2 movement = context.ReadValue<Vector2>();

            if (movement.x == 0 || movement.y < 0)
            {
                previousMovement = new Vector2(0, rb2d.velocity.y);
            }
            else if (movement.x != 0)
            {
                if (movement.x > 0) { playerAnimator.Moving("Forward"); }
                else { playerAnimator.Moving("Backward"); }

                previousMovement = new Vector2(movement.x * speed, rb2d.velocity.y);
            }
            else
            {
                previousMovement = new Vector2(0, rb2d.velocity.y);
            }

            rb2d.velocity = previousMovement;
        }

        public void Moving(Vector2 movement)
        {
            if (IsDashing) return;
            if (movement.x != 0)
            {
                if (movement.x > 0) { 
                    playerAnimator.Moving("Forward");
                    dashBuffer = Movement.Player_WalkForward;
                }
                else { 
                    playerAnimator.Moving("Backward");
                    dashBuffer = Movement.Player_WalkBackward;
                }
                dashBufferTime = dashBufferMemory;
            }

            rb2d.velocity = previousMovement;
        }

        private void Dashing(Dash dash)
        {
            Debug.Log(dash.ToString());
            if (dash == Dash.Player_DashForward) rb2d.velocity = new Vector2(dashSpeed, 0);
            else if (dash == Dash.Player_DashBackward) rb2d.velocity = new Vector2(-dashSpeed, 0);

            //previousMovement = new Vector2(dashSpeed, 0);
            dashBuffer = Movement.Player_Idle;
            dashTimer = dashDuration;
            IsDashing = true;

        }
    }

}