using System;
using UnityEngine;

public enum ConnectionSide
{
    Unknown = 0,
    Right = 1,
    Below = 2,
    Left = 3,
    Above = 4,
}

public sealed class Room
{
    public Room From { get; set; }
    public Room To { get; set; }
    public ConnectionSide ConnectionFromSide { get; set; }
    public int HallwaySize { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}

[Serializable]
public sealed class RoomHallConfiguration
{
    [Range(1, 10)]
    public int RoomMinimumHeight = 10;

    [Range(1, 20)]
    public int RoomMaximumHeight = 20;

    [Range(1, 10)]
    public int RoomMinimumWidth = 10;

    [Range(1, 20)]
    public int RoomMaximumWidth = 30;

    [Range(1, 20)]
    public int HallwayMinimumSize = 3;

    [Range(1, 40)]
    public int HallwayMaximumSize = 6;

    [Range(int.MinValue, int.MaxValue)]
    public int Seed = 128;

    [Range(1, 20)]
    public int RoomCount = 16;
}
