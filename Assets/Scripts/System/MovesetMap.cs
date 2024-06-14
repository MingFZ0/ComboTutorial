using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using System.Linq;

[CreateAssetMenu(menuName = "Characters/MovesetMap")]
public class MovesetMap:ScriptableObject
{
    public RuntimeAnimatorController controller;

    [SerializeField] private MovesetPriorityLevel Movement = new(0);
    [SerializeField] private MovesetPriorityLevel Dash = new(1);
    [SerializeField] private MovesetPriorityLevel Light = new(2);
    [SerializeField] private MovesetPriorityLevel Medium = new(3);
    [SerializeField] private MovesetPriorityLevel Heavy = new(4);
    [SerializeField] private MovesetPriorityLevel Unique = new(5);

    //public readonly Dictionary<string, Move> AllMovesByName = new Dictionary<string, Move>();
    public Dictionary<int, MovesetPriorityLevel> MovesetPriorityMap;
    public List<string> AllMappedMoves { get;  private set; } = new();
    public readonly List<Move> AllAttack = new();
    public readonly List<Move> AllMovement = new();
    public readonly List<Move> AllDash = new();

    private void OnValidate()
    {
        


        MovesetPriorityMap = ToDictionary();
        AllMappedMoves.Clear();
        AllAttack.Clear();
        AllMovement.Clear();
        AllDash.Clear();

        AllMappedMoves = VerifyMappedMoveSet();

        MovesetPriorityLevel[] attacks = {Light, Medium, Heavy, Unique };
        foreach (MovesetPriorityLevel attack in attacks) { AllAttack.AddRange(attack.Moves); }
        foreach (Move move in Movement.Moves) { AllMovement.Add(move); }
        foreach (Move move in Dash.Moves) { AllDash.Add(move); }

        Debug.Log(this.name + " Scriptable Object Was Valided..");
        //foreach (string name in AllMappedMoves) {Debug.Log(name); }
    }

    public Dictionary<int, MovesetPriorityLevel> ToDictionary()
    {
        Dictionary<int, MovesetPriorityLevel> dict = new();
        MovesetPriorityLevel[] moveSet = {Movement, Dash, Light, Medium, Heavy, Unique };

        //if (moveSet == null) { return null; }
        foreach (MovesetPriorityLevel moveSetLevel in moveSet)
        {
            //Debug.Log(moveSetLevel);
            dict.Add(moveSetLevel.PriorityLevel, moveSetLevel);
        }
        return dict;
    }

    
  

    private List<string> VerifyMappedMoveSet()
    {
        AnimationClip[] clips = controller.animationClips;
        List<string> allClips = new();
        foreach (AnimationClip clip in clips)
        {
            allClips.Add(clip.name);
        }

        List<MovesetPriorityLevel> mappedMoveLevels = MovesetPriorityMap.Values.ToList();
        List<string> allMappedMoves = new();
        foreach (MovesetPriorityLevel currentLevel in mappedMoveLevels) 
        {
            List<Move> movesInCurrentLevel = currentLevel.Moves;
            foreach (Move move in movesInCurrentLevel)
            {
                allMappedMoves.Add(move.MoveName);
            }
        }

        foreach (int priorityLevel in MovesetPriorityMap.Keys)
        {
            MovesetPriorityLevel currentLevel = MovesetPriorityMap[priorityLevel];
            List<Move> movesInCurrentLevel = currentLevel.Moves;
            foreach (Move move in movesInCurrentLevel)
            {
                string currentMoveName = move.MoveName;
                if (allClips.Contains(currentMoveName) == false) throw new UndefinedMovesetException(this.name + ": " + move + " In Move Priority " + priorityLevel + " Cannot be found in the provided animation controller, did you misspelled something?");
            }
        }

        //Debug.Log(allClips);
        foreach (string move in allClips)
        {
            if (allMappedMoves.Contains(move) == false) { Debug.LogWarning(move + " move is unmapped and will not be run."); }
        }

        //Debug.Log(allMappedMoves.Count);

        return allMappedMoves;

    }
}

[Serializable]
public class MovesetPriorityLevel
{
     public MovesetPriorityLevel(int level) 
    { 
        this.PriorityLevel = level; 
    }

    [SerializeField] public InputActionReference LevelInput;
    [HideInInspector] public int PriorityLevel { get; private set; }
    [SerializeField] public List<Move> Moves;

    public override string ToString()
    {
        return PriorityLevel.ToString() + ": " + string.Join(",", Moves);
    }

    public bool Contains(string moveName)
    {
        foreach (Move move in Moves)
        {
            if (move.MoveName == moveName) return true;
        }
        return false;
    }
}




[Serializable]
public class Move
{
    [SerializeField] public InputActionReference DirectionalInput;
    [SerializeField] public string MoveName;
    [SerializeField] public Boolean Grounded;

    public override bool Equals(object obj)
    {
        if ((Move)obj == this) return true;
        if (obj.ToString() == MoveName) return true;
        return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return MoveName;
    }
}

public class UndefinedMovesetException : Exception
{
    public UndefinedMovesetException(string message) : base(message) { }
    public UndefinedMovesetException() { }
}




