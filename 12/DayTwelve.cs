namespace AdventOfCode2023;

internal class DayTwelve
{
    public static void Solution()
    {
        var testInput = InputOutputHelper.GetInput(true, "12");
        PartOne(true, testInput);

        var input = InputOutputHelper.GetInput(false, "12");
        PartOne(false, input);

        PartTwo(true, testInput);
        PartTwo(false, input);
    }

    public static void PartOne(bool isTest, string[] input)
    {
        var totalCombinations = 0L;

        foreach (var row in input)
        {
            var plantCounts = Array.ConvertAll(row.Split(' ')[1].Split(','), int.Parse);
            totalCombinations += CountCombinations(new Dictionary<(int, long, int), long>(), row.Split(' ')[0] + ".",
                plantCounts, 0, 0, 0);
        }

        InputOutputHelper.WriteOutput(isTest, totalCombinations);
    }

    public static void PartTwo(bool isTest, string[] input)
    {
        var totalCombinations = 0L;

        foreach (var row in input)
        {
            var plantCounts = Array.ConvertAll(row.Split(' ')[1].Split(','), int.Parse);
            plantCounts = ExtendArray(plantCounts, 5);

            var modifiedLine = new string(ExtendArray((row.Split(' ')[0] + "?").ToCharArray(), 4).ToArray()) +
                               row.Split(' ')[0] + ".";
            totalCombinations += CountCombinations(new Dictionary<(int, long, int), long>(), modifiedLine, plantCounts,
                0, 0, 0);
        }

        InputOutputHelper.WriteOutput(isTest, totalCombinations);
    }

    private static T[] ExtendArray<T>(T[] array, int factor)
    {
        var result = new T[array.Length * factor];
        for (var i = 0; i < factor; i++) Array.Copy(array, 0, result, i * array.Length, array.Length);
        return result;
    }

    private static long CountCombinations(Dictionary<(int, long, int), long> memory, string line, int[] plantCounts,
        int pos, long currentCount, int countPos)
    {
        var memoryKey = (pos, currentCount, countPos);

        if (memory.ContainsKey(memoryKey))
            return memory[memoryKey];

        long combinations;
        if (pos == line.Length)
        {
            combinations = plantCounts.Length == countPos ? 1 : 0;
        }
        else if (line[pos] == '#')
        {
            combinations = CountCombinations(memory, line, plantCounts, pos + 1, currentCount + 1, countPos);
        }
        else if (line[pos] == '.' || countPos == plantCounts.Length)
        {
            if (countPos < plantCounts.Length && currentCount == plantCounts[countPos])
                combinations = CountCombinations(memory, line, plantCounts, pos + 1, 0, countPos + 1);
            else if (currentCount == 0)
                combinations = CountCombinations(memory, line, plantCounts, pos + 1, 0, countPos);
            else
                combinations = 0;
        }
        else
        {
            var hashCombinations = CountCombinations(memory, line, plantCounts, pos + 1, currentCount + 1, countPos);
            long dotCombinations = 0;

            if (currentCount == plantCounts[countPos])
                dotCombinations = CountCombinations(memory, line, plantCounts, pos + 1, 0, countPos + 1);
            else if (currentCount == 0)
                dotCombinations = CountCombinations(memory, line, plantCounts, pos + 1, 0, countPos);

            combinations = hashCombinations + dotCombinations;
        }

        memory[memoryKey] = combinations;
        return combinations;
    }
}