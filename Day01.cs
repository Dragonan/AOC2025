using AOC2025;

public static class Day01
{
    public static void Solve()
    {
        SolvePart2();
    }

    public static void SolvePart1()
    {
        var dial = 50;
        var rotations = PuzzleInput.Input;
        var zeroCount = 0;

        foreach (var rotation in rotations)
        {
            var distance = int.Parse(rotation.Substring(1));
            if (rotation[0] == 'L')
                distance *= -1;

            dial += distance;
            if (dial % 100 == 0)
                zeroCount++;
        }

        Console.WriteLine("Password: " + zeroCount);
    }

    public static void SolvePart2()
    {
        
        var dial = 50;
        var rotations = PuzzleInput.Input;
        var zeroCount = 0;

        foreach (var rotation in rotations)
        {
            var distance = int.Parse(rotation.Substring(1));

            zeroCount += distance/100;
            distance %= 100;

            if (rotation[0] == 'L')
                distance *= -1;

            var startsAtZero = dial == 0;
            dial += distance;

            if (dial == 0)
                zeroCount++;
            if (dial >= 100)
            {
                zeroCount++;
                dial %= 100;
            }
            if (dial < 0)
            {
                if (dial != distance)
                    zeroCount++;
                dial += 100;
            }
        }

        Console.WriteLine("Password: " + zeroCount);
    }
}