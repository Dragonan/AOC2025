using System.Drawing;

namespace AOC2025;

public static class Day04
{
    public static void Solve()
    {
        SolvePart2();
    }

    private static void SolvePart1()
    {
        var sum = 0;
        var map = PuzzleInput.Input;
        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map[y].Length; x++)
            {
                if (map[y][x] == '.')
                    continue;
                
                var neighbours = 0;
                var locations = new Point[]
                {
                    new (x-1,y-1),new (x,y-1),new (x+1,y-1),
                    new (x-1,y),                new (x+1,y),
                    new (x-1,y+1),new (x,y+1),new (x+1,y+1),
                };

                foreach (var location in locations)
                    if (!location.IsOutOfBounds(map[y].Length, map.Length) && map[location.Y][location.X] == '@')
                        neighbours++;

                if (neighbours < 4)
                    sum++;
            }
        }

        Console.WriteLine(sum);
    }

    private static void SolvePart2()
    {
        
        var sum = 0;
        var map = PuzzleInput.Input;
        var canRemove = true;
        while (canRemove)
        {
            var toRemove = new List<Point>();
            for (int y = 0; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x] == '.')
                        continue;
                    
                    var neighbours = 0;
                    var locations = new Point[]
                    {
                        new (x-1,y-1),new (x,y-1),new (x+1,y-1),
                        new (x-1,y),                new (x+1,y),
                        new (x-1,y+1),new (x,y+1),new (x+1,y+1),
                    };

                    foreach (var location in locations)
                        if (!location.IsOutOfBounds(map[y].Length, map.Length) && map[location.Y][location.X] == '@')
                            neighbours++;

                    if (neighbours < 4)
                    {
                        sum++;
                        toRemove.Add(new (x, y));
                    }
                }
            }

            canRemove = toRemove.Any();
            foreach (var coords in toRemove)
                map[coords.Y] = map[coords.Y].ReplaceAt(coords.X, '.');
        }

        Console.WriteLine(sum);
    }
}