namespace AdventOfCode2023;

internal class DayEightteen
{
    public static void Solution()
    {
        var testInput = InputOutputHelper.GetInput(true, "18");
        PartOne(true, testInput);

        var input = InputOutputHelper.GetInput(false, "18");
        PartOne(false, input);

        PartTwo(true, testInput);
        PartTwo(false, input);
    }

    public static void PartOne(bool isTest, string[] input)
    {
        var result = 0;

        var parsed = ParseInput(input);
        var coords = GetCoords(parsed);
        var interior = GetInteriorPoints(coords);
        coords.AddRange(interior);
        coords = coords.Distinct().ToList();
        result = coords.Count;

        InputOutputHelper.WriteOutput(isTest, result);
    }

    private static List<(int, int)> GetCoords(List<(char direction, int length, string hexCode)> input)
    {
        var coords = new List<(int, int)>();
        var curX = 0;
        var curY = 0;
        var newInput =
            new List<(char direction, int length, string hexCode)>();
        newInput.AddRange(input);

        foreach (var tuple in newInput)
            switch (tuple.direction)
            {
                case 'U':
                    var lastY = 0;
                    for (var y = curY - 1; y >= curY - tuple.length; y--)
                    {
                        coords.Add((curX, y));
                        lastY = y;
                    }

                    curY = lastY;
                    break;

                case 'L':
                    var lastX = 0;
                    for (var x = curX - 1; x >= curX - tuple.length; x--)
                    {
                        coords.Add((x, curY));
                        lastX = x;
                    }

                    curX = lastX;
                    break;

                case 'R':
                    var lastXTwo = 0;
                    for (var x = curX + 1; x <= curX + tuple.length; x++)
                    {
                        coords.Add((x, curY));
                        lastXTwo = x;
                    }

                    curX = lastXTwo;
                    break;

                case 'D':
                    var lastYTwo = 0;
                    for (var y = curY + 1; y <= curY + tuple.length; y++)
                    {
                        coords.Add((curX, y));
                        lastYTwo = y;
                    }

                    curY = lastYTwo;
                    break;
            }

        return coords;
    }
    
    private static List<(int, int)> GetInteriorPoints(List<(int, int)> frameCoords)
    {
        var interiorPoints = new List<(int, int)>();

        int minX = int.MaxValue, minY = int.MaxValue;
        int maxX = int.MinValue, maxY = int.MinValue;

        foreach (var point in frameCoords)
        {
            minX = Math.Min(minX, point.Item1);
            minY = Math.Min(minY, point.Item2);
            maxX = Math.Max(maxX, point.Item1);
            maxY = Math.Max(maxY, point.Item2);
        }

        for (var x = minX + 1; x < maxX; x++)
        for (var y = minY + 1; y < maxY; y++)
            if (IsInsideFrame(x, y, frameCoords))
                interiorPoints.Add((x, y));

        return interiorPoints;
    }

    private static bool IsInsideFrame(int x, int y, List<(int, int)> frameCoords)
    {
        var intersections = 0;
        var numPoints = frameCoords.Count;

        for (var i = 0; i < numPoints; i++)
        {
            var p1 = frameCoords[i];
            var p2 = frameCoords[(i + 1) % numPoints];

            if ((p1.Item2 > y && p2.Item2 <= y) || (p2.Item2 > y && p1.Item2 <= y))
            {
                var intersectionX = p1.Item1 + (y - p1.Item2) / (double)(p2.Item2 - p1.Item2) * (p2.Item1 - p1.Item1);

                if (x < intersectionX) intersections++;
            }
        }

        return intersections % 2 == 1;
    }

    private static List<(char, int, string)> ParseInput(string[] input)
    {
        List<(char, int, string)> parsed = new();

        foreach (var line in input)
            parsed.Add((line.Split(' ')[0][0], int.Parse(line.Split(' ')[1]), line.Split(' ')[2].Trim('(').Trim(')')));

        return parsed;
    }

    public static void PartTwo(bool isTest, string[] input)
    {
        long count = 0;
        long curX = 1;
        long curY = 1;

        foreach (var line in input)
        {
            var parts = line.Split();
            var hex = parts[^1];
            var val = Convert.ToInt32(hex.Substring(2, hex.Length - 4), 16);
            var dir = Convert.ToInt32(hex.Substring(hex.Length - 2, 1));

            var newX = curX + new[] { 1, 0, -1, 0 }[dir] * val;
            var newY = curY + new[] { 0, 1, 0, -1 }[dir] * val;

            count += curX * newY - newX * curY + Math.Abs(newX - curX) + Math.Abs(newY - curY);
            curX = newX;
            curY = newY;
        }

        InputOutputHelper.WriteOutput(isTest, count / 2 + 1);
    }
}