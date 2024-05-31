using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    //Script References
    private PlayerAnimator playerAnimator;
    private PlayerControlsInput playerControlsInput;

    //Fields
    public bool IsAttacking;

    void Awake()
    {
        playerAnimator = GetComponent<PlayerAnimator>();

        playerControlsInput = new PlayerControlsInput();
        playerControlsInput.Player.Enable();
        playerControlsInput.Player.LightAttack.performed += StartAttack;
    }

    private void StartAttack(InputAction.CallbackContext context)
    {
        IsAttacking = true;
        if (playerControlsInput.Player.Down.IsPressed()) LightAttack2L();
        else LightAttack5L();
    }

    private void LightAttack5L()
    {
        Debug.Log("5L");
        playerAnimator.Attacking(Attacks.Player_5L);
    }

    private void LightAttack2L()
    {
        Debug.Log("2L");
        playerAnimator.Attacking(Attacks.Player_2L);
    }

    public void FinishAttack()
    {
        IsAttacking = false;
        playerAnimator.Moving(Movement.Player_Idle.ToString());
    }

}
