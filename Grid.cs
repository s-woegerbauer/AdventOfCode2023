namespace AdventOfCode2023;

public class Grid<T>
{
    public Grid(string[] lines, string splitter)
    {
        CurrentX = 0;
        CurrentY = 0;
        VerticalLength = lines.Length;

        foreach (var line in lines)
            if (line.Length > HorizontalLength)
                HorizontalLength = line.Length;

        var array = new T[VerticalLength, HorizontalLength];

        for (var vertical = 0; vertical < VerticalLength; vertical++)
        {
            var line = lines[vertical];
            for (var horizontal = 0; horizontal < HorizontalLength; horizontal++)
            {
                var valueString = line.Split(splitter)[horizontal];
                array[vertical, horizontal] = ConvertStringToValue(valueString);
            }
        }

        Array = array;
    }

    public Grid(T[,] array)
    {
        VerticalLength = array.GetLength(0);
        HorizontalLength = array.GetLength(1);
        CurrentX = 0;
        CurrentY = 0;
        Array = new T[VerticalLength, HorizontalLength];

        for (var y = 0; y < VerticalLength; y++)
        for (var x = 0; x < HorizontalLength; x++)
            Array![y, x] = ConvertStringToValue(array[y, x]!.ToString()!);
    }

    public Grid(string[] lines)
    {
        CurrentX = 0;
        CurrentY = 0;
        VerticalLength = lines.Length;

        foreach (var line in lines)
            if (line.Length > HorizontalLength)
                HorizontalLength = line.Length;

        var array = new T[VerticalLength, HorizontalLength];

        for (var vertical = 0; vertical < VerticalLength; vertical++)
        {
            var line = lines[vertical];
            for (var horizontal = 0; horizontal < HorizontalLength; horizontal++)
            {
                var valueString = line[horizontal].ToString();
                array[vertical, horizontal] = ConvertStringToValue(valueString);
            }
        }

        Array = array;
    }

    public Grid(int horizontalLength, int verticalLength)
    {
        HorizontalLength = horizontalLength;
        VerticalLength = verticalLength;
        Array = new T[horizontalLength, verticalLength];
        CurrentX = 0;
        CurrentY = 0;
    }

    public T[,] Array { get; set; }
    public int HorizontalLength { get; set; }
    public int VerticalLength { get; set; }
    public int CurrentX { get; set; }
    public int CurrentY { get; set; }
    public int CurrentDir { get; set; }

    public int AmountOf(T value)
    {
        var counter = 0;

        foreach (var item in Array)
            if (item!.Equals(value))
                counter++;

        return counter;
    }

    public T Above(int horizontal, int vertical)
    {
        if (horizontal >= 0 && horizontal < HorizontalLength && vertical - 1 < VerticalLength && vertical - 1 >= 0)
            return Array[vertical - 1, horizontal];
        return default!;
    }

    public T Right(int horizontal, int vertical)
    {
        if (horizontal + 1 >= 0 && horizontal + 1 < HorizontalLength && vertical < VerticalLength && vertical >= 0)
            return Array[vertical, horizontal + 1];
        return default!;
    }

    public T Below(int horizontal, int vertical)
    {
        if (horizontal >= 0 && horizontal < HorizontalLength && vertical + 1 < VerticalLength && vertical + 1 >= 0)
            return Array[vertical + 1, horizontal];
        return default!;
    }

    public T Left(int horizontal, int vertical)
    {
        if (horizontal - 1 >= 0 && horizontal - 1 < HorizontalLength && vertical < VerticalLength && vertical >= 0)
            return Array[vertical, horizontal - 1];
        return default!;
    }

    public int GetLongestPath(T startValue, Func<T, T, bool> barrier)
    {
        int[] dx = { 0, 1, 0, -1 };
        int[] dy = { -1, 0, 1, 0 };

        var rows = Array.GetLength(0);
        var cols = Array.GetLength(1);

        var distances = new int[rows, cols];
        for (var i = 0; i < rows; i++)
        for (var j = 0; j < cols; j++)
            distances[i, j] = 0;

        var distanceList = new List<int[,]>();

        foreach (var startCoords in GetStartCoords(startValue))
            distanceList.Add(Dijkstra(startCoords.Item1, startCoords.Item2, barrier, distances));

        var maxDistance = int.MinValue;
        var maxX = 0;
        var maxY = 0;

        for (var i = 0; i < rows; i++)
        for (var j = 0; j < cols; j++)
            if (distances[i, j] < int.MaxValue)
            {
                maxDistance = Math.Max(maxDistance, distances[i, j]);
                maxX = j;
                maxY = i;
            }

        var path = ReconstructPath(distances, maxX, maxY);
        //PrintPath(path);

        return maxDistance == int.MinValue ? -1 : maxDistance;
    }


    public int GetShortestPath(T startValue, T endValue, Func<T, T, bool> barrier)
    {
        int[] dx = { 0, 1, 0, -1 };
        int[] dy = { -1, 0, 1, 0 };

        var rows = Array.GetLength(0);
        var cols = Array.GetLength(1);

        var distances = new int[rows, cols];
        for (var i = 0; i < rows; i++)
        for (var j = 0; j < cols; j++)
            distances[i, j] = int.MaxValue;

        var distanceList = new List<int[,]>();

        foreach (var startCoords in GetStartCoords(startValue))
            distanceList.Add(Dijkstra(startCoords.Item1, startCoords.Item2, barrier, distances));

        int endX = -1, endY = -1;
        for (var i = 0; i < rows; i++)
        for (var j = 0; j < cols; j++)
            if (Array[i, j]!.Equals(endValue))
            {
                endX = j;
                endY = i;
                break;
            }

        var distance = distanceList
            .OrderBy(array => array[endY, endX])
            .FirstOrDefault()!;

        var path = ReconstructPath(distance, endX, endY);
        PrintPath(path, startValue, endValue);

        return distance[endY, endX] == int.MaxValue ? -1 : distance[endY, endX];
    }

    private List<(int, int)> GetStartCoords(T startValue)
    {
        var rows = Array.GetLength(0);
        var cols = Array.GetLength(1);

        var startCoords = new List<(int, int)>();

        for (var i = 0; i < rows; i++)
        for (var j = 0; j < cols; j++)
            if (Array[i, j]!.Equals(startValue))
                startCoords.Add((j, i));

        return startCoords;
    }

    private int[,] Dijkstra(int startX, int startY, Func<T, T, bool> barrier, int[,] distances)
    {
        var rows = Array.GetLength(0);
        var cols = Array.GetLength(1);

        var maxHeap = new MaxHeap<(int, int, int)>();


        if (startX != -1 && startY != -1)
        {
            maxHeap.Insert((0, startX, startY));
            distances[startY, startX] = 0;
        }

        while (maxHeap.Count > 0)
        {
            var (dist, x, y) = maxHeap.ExtractMax();
            int[] dx = { 0, 0, 1, -1 };
            int[] dy = { 1, -1, 0, 0 };

            for (var dir = 0; dir < 4; dir++)
            {
                var nextX = x + dx[dir];
                var nextY = y + dy[dir];

                if (nextX >= 0 && nextX < cols && nextY >= 0 && nextY < rows)
                {
                    CurrentX = x;
                    CurrentY = y;
                    CurrentDir = dir;
                    if (!barrier(Array[y, x], Array[nextY, nextX]))
                    {
                        var newDist = dist + 1;
                        if (newDist > distances[nextY, nextX])
                        {
                            distances[nextY, nextX] = newDist;
                            maxHeap.Insert((newDist, nextX, nextY));
                        }
                    }
                }
            }
        }

        return distances;
    }

    private List<(int, int)> ReconstructPath(int[,] distances, int endX, int endY)
    {
        var currentX = endX;
        var currentY = endY;

        var path = new List<(int, int)>();
        path.Add((currentX, currentY));


        while (distances[currentY, currentX] != 0)
        {
            int[] dx = { 0, 0, 1, -1 };
            int[] dy = { 1, -1, 0, 0 };
            for (var dir = 0; dir < 4; dir++)
            {
                var nextX = currentX + dx[dir];
                var nextY = currentY + dy[dir];

                if (nextX >= 0 && nextX < HorizontalLength && nextY >= 0 && nextY < VerticalLength &&
                    distances[nextY, nextX] == distances[currentY, currentX] - 1)
                {
                    path.Add((nextX, nextY));
                    currentX = nextX;
                    currentY = nextY;
                    break;
                }
            }
        }

        path.Reverse();
        return path;
    }

    private void PrintPath(List<(int, int)> path)
    {
        var pathGrid = new char[VerticalLength, HorizontalLength];

        // Initialize the path grid with empty cells
        for (var y = 0; y < VerticalLength; y++)
        for (var x = 0; x < HorizontalLength; x++)
            pathGrid[y, x] = '.';

        // Mark the start and end positions
        pathGrid[path[0].Item2, path[0].Item1] = 'S';
        pathGrid[path[path.Count - 1].Item2, path[path.Count - 1].Item1] = 'E';

        // Mark the path with arrows
        for (var i = 1; i < path.Count - 1; i++)
        {
            var current = path[i];
            var previous = path[i - 1];
            var next = path[i + 1];

            if (previous.Item1 < current.Item1 && previous.Item2 < current.Item2)
                pathGrid[current.Item2, current.Item1] = '>'; // Down-right
            else if (previous.Item1 > current.Item1 && previous.Item2 < current.Item2)
                pathGrid[current.Item2, current.Item1] = '<'; // Down-left
            else if (previous.Item1 < current.Item1 && previous.Item2 > current.Item2)
                pathGrid[current.Item2, current.Item1] = '>'; // Up-right
            else if (previous.Item1 > current.Item1 && previous.Item2 > current.Item2)
                pathGrid[current.Item2, current.Item1] = '<'; // Up-left
            else if (previous.Item1 < current.Item1 && current.Item1 < next.Item1)
                pathGrid[current.Item2, current.Item1] = '>'; // Right
            else if (previous.Item1 > current.Item1 && current.Item1 > next.Item1)
                pathGrid[current.Item2, current.Item1] = '<'; // Left
            else if (previous.Item2 < current.Item2 && current.Item2 < next.Item2)
                pathGrid[current.Item2, current.Item1] = 'v'; // Down
            else if (previous.Item2 > current.Item2 && current.Item2 > next.Item2)
                pathGrid[current.Item2, current.Item1] = '^'; // Up
        }

        for (var y = 0; y < VerticalLength; y++)
        {
            for (var x = 0; x < HorizontalLength; x++) Console.Write(pathGrid[y, x]);
            Console.WriteLine();
        }
    }

    private void PrintPath(List<(int, int)> path, T startValue, T endValue)
    {
        var pathGrid = new char[VerticalLength, HorizontalLength];

        // Initialize the path grid with empty cells
        for (var y = 0; y < VerticalLength; y++)
        for (var x = 0; x < HorizontalLength; x++)
            pathGrid[y, x] = '.';

        // Mark the start and end positions
        pathGrid[path[0].Item2, path[0].Item1] = 'S';
        pathGrid[path[path.Count - 1].Item2, path[path.Count - 1].Item1] = 'E';

        // Mark the path with arrows
        for (var i = 1; i < path.Count - 1; i++)
        {
            var current = path[i];
            var previous = path[i - 1];
            var next = path[i + 1];

            if (previous.Item1 < current.Item1 && previous.Item2 < current.Item2)
                pathGrid[current.Item2, current.Item1] = '>'; // Down-right
            else if (previous.Item1 > current.Item1 && previous.Item2 < current.Item2)
                pathGrid[current.Item2, current.Item1] = '<'; // Down-left
            else if (previous.Item1 < current.Item1 && previous.Item2 > current.Item2)
                pathGrid[current.Item2, current.Item1] = '>'; // Up-right
            else if (previous.Item1 > current.Item1 && previous.Item2 > current.Item2)
                pathGrid[current.Item2, current.Item1] = '<'; // Up-left
            else if (previous.Item1 < current.Item1 && current.Item1 < next.Item1)
                pathGrid[current.Item2, current.Item1] = '>'; // Right
            else if (previous.Item1 > current.Item1 && current.Item1 > next.Item1)
                pathGrid[current.Item2, current.Item1] = '<'; // Left
            else if (previous.Item2 < current.Item2 && current.Item2 < next.Item2)
                pathGrid[current.Item2, current.Item1] = 'v'; // Down
            else if (previous.Item2 > current.Item2 && current.Item2 > next.Item2)
                pathGrid[current.Item2, current.Item1] = '^'; // Up
        }

        for (var y = 0; y < VerticalLength; y++)
        {
            for (var x = 0; x < HorizontalLength; x++) Console.Write(pathGrid[y, x]);
            Console.WriteLine();
        }
    }

    private static T ConvertStringToValue(string input)
    {
        var underlyingType = Nullable.GetUnderlyingType(typeof(T))!;

        if (underlyingType != null)
        {
            if (string.IsNullOrWhiteSpace(input)) return default!;
            return (T)Convert.ChangeType(input, underlyingType);
        }

        if (string.IsNullOrWhiteSpace(input)) return default!;
        return (T)Convert.ChangeType(input, typeof(T));
    }
}