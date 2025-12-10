using System.Drawing;

namespace AOC2025;

public enum Directions
{
    Up = 1,
    Right = 2,
    Down = 3,
    Left = 4
}

public struct PointWithDir : IEquatable<PointWithDir>
{
    public Point Point { get; set; }
    public Directions Dir { get; set; }

    public PointWithDir(Point point, Directions dir)
    {
        Point = point;
        Dir = dir;
    }

    public override int GetHashCode() => (Point, Dir).GetHashCode();
    public override bool Equals(object? obj) => base.Equals(obj);

    public bool Equals(PointWithDir other)
    {
        return this.Point == other.Point && this.Dir == other.Dir;
    }

    public static bool operator ==(PointWithDir a, PointWithDir b) =>  a.Equals(b);

    public static bool operator !=(PointWithDir a, PointWithDir b) => !a.Equals(b);
}