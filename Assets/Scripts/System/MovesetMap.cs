using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.Animations;
using UnityEngine.InputSystem;
using System.Linq;

[CreateAssetMenu(menuName = "Characters/MovesetMap")]
public class MovesetMap: ScriptableObject
{
    public AnimatorController controller;

    [SerializeField] private MoveSetLevel Movement = new(0);
    [SerializeField] private MoveSetLevel Dash = new(1);
    [SerializeField] private MoveSetLevel Light = new(2);
    [SerializeField] private MoveSetLevel Medium = new(3);
    [SerializeField] private MoveSetLevel HeavyAndUnique = new(4);
    public Dictionary<int, List<Move>> MovesetPriorityMap;

    //public readonly Dictionary<string, Move> AllMovesByName = new Dictionary<string, Move>();
    public List<string> AllMappedMoves { get;  private set; } = new();
    public readonly List<Move> AllAttack = new();
    public readonly List<Move> AllMovement = new();
    public readonly List<Move> AllDash = new();

    private void OnValidate()
    {
        Debug.Log("Validating..");


        MovesetPriorityMap = ToDictionary();
        AllMappedMoves.Clear();
        AllAttack.Clear();
        AllMovement.Clear();
        AllDash.Clear();

        AllMappedMoves = VerifyMappedMoveSet();

        MoveSetLevel[] attacks = { Light, Medium, HeavyAndUnique };
        foreach (MoveSetLevel attack in attacks) { AllAttack.AddRange(attack.Moves); }
        foreach (Move move in Movement.Moves) { AllMovement.Add(move); }
        foreach (Move move in Dash.Moves) { AllDash.Add(move); }
        
        //foreach (string name in AllMappedMoves) {Debug.Log(name); }
    }

    public Dictionary<int, List<Move>> ToDictionary()
    {
        Dictionary<int, List<Move>> dict = new();
        MoveSetLevel[] moveSet = { Movement, Dash, Light, Medium };

        if (moveSet == null) { return null; }
        foreach (MoveSetLevel moveSetLevel in moveSet)
        {
            dict.Add(moveSetLevel.PriorityLevel, moveSetLevel.Moves);
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

        List<List<Move>> mappedMoveLevels = MovesetPriorityMap.Values.ToList();
        List<string> allMappedMoves = new();
        foreach (List<Move> movesInCurrentLevel in mappedMoveLevels) 
        { 
            foreach (Move move in movesInCurrentLevel)
            {
                allMappedMoves.Add(move.MoveName);
            }
        }

        foreach (int priorityLevel in MovesetPriorityMap.Keys)
        {
            List<Move> movesInCurrentLevel = MovesetPriorityMap[priorityLevel];
            foreach (Move move in movesInCurrentLevel)
            {
                string currentMoveName = move.MoveName;
                if (allClips.Contains(currentMoveName) == false) throw new MissingReferenceException(move + " In Move Priority " + priorityLevel + " Cannot be found in the provided animation controller, did you misspelled something?");
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
public class MoveSetLevel
{
     public MoveSetLevel(int level) 
    { 
        this.PriorityLevel = level; }

    [HideInInspector] public int PriorityLevel { get; private set; }
    [SerializeField] public List<Move> Moves;
}

[Serializable]
public class Move
{
    [SerializeField] public InputActionReference DirectionalInput;
    [SerializeField] public string MoveName;

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


