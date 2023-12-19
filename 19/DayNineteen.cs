namespace AdventOfCode2023;

public static class DayNineteen
{
    private static readonly Dictionary<string, List<string>> _flows = new();

    public static void Solution()
    {
        var testInput = InputOutputHelper.GetInput(true, "19");
        PartOne(true, testInput);

        var input = InputOutputHelper.GetInput(false, "19");
        PartOne(false, input);

        PartTwo(true, testInput);
        PartTwo(false, input);
    }

    public static void PartOne(bool isTest, string[] input)
    {
        var result = 0;
        var firstPart = new List<string>();
        var secondPart = new List<string>();
        var isFirstPart = true;

        foreach (var line in input)
            if (line == "")
            {
                isFirstPart = false;
            }
            else
            {
                if (isFirstPart)
                    firstPart.Add(line);
                else
                    secondPart.Add(line);
            }

        var parsedOne = ParseFirstPart(firstPart.ToArray());
        var parsedTwo = ParseSecondPart(secondPart.ToArray());

        foreach ((int, int, int, int) tuple in parsedTwo) result += GetWorkflowAmount(tuple, parsedOne);

        InputOutputHelper.WriteOutput(isTest, result);
    }

    private static List<(int x, int m, int a, int s)> ParseSecondPart(string[] secondPart)
    {
        List<(int x, int m, int a, int s)> parsed = new();

        foreach (var line in secondPart)
        {
            var parts = line.TrimStart('{').TrimEnd('}').Split(',');
            parsed.Add((int.Parse(parts[0].Substring(2)), int.Parse(parts[1].Substring(2)),
                int.Parse(parts[2].Substring(2)), int.Parse(parts[3].Substring(2))));
        }

        return parsed;
    }

    private static Dictionary<string, (List<(string, int, string)>, string)> ParseFirstPart(string[] firstPart)
    {
        Dictionary<string, (List<(string, int, string)>, string)> parsed = new();

        foreach (var line in firstPart)
        {
            var key = line.Split('{')[0];
            List<(string, int, string)> values = new();
            var lastString = "";

            foreach (var listItem in line.Split('{')[1].TrimEnd('}').Split(','))
            {
                if (!listItem.Contains(':'))
                {
                    lastString = listItem;
                    break;
                }

                values.Add((listItem.Substring(0, 2), int.Parse(listItem.Substring(2).Split(':')[0]),
                    listItem.Split(':')[1]));
            }

            parsed.Add(key, (values, lastString));
        }

        return parsed;
    }

    private static int GetWorkflowAmount((int x, int m, int a, int s) workflow,
        Dictionary<string, (List<(string, int, string)>, string)> map)
    {
        var current = "in";
        var amount = 0;

        while (!(current is "A" or "R"))
        {
            (List<(string, int, string)> values, string defaultKey) value = map[current];
            var newKey = value.defaultKey;

            foreach (var (operation, valueToOperate, key) in value.values)
            {
                var isRight = false;

                switch (operation[0])
                {
                    case 'x':
                        isRight = IsLikeThat(operation[1], workflow.x, valueToOperate);
                        break;

                    case 'm':
                        isRight = IsLikeThat(operation[1], workflow.m, valueToOperate);
                        break;

                    case 'a':
                        isRight = IsLikeThat(operation[1], workflow.a, valueToOperate);
                        break;

                    case 's':
                        isRight = IsLikeThat(operation[1], workflow.s, valueToOperate);
                        break;
                }

                if (isRight)
                {
                    newKey = key;
                    break;
                }
            }

            current = newKey;
        }

        amount = workflow.s + workflow.m + workflow.a + workflow.x;

        if (current is "A")
            return amount;
        return 0;
    }

    private static bool IsLikeThat(char operand, int value1, int value2)
    {
        if (operand == '<')
            return value1 < value2;
        return value1 > value2;
    }

    public static void PartTwo(bool isTest, string[] input)
    {
        var firstPart = new List<string>();
        var isFirstPart = true;

        foreach (var line in input)
            if (line == "")
            {
                isFirstPart = false;
            }
            else
            {
                if (isFirstPart) firstPart.Add(line);
            }

        foreach (var i in firstPart)
        {
            var keyValue = i.Split("{");
            var k = keyValue[0].Trim();
            var v = keyValue[1].Substring(0, keyValue[1].Length - 1).Split(",");
            _flows[k] = new List<string>(v);
        }

        var dict = new Dictionary<string, int[]>
        {
            { "x", new[] { 1, 4000 } },
            { "m", new[] { 1, 4000 } },
            { "a", new[] { 1, 4000 } },
            { "s", new[] { 1, 4000 } }
        };

        var result = Run(dict, "in");

        InputOutputHelper.WriteOutput(isTest, result);
    }

    private static ulong GetResult(Dictionary<string, int[]> dict)
    {
        ulong result = 1;
        foreach (var item in dict.Values) result *= (ulong)(item[1] - item[0] + 1);
        return result;
    }

    private static ulong Run(Dictionary<string, int[]> dict, string flow)
    {
        ulong result = 0;
        foreach (var curFlow in _flows[flow])
            if (curFlow.Contains(":"))
            {
                var parts = curFlow.Split(":");
                var operation = parts[0].Trim();
                var key = parts[1].Trim();

                if (operation.Contains("<"))
                {
                    var partsOperatorSplit = operation.Split("<");
                    var xmas = partsOperatorSplit[0].Trim();
                    var number = int.Parse(partsOperatorSplit[1].Trim());

                    var newDict = Copy(dict);
                    if (newDict[xmas][0] < number)
                    {
                        newDict[xmas][1] = Math.Min(newDict[xmas][1], number - 1);
                        if (key is "A")
                            result += GetResult(newDict);
                        else if (key is not "R")
                            result += Run(newDict, key);
                        dict[xmas][0] = Math.Max(dict[xmas][0], number);
                    }
                }
                else if (operation.Contains(">"))
                {
                    var partsOperatorSplit = operation.Split(">");
                    var xmas = partsOperatorSplit[0].Trim();
                    var number = int.Parse(partsOperatorSplit[1].Trim());

                    var newDict = Copy(dict);
                    if (newDict[xmas][1] > number)
                    {
                        newDict[xmas][0] = Math.Max(newDict[xmas][0], number + 1);
                        if (key is "A")
                            result += GetResult(newDict);
                        else if (key is not "R")
                            result += Run(newDict, key);
                        dict[xmas][1] = Math.Min(dict[xmas][1], number);
                    }
                }
            }
            else
            {
                if (curFlow == "A")
                    result += GetResult(dict);
                else if (curFlow != "R")
                    result += Run(dict, curFlow);
            }

        return result;
    }

    private static Dictionary<string, int[]> Copy(Dictionary<string, int[]> from)
    {
        var to = new Dictionary<string, int[]>();
        foreach (var item in from) to[item.Key] = new[] { item.Value[0], item.Value[1] };
        return to;
    }
}