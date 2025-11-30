using UnityEngine;

public sealed class UnityLevelGenerator : MonoBehaviour
{
    public RoomHallConfiguration RoomHallConfig;

    public void GenerateLevel()
    {
        RoomHallGenerator generator = new RoomHallGenerator(RoomHallConfig);
        generator.Generate();
        foreach (var room in generator.Rooms)
        {
            Debug.Log($"Room at ({room.X}, {room.Y}) size ({room.Width}x{room.Height})");
            CellularAutomataLevelGenerator caGenerator = new CellularAutomataLevelGenerator(
                new CellularAutomataConfiguration
                {
                    Width = room.Width,
                    Height = room.Height,
                    Seed = RoomHallConfig.Seed + room.X + room.Y,
                    RandomFillFrequency = 0.45f,
                    TotalIterations = 5,
                    BirthLimit = 5,
                    DeathLimit = 2,
                }
            );
        }
    }
}
