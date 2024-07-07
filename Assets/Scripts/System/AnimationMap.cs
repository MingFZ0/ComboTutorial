using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/AnimationMap")]
public class AnimationMap : ScriptableObject
{
    public RuntimeAnimatorController AnimatorController;
    public ActionAnimationMap ActionAnimationMap;
    public StateAnimationMap StateAnimationMap;
}


