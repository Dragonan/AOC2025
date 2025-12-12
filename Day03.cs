namespace AOC2025;

public static class Day03
{
    public static void Solve()
    {
        SolvePart2();
    }

    private static void SolvePart1()
    {
        var banks = PuzzleInput.Input;
        var sum = 0;
        foreach (var bank in banks)
        {
            var firstDigit = bank.Substring(0, bank.Length - 1).Max();
            var secondDigit = bank.Substring(bank.IndexOf(firstDigit) + 1).Max();
            sum += int.Parse(new string([firstDigit, secondDigit]));
        }

        Console.WriteLine(sum);
    }

    private static void SolvePart2()
    {
        var banks = PuzzleInput.Input;
        var sum = 0L;
        foreach (var bank in banks)
            sum += long.Parse(new string (GetHighestDigit(bank, 12)));

        Console.WriteLine(sum);
    }

    private static char[] GetHighestDigit(string number, int digitsToFind)
    {
        var digit = number.Substring(0, number.Length - digitsToFind + 1).Max();
        if (digitsToFind == 1)
            return [digit];
        return GetHighestDigit(number.Substring(number.IndexOf(digit)+1), digitsToFind-1).Prepend(digit).ToArray();
    }
}