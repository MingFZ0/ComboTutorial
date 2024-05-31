using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A representation of gameEvents (with 1 parameter) in Scriptable Objects 
/// Constain 3 seperate channels for events: string, int, float
/// </summary>
[CreateAssetMenu(menuName = "RuntimeSet/GameEvent<T>")]
public class GameEventP1SO : ScriptableObject
{
    /// <summary>
    /// String Channel: UnityEvent with String Parameter
    /// </summary>
    private UnityP1Event<String> stringEvent;

    /// <summary>
    /// Int Channel: UnityEvent with int Parameter
    /// </summary>
    private UnityP1Event<int> intEvent;

    /// <summary>
    /// Float Channel: UnityEvent with float Parameter
    /// </summary>
    private UnityP1Event<float> floatEvent;

    /// <summary>
    /// Adds an observer to the string channel of the event
    /// </summary>
    /// <param name="del"></param>
    public void AddObserver(UnityAction<string> del)
    {
        stringEvent.AddListener((UnityAction<string>) del);
    }

    /// <summary>
    /// Adds an observer to the int channel of the event
    /// </summary>
    /// <param name="del"></param>
    public void AddObserver(UnityAction<int> del)
    {
        intEvent.AddListener(del);
    }

    /// <summary>
    /// Adds an observer to the float channel of the event
    /// </summary>
    /// <param name="del"></param>
    public void AddObserver(UnityAction<float> del)
    {
        floatEvent.AddListener(del);
    }

    /// <summary>
    /// Invoke the specific channel of the event based on the type of the parameter passed, and
    /// notify all observers within that channel of the event
    /// </summary>
    /// <param name="param"></param>
    public void InvokeEvent(System.Object param)
    {
        if (param.GetType() == typeof(String)) { stringEvent.Invoke((string)param); }
        if (param.GetType() == typeof(int)) { intEvent.Invoke((int)param); }
        if (param.GetType() == typeof(float)) { floatEvent.Invoke((float)param); }
    }
}

[System.Serializable]
public class UnityP1Event<T> : UnityEvent<T>
{
}
