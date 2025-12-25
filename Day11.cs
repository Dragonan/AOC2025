using System.Diagnostics;
using System.Security.Authentication;

namespace AOC2025;

public static class Day11
{
    public static void Solve()
    {
        SolvePart2();
    }

    private static void SolvePart1()
    {
        var devices = PuzzleInput.Input.Select(i => i.Split(':').ToArray()).ToDictionary(k => k[0], v => v[1].Substring(1).Split(' ').ToArray());
        var count = 0;
        string[] paths = [ "you" ];
        while (paths.Any())
        {
            var nextPaths = new List<string>();
            foreach (var path in paths)
            {
                var outputs = devices[path];
                foreach (var output in outputs)
                {
                    if (output == "out")
                        count++;
                    else
                        nextPaths.Add(output);
                }
            }
            paths = nextPaths.ToArray();
        }

        Console.WriteLine(count);
    }

    private static void SolvePart2()
    {
        
        var input = PuzzleInput.Input.Select(i => i.Split(':').ToArray()).ToDictionary(k => k[0], v => v[1].Substring(1).Split(' ').ToArray());

        var devices = input.ToDictionary(k => k.Key, v => new DeviceNode(v.Key));
        devices.Add("out", new DeviceNode("out"));
        foreach (var device in devices.Values)
            if (device.Name != "out")
                foreach (var name in input[device.Name])
                    device.Outputs.Add(devices[name]);


        var svrToFft = CountPaths(devices["svr"], devices["fft"]);
        var fftToDac = CountPaths(devices["fft"], devices["dac"]);
        var dacToOut = CountPaths(devices["dac"], devices["out"]);
        var svrToDac = CountPaths(devices["svr"], devices["dac"]);
        var dacToFft = CountPaths(devices["dac"], devices["fft"]);
        var fftToOut = CountPaths(devices["fft"], devices["out"]);
        Console.WriteLine(Math.Max(svrToFft*fftToDac*dacToOut, svrToFft*fftToDac*dacToOut));
    }

    private static long CountPaths(DeviceNode start, DeviceNode end)
    {
        var paths = 0L;
        var nodesToMap = new List<(DeviceNode Node, long Paths)> { (start, 1) };
        while (nodesToMap.Any())
        {
            var nextToMap = new List<(DeviceNode Node, long Paths)>();
            foreach (var node in nodesToMap)
            {
                foreach (var next in node.Node.Outputs)
                {
                    if (next == end)
                        paths += node.Paths;
                    else
                        nextToMap.Add((next, node.Paths));
                }
            }
            
            nodesToMap = nextToMap.GroupBy(n => n.Node).Select(g => (g.Key, g.Sum(n => n.Paths))).ToList();
        }

        return paths;
    }
}

public class DeviceNode
{
    public string Name { get; }
    public List<DeviceNode> Outputs { get; }

    public DeviceNode(string name)
    {
        Name = name;
        Outputs = new List<DeviceNode>();
    }

    public override string ToString()
    {
        return Name;
    }
}