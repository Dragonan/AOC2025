namespace AOC2025;

public static class Day05
{
    public static void Solve()
    {
        SolvePart2();
    }

    private static void SolvePart1()
    {
        var sum = 0;
        var freshRanges = PuzzleInput.Input.Take(PuzzleInput.Input.IndexOf("")).Select(x => x.Split('-').Select(long.Parse).ToArray()).ToArray();
        var ids = PuzzleInput.Input.Skip(PuzzleInput.Input.IndexOf("")+1).Select(long.Parse).ToArray();
        
        foreach(var id in ids)
            foreach(var range in freshRanges)
                if (range[0] <= id && id <= range[1])
                {
                    sum++;
                    break;
                }
        
        Console.WriteLine(sum);
    }

    private static void SolvePart2()
    {
        var sum = 0UL;
        var freshRanges = PuzzleInput.Input
            .Take(PuzzleInput.Input.IndexOf(""))
            .Select(x => x.Split('-').Select(ulong.Parse).ToArray())
            .OrderBy(r => r[0])
            .ToArray();

        var mergedRanges = new List<ulong[]> { freshRanges[0] };

        foreach(var range in freshRanges)
        {
            var mRange = mergedRanges.Last();
            if (mRange[1] >= range[0])
                mRange[1] = Math.Max(mRange[1], range[1]);
            else
                mergedRanges.Add([range[0], range[1]]);
        }

        foreach (var range in mergedRanges)
            sum += range[1] - range[0] + 1;
        
        Console.WriteLine(sum);
    }
}