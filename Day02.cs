using System.Text.RegularExpressions;

namespace AOC2025;

public static class Day02
{
    public static void Solve()
    {
        SolvePart2();
    }

    private static void SolvePart1()
    {
        var sum = 0ul;
        var ranges = PuzzleInput.Text.ReplaceLineEndings("").Split(',').Select(r => r.Split('-').Select(n => ulong.Parse(n)).ToArray());
        foreach (var range in ranges)
        {
            for (ulong i = range[0]; i <= range[1]; i++)
            {
                var number = i.ToString();
                if (number.Length % 2 == 1)
                {
                    i = (ulong)Math.Pow(10, number.Length) - 1;
                    continue;
                }
                if (Regex.IsMatch(number, "^(\\d+)\\1$"))
                    sum += i;
            }
        }

        Console.WriteLine(sum);
    }

    private static void SolvePart2()
    {
        var sum = 0ul;
        var ranges = PuzzleInput.Text.ReplaceLineEndings("").Split(',').Select(r => r.Split('-').Select(n => ulong.Parse(n)).ToArray());
        foreach (var range in ranges)
        {
            for (ulong i = range[0]; i <= range[1]; i++)
            {
                if (Regex.IsMatch(i.ToString(), "^(\\d+)\\1+$"))
                    sum += i;
            }
        }

        Console.WriteLine(sum);
    }
}