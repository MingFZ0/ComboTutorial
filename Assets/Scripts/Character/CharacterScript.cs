using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour, ICharacterScript
{
    private MovementScript movementScript;

    public bool GetIsGrounded() {return movementScript.IsGroundedWithJumping();}


}

public interface ICharacterScript
{
    bool GetIsGrounded();
}
