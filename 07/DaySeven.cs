namespace AdventOfCode2023;

internal class DaySeven
{
    public static void Solution()
    {
        var testInput = InputOutputHelper.GetInput(true, "07");
        PartOne(true, testInput);

        var input = InputOutputHelper.GetInput(false, "07");
        PartOne(false, input);

        PartTwo(true, testInput);
        PartTwo(false, input);
    }


    private static void PartOne(bool isTest, string[] input)
    {
        var result = Solve(true, input.ToList());

        InputOutputHelper.WriteOutput(isTest, result);
    }

    private static void PartTwo(bool isTest, string[] input)
    {
        var result = Solve(false, input.ToList());

        InputOutputHelper.WriteOutput(isTest, result);
    }

    public static long Solve(bool part1, List<string> inputLines)
    {
        long answer = 0;
        var hands = new List<Hand>();
        foreach (var line in inputLines) hands.Add(new Hand(line.Split(' ')[0], int.Parse(line.Split(' ')[1]), part1));

        hands.Sort();
        for (var i = 0; i < hands.Count; i++) answer += hands[i].Bid * (i + 1);

        return answer;
    }
}

public class Hand : IComparable
{
    public int Bid;
    public int Strength;
    public int[] Cards = new int[5];
    public int[] Frequences = new int[13];
    public string[] Ranks = { "A", "K", "Q", "J", "T", "9", "8", "7", "6", "5", "4", "3", "2" };

    public Hand(string line, int bid, bool part1)
    {
        if (!part1) Ranks = new[] { "A", "K", "Q", "T", "9", "8", "7", "6", "5", "4", "3", "2", "J" };
        Bid = bid;
        var numJokers = 0;
        var s = line.ToCharArray();
        for (var i = 0; i < s.Length; i++)
        for (var j = 0; j < Ranks.Length; j++)
            if (s[i].ToString() == Ranks[j])
            {
                if (!(s[i].ToString() == "J") || part1) Frequences[j]++;
                if (s[i].ToString() == "J" && !part1) numJokers++;
                Cards[i] = j;
            }

        Array.Sort(Frequences);
        Frequences[Frequences.Length - 1] += numJokers;
        Strength = 2 * Frequences[Frequences.Length - 1];
        if (Frequences[Frequences.Length - 2] == 2) Strength += 1;
    }

    public int CompareTo(object obj)
    {
        var other = (Hand)obj;
        if (Strength != other.Strength)
        {
            return Strength - other.Strength;
        }

        for (var i = 0; i < Cards.Length; i++)
            if (Cards[i] != other.Cards[i])
                return other.Cards[i] - Cards[i];
        return 0;
    }
}