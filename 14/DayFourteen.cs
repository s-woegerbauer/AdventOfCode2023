using System.Text;

namespace AdventOfCode2023;

internal class DayFourteen
{
    public static void Solution()
    {
        var testInput = InputOutputHelper.GetInput(true, "14");
        PartOne(true, testInput);

        var input = InputOutputHelper.GetInput(false, "14");
        PartOne(false, input);

        PartTwo(true, testInput);
        PartTwo(false, input);
    }

    public static void PartOne(bool isTest, string[] input)
    {
        var result = 0;

        var grid = new Grid<char>(input);

        for (var row = 0; row < grid.VerticalLength; row++)
        for (var col = 0; col < grid.HorizontalLength; col++)
            if (grid.Array[row, col] == 'O')
                result += MoveOneRock(col, row, grid);

        InputOutputHelper.WriteOutput(isTest, result);
    }

    public static int MoveOneRock(int x, int y, Grid<char> grid)
    {
        var count = 0;

        while (y > 0 && grid.Array[y - 1, x] != '#' && grid.Array[y - 1, x] != 'O')
        {
            y--;

            grid.Array[y + 1, x] = '.';
            grid.Array[y, x] = 'O';
        }

        count = grid.VerticalLength - y;

        return count;
    }
    
    private static Grid<char> Rotate(Grid<char> grid)
    {
        var verticalLength = grid.VerticalLength;
        var horizontalLength = grid.HorizontalLength;

        var newGrid = new Grid<char>(grid.HorizontalLength, grid.VerticalLength);

        for (var y = 0; y < verticalLength; y++)
        for (var x = 0; x < horizontalLength; x++)
            newGrid.Array[x, verticalLength - y - 1] = grid.Array[y, x];

        return newGrid;
    }

    private static void Slide(Grid<char> grid)
    {
        var verticalLength = grid.VerticalLength;
        var horizontalLength = grid.HorizontalLength;

        for (var j = 0; j < horizontalLength; j++)
        {
            var count = 0;

            for (var i = 0; i < verticalLength; i++)
            {
                if (grid.Array[i, j] == '#') count = i + 1;

                if (grid.Array[i, j] == 'O')
                {
                    grid.Array[i, j] = '.';
                    grid.Array[count, j] = 'O';
                    count++;
                }
            }
        }
    }

    public static void PartTwo(bool isTest, string[] input)
    {
        var grid = new Grid<char>(input);

        var dictionary = new Dictionary<string, Tuple<int, int>>();
        const int CYCLES = 1000000000;
        var index = 1;

        while (true)
        {
            for (var j = 0; j < 4; j++)
            {
                Slide(grid);
                grid = Rotate(grid);
            }

            var gridString = GridToString(grid.Array);

            if (dictionary.TryGetValue(gridString, out var value))
            {
                var cycleLength = index - value.Item1;

                foreach (var pair in dictionary.Values)
                    if (pair.Item1 >= value.Item1 && pair.Item1 % cycleLength == CYCLES % cycleLength)
                        InputOutputHelper.WriteOutput(isTest, pair.Item2);

                break;
            }

            dictionary[gridString] = new Tuple<int, int>(index, GetScore(grid));
            index++;
        }
    }

    private static int GetScore(Grid<char> grid)
    {
        var n = grid.VerticalLength;
        var ans = 0;

        for (var i = 0; i < n; i++) ans += (n - i) * CountChar(grid.Array, i, 'O');

        return ans;
    }

    private static int CountChar(char[,] grid, int row, char target)
    {
        var count = 0;
        var col = grid.GetLength(1);

        for (var j = 0; j < col; j++)
            if (grid[row, j] == target)
                count++;

        return count;
    }

    private static string GridToString(char[,] grid)
    {
        var rows = grid.GetLength(0);
        var cols = grid.GetLength(1);

        var builder = new StringBuilder();

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++) builder.Append(grid[i, j]);
            builder.AppendLine();
        }

        return builder.ToString();
    }
}