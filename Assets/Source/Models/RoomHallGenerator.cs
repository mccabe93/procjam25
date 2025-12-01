using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Source.Generators;

public sealed class RoomHallGenerator
{
    private readonly RoomHallConfiguration _configuration;
    private System.Random _rng;
    public readonly int[,] Level;
    private readonly List<Room> _rooms = new List<Room>();
    private readonly int[] _sides = new int[] { 0, 1, 2, 3 };

    public IReadOnlyList<Room> Rooms => _rooms.AsReadOnly();

    public RoomHallGenerator(RoomHallConfiguration configuration)
    {
        _configuration = configuration;
        int maximumWidth =
            _configuration.RoomMaximumWidth * _configuration.RoomCount
            + _configuration.HallwayMaximumSize * _configuration.RoomCount;
        int maximumHeight =
            _configuration.RoomMaximumHeight * _configuration.RoomCount
            + _configuration.HallwayMaximumSize * _configuration.RoomCount;
        _rng = new Random(_configuration.Seed);
        Level = new int[maximumWidth, maximumHeight];
    }

    public void Generate()
    {
        PlaceRooms();
    }

    private void PlaceRooms()
    {
        int x = 0,
            y = 0;
        List<int> sides = new List<int>();
        for (int i = 0; i < _configuration.RoomCount; i++)
        {
            int roomHeight = _rng.Next(
                _configuration.RoomMinimumHeight,
                _configuration.RoomMaximumHeight
            );
            int roomWidth = _rng.Next(
                _configuration.RoomMinimumWidth,
                _configuration.RoomMaximumWidth
            );
            Room lastRoom = _rooms.LastOrDefault();
            Room newRoom = new Room()
            {
                X = x,
                Y = y,
                Width = roomWidth,
                Height = roomHeight,
                HallwaySize = _rng.Next(
                    _configuration.HallwayMinimumSize,
                    _configuration.HallwayMaximumSize
                ),
            };
            if (lastRoom != null)
            {
                lastRoom.To = newRoom;
                newRoom.From = lastRoom;
            }
            PlaceRoom(newRoom, sides);
            PlaceHallway(newRoom);
        }
    }

    private void PlaceRoom(Room room, List<int> sides)
    {
        sides.Clear();
        sides.AddRange(_sides);
        bool success = false;
        while (!success && sides.Count > 0)
        {
            int index = _rng.Next(sides.Count);
            room.ConnectionFromSide = (ConnectionSide)(sides[index] + 1);
            switch (room.ConnectionFromSide)
            {
                case ConnectionSide.Left:
                    success = TryPlaceRight(room);
                    break;
                case ConnectionSide.Below:
                    success = TryPlaceBelow(room);
                    break;
                case ConnectionSide.Right:
                    success = TryPlaceLeft(room);
                    break;
                case ConnectionSide.Above:
                    success = TryPlaceAbove(room);
                    break;
            }
            sides.RemoveAt(index);
        }
        FillRoom(room);
        _rooms.Add(room);
    }

    private void PlaceHallway(Room room)
    {
        if (room.From == null)
        {
            return;
        }
        switch (room.ConnectionFromSide)
        {
            case ConnectionSide.Left:
            case ConnectionSide.Right:
                int[] ys = GetParallelVerticalCells(room);
                _rng.Shuffle(ys);
                FillHallway(room.X, room.From.X, ys[0], ys[0]);
                room.HallwayX = room.X;
                room.HallwayY = ys[0];
                break;
            case ConnectionSide.Above:
            case ConnectionSide.Below:
                int[] xs = GetParallelHorizontalCells(room);
                _rng.Shuffle(xs);
                FillHallway(xs[0], xs[0], room.Y, room.From.Y);
                room.HallwayX = xs[0];
                room.HallwayY = room.Y;
                break;
        }
    }

    private bool TryPlaceRight(Room room)
    {
        int[] match;
        if (room.From == null) // First room can always be placed right.
        {
            match = Enumerable.Range(0, Level.GetLength(1) - room.Width).ToArray();
            _rng.Shuffle(match);
            room.X = match[0];
            return true;
        }
        match = Enumerable.Range(room.From.Y, room.From.Height).ToArray();
        if (match.Length == 0)
        {
            return false;
        }
        int i = 0;
        do
        {
            if (i >= match.Length)
            {
                return false;
            }
            _rng.Shuffle(match);
            room.Y = match[i++];
            room.X = room.From.X + room.From.Width + room.HallwaySize;
            if (room.X + room.Width >= Level.GetLength(0))
            {
                return false;
            }
        } while (DoesRoomOverlap(room));
        return true;
    }

    private bool TryPlaceLeft(Room room)
    {
        if (room.From == null) // First room cannot be placed left.
        {
            return false;
        }
        int[] match = Enumerable.Range(room.From.Y, room.From.Height).ToArray();
        if (match.Length == 0)
        {
            return false;
        }
        int i = 0;
        do
        {
            if (i >= match.Length)
            {
                return false;
            }
            _rng.Shuffle(match);
            room.Y = match[i++];
            room.X = room.From.X - room.Width - room.HallwaySize;
            if (room.X - room.Width < 0)
            {
                return false;
            }
        } while (DoesRoomOverlap(room));
        return true;
    }

    private bool TryPlaceBelow(Room room)
    {
        int[] match;
        if (room.From == null) // First room can always be placed below.
        {
            match = Enumerable.Range(0, Level.GetLength(0) - room.Height).ToArray();
            _rng.Shuffle(match);
            room.Y = match[0];
            return true;
        }
        match = Enumerable.Range(room.From.X, room.From.Width).ToArray();
        if (match.Length == 0)
        {
            return false;
        }
        int i = 0;
        do
        {
            if (i >= match.Length)
            {
                return false;
            }
            _rng.Shuffle(match);
            room.X = match[i++];
            room.Y = room.From.Y + room.From.Height + room.HallwaySize;
            if (room.Y + room.Height >= Level.GetLength(1))
            {
                return false;
            }
        } while (DoesRoomOverlap(room));
        return true;
    }

    private bool TryPlaceAbove(Room room)
    {
        int[] match;
        if (room.From == null) // First room cannot be placed above.
        {
            return false;
        }
        match = Enumerable.Range(room.From.X, room.From.Width).ToArray();
        if (match.Length == 0)
        {
            return false;
        }
        int i = 0;
        do
        {
            if (i >= match.Length)
            {
                return false;
            }
            _rng.Shuffle(match);
            room.X = match[i++];
            room.Y = room.From.Y - room.From.Height - room.HallwaySize;
            if (room.Y - room.Height < 0)
            {
                return false;
            }
        } while (DoesRoomOverlap(room));
        return true;
    }

    private void FillHallway(int fromX, int toX, int fromY, int toY)
    {
        int startX = Math.Min(fromX, toX);
        int endX = Math.Max(fromX, toX);
        for (int x = startX; x < endX; x++)
        {
            Level[x, fromY] = 1;
        }
        int startY = Math.Min(fromY, toY);
        int endY = Math.Max(fromY, toY);
        for (int y = startY; y < endY; y++)
        {
            Level[fromX, y] = 1;
        }
    }

    private void FillRoom(Room room)
    {
        for (int i = room.X; i < room.X + room.Width; i++)
        {
            for (int j = room.Y; j < room.Y + room.Height; j++)
            {
                Level[i, j] = 1;
            }
        }
    }

    private int[] GetParallelVerticalCells(Room room)
    {
        List<int> parallelCells = new List<int>();
        for (int y = room.Y; y < room.Y + room.Height; y++)
        {
            if (y >= room.From.Y && y <= room.From.Y + room.From.Height)
            {
                parallelCells.Add(y);
            }
        }
        return parallelCells.ToArray();
    }

    private int[] GetParallelHorizontalCells(Room room)
    {
        List<int> parallelCells = new List<int>();
        for (int x = room.X; x < room.X + room.Width; x++)
        {
            if (x >= room.From.X && x <= room.From.X + room.From.Width)
            {
                parallelCells.Add(x);
            }
        }
        return parallelCells.ToArray();
    }

    private bool DoesRoomOverlap(Room room)
    {
        // We add HallwayMaximumSize since any lesser size will implicitly not overlap if this is true.
        return _rooms.Find(otherRoom =>
                otherRoom.X < room.X + room.Width
                && otherRoom.X + otherRoom.Width > room.X
                && otherRoom.Y < room.Y + room.Height
                && otherRoom.Y + otherRoom.Height > room.Y
            ) != null;
    }

    public override string ToString()
    {
        int firstX = _rooms.Min(t => t.X);
        int lastX = _rooms.Max(t => t.X + t.Width);
        int firstY = _rooms.Min(t => t.Y);
        int lastY = _rooms.Max(t => t.Y + t.Height);
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int j = firstY; j < lastY; j++)
        {
            for (int i = firstX; i < lastX; i++)
            {
                bool isRoom = Level[i, j] == 1;
                sb.Append(isRoom ? "." : "#");
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }
}
