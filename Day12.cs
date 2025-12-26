using System.Drawing;
using System.Text.RegularExpressions;

namespace AOC2025;

public static class Day12
{
    public static void Solve()
    {
        SolvePart1();
    }

    private static void SolvePart1()
    {
        var input = PuzzleInput.Input;
        var presents = new List<List<bool[,]>>();
        var trees = new List<(Point Size, int[] Slots)>();
        for (int i = 1; i < 29; i+=5)
        {
            var positions = new List<bool[,]>
            { 
                new bool[3,3], new bool[3,3], new bool[3,3], new bool[3,3],
                new bool[3,3], new bool[3,3], new bool[3,3], new bool[3,3],                
            };

            for (int y = 0; y < 3; y++)
                for (int x = 0; x < 3; x++)
                    if (input[y+i][x] == '#')
                    {
                        positions[0][x,y] = true; //normal
                        positions[1][2-y,x] = true; //normal 90
                        positions[2][2-x,2-y] = true; //normal 180
                        positions[3][y,2-x] = true; //normal 270
                        positions[4][2-x,y] = true; //flipped
                        positions[5][2-y,2-x] = true; //flipped 90
                        positions[6][x,2-y] = true; //flipped 180
                        positions[7][y,x] = true; //flipped 270
                    }

            var allPositions = new List<bool[,]> { positions[0] };
            for (int j = 1; j < positions.Count; j++)
                if (!allPositions.Any(p => Are2DArraysEqual(p, positions[j])))
                    allPositions.Add(positions[j]);

            presents.Add(allPositions);
        }

        for (int i = 30; i < input.Length; i++)
        {
            var halves = input[i].Split(':');
            var size = halves[0].Split('x').Select(int.Parse).ToArray();
            var slots = halves[1].Substring(1).Split(' ').Select(int.Parse).ToArray();
            trees.Add((new (size[0],size[1]), slots));
        }

        var count = 0;
        foreach (var tree in trees)
        {
            var area = new bool[tree.Size.X, tree.Size.Y];
            var presentsToAdd = new List<(List<bool[,]> Present, int Copies)>();
            for (int i = 0; i < presents.Count; i++)
                if (tree.Slots[i] > 0)
                    presentsToAdd.Add((presents[i], tree.Slots[i]));
            
            if (presentsToAdd.Sum(p => p.Present[0].Cast<bool>().Sum(p => p ? 1 : 0) * p.Copies) > tree.Size.X * tree.Size.Y)
                continue;
            
            if (CanPresentsFit(presentsToAdd, area))
                count++;
        }

        Console.WriteLine(count);
    }

    private static bool Are2DArraysEqual(bool[,] a, bool[,] b)
    {
        if (a.GetLength(0) != b.GetLength(0) || a.GetLength(1) != b.GetLength(1))
            return false;
        
        for (int i = 0; i < a.GetLength(0); i++)
            for (int j = 0; j < a.GetLength(1); j++)
                if (a[i,j] != b[i,j])
                    return false;
        
        return true;
    }

    private static bool CanPresentsFit(List<(List<bool[,]> Present, int Copies)> presentsToAdd, bool[,] area)
    {
        foreach (var present in presentsToAdd)
        {
            for (int i = 0; i < present.Copies; i++)
            {
                var offset = new Point(0, 0);
                var pos = 0;
                while (true)
                {
                    var space = FindSpaceForPresent(present.Present, area, offset, pos);
                    if (space == null)
                        return false;

                    var nextPresentsToAdd = presentsToAdd.Skip(1).Select(p => (p.Present, p.Copies)).ToList();
                    if (present.Copies > 1)
                        nextPresentsToAdd.Insert(0, (presentsToAdd[0].Present, presentsToAdd[0].Copies-1));

                    if (!nextPresentsToAdd.Any())
                        return true;

                    var nextArea = (bool[,])area.Clone();
                    for (int x = 0; x < 3; x++)
                        for (int y = 0; y < 3; y++)
                            if (present.Present[space.Value.Pos][x,y])
                                nextArea[space.Value.Coords.X+x,space.Value.Coords.Y+y] = true;

                    if (CanPresentsFit(nextPresentsToAdd, nextArea))
                        return true;
                    
                    pos++;
                    if (pos == present.Present.Count)
                    {
                        pos = 0;
                        offset.Y++;
                        if (offset.Y == area.GetLength(1))
                        {
                            offset.Y = 0;
                            offset.X++;
                            if (offset.X == area.GetLength(0))
                                return false;
                        }
                    }
                }
            }
        }

        return false;
    }

    private static (Point Coords, int Pos)? FindSpaceForPresent(List<bool[,]> present, bool[,] area, Point offset, int pos)
    {
        for (int i = pos; i < present.Count; i++)
            for (int x = offset.X; x < area.GetLength(0)-2; x++)
                for (int y = offset.Y; y < area.GetLength(1)-2; y++)
                    if (DoesPresentFit(present[i], area, new Point(x,y)))
                        return new (new Point(x,y), i);
        
        return null;
    }

    private static bool DoesPresentFit(bool[,] present, bool[,] area, Point offset)
    {
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (present[x,y] && area[offset.X+x,offset.Y+y])
                    return false;
            }
        }

        return true;
    }
}