using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    //Script References
    private PlayerScript playerScript;
    private PlayerAnimator playerAnimator;
    private PlayerControlsInput playerControlsInput;

    //Fields
    public bool IsAttacking;

    void Awake()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        playerScript = GetComponent<PlayerScript>();

        playerControlsInput = new PlayerControlsInput();
        playerControlsInput.Player.Enable();
        playerControlsInput.Player.LightAttack.performed += StartAttack;
    }

    private void StartAttack(InputAction.CallbackContext context)
    {
        if (playerControlsInput.Player.Down.IsPressed()) LightAttack2L();
        else LightAttack5L();
    }

    private void LightAttack5L()
    {
        Debug.Log("5L");
        if(playerScript.Action(Attacks.Player_5L.ToString())) IsAttacking = true;
    }

    private void LightAttack2L()
    {
        Debug.Log("2L");
        if(playerScript.Action(Attacks.Player_2L.ToString())) IsAttacking = true;
    }

    public void FinishAttack()
    {
        Debug.Log("Attack Finished");
        IsAttacking = false;
        playerScript.Action(Movement.Player_Idle.ToString());
    }

}
