namespace AdventOfCode2023;

internal class DayFifteen
{
    public static void Solution()
    {
        var testInput = InputOutputHelper.GetInput(true, "15");
        PartOne(true, testInput);

        var input = InputOutputHelper.GetInput(false, "15");
        PartOne(false, input);

        PartTwo(true, testInput);
        PartTwo(false, input);
    }

    public static void PartOne(bool isTest, string[] input)
    {
        long total = 0;

        foreach (var line in input[0].Split(',')) total += GetHash(line);

        InputOutputHelper.WriteOutput(isTest, total);
    }

    public static int GetHash(string input)
    {
        var result = 0;
        foreach (var c in input)
        {
            result += c;
            result *= 17;
            result %= 256;
        }

        return result;
    }

    public static void PartTwo(bool isTest, string[] input)
    {
        var hashMap = InitializeHashMap();

        ProcessInputString(input[0], hashMap);

        var answer = CalculateAnswer(hashMap);

        InputOutputHelper.WriteOutput(isTest, answer);
    }

    private static List<List<(string, int)>> InitializeHashMap()
    {
        var hashMap = new List<List<(string, int)>>();

        for (var i = 0; i < 256; i++) hashMap.Add(new List<(string, int)>());

        return hashMap;
    }

    private static void ProcessInputString(string input, List<List<(string, int)>> hashMap)
    {
        var parts = input.Split(',');

        foreach (var part in parts)
            if (part.Contains('='))
                ProcessEqualsPart(part, hashMap);
            else if (part.Contains('-')) ProcessMinusPart(part, hashMap);
    }

    private static void ProcessEqualsPart(string part, List<List<(string, int)>> hashMap)
    {
        var label = part.Substring(0, part.IndexOf('='));
        var focal = int.Parse(part.Substring(part.IndexOf('=') + 1));

        var boxIdx = GetHash(label);
        var box = hashMap[boxIdx];

        var replaced = TryReplaceLabel(box, label, focal);

        if (!replaced) box.Add((label, focal));
    }

    private static bool TryReplaceLabel(List<(string, int)> box, string label, int focal)
    {
        var slot = 0;

        foreach (var tuple in box)
        {
            if (tuple.Item1 == label)
            {
                box[slot] = (tuple.Item1, focal);
                return true;
            }

            slot++;
        }

        return false;
    }

    private static void ProcessMinusPart(string part, List<List<(string, int)>> hashMap)
    {
        var label = part.Substring(0, part.IndexOf('-'));

        var boxIdx = GetHash(label);
        var box = hashMap[boxIdx];

        RemoveLabel(box, label);
    }

    private static void RemoveLabel(List<(string, int)> box, string label)
    {
        for (var i = 0; i < box.Count; i++)
            if (box[i].Item1 == label)
            {
                box.RemoveAt(i);
                break;
            }
    }

    private static int CalculateAnswer(List<List<(string, int)>> hashMap)
    {
        var answer = 0;

        for (var boxIdx = 0; boxIdx < hashMap.Count; boxIdx++)
        {
            var box = hashMap[boxIdx];

            for (var idx = 0; idx < box.Count; idx++)
            {
                var tuple = box[idx];
                answer += (boxIdx + 1) * (idx + 1) * tuple.Item2;
            }
        }

        return answer;
    }
}