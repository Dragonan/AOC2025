using System.Drawing;

namespace AOC2025;

public static class Day09
{
    public static void Solve()
    {
        SolvePart2();
    }

    private static void SolvePart1()
    {
        var redTiles = PuzzleInput.Input.Select(r => r.Split(',').Select(int.Parse).ToArray()).Select(r => new Point(r[0],r[1])).ToArray();
        var max = 0L;
        for (int i = 0; i < redTiles.Length-1; i++)
            for (int j = 0; j < redTiles.Length; j++)
                max = Math.Max(max, Math.Abs(redTiles[i].X-redTiles[j].X+1L)*Math.Abs(redTiles[i].Y-redTiles[j].Y+1L));
        
        Console.WriteLine(max);
    }

    private static void SolvePart2()
    {
        var redTiles = PuzzleInput.Input
            .Select(r => r.Split(',').Select(int.Parse).ToArray())
            .Select(r => new Point(r[0],r[1]))
            .ToArray();

        var sides = new List<Line>();
        var rectangles = new List<RectangleWithSurface>();
        for (int i = 0; i < redTiles.Length-1; i++)
        {
            var tileA = redTiles[i];
            var tileB = redTiles[i+1];
            sides.Add(new (tileA, tileB));
            for (int j = i+1; j < redTiles.Length; j++)
            {
                tileB = redTiles[j];
                if (tileA.X != tileB.X && tileA.Y != tileB.Y)
                    rectangles.Add(new (tileA, tileB));
            }
        }
        sides.Add(new (redTiles.First(), redTiles.Last()));
        rectangles = rectangles.OrderByDescending(r => r.Surface).ToList();
        var verticalSides = sides.Where(s => s.A.X == s.B.X);
        var horizontalSides = sides.Where(s => s.A.Y == s.B.Y);

        foreach(var rect in rectangles)
        {
            var countTopLeft = verticalSides.Count(s => s.A.Y <= rect.TopY && rect.TopY < s.B.Y && s.A.X <= rect.LeftX);
            var countTopRight = verticalSides.Count(s => s.A.Y <= rect.TopY && rect.TopY < s.B.Y && s.A.X < rect.RightX);
            var countBottomRight = verticalSides.Count(s => s.A.Y < rect.BottomY && rect.BottomY <= s.B.Y && s.A.X < rect.RightX); 
            var countBottomLeft = verticalSides.Count(s => s.A.Y < rect.BottomY && rect.BottomY <= s.B.Y && s.A.X <= rect.LeftX);
            if (countTopLeft == countTopRight && countTopRight == countBottomRight &&
                countBottomRight == countBottomLeft && countBottomLeft == countTopLeft &&
                countTopLeft % 2 == 1 && countTopRight % 2 == 1 && countBottomLeft % 2 == 1 && countBottomRight % 2 == 1)
            {
                countTopLeft = horizontalSides.Count(s => s.A.X <= rect.LeftX && rect.LeftX < s.B.X && s.A.Y <= rect.TopY);
                countTopRight = horizontalSides.Count(s => s.A.X < rect.RightX && rect.RightX <= s.B.X  && s.A.Y <= rect.TopY);
                countBottomRight = horizontalSides.Count(s => s.A.X < rect.RightX && rect.RightX <= s.B.X && s.A.Y < rect.BottomY);
                countBottomLeft = horizontalSides.Count(s => s.A.X <= rect.LeftX && rect.LeftX < s.B.X && s.A.Y < rect.BottomY);
                if (countTopLeft == countTopRight && countTopRight == countBottomRight &&
                countBottomRight == countBottomLeft && countBottomLeft == countTopLeft &&
                countTopLeft % 2 == 1 && countTopRight % 2 == 1 && countBottomLeft % 2 == 1 && countBottomRight % 2 == 1)
                {
                    Console.WriteLine(rect.Surface);
                    break;
                }
            }
        }
    }

    public struct Line
    {
        public Point A { get; set; }
        public Point B { get; set; }

        public Line(Point a, Point b)
        {
            A = a.X == b.X ? (b.Y < a.Y ? b : a) : (b.X < a.X ? b : a);
            B = A == a ? b : a;
        }
    }

    public class RectangleWithSurface
    {
        public int LeftX { get; set; }
        public int RightX { get; set; }
        public int TopY { get; set; }
        public int BottomY { get; set; }
        public int Surface { get; }
        public RectangleWithSurface(Point a, Point b)
        {
            Surface = (Math.Abs(a.X - b.X)+1) * (Math.Abs(a.Y - b.Y)+1);
            LeftX = Math.Min(a.X, b.X);
            RightX = Math.Max(a.X, b.X);
            TopY = Math.Min(a.Y, b.Y);
            BottomY = Math.Max(a.Y, b.Y);
        }
    }
}