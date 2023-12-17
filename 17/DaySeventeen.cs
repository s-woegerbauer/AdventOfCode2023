namespace AdventOfCode2023;

internal class DaySeventeen
{
    private static int[][]? _grid;

    private static readonly int[][] Directions = { new[] { 0, 1 }, new[] { 1, 0 }, new[] { 0, -1 }, new[] { -1, 0 } };

    public static void Solution()
    {
        var testInput = InputOutputHelper.GetInput(true, "17");
        PartOne(true, testInput);


        var input = InputOutputHelper.GetInput(false, "17");
        PartOne(false, input);

        PartTwo(true, testInput);
        PartTwo(false, input);
    }

    private static bool IsPositionInRange((int, int) position, int[][] array)
    {
        return position.Item1 >= 0 && position.Item1 < array.Length && position.Item2 >= 0 &&
               position.Item2 < array[0].Length;
    }

    private static int FindShortestPath(int minDistance, int maxDistance)
    {
        var queue = new List<(int, int, int, int)>();
        queue.Add((0, 0, 0, -1));
        var seen = new HashSet<(int, int, int)>();
        var costs = new Dictionary<(int, int, int), int>();

        while (queue.Count > 0)
        {
            queue.Sort((a, b) => a.Item1.CompareTo(b.Item1));
            var (cost, x, y, prevDirection) = queue[0];
            queue.RemoveAt(0);

            if (x == _grid.Length - 1 && y == _grid[0].Length - 1) return cost;

            if (seen.Contains((x, y, prevDirection))) continue;

            seen.Add((x, y, prevDirection));

            for (var direction = 0; direction < 4; direction++)
            {
                var costIncrease = 0;

                if (direction == prevDirection || (direction + 2) % 4 == prevDirection) continue;

                for (var distance = 1; distance <= maxDistance; distance++)
                {
                    var newX = x + Directions[direction][0] * distance;
                    var newY = y + Directions[direction][1] * distance;

                    if (IsPositionInRange((newX, newY), _grid))
                    {
                        costIncrease += _grid[newX][newY];

                        if (distance < minDistance) continue;

                        var newCost = cost + costIncrease;

                        if (costs.TryGetValue((newX, newY, direction), out var existingCost) &&
                            existingCost <= newCost) continue;

                        costs[(newX, newY, direction)] = newCost;
                        queue.Add((newCost, newX, newY, direction));
                    }
                }
            }
        }

        return -1;
    }

    public static void PartOne(bool isTest, string[] input)
    {
        _grid = ConvertToIntegerArrays(input);

        var result = FindShortestPath(1, 3);

        InputOutputHelper.WriteOutput(isTest, result);
    }

    public static void PartTwo(bool isTest, string[] input)
    {
        _grid = ConvertToIntegerArrays(input);

        var result = FindShortestPath(4, 10);

        InputOutputHelper.WriteOutput(isTest, result);
    }

    private static int[][] ConvertToIntegerArrays(string[] stringArray)
    {
        var rows = stringArray.Length;
        var cols = stringArray[0].Length;

        var intArray = new int[rows][];

        for (var i = 0; i < rows; i++)
        {
            intArray[i] = new int[cols];
            for (var j = 0; j < cols; j++) intArray[i][j] = stringArray[i][j] - '0';
        }

        return intArray;
    }
}