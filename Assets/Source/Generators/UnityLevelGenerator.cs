using UnityEngine;

public sealed class UnityLevelGenerator : MonoBehaviour
{
    // This will move around the level space and generate the rooms.
    public GameObject RoomGenerator;
    public GameObject Player;

    [SerializeField]
    public UnityRoomHallConfiguration RoomHallConfig;
    public bool DebugMode = false;

    public void Start()
    {
        if (DebugMode)
        {
            GenerateLevel();
        }
    }

    public void GenerateLevel()
    {
        UnityRoomGenerator roomGenerator = RoomGenerator.GetComponent<UnityRoomGenerator>();
        if (RoomHallConfig.Seed == 0)
        {
            RoomHallConfig.Seed = Random.Range(int.MinValue, int.MaxValue);
            roomGenerator.CAConfig.Seed = RoomHallConfig.Seed;
        }
        GameObject defaultTile = roomGenerator.CAConfig.FloorDefault;
        RoomHallGenerator layoutGenerator = new RoomHallGenerator(RoomHallConfig.ToConfiguration());
        layoutGenerator.Generate();
        foreach (var room in layoutGenerator.Rooms)
        {
            roomGenerator.CAConfig.Width = room.Width;
            roomGenerator.CAConfig.Height = room.Height;
            Debug.Log($"Room at ({room.X}, {room.Y}) size ({room.Width}x{room.Height})");
            roomGenerator.GenerateRoom(new Vector3(room.X, 0, room.Y));
        }

        if (layoutGenerator.Rooms.Count > 0)
        {
            Room firstRoom = layoutGenerator.Rooms[0];
            float playerX =
                (firstRoom.X * roomGenerator.FloorBounds.size.x)
                + (roomGenerator.FloorBounds.size.x * firstRoom.Width / 2f);
            float playerZ =
                (firstRoom.Y * roomGenerator.FloorBounds.size.z)
                + (roomGenerator.FloorBounds.size.z * firstRoom.Height / 2f);
            Player.transform.position = new Vector3(playerX, 0, playerZ);
            Player = Instantiate(Player);
        }
    }
}
