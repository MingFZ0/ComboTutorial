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
        private Rigidbody2D rb2d;
        private PlayerControlsInput playerControlsInput;
        private bool dashHeld;

        private ActionScript actionScript;
        private PlayerAttack playerAttack;

        [SerializeField] private MovesetMap movesetMap;
        private Dictionary<int, MovesetPriorityLevel> movesetPriorityMap;

        //Attributes

        public bool IsDashing;
        private Boolean IsDash;

        // jumpuing variables
        private bool airborne;
        private bool jumpHeld;
        private float jumpForce;
        private float currentJumpDisplacement;
        private Vector2 jumpVector;
        private Vector2 dashVector;
        private Vector2 movementVector;

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
            jumpVector = new Vector2(0, jumpHeight);
            dashVector = new Vector2(dashSpeed, 0);
            movesetPriorityMap = movesetMap.MovesetPriorityMap;

        }

        private void Update()
        {

            for (int i = 0; i <= 0; i++)
            {
                MovesetPriorityLevel movesetPriorityLevel = movesetPriorityMap[i];
                InputActionReference movesetLevelInput = movesetPriorityLevel.LevelInput;

                if (movesetLevelInput.action.WasPressedThisFrame())
                {
                    foreach (Move move in movesetPriorityLevel.Moves)
                    {
                        if (move.DirectionalInput.action.IsPressed())
                        {
                            if (actionScript.Action(move.MoveName))
                            {
                                switch (move.DirectionalInput.action.name)
                                {
                                    case "Left":
                                        rb2d.velocity = new Vector2(-1, 0);
                                        break;
                                    case "Right":
                                        rb2d.velocity = new Vector2(1, 0);
                                        break;
                                }
                            }
                            return;
                        }
                    }
                }
            }
        }


        private void Moving()
        {
            Vector2 movement = playerControlsInput.Player.Move.ReadValue<Vector2>();
            movement.y = 0;
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

            // TODO: change later
            rb2d.velocity = new Vector2(movement.x * speed, rb2d.velocity.y);
        }

        /// <summary>
        /// Storing Buffer for Dash && Start BufferTimer if the player moves
        /// </summary>
        /// <returns></returns>
        private void StoreDashBuffer()
        {
            Vector2 movement = playerControlsInput.Player.Move.ReadValue<Vector2>();
            movement.y = 0;
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

    }
}