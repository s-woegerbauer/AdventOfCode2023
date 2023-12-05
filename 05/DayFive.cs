namespace AdventOfCode2023;

public class DayFive
{
    public static List<List<(long from, long to, long range)>> Maps = new();

    public static void Solution()
    {
        var testInput = InputOutputHelper.GetInput(true, "05");
        PartOne(true, testInput);

        var input = InputOutputHelper.GetInput(false, "05");
        PartOne(false, input);

        PartTwo(true, testInput);
        PartTwo(false, input);
    }

    public static void PartOne(bool isTest, string[] input)
    {
        var inputString = string.Join('\n', input);
        var seedsAndMaps = inputString.Split("\n\n");

        var seedsData = seedsAndMaps[0];
        var mapData = seedsAndMaps.Skip(1).ToArray();

        foreach (string line in mapData)
        {
            Maps.Add(ParseMap(line));
        }
        
        var minimumResult = seedsData.Split().Skip(1).Select(long.Parse).Min(seed => ProcessSeed(seed));

        
        
        InputOutputHelper.WriteOutput(isTest, minimumResult);
    }

    public static void PartTwo(bool isTest, string[] input)
    {
        var seeds = input[0].Split(' ').Skip(1).Select(x => long.Parse(x)).ToList();
        List<(long from, long to, long adjustment)>? currentMap = null;
        Maps = new List<List<(long from, long to, long range)>>();

        foreach (var line in input.Skip(2))
        {
            if (line.EndsWith(':'))
            {
                currentMap = new List<(long from, long to, long adjustment)>();
                continue;
            }

            if (line.Length == 0 && currentMap != null)
            {
                Maps.Add(currentMap!);
                currentMap = null;
                continue;
            }

            var nums = line.Split(' ').Select(x => long.Parse(x)).ToArray();
            currentMap!.Add((nums[1], nums[1] + nums[2] - 1, nums[0] - nums[1]));
        }

        if (currentMap != null) Maps.Add(currentMap);

        var ranges = new List<(long from, long to)>();
        for (var i = 0; i < seeds.Count; i += 2) ranges.Add((from: seeds[i], to: seeds[i] + seeds[i + 1] - 1));

        foreach (var map in Maps)
        {
            var orderedmap = map.OrderBy(x => x.from).ToList();

            var newranges = new List<(long from, long to)>();
            foreach (var r in ranges)
            {
                var range = r;
                foreach (var mapping in orderedmap)
                {
                    if (range.from < mapping.from)
                    {
                        newranges.Add((range.from, Math.Min(range.to, mapping.from - 1)));
                        range.from = mapping.from;
                        if (range.from >= range.to)
                            break;
                    }

                    if (range.from <= mapping.to)
                    {
                        newranges.Add((range.from + mapping.range, Math.Min(range.to, mapping.to) + mapping.range));
                        range.from = mapping.to + 1;
                        if (range.from >= range.to)
                            break;
                    }
                }

                if (range.from < range.to)
                    newranges.Add(range);
            }

            ranges = newranges;
        }

        var result2 = ranges.Min(r => r.from);

        InputOutputHelper.WriteOutput(isTest, result2);
    }

    private static List<(long from, long to, long range)> ParseMap(string map)
    {
        var mapData =
            new List<(long source, long destination, long range)>();
        var mapLines = map.Split('\n');
        foreach (var line in mapLines.Skip(1))
        {
            var parts = line.Split();
            var destination = long.Parse(parts[0]);
            var source = long.Parse(parts[1]);
            var range = long.Parse(parts[2]);
            mapData.Add((source, destination, range));
        }

        return mapData;
    }

    private static long LookupValue(long value, List<(long source, long destination, long range)> mapData)
    {
        foreach (var mapEntry in mapData)
        {
            if (mapEntry.source <= value && value < mapEntry.source + mapEntry.range)
            {
                return value - mapEntry.source + mapEntry.destination;
            }
        }

        return value;
    }

    private static long ProcessSeed(long seed)
    {
        foreach (var map in Maps)
        {
            seed = LookupValue(seed, map);
        }

        return seed;
    }
}