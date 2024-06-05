using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    //Script References
    private ActionScript actionScript;
    private PlayerAnimator playerAnimator;
    private PlayerControlsInput playerControlsInput;

    //Fields
    public bool IsAttacking;

    void Awake()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        actionScript = GetComponent<ActionScript>();

        playerControlsInput = new PlayerControlsInput();
        playerControlsInput.Player.Enable();
    }

    private void Update()
    {
        StartAttack();
    }

    private void StartAttack()
    {
        if (playerControlsInput.Player.Down.IsPressed() && playerControlsInput.Player.HeavyAttack.WasPressedThisFrame()) HeavyAttack2H();
        else if (playerControlsInput.Player.HeavyAttack.WasPressedThisFrame()) HeavyAttack5H();
        else if (playerControlsInput.Player.Down.IsPressed() && playerControlsInput.Player.LightAttack.WasPressedThisFrame()) LightAttack2L();
        else if (playerControlsInput.Player.LightAttack.WasPressedThisFrame()) LightAttack5L();
    }

    private void LightAttack5L()
    {
        //Debug.Log("5L");
        if(actionScript.Action(Attacks.Player_5L.ToString())) IsAttacking = true;
    }

    private void LightAttack2L()
    {
        //Debug.Log("2L");
        if(actionScript.Action(Attacks.Player_2L.ToString())) IsAttacking = true;
    }

    private void HeavyAttack5H()
    {
        //Debug.Log("5H");
        if (actionScript.Action(Attacks.Player_5H.ToString())) IsAttacking = true;
    }

    private void HeavyAttack2H()
    {
        //Debug.Log("2H");
        if (actionScript.Action(Attacks.Player_2H.ToString())) IsAttacking = true;
    }

    //public void FinishAttack()
    //{
    //    //Debug.Log("Attack Finished");
    //    IsAttacking = false;
    //    actionScript.Action(Movement.Player_Idle.ToString());
    //}

}
