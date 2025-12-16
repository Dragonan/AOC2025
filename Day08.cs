using System.Numerics;

namespace AOC2025;

public static class Day08
{
    public static void Solve()
    {
        SolvePart2();
    }

    private static void SolvePart1()
    {
        var junctions = PuzzleInput.Input
            .Select(l => l.Split(',').Select(ulong.Parse).ToArray())
            .Select(l => new Vector3(l[0], l[1], l[2])).ToList();
        
        var pairs = new List<VectorPair>();
        for (int i = 0; i < junctions.Count-1; i++)
            for (int j = i+1; j < junctions.Count; j++)
                pairs.Add(new (junctions[i], junctions[j]));
        
        pairs = pairs.OrderBy(p => p.Distance).ToList();

        var connections = new List<List<Vector3>>();
        for(int i = 0; i < 1000; i++)
        {
            var pair = pairs[i];
            var existingA = connections.FirstOrDefault(c => c.Contains(pair.A)); 
            var existingB = connections.FirstOrDefault(c => c.Contains(pair.B));
            if (existingA != null)
            {
                if (existingB == null)
                    existingA.Add(pair.B);
                else if (existingA != existingB)
                {
                    existingA.AddRange(existingB);
                    connections.Remove(existingB);
                }
            }
            else if (existingB != null)
                existingB.Add(pair.A);
            else
                connections.Add(new List<Vector3>([pair.A, pair.B]));
        }

        connections = connections.OrderByDescending(c => c.Count).ToList();
        var prod = connections[0].Count * connections[1].Count * connections[2].Count;

        Console.WriteLine(prod);
    }

    private static void SolvePart2()
    {
        var junctions = PuzzleInput.Input
            .Select(l => l.Split(',').Select(ulong.Parse).ToArray())
            .Select(l => new Vector3(l[0], l[1], l[2])).ToList();
        
        var pairs = new List<VectorPair>();
        for (int i = 0; i < junctions.Count-1; i++)
            for (int j = i+1; j < junctions.Count; j++)
                pairs.Add(new (junctions[i], junctions[j]));
        
        pairs = pairs.OrderBy(p => p.Distance).ToList();

        var connections = new List<List<Vector3>>();
        for(int i = 0; i < pairs.Count; i++)
        {
            var pair = pairs[i];
            var existingA = connections.FirstOrDefault(c => c.Contains(pair.A)); 
            var existingB = connections.FirstOrDefault(c => c.Contains(pair.B));
            if (existingA != null)
            {
                if (existingB == null)
                    existingA.Add(pair.B);
                else if (existingA != existingB)
                {
                    existingA.AddRange(existingB);
                    connections.Remove(existingB);
                }
            }
            else if (existingB != null)
                existingB.Add(pair.A);
            else
                connections.Add(new List<Vector3>([pair.A, pair.B]));
            
            if (connections.Count == 1 && connections[0].Count == junctions.Count)
            {
                Console.WriteLine((long)pair.A.X * (long)pair.B.X);
                break;
            }
        }
    }

    public class VectorPair
    {
        public Vector3 A { get; }
        public Vector3 B { get; }
        public double Distance { get; }

        public VectorPair(Vector3 a, Vector3 b)
        {
            A = a;
            B = b;
            Distance = Vector3.Distance(A,B);
        }
    }
}