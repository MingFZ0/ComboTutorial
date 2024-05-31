using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A representation of gameEvents (with no parameters) in Scriptable Objects 
/// </summary>
[CreateAssetMenu(menuName = "RuntimeSet/GameEvent")]
public class GameEventSO : ScriptableObject     
{   

    private UnityEvent Event;
    public readonly List<String> Observers;

    public void AddObserver(UnityAction del)
    {
        //Observers.Add(observerName);
        Event.AddListener(del);
    }

    public void InvokeEvent()
    {
        Event?.Invoke();
    }
}
