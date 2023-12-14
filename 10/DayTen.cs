namespace AdventOfCode2023;

internal class DayTen
{
    private const char StartSymbol = 'S';

    public static void Solution()
    {
        var testInput = InputOutputHelper.GetInput(true, "10");
        PartOne(true, testInput);

        var input = InputOutputHelper.GetInput(false, "10");
        PartOne(false, input);

        PartTwo(true, testInput);
        PartTwo(false, input);
    }

    private static (int, int) FindStartPosition(string[] grid)
    {
        for (var i = 0; i < grid.Length; i++)
        {
            var index = grid[i].IndexOf(StartSymbol);
            if (index != -1) return (i, index);
        }

        return (-1, -1);
    }

    private static List<int> GetValidDirections(string[] grid, int startX, int startY)
    {
        int[][] directions = { new[] { 0, 1 }, new[] { 1, 0 }, new[] { 0, -1 }, new[] { -1, 0 } };
        string[] happyChars = { "-7J", "|LJ", "-FL", "|F7" };

        var validDirections = new List<int>();
        for (var i = 0; i < 4; i++)
        {
            var position = directions[i];
            var nextX = startX + position[0];
            var nextY = startY + position[1];
            if (nextX >= 0 && nextX < grid.Length && nextY >= 0 && nextY < grid[0].Length &&
                happyChars[i].Contains(grid[nextX][nextY])) validDirections.Add(i);
        }

        return validDirections;
    }

    private static int TransformDirection(int currentDirection, char gridChar)
    {
        var directionTransformations = new Dictionary<(int, char), int>
        {
            { (0, '-'), 0 },
            { (0, '7'), 1 },
            { (0, 'J'), 3 },
            { (2, '-'), 2 },
            { (2, 'F'), 1 },
            { (2, 'L'), 3 },
            { (1, '|'), 1 },
            { (1, 'L'), 0 },
            { (1, 'J'), 2 },
            { (3, '|'), 3 },
            { (3, 'F'), 0 },
            { (3, '7'), 2 }
        };

        return directionTransformations[(currentDirection, gridChar)];
    }

    private static void CalculatePathLength(string[] grid, int startX, int startY, List<int> validDirections,
        bool isTest)
    {
        int[][] directions = { new[] { 0, 1 }, new[] { 1, 0 }, new[] { 0, -1 }, new[] { -1, 0 } };

        var currentDirection = validDirections[0];
        var currentX = startX + directions[currentDirection][0];
        var currentY = startY + directions[currentDirection][1];
        var pathLength = 1;

        while ((currentX, currentY) != (startX, startY))
        {
            pathLength++;
            currentDirection = TransformDirection(currentDirection, grid[currentX][currentY]);
            currentX += directions[currentDirection][0];
            currentY += directions[currentDirection][1];
        }

        OutputResult(isTest, pathLength / 2);
    }

    private static void OutputResult(bool isTest, int result)
    {
        InputOutputHelper.WriteOutput(isTest, result);
    }

    public static void PartOne(bool isTest, string[] grid)
    {
        var (startX, startY) = FindStartPosition(grid);
        var validDirections = GetValidDirections(grid, startX, startY);
        CalculatePathLength(grid, startX, startY, validDirections, isTest);
    }

    public static void PartTwo(bool isTest, string[] grid)
    {
        var (startX, startY) = FindStartPosition(grid);
        var validDirections = GetValidDirections(grid, startX, startY);
        CalculateInnerPathCount(grid, startX, startY, validDirections, isTest);
    }

    private static void CalculateInnerPathCount(string[] grid, int startX, int startY, List<int> validDirections,
        bool isTest)
    {
        var rows = grid.Length;
        var columns = grid[0].Length;

        var outputGrid = new int[rows][];
        for (var i = 0; i < rows; i++) outputGrid[i] = new int[columns];

        int[][] directions = { new[] { 0, 1 }, new[] { 1, 0 }, new[] { 0, -1 }, new[] { -1, 0 } };

        var isValidDirection = validDirections.Contains(3);

        var currentDirection = validDirections[0];
        var currentX = startX + directions[currentDirection][0];
        var currentY = startY + directions[currentDirection][1];
        var pathLength = 1;
        outputGrid[startX][startY] = 1;

        while ((currentX, currentY) != (startX, startY))
        {
            outputGrid[currentX][currentY] = 1;
            pathLength++;
            currentDirection = TransformDirection(currentDirection, grid[currentX][currentY]);
            currentX += directions[currentDirection][0];
            currentY += directions[currentDirection][1];
        }

        var count = 0;

        for (var i = 0; i < rows; i++)
        {
            var inside = false;
            for (var j = 0; j < columns; j++)
            {
                var color = inside ? ConsoleColor.Green : ConsoleColor.White;

                Console.ForegroundColor = color;
                if (outputGrid[i][j] != 0)
                {
                    if ("|JL".Contains(grid[i][j]) || (grid[i][j] == StartSymbol && isValidDirection)) inside = !inside;
                    Console.Write("0");
                }
                else if (inside)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("I");
                    count++;
                }
                else
                {
                    Console.Write(".");
                }
            }

            Console.WriteLine();
        }

        OutputResult(isTest, count);
    }
}