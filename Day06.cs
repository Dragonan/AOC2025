using System.Security.Cryptography.X509Certificates;

namespace AOC2025;

public static class Day06
{
    public static void Solve()
    {
        SolvePart2();
    }

    private static void SolvePart1()
    {
        var sum = 0L;
        var input = PuzzleInput.Input.Select(x => x.Split(' ').Where(y => y != "").ToArray()).ToArray();
        var operators = input.Last();
        var problems = input.Take(input.Length - 1).Select(x => x.Select(long.Parse).ToArray()).ToArray();
        for (int i = 0; i < operators.Length; i++)
        {
            var subSum = 0L;

            if (operators[i] == "+")
                foreach (var numbers in problems)
                    subSum += numbers[i];

            else if (operators[i] == "*")
            {
                subSum = 1;
                foreach (var numbers in problems)
                    subSum *= numbers[i];
            }
            
            sum += subSum;
        }

        Console.WriteLine(sum);
    }

    private static void SolvePart2()
    {
        var sum = 0L;
        var input = PuzzleInput.Input;
        var operandRow = input.Length - 1;
        var maxColumns = input[0].Length;
        var operand = ' ';
        var subSum = 0L;
        for (int c = 0; c < maxColumns; c++)
        {
            if (input[operandRow][c] != ' ')
            {
                sum += subSum;
                operand = input[operandRow][c];
                subSum = operand == '*' ? 1 : 0;
            }

            var number = "";
            for (int r = 0; r < operandRow; r++)
            {
                if (input[r][c] != ' ')
                    number += input[r][c];
            }

            if (number == "")
                continue;

            if (operand == '+')
                subSum += long.Parse(number);

            else if (operand == '*')
                subSum *= long.Parse(number);
        }

        sum += subSum;
        Console.WriteLine(sum);
    }
}