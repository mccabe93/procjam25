using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// Reference:
// https://github.com/D-three/First-Person-Movement-Script-For-Unity/blob/main/First_Person_Movement.cs
public class UnityRoomGenerator : MonoBehaviour
{
    [SerializeField]
    private CellularAutomataConfiguration CAConfig;

    [SerializeField]
    private GameObject[] FloorTiles;

    [SerializeField]
    private GameObject[] WallLowerTiles;

    [SerializeField]
    private GameObject[] WallUpperTiles;

    private List<GameObject> _floorTilesList = new List<GameObject>();
    private List<GameObject> _wallLowerTilesList = new List<GameObject>();
    private List<GameObject> _wallUpperTilesList = new List<GameObject>();

    private List<GameObject> _fillableFloorTileList = new List<GameObject>();

    private CellularAutomataLevelGenerator _generator;

    private void Start()
    {
        _fillableFloorTileList = FloorTiles
            .Where(tile => tile.GetComponent<VariantSettings>().CanFill)
            .ToList();
        _generator = new CellularAutomataLevelGenerator(CAConfig);
        _generator.RandomFill();
        for (int i = 0; i < CAConfig.TotalIterations; i++)
        {
            int[,] state = _generator.Iterate();
            switch (_generator.Step)
            {
                case CellularAutomataStep.PopulateFloorVariants:
                    break;
            }
        }
        int[,] level = _generator.Level;
        StringBuilder sb = new StringBuilder();
        for (int y = 0; y < CAConfig.Height; y++)
        {
            for (int x = 0; x < CAConfig.Width; x++)
            {
                sb.Append(level[x, y] == 1 ? '#' : '.');
                if (level[x, y] != 1)
                {
                    GameObject floorTile = Instantiate(
                        FloorTiles[Random.Range(0, FloorTiles.Length)]
                    );
                    floorTile.transform.position = new Vector3(x, 0, y);
                    _floorTilesList.Add(floorTile);
                }
                else
                {
                    GameObject wallLowerTile = Instantiate(
                        WallLowerTiles[Random.Range(0, WallLowerTiles.Length)]
                    );
                    wallLowerTile.transform.position = new Vector3(x, 0, y);
                    _wallLowerTilesList.Add(wallLowerTile);
                    GameObject wallUpperTile = Instantiate(
                        WallUpperTiles[Random.Range(0, WallUpperTiles.Length)]
                    );
                    wallUpperTile.transform.position = new Vector3(x, 1, y);
                    _wallUpperTilesList.Add(wallUpperTile);
                }
                sb.AppendLine();
            }
        }
        Debug.Log(sb.ToString());
    }

    private void AddFloorVariants(int[,] state)
    {
        for (int y = 0; y < CAConfig.Height; y++)
        {
            for (int x = 0; x < CAConfig.Width; x++)
            {
                if (state[x, y] == 1)
                {
                    GameObject floorTile = Instantiate(
                        FloorTiles[Random.Range(0, FloorTiles.Length)]
                    );
                    floorTile.transform.position = new Vector3(x, 0, y);
                    _floorTilesList.Add(floorTile);
                }
                else if (state[x, y] == 2) // Fill tile variant
                {
                    GameObject floorTile = Instantiate(
                        _fillableFloorTileList[Random.Range(0, _fillableFloorTileList.Count)]
                    );
                    floorTile.transform.position = new Vector3(x, 0, y);
                    _floorTilesList.Add(floorTile);
                }
                /*
                    GameObject wallLowerTile = Instantiate(
                        WallLowerTiles[Random.Range(0, WallLowerTiles.Length)]
                    );
                    wallLowerTile.transform.position = new Vector3(x, 0, y);
                    WallLowerTilesList.Add(wallLowerTile);
                    GameObject wallUpperTile = Instantiate(
                        WallUpperTiles[Random.Range(0, WallUpperTiles.Length)]
                    );
                    wallUpperTile.transform.position = new Vector3(x, 1, y);
                    WallUpperTilesList.Add(wallUpperTile);
                }
                */
            }
        }
    }
}
