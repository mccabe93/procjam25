using System;
using UnityEngine;

// Each step is an iteration. RandomFill is iteration 0.
public enum CellularAutomataStep
{
    Unknown = 0,
    InitialState = 1,
    PopulateFloorVariants = 2,
    AddDecoration = 3,
    SpawnActors = 4,
    PlaceItems = 5,
}

[Serializable]
public sealed class VariantSettings : MonoBehaviour
{
    public GameObject Variant;

    /// <summary>
    /// If true, a fillable tile area can be filled with this variant.
    /// </summary>
    public bool CanFill;

    [Range(0.0f, 1.0f)]
    public float Frequency;
}

[Serializable]
public sealed class CellularAutomataConfiguration
{
    [Range(0f, 1f)]
    public float RandomFillFrequency = 0.45f;

    [Range(0, 100)]
    public int TotalIterations = 5;

    [Range(1, 10)]
    public int BirthLimit = 5;

    [Range(1, 10)]
    public int DeathLimit = 2;

    [Range(1, 10)]
    public int Width = 10;

    [Range(1, 10)]
    public int Height = 10;

    [Range(0, int.MaxValue)]
    public int Seed = 64;

    // FloorVariant Iteration
    [Range(0.0f, 1.0f)]
    public float FillFloorVariantFrequency = 0.05f;

    [Range(0.0f, 1.0f)]
    public float FloorVariantFrequency = 0.25f;
    public GameObject[] FloorVariants;

    [Range(0.0f, 1.0f)]
    public float LowerWallVariantFrequency = 0.0f;
    public GameObject[] LowerWallVariants;

    [Range(0.0f, 1.0f)]
    public float UpperWallVariantFrequency = 0.10f;
    public GameObject[] UpperWallVariants;

    // Decorations Iteration

    // Actors Iteartion
    public float ActorProbability = 0.1f;

    // An adversary to the room's population.
    public float AdversarialActorProbability = 0.2f;

    // Items Iteration
    public float ItemProbability = 0.10f;

    // An item may be instead a trap.
    public float TrapProbability = 0.20f;
}
