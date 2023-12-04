namespace AdventOfCode2023;

internal class DayTwo
{
    public static void Solution()
    {
        var testInput = InputOutputHelper.GetInput(true, "02");
        PartOne(true, testInput);

        var input = InputOutputHelper.GetInput(false, "02");
        PartOne(false, input);

        PartTwo(true, testInput);
        PartTwo(false, input);
    }

    public static void PartOne(bool isTest, string[] input)
    {
        var result = 0;
        var id = 1;

        foreach (var line in input)
        {
            var splitted = line.Split(": ")[1].Split("; ");
            var revealed = new Dictionary<string, int>();
            revealed.Add("blue", 0);
            revealed.Add("red", 0);
            revealed.Add("green", 0);

            for (var i = 0; i < splitted.Length; i++)
            {
                var current = new Dictionary<string, int>();
                current.Add("blue", 0);
                current.Add("red", 0);
                current.Add("green", 0);
                var currentRevealed = splitted[i].Split(", ");

                foreach (var rev in currentRevealed)
                    if (!rev.StartsWith("Game"))
                    {
                        var color = rev.Split(' ')[1];
                        var amount = int.Parse(rev.Split(' ')[0]);
                        current[color] = amount;
                    }

                revealed["blue"] = Math.Max(revealed["blue"], current["blue"]);
                revealed["red"] = Math.Max(revealed["red"], current["red"]);
                revealed["green"] = Math.Max(revealed["green"], current["green"]);
            }

            if (revealed["blue"] <= 14 && revealed["green"] <= 13 && revealed["red"] <= 12) result += id;

            id++;
        }

        InputOutputHelper.WriteOutput(isTest, result);
    }

    public static void PartTwo(bool isTest, string[] input)
    {
        var result = 0;
        var id = 1;

        foreach (var line in input)
        {
            var splitted = line.Split(": ")[1].Split("; ");
            var revealed = new Dictionary<string, int>();
            revealed.Add("blue", 0);
            revealed.Add("red", 0);
            revealed.Add("green", 0);

            for (var i = 0; i < splitted.Length; i++)
            {
                var current = new Dictionary<string, int>();
                current.Add("blue", 0);
                current.Add("red", 0);
                current.Add("green", 0);
                var currentRevealed = splitted[i].Split(", ");

                foreach (var rev in currentRevealed)
                    if (!rev.StartsWith("Game"))
                    {
                        var color = rev.Split(' ')[1];
                        var amount = int.Parse(rev.Split(' ')[0]);
                        current[color] = amount;
                    }

                revealed["blue"] = Math.Max(revealed["blue"], current["blue"]);
                revealed["red"] = Math.Max(revealed["red"], current["red"]);
                revealed["green"] = Math.Max(revealed["green"], current["green"]);
            }

            result += revealed["blue"] * revealed["red"] * revealed["green"];
        }


        InputOutputHelper.WriteOutput(isTest, result);
    }
}