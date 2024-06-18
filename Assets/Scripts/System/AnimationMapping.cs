using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/AnimationMapping")]
public class AnimationMapping : ScriptableObject
{
    public RuntimeAnimatorController AnimatorController;
    public ActionMap ActionAnimationMap;
}
