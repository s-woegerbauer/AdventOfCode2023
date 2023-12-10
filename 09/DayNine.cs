namespace AdventOfCode2023;

internal class DayNine
{
    public static void Solution()
    {
        var testInput = InputOutputHelper.GetInput(true, "09");
        PartOne(true, testInput);

        var input = InputOutputHelper.GetInput(false, "09");
        PartOne(false, input);

        PartTwo(true, testInput);
        PartTwo(false, input);
    }

    public static void PartOne(bool isTest, string[] input)
    {
        var result = 0;

        foreach (var line in input)
        {
            var sequenceList = new List<List<int>>();
            var currentSequence = new List<int>();

            foreach (var split in line.Split(' ')) currentSequence.Add(int.Parse(split));

            sequenceList.Add(currentSequence);

            while (currentSequence.Any(x => x != 0))
            {
                var newSequence = new List<int>();
                for (var i = 0; i < currentSequence.Count - 1; i++)
                    newSequence.Add(currentSequence[i + 1] - currentSequence[i]);

                currentSequence = newSequence;
                sequenceList.Add(newSequence);
            }

            for (var i = sequenceList.Count - 1; i >= 0; i--)
                if (i == sequenceList.Count - 1)
                    sequenceList[i].Insert(0, 0);
                else
                    sequenceList[i].Insert(0, sequenceList[i][0] - sequenceList[i + 1][0]);

            result += sequenceList[0][0];
        }

        InputOutputHelper.WriteOutput(isTest, result);
    }

    public static void PartTwo(bool isTest, string[] input)
    {
        var result = 0;


        InputOutputHelper.WriteOutput(isTest, result);
    }
}