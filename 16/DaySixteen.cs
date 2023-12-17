namespace AdventOfCode2023;

internal class DaySixteen
{
    private static readonly Dictionary<char, (int, int)> directions = new()
    {
        { 'U', (-1, 0) },
        { 'D', (1, 0) },
        { 'R', (0, 1) },
        { 'L', (0, -1) }
    };

    private static readonly Dictionary<char, Dictionary<char, char>> reflections = new()
    {
        { 'R', new Dictionary<char, char> { { '/', 'U' }, { '\\', 'D' } } },
        { 'L', new Dictionary<char, char> { { '/', 'D' }, { '\\', 'U' } } },
        { 'U', new Dictionary<char, char> { { '/', 'R' }, { '\\', 'L' } } },
        { 'D', new Dictionary<char, char> { { '/', 'L' }, { '\\', 'R' } } }
    };

    public static void Solution()
    {
        var testInput = InputOutputHelper.GetInput(true, "16");
        PartOne(true, testInput);

        var input = InputOutputHelper.GetInput(false, "16");
        PartOne(false, input);

        PartTwo(true, testInput);
        PartTwo(false, input);
    }


    public static void PartOne(bool isTest, string[] input)
    {
        var grid = new Grid<char>(input);

        var result = Energized((0, -1, 'R'), grid);

        InputOutputHelper.WriteOutput(isTest, result);
    }

    private static List<char> BeamDeflect((int, int, char) beam, char ch)
    {
        var beams = new List<char>();

        if (ch == '-')
        {
            beams.Add(beam.Item3 == 'R' || beam.Item3 == 'L' ? beam.Item3 : 'R');
            beams.Add(beam.Item3 == 'R' || beam.Item3 == 'L' ? beam.Item3 : 'L');
        }
        else if (ch == '|')
        {
            beams.Add(beam.Item3 == 'U' || beam.Item3 == 'D' ? beam.Item3 : 'U');
            beams.Add(beam.Item3 == 'U' || beam.Item3 == 'D' ? beam.Item3 : 'D');
        }
        else
        {
            var deflection = reflections[beam.Item3][ch];
            beams.Add(deflection);
        }

        return beams;
    }

    public static void PartTwo(bool isTest, string[] input)
    {
        var grid = new Grid<char>(input);

        var result = 0;

        for (var j = 0; j < grid.HorizontalLength; j++)
        {
            result = Math.Max(Energized((-1, j, 'D'), grid), result);
            result = Math.Max(Energized((grid.VerticalLength, j, 'U'), grid), result);
        }

        for (var i = 0; i < grid.VerticalLength; i++)
        {
            result = Math.Max(Energized((i, -1, 'R'), grid), result);
            result = Math.Max(Energized((i, grid.HorizontalLength, 'L'), grid), result);
        }

        InputOutputHelper.WriteOutput(isTest, result);
    }

    private static int Energized((int, int, char) initial, Grid<char> grid)
    {
        var beams = new List<(int, int, char)> { initial };
        var row = grid.VerticalLength;
        var col = grid.HorizontalLength;

        var energized = new HashSet<(int, int)>();
        var seen = new HashSet<(int, int, char)>();

        while (beams.Count > 0)
        {
            var beam = beams[^1];
            beams.RemoveAt(beams.Count - 1);
            var i = beam.Item1 + directions[beam.Item3].Item1;
            var j = beam.Item2 + directions[beam.Item3].Item2;

            if (i >= row || j >= col || i < 0 || j < 0 || seen.Contains(beam)) continue;

            seen.Add(beam);
            energized.Add((i, j));
            if (grid.Array[i, j] == '.')
            {
                beam = (i, j, beam.Item3);
                beams.Add(beam);
                continue;
            }

            foreach (var d in BeamDeflect(beam, grid.Array[i, j]))
            {
                beam = (i, j, d);
                beams.Add(beam);
            }
        }

        return energized.Count;
    }
}