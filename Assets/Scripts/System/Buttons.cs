using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Attacks
{
    Player_5L,
    Player_2L,
    Player_4L,
    Player_5M,
    Player_2M,
    Player_5H,
    Player_2H,
    Player_5S,
}

public enum Lights
{
    Player_5L,
    Player_2L,
    Player_4L,
}

public enum Mediums
{
    Player_5M,
    Player_2M,
}

public enum Heavies
{
    Player_5H,
    Player_2H,
}

public enum Uniques
{
    Player_5S,
}

public enum Movement
{
    Player_WalkForward,
    Player_WalkBackward,
    Player_Jump,
    Player_Crouch,
    Player_Idle,
}

public enum Dash
{
    Player_DashForward,
    Player_DashBackward,
}