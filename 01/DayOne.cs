namespace AdventOfCode2023;

internal class DayOne
{
    public static Dictionary<string, int> letters = new();

    public static void Solution()
    {
        letters.Add("one", 1);
        letters.Add("two", 2);
        letters.Add("three", 3);
        letters.Add("four", 4);
        letters.Add("five", 5);
        letters.Add("six", 6);
        letters.Add("seven", 7);
        letters.Add("eight", 8);
        letters.Add("nine", 9);
        letters.Add("1", 1);
        letters.Add("2", 2);
        letters.Add("3", 3);
        letters.Add("4", 4);
        letters.Add("5", 5);
        letters.Add("6", 6);
        letters.Add("7", 7);
        letters.Add("8", 8);
        letters.Add("9", 9);

        var testInput = InputOutputHelper.GetInput(true, "01");
        //PartOne(true, testInput);

        var input = InputOutputHelper.GetInput(false, "01");
        //PartOne(false, input);

        PartTwo(true, testInput);
        PartTwo(false, input);
    }

    public static void PartOne(bool isTest, string[] input)
    {
        var result = 0;

        foreach (var line in input)
        {
            //int value = int.Parse(line);
            var firstChar = ' ';
            var secondChar = ' ';

            foreach (var c in line)
                if (char.IsDigit(c))
                {
                    if (firstChar == ' ')
                    {
                        firstChar = c;
                        secondChar = c;
                    }
                    else
                    {
                        secondChar = c;
                    }
                }

            result += int.Parse(firstChar + secondChar.ToString());
        }

        InputOutputHelper.WriteOutput(isTest, result);
    }

    public static void PartTwo(bool isTest, string[] input)
    {
        var result = 0;

        foreach (var line in input)
        {
            var currentLine = line;
            var firstChar = ' ';
            var secondChar = ' ';
            var index = 0;

            foreach (var c in currentLine)
            {
                var number = IsNumber(currentLine);
                currentLine = line.Substring(index + 1);

                if (number != -1)
                {
                    if (firstChar == ' ')
                    {
                        firstChar = char.Parse(number.ToString());
                        secondChar = c;
                    }
                    else
                    {
                        secondChar = char.Parse(number.ToString());
                    }
                }

                index++;
            }


            result += int.Parse(firstChar + secondChar.ToString());
        }

        InputOutputHelper.WriteOutput(isTest, result);
    }

    public static int IsNumber(string substring)
    {
        var returner = -1;

        for (var i = 0; i < substring.Length; i++)
        for (var j = substring.Length; j >= 0; j--)
            if (i < j)
                if (letters.ContainsKey(substring.Substring(i, j - i)))
                {
                    returner = letters[substring.Substring(i, Math.Abs(j - i))];
                    return returner;
                }

        return returner;
    }
}