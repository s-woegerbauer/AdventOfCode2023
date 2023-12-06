namespace AdventOfCode2023;

internal class DaySix
{
    public static void Solution()
    {
        int[] times = { 45, 97, 72, 95 };
        int[] distances = { 305, 1062, 1110, 1695 };

        int[] testTimes = { 7, 15, 30 };
        int[] testDistances = { 9, 40, 200 };

        PartOne(true, testTimes, testDistances);
        PartOne(false, times, distances);

        PartTwo(true, 71530, 940200);
        PartTwo(false, 45977295, 305106211101695);
    }

    public static void PartOne(bool isTest, int[] times, int[] distances)
    {
        var result = 1;


        for (var i = 0; i < times.Length; i++)
        {
            var time = times[i];
            var distance = distances[i];
            var wins = 0;

            for (var t = 1; t < time; t++)
                if ((time - t) * t > distance)
                    wins++;

            result *= wins;
        }

        InputOutputHelper.WriteOutput(isTest, result);
    }

    public static void PartTwo(bool isTest, long time, long distance)
    {
        long result = 0;

        for (long t = 1; t < time; t++)
            if ((time - t) * t > distance)
                result++;

        InputOutputHelper.WriteOutput(isTest, result);
    }
}

public class Race
{
    public Race(int time, int distance)
    {
        Time = time;
        Distance = distance;
    }

    public int Time { get; set; }
    public int Distance { get; set; }
}