namespace AOC2025;

public static class Day07
{
    public static void Solve()
    {
        SolvePart2();
    }

    private static void SolvePart1()
    {
        var diagram = PuzzleInput.Input;
        var startIndex = diagram[0].IndexOf('S');
        var beams = new List<int> { startIndex };
        var count = 0;

        for(int i = 1; i < diagram.Length; i++)
        {
            var splits = new List<int>();
            var toRemove = new List<int>();
            foreach (var beam in beams)
            {
                if (diagram[i][beam] == '^')
                {
                    count++;
                    toRemove.Add(beam);
                    if (beam > 0 && !beams.Contains(beam-1) && !splits.Contains(beam-1))
                        splits.Add(beam-1);
                    if (beam < diagram[0].Length - 1 && !beams.Contains(beam+1) && !splits.Contains(beam+1))
                        splits.Add(beam+1);
                }
            }
            beams.RemoveAll(toRemove.Contains);
            beams.AddRange(splits);
        }

        Console.WriteLine(count);
    }

    private static void SolvePart2()
    {
        var diagram = PuzzleInput.Input;
        var startIndex = diagram[0].IndexOf('S');
        var beams = new List<Beam> { new (1, startIndex) };

        for(int i = 1; i < diagram.Length; i++)
        {
            var splits = new List<Beam>();
            var toRemove = new List<Beam>();
            foreach (var beam in beams)
            {
                if (diagram[i][beam.Column] == '^')
                {
                    toRemove.Add(beam);

                    var leftBeam = beams.FirstOrDefault(b => b.Column == beam.Column - 1);
                    var leftSplit = splits.FirstOrDefault(s => s.Column == beam.Column - 1);
                    if (leftBeam != null)
                        leftBeam.Paths += beam.Paths;
                    else if (leftSplit != null)
                        leftSplit.Paths += beam.Paths;
                    else
                        splits.Add(new (beam.Paths, beam.Column - 1));
                    
                    var rightBeam = beams.FirstOrDefault(b => b.Column == beam.Column + 1);
                    var rightSplit = splits.FirstOrDefault(s => s.Column == beam.Column + 1);
                    if (rightBeam != null)
                        rightBeam.Paths += beam.Paths;
                    else if (rightSplit != null)
                        rightSplit.Paths += beam.Paths;
                    else
                        splits.Add(new (beam.Paths, beam.Column + 1));
                    
                }
            }
            beams.RemoveAll(toRemove.Contains);
            beams.AddRange(splits);
        }

        Console.WriteLine(beams.Sum(b => b.Paths));
    }

    public class Beam
    {
        public long Paths { get; set; }
        public int Column { get; set; }

        public Beam(long Paths, int Column)
        {
            this.Paths = Paths;
            this.Column = Column;
        }
    }
}