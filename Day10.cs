namespace AOC2025;

public static class Day10
{
    public static void Solve()
    {
        SolvePart2();
    }

    private static void SolvePart1()
    {
        var machines = PuzzleInput.Input.Select(line => new Machine(line)).ToArray();
        var total = 0;
        
        foreach (var machine in machines)
        {
            total += TurnOnMachine(machine);
        }

        Console.WriteLine(total);
    }

    private static void SolvePart2()
    {
        
        var machines = PuzzleInput.Input.Select(line => new Machine(line)).ToArray();
        var total = 0L;
        foreach (var machine in machines)
            total += ReduceAndBackSubstitute(machine);

        Console.WriteLine(total);
    }

    private static int TurnOnMachine(Machine machine)
    {
        var presses = 0;
        var buttonsToTry = new List<(bool[] Lights, int[][] Buttons)> { (Enumerable.Repeat(false, machine.Lights.Length).ToArray(), machine.Buttons.ToArray()) };
        while (buttonsToTry.Any())
        {
            var nextButtonsToTry = new List<(bool[] Lights, int[][] Buttons)>();
            presses++;
            foreach (var attempt in buttonsToTry)
            {
                for(int i = 0; i < attempt.Buttons.Length; i++)
                {
                    var newLights = attempt.Lights.ToArray();
                    foreach (var trigger in attempt.Buttons[i])
                        newLights[trigger] = !attempt.Lights[trigger];

                    if (newLights.SequenceEqual(machine.Lights))
                        return presses;
                    
                    nextButtonsToTry.Add((newLights, attempt.Buttons.RemoveAt(i)));
                }
            }
            buttonsToTry = nextButtonsToTry;
        }

        return int.MaxValue;
    }

    // These calculations, ReduceAndBackSubstitute, BackSub, and Swap, are copied from https://github.com/TrueNorthIT
    public static long ReduceAndBackSubstitute(Machine machine)
    {
        (int R, int C) = (machine.Joltages.Length, machine.Buttons.Length);

        //create our matrix, Ax = b
        int[][] A = new int[R][];
        for (int r = 0; r < R; r++)
        {
            //make an extra col. for the b values
            A[r] = new int[C+1];
            A[r][C] = machine.Joltages[r];
        }
        foreach (var button in machine.Buttons.Index())
            foreach (var toggle in button.Item)
                A[toggle][button.Index] = 1;

        //we are going to reduce out matrix as much as possible
        for (int r = 0; r < R && r < C; r++)
        {
            //for row i, we need to reduce column i if required
            int pivot = -1;
            for (int p = r; p < R; p++)
                if (A[p][r] != 0)
                    pivot = p;

            if (pivot == -1)
                //column is all zeroes already so we are done
                continue;

            //swap our pivot row (and values)
            Swap(A, pivot, r);
            //and reduce all the rows below our pivot row
            for (int p = r + 1; p < R; p++)
            {
                if (A[p][r] == 0)
                    continue;

                //make sure we keep our maths to integers!
                (var deltaNom, var deltaDen) = (A[p][r], A[r][r]);
                for(int c = 0; c < C; c++)
                {
                    A[p][c] = deltaDen * A[p][c] - A[r][c] * deltaNom;
                }
                A[p][C] = A[p][C] * deltaDen - A[r][C] * deltaNom;
            }
            //we have fully reduced from r+1 to the end
        }
        //now we have to start substituting in
        //and if we get stuck start brute forcing the parameters
        return BackSub(machine, R, C, A);
    }

    private static int BackSub(Machine machine, int R, int C, int[][] A)
    {
        var maximums = machine.Buttons.Select(toggles => toggles.Min(bi => machine.Joltages[bi])).ToArray();

        int best = int.MaxValue;
        Stack<(int row, int[] pressed)> stack = new();

        stack.Push((R - 1, Enumerable.Repeat(-1,C).ToArray()));
        while (stack.Any())
        {
            (var r, var pressed) = stack.Pop();
            if (r < 0)
            {
                //we've reached the end with no failures, this must be a solution
                best = Math.Min(pressed.Sum(), best);
                continue;
            }

            //we have a row of the form 0 + 0 + ... xr + r(r+1) ... xfinal = br
            //we need to sub in any values we know and then start brute forcing the rest
            int rowTotal = A[r][C];
            for (int c = r; c < C; c++)
                if (A[r][c] != 0)
                    if (pressed[c] >= 0)
                        rowTotal -= A[r][c] * pressed[c];                    
                    else
                        goto bruteForce;

            //1) rowTotal = 0, the row is consistent and all values are specified, move to next row
            //2) rowTotal != 0, this was a failed substitution so quit this path
            if (rowTotal == 0)
                stack.Push((r - 1, pressed));
            continue;
        bruteForce:;
            //we need to choose our next value
            //so push all the options to test
            for (int c = r; c < C; c++)
            {
                if (A[r][c] != 0 && pressed[c] == -1)
                {
                    //upper bound of our choice is confusing because of -'ve numbers in the matrix
                    //so just use an easily calculated pre-computed max
                    var max = maximums[c];

                    //this tigher bound reduces runtime by about 20%
                    //even better might be to find the min number of presses for any button to reach
                    //each joltage total this button doesn't impact
                    var pressedSoFar = pressed.Sum();
                    if (pressedSoFar + max >= best)
                    {
                        max = best - pressedSoFar;
                    }

                    for (int p = 0; p <= max; p++)
                    {
                        var newPressed = pressed.ToArray();
                        newPressed[c] = p;
                        stack.Push((r, newPressed));
                    }
                    break;
                }
            }
        }

        return best;
    }

    private static void Swap<T>(T[] arr, int i0, int i1)
    {
        var tmp = arr[i0];
        arr[i0] = arr[i1];
        arr[i1] = tmp;
    }
}

public class Machine
{
    public bool[] Lights { get; }
    public int[][] Buttons { get; }
    public int[] Joltages { get; }

    public Machine(string input)
    {
        var parts = input.Split(' ');
        Lights = parts[0].Substring(1, parts[0].Length-2).Select(c => c == '#').ToArray();
        Joltages = parts.Last().Substring(1, parts.Last().Length-2).Split(',').Select(int.Parse).ToArray();
        Buttons = parts.Skip(1).SkipLast(1).Select(b => b.Substring(1, b.Length-2).Split(',').Select(int.Parse).ToArray()).ToArray();
    }
}