namespace AdventOfCode2023;

internal class DayFour
{
    public static void Solution()
    {
        var testInput = InputOutputHelper.GetInput(true, "04");
        PartOne(true, testInput);

        var input = InputOutputHelper.GetInput(false, "04");
        PartOne(false, input);

        PartTwo(true, testInput);
        PartTwo(false, input);
    }

    public static void PartOne(bool isTest, string[] input)
    {
        var result = 0;

        foreach (var line in input)
        {
            var parts = line.Split(':');

            for (var i = 0; i < parts.Length; i++) parts[i] = parts[i].Replace("  ", " ");

            var cardNumber = int.Parse(parts[0].Substring(5).Trim());

            var winningStr = parts[1].Split('|')[0].Trim().Split(' ');
            var haveStr = parts[1].Split('|')[1].Trim().Split(' ');

            var winning = Array.ConvertAll(winningStr, int.Parse);
            var have = Array.ConvertAll(haveStr, int.Parse);

            var score = 0;

            for (var i = 0; i < winningStr.Length; i++)
            for (var j = 0; j < haveStr.Length; j++)
                if (winning[i] == have[j])
                    score = score == 0 ? 1 : score * 2;

            result += score;
        }

        InputOutputHelper.WriteOutput(isTest, result);
    }

    public static void PartTwo(bool isTest, string[] input)
    {
        var result = 0;
        int NUM_WIN, NUM_HAVE, NUM_CARDS;

        if (isTest)
        {
            NUM_WIN = 5;
            NUM_HAVE = 8;
            NUM_CARDS = 6;
        }
        else
        {
            NUM_WIN = 10;
            NUM_HAVE = 25;
            NUM_CARDS = 202;
        }

        var copies = new int[NUM_CARDS];

        for (var i = 0; i < NUM_CARDS; i++) copies[i] = 1;

        foreach (var line in input)
        {
            var parts = line.Split(':');

            for (var i = 0; i < parts.Length; i++) parts[i] = parts[i].Replace("  ", " ");

            var cardNumber = int.Parse(parts[0].Substring(5).Trim()) - 1;

            var winningStr = parts[1].Split('|')[0].Trim().Split(' ');
            var haveStr = parts[1].Split('|')[1].Trim().Split(' ');

            var winning = Array.ConvertAll(winningStr, int.Parse);
            var have = Array.ConvertAll(haveStr, int.Parse);

            var score = 0;

            for (var i = 0; i < NUM_WIN; i++)
            for (var j = 0; j < NUM_HAVE; j++)
                if (winning[i] == have[j])
                    score++;

            for (var i = 1; i <= score; i++) copies[cardNumber + i] += copies[cardNumber];

            result += copies[cardNumber];
        }

        InputOutputHelper.WriteOutput(isTest, result);
    }
}