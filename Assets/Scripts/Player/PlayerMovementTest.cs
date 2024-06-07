using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player
{
    public class PlayerMovementTest : MonoBehaviour
    {
        private PlayerControlsInput input;
        private Rigidbody2D rigidbody;

        private bool dashing;
        private Vector2 movement;


        private void Awake()
        {
            input = GetComponent<PlayerControlsInput>();
            rigidbody = GetComponent<Rigidbody2D>();
            movement = new Vector2(0, 0);
        }

        private void Update()
        {
            if (!dashing)
            {
                movement.x = input.Player.Move.ReadValue<Vector2>().x;
            }
        }
    }
}
