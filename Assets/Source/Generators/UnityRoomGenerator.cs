using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Reference:
// https://github.com/D-three/First-Person-Movement-Script-For-Unity/blob/main/First_Person_Movement.cs
public class UnityRoomGenerator : MonoBehaviour
{
    [SerializeField]
    public UnityCellularAutomataSettings CAConfig;
    public bool DebugMode = false;

    private List<GameObject> _floorTilesList = new List<GameObject>();
    private List<GameObject> _wallLowerTilesList = new List<GameObject>();
    private List<GameObject> _wallUpperTilesList = new List<GameObject>();

    private List<GameObject> _fillableFloorTileList = new List<GameObject>();

    private CellularAutomataLevelGenerator<GameObject> _generator;

    // We assume uniformity
    private Bounds _floorBounds;
    private Bounds _wallBounds;

    public Bounds FloorBounds => _floorBounds;
    public Bounds WallBounds => _wallBounds;

    private void Awake()
    {
        _fillableFloorTileList = CAConfig
            .FloorVariants.Where(t => t.GetComponent<UnityVariantSettings>().CanFill)
            .ToList();
        _floorBounds = CAConfig.FloorDefault.GetComponent<SpriteRenderer>().bounds;
        _wallBounds = CAConfig.LowerWallDefault.GetComponent<SpriteRenderer>().bounds;
        if (DebugMode)
        {
            GenerateRoom(Vector3.zero);
        }
    }

    public void GenerateRoom(Vector3 position)
    {
        _generator = new CellularAutomataLevelGenerator<GameObject>(CAConfig.ToConfiguration());
        _generator.RandomFill();
        for (int i = 0; i < CAConfig.TotalIterations; i++)
        {
            int[,] state = _generator.Iterate();
            switch (_generator.Step)
            {
                case CellularAutomataStep.PopulateFloorVariants:
                    AddFloorVariants(state, position);
                    break;
                case CellularAutomataStep.SpawnActors:
                    AddActors(state, position);
                    break;
            }
        }
        AddWalls(position);
        AddFloorCollider(position);
    }

    private void AddFloorVariants(int[,] state, Vector3 position)
    {
        Transform t = GetComponent<Transform>();
        for (int y = 0; y < CAConfig.Height; y++)
        {
            for (int x = 0; x < CAConfig.Width; x++)
            {
                GameObject tile;
                if (state[x, y] == 1)
                {
                    tile = Instantiate(
                        CAConfig.FloorVariants[Random.Range(0, CAConfig.FloorVariants.Length)]
                    );
                }
                else if (state[x, y] == 2) // Fill tile variant
                {
                    tile = Instantiate(
                        _fillableFloorTileList[Random.Range(0, _fillableFloorTileList.Count)]
                    );
                }
                else
                {
                    tile = Instantiate(CAConfig.FloorDefault);
                }
                tile.transform.position = new Vector3(
                    (position.x + x) * _floorBounds.size.x,
                    0,
                    (position.z + y) * _floorBounds.size.z
                );
                _floorTilesList.Add(tile);
            }
        }
    }

    private void AddActors(int[,] state, Vector3 position)
    {
        Transform t = GetComponent<Transform>();
        for (int y = 0; y < CAConfig.Height; y++)
        {
            for (int x = 0; x < CAConfig.Width; x++)
            {
                if (state[x, y] == 2) // Actor spawn
                {
                    GameObject actor = Instantiate(
                        CAConfig.ActorDefault,
                        new Vector3(
                            (position.x + x) * _floorBounds.size.x,
                            0,
                            (position.z + y) * _floorBounds.size.z
                        ),
                        Quaternion.identity
                    );
                }
            }
        }
    }

    private void AddWalls(Vector3 position)
    {
        Quaternion leftWallRotation = Quaternion.Euler(0, 270, 0);
        Quaternion rightWallRotation = Quaternion.Euler(0, -90, 0);
        Quaternion belowWallRotation = Quaternion.Euler(0, 0, 0);
        Quaternion aboveWallRotation = Quaternion.Euler(0, 0, 0);
        Transform t = GetComponent<Transform>();
        for (int y = 0; y < CAConfig.Height; y++)
        {
            CreateWallAtPosition(
                new Vector3(
                    position.x * _floorBounds.size.x,
                    0,
                    (position.z + y) * _floorBounds.size.z
                ),
                leftWallRotation
            );
            CreateWallAtPosition(
                new Vector3(
                    (position.x + CAConfig.Width) * _floorBounds.size.x,
                    0,
                    (position.z + y) * _floorBounds.size.z
                ),
                rightWallRotation
            );
        }
        for (int x = 0; x < CAConfig.Width; x++)
        {
            CreateWallAtPosition(
                new Vector3(
                    (position.x + x) * _floorBounds.size.x,
                    0,
                    position.z * _floorBounds.size.z
                ),
                belowWallRotation
            );
            CreateWallAtPosition(
                new Vector3(
                    (position.x + x) * _floorBounds.size.x,
                    0,
                    (position.z + CAConfig.Height) * _floorBounds.size.z
                ),
                aboveWallRotation
            );
        }
    }

    private void AddFloorCollider(Vector3 position)
    {
        float width = CAConfig.Width * _floorBounds.size.x;
        float height = CAConfig.Height * _floorBounds.size.z;
        GameObject colliderObj = new GameObject($"RoomFloorCollider-{System.Guid.NewGuid()}");
        colliderObj.transform.parent = this.transform;
        colliderObj.transform.position = new Vector3(
            (position.x + CAConfig.Width / 2f) * _floorBounds.size.x,
            0f,
            (position.z + CAConfig.Height / 2f) * _floorBounds.size.z
        );

        BoxCollider collider = colliderObj.AddComponent<BoxCollider>();
        collider.size = new Vector3(width, 0.01f, height);
        collider.center = Vector3.zero;
    }

    private void CreateWallAtPosition(Vector3 position, Quaternion rotation)
    {
        GameObject lowerWall = Instantiate(CAConfig.LowerWallDefault);
        lowerWall.transform.position = position;
        lowerWall.transform.rotation = rotation;
        _wallLowerTilesList.Add(lowerWall);
        GameObject upperWall = Instantiate(CAConfig.UpperWallDefault);
        position.y += _wallBounds.size.y;
        upperWall.transform.position = position;
        upperWall.transform.rotation = rotation;
        _wallUpperTilesList.Add(upperWall);
    }
}
