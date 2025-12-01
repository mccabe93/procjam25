using System;

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

public sealed class CellularAutomataConfiguration<T>
{
    public float RandomFillFrequency = 0.45f;

    public int TotalIterations = 5;

    public int BirthLimit = 5;

    public int DeathLimit = 2;

    public int Width = 10;

    public int Height = 10;

    public int Seed = 64;

    // FloorVariant Iteration
    public T FloorDefault;

    public float FillFloorVariantFrequency = 0.05f;

    public float FloorVariantFrequency = 0.25f;
    public T[] FloorVariants;

    public T LowerWallDefault;

    public float LowerWallVariantFrequency = 0.0f;
    public T[] LowerWallVariants;

    public T UpperWallDefault;

    public float UpperWallVariantFrequency = 0.10f;
    public T[] UpperWallVariants;

    // Decorations Iteration

    // Actors Iteartion
    public float ActorFrequency = 0.1f;

    public T ActorDefault;
    public T[] Actors;

    // An adversary to the room's population.
    public float AdversarialActorProbability = 0.2f;

    // Items Iteration
    public float ItemProbability = 0.10f;

    // An item may be instead a trap.
    public float TrapProbability = 0.20f;
}
