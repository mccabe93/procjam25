using System;
using System.Text;

/// <summary>
/// We use the CA to populate rooms.
/// Reference: https://www.roguebasin.com/index.php?title=Cellular_Automata_Method_for_Generating_Random_Cave-Like_Levels
/// </summary>
public class CellularAutomataLevelGenerator
{
    private readonly CellularAutomataConfiguration _configuration;
    private readonly int[,] _automataLevel;
    private CellularAutomataStep _iterations = 0;
    public CellularAutomataStep Step => _iterations;

    public readonly int[,] Level;

    // Chosen over Unity for unique seeds per generator.
    private readonly System.Random _rng;

    public CellularAutomataLevelGenerator(CellularAutomataConfiguration configuration)
    {
        _configuration = configuration;
        _automataLevel = new int[_configuration.Width, _configuration.Height];
        _rng = new System.Random(configuration.Seed);
        Level = new int[_configuration.Width, _configuration.Height];
    }

    public int[,] RandomFill()
    {
        for (int x = 0; x < _configuration.Width; x++)
        {
            for (int y = 0; y < _configuration.Height; y++)
            {
                if (x == 0 || x == _configuration.Width - 1)
                {
                    _automataLevel[x, y] = 1; // Wall
                }
                else if (y == 0 || y == _configuration.Height - 1)
                {
                    _automataLevel[x, y] = 1; // Wall
                }
                else if (_rng.NextDouble() < _configuration.RandomFillFrequency)
                {
                    _automataLevel[x, y] = 1;
                }
                else
                {
                    _automataLevel[x, y] = 0; // Floor
                }
            }
        }
        return UpdateLevel();
    }

    public int[,] Iterate()
    {
        // Copy the state and edit the actual level based on the last (copy) state.
        int[,] state = new int[_configuration.Width, _configuration.Height];
        Array.Copy(_automataLevel, state, _automataLevel.Length);
        for (int x = 1; x < _configuration.Width - 1; x++)
        {
            for (int y = 1; y < _configuration.Height - 1; y++)
            {
                if (ShouldPlaceWall(state, x, y))
                {
                    _automataLevel[x, y] = 1;
                }
                else
                {
                    _automataLevel[x, y] = 0;
                }
            }
        }
        _iterations++;
        return UpdateLevel();
    }

    public int[,] UpdateLevel()
    {
        int[,] state = new int[_configuration.Width, _configuration.Height];
        Array.Copy(_automataLevel, state, _automataLevel.Length);
        switch (_iterations)
        {
            case CellularAutomataStep.PopulateFloorVariants:
                PopulateFloorVariants(state);
                break;

            case CellularAutomataStep.AddDecoration:
                break;

            case CellularAutomataStep.SpawnActors:
                break;

            case CellularAutomataStep.PlaceItems:
                break;
        }
        return state;
    }

    public int[,] GetAutomataState()
    {
        int[,] copy = new int[_automataLevel.GetLength(0), _automataLevel.GetLength(1)];
        return copy;
    }

    private void PopulateFloorVariants(int[,] state)
    {
        for (int x = 1; x < _configuration.Width - 1; x++)
        {
            for (int y = 1; y < _configuration.Height - 1; y++)
            {
                if (state[x, y] == 1)
                {
                    double roll = _rng.NextDouble();
                    if (roll < _configuration.FillFloorVariantFrequency)
                    {
                        int[,] connected = new int[_configuration.Width, _configuration.Height];
                        GetConnectedCells(x, y, 1, ref connected);
                        for (int i = 0; i < connected.GetLength(0); i++)
                        {
                            for (int j = 0; j < connected.GetLength(1); j++)
                            {
                                if (connected[i, j] == 1)
                                {
                                    state[i, j] = 2;
                                    Level[i, j] = 1;
                                }
                            }
                        }
                    }
                    else if (roll < _configuration.FloorVariantFrequency)
                    {
                        state[x, y] = 1;
                        Level[x, y] = 1;
                    }
                }
            }
        }
    }

    private void GetConnectedCells(int x, int y, int cellType, ref int[,] state)
    {
        // No corners. Check them separately
        // nx0 x nx1
        // ny0
        // y
        // ny1
        for (int nx = x - 1; nx <= x + 1; nx++)
        {
            if (nx == x || nx <= 0 || nx >= _configuration.Width - 1 || state[nx, y] == 1)
                continue;
            if (_automataLevel[nx, y] == cellType)
            {
                state[nx, y] = 1;
                GetConnectedCells(nx, y, cellType, ref state);
            }
        }

        for (int ny = y - 1; ny <= y + 1; ny++)
        {
            if (ny == y || ny <= 0 || ny >= _configuration.Height - 1 || state[x, ny] == 1)
                continue;
            if (_automataLevel[x, ny] == cellType)
            {
                state[x, ny] = 1;
                GetConnectedCells(x, ny, cellType, ref state);
            }
        }
    }

    public bool ShouldPlaceWall(int[,] state, int x, int y) =>
        CountAdjacentWalls(state, x, y) >= _configuration.BirthLimit
        || CountNearbyWalls(state, x, y) <= _configuration.DeathLimit;

    public int CountNearbyWalls(int[,] state, int x, int y)
    {
        int walls = 0;

        for (int mapX = x - 2; mapX < _configuration.Width; mapX++)
        {
            for (int mapY = y - 2; mapY < _configuration.Height; mapY++)
            {
                if ((Math.Abs(mapX - x) == 2 && Math.Abs(mapY - y) == 2))
                {
                    continue;
                }

                if (
                    mapX < 0
                    || mapY < 0
                    || mapX >= _configuration.Width
                    || mapY >= _configuration.Height
                )
                {
                    walls++;
                    continue;
                }

                if (state[mapX, mapY] == 1)
                {
                    walls++;
                }
            }
        }
        return walls;
    }

    public int CountAdjacentWalls(int[,] state, int x, int y)
    {
        int count = 0;
        for (int nx = x - 1; nx <= x + 1; nx++)
        {
            for (int ny = y - 1; ny <= y + 1; ny++)
            {
                if (nx == x && ny == y)
                    continue;
                if (state[nx, ny] == 1)
                {
                    count++;
                }
            }
        }
        return count;
    }

    public string AutomataLevelToString()
    {
        StringBuilder sb = new StringBuilder();
        for (int y = 0; y < _configuration.Height; y++)
        {
            for (int x = 0; x < _configuration.Width; x++)
            {
                switch (_automataLevel[x, y])
                {
                    case 0:
                        sb.Append('.');
                        break;
                    case 1:
                        sb.Append('#');
                        break;
                }
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        for (int y = 0; y < _configuration.Height; y++)
        {
            for (int x = 0; x < _configuration.Width; x++)
            {
                switch (Level[x, y])
                {
                    case 0:
                        sb.Append('.');
                        break;
                    case 10:
                        sb.Append('~');
                        break;
                    case 12:
                        sb.Append('^');
                        break;
                    default:
                        sb.Append('#');
                        break;
                }
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }
}
