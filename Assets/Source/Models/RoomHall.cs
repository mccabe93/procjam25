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
    public int HallwayX { get; set; }
    public int HallwayY { get; set; }
    public int HallwaySize { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}

public sealed class RoomHallConfiguration
{
    public int RoomMinimumHeight = 10;

    public int RoomMaximumHeight = 20;
    public int RoomMinimumWidth = 10;

    public int RoomMaximumWidth = 30;

    // Needs more work for unity integration . . .
    public int HallwayMinimumSize = 0;

    public int HallwayMaximumSize = 0;

    public int Seed = 128;

    public int RoomCount = 16;
}
