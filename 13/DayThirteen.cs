namespace AdventOfCode2023;

internal class DayThirteen
{
    public static void Solution()
    {
        var testInput = InputOutputHelper.GetInput(true, "13");
        PartOne(true, testInput);

        var input = InputOutputHelper.GetInput(false, "13");
        PartOne(false, input);

        PartTwo(true, testInput);
        PartTwo(false, input);
    }

    public static void PartOne(bool isTest, string[] input)
    {
        var result = Solve(input, 0);

        InputOutputHelper.WriteOutput(isTest, result);
    }

    private static string ConvertToString(string[] array)
    {
        return string.Join("\n", array);
    }

    private static Dictionary<(int, int), char> ParseGrid(string input)
    {
        return input.Split('\n')
            .SelectMany((row, rowIndex) => row.Select((character, columnIndex) => ((columnIndex, rowIndex), character)))
            .ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);
    }

    private static int CheckPatterns(Dictionary<(int x, int y), char> grid, int width, int height, int distance)
    {
        var totalScore = 0;

        for (var horizontalOffset = 1; horizontalOffset < width; horizontalOffset++)
            if (CheckHorizontalPattern(grid, horizontalOffset, height, distance))
                totalScore += horizontalOffset;

        for (var verticalOffset = 1; verticalOffset < height; verticalOffset++)
            if (CheckVerticalPattern(grid, width, verticalOffset, distance))
                totalScore += 100 * verticalOffset;

        return totalScore;
    }

    private static bool CheckHorizontalPattern(Dictionary<(int x, int y), char> grid, int patternWidth, int height,
        int expectedDifferences)
    {
        return Enumerable.Range(0, patternWidth)
            .SelectMany(columnOffset => Enumerable.Range(0, height)
                .Select(row => grid.TryGetValue((columnOffset, row), out var value) &&
                               grid.TryGetValue((patternWidth - columnOffset + patternWidth - 1, row),
                                   out var value2) &&
                               value != value2))
            .Count(diff => diff) == expectedDifferences;
    }

    private static bool CheckVerticalPattern(Dictionary<(int x, int y), char> grid, int width, int patternHeight,
        int expectedDifferences)
    {
        return Enumerable.Range(0, patternHeight)
            .SelectMany(rowOffset => Enumerable.Range(0, width)
                .Select(column => grid.TryGetValue((column, rowOffset), out var value) &&
                                  grid.TryGetValue((column, patternHeight - rowOffset + patternHeight - 1),
                                      out var value2) &&
                                  value != value2))
            .Count(diff => diff) == expectedDifferences;
    }

    public static void PartTwo(bool isTest, string[] input)
    {
        var result = Solve(input, 1);

        InputOutputHelper.WriteOutput(isTest, result);
    }

    public static int Solve(string[] input, int misMatches)
    {
        var result = 0;

        var inputString = ConvertToString(input);
        var parts = inputString.Split("\n\n");

        foreach (var s in parts)
        {
            var grid = ParseGrid(s);
            var width = grid.Keys.Max(tuple => tuple.Item1) + 1;
            var height = grid.Keys.Max(tuple => tuple.Item2) + 1;

            result += CheckPatterns(grid, width, height, misMatches);
        }

        return result;
    }
}