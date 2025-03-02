using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class Vector
{
    public int X { get; }
    public int Y { get; }

    public Vector(int x, int y)
    {
        X = x;
        Y = y;
    }
}
