using System;
using UnityEngine;

[Serializable]
public class UnityRoomHallConfiguration
{
    [Range(1, 10)]
    public int RoomMinimumHeight = 10;

    [Range(1, 20)]
    public int RoomMaximumHeight = 20;

    [Range(1, 10)]
    public int RoomMinimumWidth = 10;

    [Range(1, 20)]
    public int RoomMaximumWidth = 30;

    // Needs more work for unity integration . . .
    [Range(0, 0)]
    public int HallwayMinimumSize = 0;

    [Range(0, 0)]
    public int HallwayMaximumSize = 0;

    [Range(short.MinValue, short.MaxValue)]
    public int Seed = 128;

    [Range(1, 20)]
    public int RoomCount = 16;

    public RoomHallConfiguration ToConfiguration()
    {
        return new RoomHallConfiguration
        {
            RoomMinimumHeight = RoomMinimumHeight,
            RoomMaximumHeight = RoomMaximumHeight,
            RoomMinimumWidth = RoomMinimumWidth,
            RoomMaximumWidth = RoomMaximumWidth,
            HallwayMinimumSize = HallwayMinimumSize,
            HallwayMaximumSize = HallwayMaximumSize,
            Seed = Seed,
            RoomCount = RoomCount,
        };
    }
}
