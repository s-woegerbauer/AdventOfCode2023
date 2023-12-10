namespace AdventOfCode2023;

internal class DayEight
{
    public static void Solution()
    {
        var testInput = InputOutputHelper.GetInput(true, "08");
        //PartOne(true, testInput);

        var input = InputOutputHelper.GetInput(false, "08");
        //PartOne(false, input);

        PartTwo(true, testInput);
        PartTwo(false, input);
    }

    public static void PartOne(bool isTest, string[] input)
    {
        var result = 0;
        var dict = ParseInput(input, out var moves);

        result = Steps(dict, moves);

        InputOutputHelper.WriteOutput(isTest, result);
    }

    public static void PartTwo(bool isTest, string[] input)
    {
        long result = 0;
        var dict = ParseInput(input, out var moves);

        result = dict.Keys.Where(x => x[2] == 'A')
            .Select(x => StepsTwo(x, dict, moves))
            .SelectMany(x => Factor(x).Where(w => w != x).Select(x => (long)x))
            .Distinct()
            .Aggregate((x, y) => x * y);

        InputOutputHelper.WriteOutput(isTest, result);
    }

    private static List<int> Factor(int number)
    {
        var factors = new List<int>();
        var max = (int)Math.Sqrt(number);

        for (var factor = 1; factor <= max; ++factor)
            if (number % factor == 0)
            {
                factors.Add(factor);
                if (factor != number / factor)
                    factors.Add(number / factor);
            }

        return factors;
    }

    public static int StepsTwo(string startPos, Dictionary<string, (string, string)> dict, List<char> moves)
    {
        var currentPos = startPos;
        var count = 0;

        while (!currentPos.Contains("Z"))
        {
            var move = moves[count % moves.Count];

            if (move == 'R')
                currentPos = dict[currentPos].Item2;
            else
                currentPos = dict[currentPos].Item1;

            count++;
        }

        return count;
    }

    public static int Steps(Dictionary<string, (string, string)> dict, List<char> moves)
    {
        var currentPos = "AAA";
        var count = 0;

        while (currentPos != "ZZZ")
        {
            var move = moves[count % moves.Count];

            if (move == 'R')
                currentPos = dict[currentPos].Item2;
            else
                currentPos = dict[currentPos].Item1;

            count++;
        }

        return count;
    }

    public static Dictionary<string, (string, string)> ParseInput(string[] input, out List<char> moves)
    {
        moves = input[0].ToList();
        Dictionary<string, (string, string)> output = new();

        foreach (var line in input.Skip(2))
        {
            var currentLine = line;
            currentLine = currentLine.Replace("(", "");
            currentLine = currentLine.Replace(")", "");
            var key = currentLine.Split(" = ")[0];
            var val1 = currentLine.Split(" = ")[1].Split(", ")[0];
            var val2 = currentLine.Split(" = ")[1].Split(", ")[1];

            output.Add(key, (val1, val2));
        }

        return output;
    }
}