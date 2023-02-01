using System;

public struct Coordinate
{
    private readonly int x, y;


    public int X { get { return x; } }
    public int Y { get { return y; } }

    public static Coordinate Zero { get => new Coordinate(0, 0); }

    public static Coordinate N { get => new Coordinate(0, 1); }
    public static Coordinate S { get => new Coordinate(0, -1); }
    public static Coordinate W { get => new Coordinate(-1, 0); }
    public static Coordinate E { get => new Coordinate(1, 0); }

    public static Coordinate NW { get => new Coordinate(-1, 1); }
    public static Coordinate NE { get => new Coordinate(1, 1); }
    public static Coordinate SW { get => new Coordinate(-1, -1); }
    public static Coordinate SE { get => new Coordinate(1, -1); }

    public static Coordinate[] Directions
    {
        get
        {
            return new Coordinate[]
            {
                NW,
                N,
                NE,
                W,
                E,
                SW,
                S,
                SE
            };
        }
    }

    public Coordinate(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    // override methods
    public override int GetHashCode()
    {
        return X.GetHashCode() ^ Y.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        return obj is Coordinate t && this == t;
    }

    public override string ToString()
    {
        return GetGridName() + $"({X},{Y})";
    }

    // methods
    public string GetGridName()
    {
        return XCoorToLetter(X) + YCoorToNumber(Y);
    }

    public bool isLightSquare()
    {
        return (X + Y) % 2 == 1;
    }

    public SquareColor GetSquareColor() => ((X + Y) % 2 == 1) ? SquareColor.Light : SquareColor.Dark; 

    public Coordinate GetLocal(int x, int y) => new Coordinate(this.X + x, this.Y + y);

    public Coordinate GetLocal(Coordinate dir) => new Coordinate(this.X + dir.X, this.Y + dir.Y);

    public Coordinate GetLocal(int x, int y, out Coordinate newTile) => newTile = GetLocal(x, y);

    public Coordinate GetLocal(Coordinate dir, out Coordinate newTile) => newTile = GetLocal(dir.X, dir.Y);



    // static methods
    public static bool operator ==(Coordinate a, Coordinate b)
        => a.X == b.X && a.Y == b.Y;

    public static bool operator !=(Coordinate a, Coordinate b)
        => a.X != b.X || a.Y != b.Y;

    public static Coordinate operator *(Coordinate a, int b)
        => new Coordinate(a.X * b, a.Y * b);

    public static Coordinate operator +(Coordinate a, Coordinate b)
        => new Coordinate(a.X + b.X, a.Y + b.Y);

    public static string XCoorToLetter(int x)
    {
        char c = (char)(97 + (x));
        return c.ToString();
    }

    public static int YCoorToNumber(int y) { return (y + 1); }

    public static int NumberToYCoor(int y) { return (y - 1); }

    public static int LetterToXCoor(char c) { throw new NotImplementedException(); }

    public static string GetGridName(int x, int y)
    {
        return GetGridName(new Coordinate(x, y));
    }

    public static string GetGridName(Coordinate tile)
    {
        return tile.GetGridName();
    }
}