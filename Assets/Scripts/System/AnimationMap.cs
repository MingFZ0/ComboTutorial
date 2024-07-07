using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/AnimationMap")]
public class AnimationMap : ScriptableObject
{
    public RuntimeAnimatorController AnimatorController;
    public ActionAnimationMap ActionAnimationMap = new();
    public StateAnimationMap StateAnimationMap = new();

    private void OnValidate()
    {
        ActionAnimationMap.UpdatePriorityMap();
    }
}


