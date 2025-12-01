using System;
using UnityEngine;

[Serializable]
public class UnityCellularAutomataSettings
{
    [Range(0f, 1f)]
    public float RandomFillFrequency = 0.45f;

    [Range(0, 100)]
    public int TotalIterations = 5;

    [Range(0, 100)]
    public int BirthLimit = 5;

    [Range(0, 100)]
    public int DeathLimit = 2;

    [Range(0, 100)]
    public int Width = 10;

    [Range(0, 100)]
    public int Height = 10;

    [Range(0, 100)]
    public int Seed = 64;

    // FloorVariant Iteration
    public GameObject FloorDefault;

    [Range(0f, 1f)]
    public float FillFloorVariantFrequency = 0.05f;

    [Range(0f, 1f)]
    public float FloorVariantFrequency = 0.25f;
    public GameObject[] FloorVariants;

    public GameObject LowerWallDefault;

    [Range(0f, 1f)]
    public float LowerWallVariantFrequency = 0.0f;
    public GameObject[] LowerWallVariants;

    public GameObject UpperWallDefault;

    [Range(0f, 1f)]
    public float UpperWallVariantFrequency = 0.10f;
    public GameObject[] UpperWallVariants;

    // Decorations Iteration

    // Actors Iteartion
    public float ActorFrequency = 0.1f;

    public GameObject ActorDefault;
    public GameObject[] Actors;

    public CellularAutomataConfiguration<GameObject> ToConfiguration()
    {
        return new CellularAutomataConfiguration<GameObject>
        {
            RandomFillFrequency = RandomFillFrequency,
            TotalIterations = TotalIterations,
            BirthLimit = BirthLimit,
            DeathLimit = DeathLimit,
            Width = Width,
            Height = Height,
            Seed = Seed,
            FloorDefault = FloorDefault,
            FillFloorVariantFrequency = FillFloorVariantFrequency,
            FloorVariantFrequency = FloorVariantFrequency,
            FloorVariants = FloorVariants,
            LowerWallDefault = LowerWallDefault,
            LowerWallVariantFrequency = LowerWallVariantFrequency,
            LowerWallVariants = LowerWallVariants,
            UpperWallDefault = UpperWallDefault,
            UpperWallVariantFrequency = UpperWallVariantFrequency,
            UpperWallVariants = UpperWallVariants,
            ActorFrequency = ActorFrequency,
            ActorDefault = ActorDefault,
            Actors = Actors,
        };
    }
}
