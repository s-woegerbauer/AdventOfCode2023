namespace AdventOfCode2023
{
    internal class DayTwo
    {
        public static void Solution()
        {
            string[] testInput = InputOutputHelper.GetInput(true, "02");
            PartOne(true, testInput);

            string[] input = InputOutputHelper.GetInput(false, "02");
            PartOne(false, input);

            //PartTwo(true, testInput);
            //PartTwo(false, input);
        }

        public static void PartOne(bool isTest, string[] input)
        {
            int result = 0;
            int id = 1;

            foreach (string line in input)
            {
                string[] splitted = line.Split(": ")[1].Split("; ");
                Dictionary<string, int> revealed = new Dictionary<string, int>();
                revealed.Add("blue", 0);
                revealed.Add("red", 0);
                revealed.Add("green", 0);

                for (int i = 0; i < splitted.Length; i++)
                {
                    Dictionary<string, int> current = new Dictionary<string, int>();
                    current.Add("blue", 0);
                    current.Add("red", 0);
                    current.Add("green", 0);
                    string[] currentRevealed = splitted[i].Split(", ");

                    foreach (string rev in currentRevealed)
                    {
                        if (!rev.StartsWith("Game"))
                        {
                            string color = rev.Split(' ')[1];
                            int amount = int.Parse(rev.Split(' ')[0]);
                            current[color] = amount;
                        }
                    }

                    revealed["blue"] = Math.Max(revealed["blue"], current["blue"]);
                    revealed["red"] = Math.Max(revealed["red"], current["red"]);
                    revealed["green"] = Math.Max(revealed["green"], current["green"]);
                }

                result += revealed["blue"] * revealed["red"] * revealed["green"];
            }

            InputOutputHelper.WriteOutput<int>(isTest, result);
        }

        public static void PartTwo(bool isTest, string[] input)
        {
            int result = 0;


            InputOutputHelper.WriteOutput<int>(isTest, result);
        }
    }
}