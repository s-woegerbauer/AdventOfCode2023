namespace AdventOfCode2023;

internal class DayNine
{
    public static void Solution()
    {
        var testInput = InputOutputHelper.GetInput(true, "09");
        PartOne(true, testInput);

        string[] input = InputOutputHelper.GetInput(false, "09");
        PartOne(false, input);
        
        PartTwo(true, testInput);
        PartTwo(false, input);
    }

    public static void PartOne(bool isTest, string[] input)
    {
        var result = 0;

        foreach (string line in input)
        {
            List<List<int>> sequenceList = new List<List<int>>();
            List<int> currentSequence = new List<int>();

            foreach (string split in line.Split(' '))
            {
                currentSequence.Add(int.Parse(split));
            }
            
            sequenceList.Add(currentSequence);

            while (currentSequence.Any(x => x != 0))
            {
                List<int> newSequence = new List<int>();
                for (int i = 0; i < currentSequence.Count - 1; i++)
                {
                    newSequence.Add(currentSequence[i + 1] - currentSequence[i]);
                }

                currentSequence = newSequence;
                sequenceList.Add(newSequence);
            }

            for (int i = sequenceList.Count - 1; i >= 0; i--)
            {
                if (i == sequenceList.Count - 1)
                {
                    sequenceList[i].Insert(0, 0);
                }
                else
                {
                    sequenceList[i].Insert(0, sequenceList[i][0] - sequenceList[i + 1][0]);
                }
            }
            
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