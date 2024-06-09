using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    //Script References
    private ActionScript actionScript;
    [SerializeField] private MovesetMap movesetMap;
    private Dictionary<int, MovesetPriorityLevel> movesetPriorityMap;
    private PlayerControlsInput playerControlsInput;

    //Fields
    public bool IsAttacking;
    public Boolean IsCancelable;
    public Boolean HitBoxCollided { get; private set; }

    void Awake()
    {
        actionScript = GetComponent<ActionScript>();

        playerControlsInput = new PlayerControlsInput();
        playerControlsInput.Player.Enable();

        movesetPriorityMap = movesetMap.MovesetPriorityMap;

    }

    private void Update()
    {
        for (int i = 2; i < movesetPriorityMap.Keys.Count; i++)
        {
            MovesetPriorityLevel movesetPriorityLevel = movesetPriorityMap[i];
            InputActionReference movesetLevelInput = movesetPriorityLevel.LevelInput;

            if (movesetLevelInput.action.WasPressedThisFrame())
            {
                foreach (Move move in movesetPriorityLevel.Moves)
                {
                    Debug.Log(i + " level was pressed");
                    if (move.DirectionalInput.action.IsPressed()) 
                    {
                        Debug.Log(move + " should be ran pressed");
                        IsAttacking = true;
                        actionScript.Action(move.MoveName);
                        return;
                    }
                }
            }
        }
    }

    public void OnHitBoxCollide() { HitBoxCollided = true; }
    public void OnHitBoxExit() {HitBoxCollided=false; }
}
