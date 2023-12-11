namespace AdventOfCode2023;

internal class DayEleven
{
    private static List<int> _rowsToExpand = new();
    private static List<int> _columnsToExpand = new();

    public static void Solution()
    {
        var testInput = InputOutputHelper.GetInput(true, "11");
        PartOne(true, testInput);

        var input = InputOutputHelper.GetInput(false, "11");
        PartOne(false, input);

        PartTwo(true, testInput);
        PartTwo(false, input);
    }

    public static void PartOne(bool isTest, string[] input)
    {
        var grid = ParseInput(input);
        var charCount = grid.AmountOf('#');

        for (var i = 0; i < charCount; i++)
        {
            var found = false;

            for (var y = 0; y < grid.VerticalLength && !found; y++)
            for (var x = 0; x < grid.HorizontalLength && !found; x++)
                if (grid.Array[y, x] == '#')
                {
                    found = true;
                    grid.Array[y, x] = (char)('0' + i);
                }
        }

        var result = CalculatePaths(charCount, grid);
        InputOutputHelper.WriteOutput(isTest, result);
    }

    private static int CalculatePaths(int charCount, Grid<char> grid)
    {
        var result = 0;
        var alreadyBeen = new List<(int, int)>();
        var memoizationCache = new Dictionary<(int, int), int>();

        for (var i = 0; i < charCount; i++)
        for (var j = 0; j < charCount; j++)
            if (j != i && !alreadyBeen.Contains((i, j)))
            {
                alreadyBeen.Add((i, j));
                var pathLength = memoizationCache.TryGetValue((i, j), out var cachedLength)
                    ? cachedLength
                    : grid.GetShortestPath((char)('0' + i), (char)('0' + j), Barrier);

                memoizationCache[(i, j)] = pathLength;
                result += pathLength;
            }

        return result;
    }

    public static bool Barrier(char charOne, char charTwo)
    {
        return false;
    }

    private static Grid<char> ParseInput(string[] input)
    {
        var charList = input.Select(line => line.ToCharArray().ToList()).ToList();

        for (var i = 0; i < input.Length; i++)
            if (!charList[i].Any(cell => cell != '.'))
            {
                charList.Insert(i, new string('.', input[0].Length).ToCharArray().ToList());
                i++;
            }

        for (var i = 0; i < charList[0].Count; i++)
            if (charList.All(row => row[i] == '.'))
            {
                foreach (var row in charList) row.Insert(i, '.');

                i++;
            }

        return new Grid<char>(ConvertToStringArray(charList));
    }

    private static string[] ConvertToStringArray(List<List<char>> charList)
    {
        return charList.Select(charInnerList => new string(charInnerList.ToArray())).ToArray();
    }

    public static void PartTwo(bool isTest, string[] input)
    {
        var galaxy = input.Select(line => line.ToCharArray()).ToArray();

        _rowsToExpand = galaxy.Select((row, idx) => new { Index = idx, Row = row })
            .Where(item => item.Row.All(cell => cell == '.'))
            .Select(item => item.Index)
            .ToList();

        _columnsToExpand = galaxy.First()
            .Select((cell, idx) => new { Index = idx, Col = galaxy.Select(row => row[idx]) })
            .Where(item => item.Col.All(cell => cell == '.'))
            .Select(item => item.Index)
            .ToList();

        var coords = galaxy.SelectMany((row, r) => row.Select((cell, c) => new { Row = r, Col = c, Cell = cell }))
            .Where(item => item.Cell != '.')
            .Select(item => new Point(item.Row, item.Col))
            .ToList();

        var result = CalculateDistances(coords);
        InputOutputHelper.WriteOutput(isTest, result);
    }

    private static long CalculateDistances(List<Point> coords)
    {
        var result = 0L;

        for (var i = 0; i < coords.Count; i++)
        for (var j = i + 1; j < coords.Count; j++)
            result += Distance(coords[i], coords[j], 1_000_000);

        return result;
    }

    private static int Distance(Point a, Point b, int expansion)
    {
        expansion -= 1;

        var distance = Math.Abs(a.Row - b.Row) + Math.Abs(a.Col - b.Col);
        var rowRange = Enumerable.Range(Math.Min(a.Row, b.Row), Math.Abs(a.Row - b.Row));
        var colRange = Enumerable.Range(Math.Min(a.Col, b.Col), Math.Abs(a.Col - b.Col));

        distance += rowRange.Count(_rowsToExpand.Contains) * expansion;
        distance += colRange.Count(_columnsToExpand.Contains) * expansion;

        return distance;
    }
}

public class Point
{
    public Point(int row, int col)
    {
        Row = row;
        Col = col;
    }

    public int Row { get; set; }
    public int Col { get; set; }
}