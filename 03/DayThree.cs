namespace AdventOfCode2023;

internal class DayThree
{
    public static void Solution()
    {
        var testInput = InputOutputHelper.GetInput(true, "03");
        PartOne(true, testInput);

        var input = InputOutputHelper.GetInput(false, "03");
        PartOne(false, input);

        PartTwo(true, testInput);
        PartTwo(false, input);
    }

    public static void PartOne(bool isTest, string[] input)
    {
        var result = 0;

        var numbers = new List<Num>();
        var symbols = new List<Symbol>();

        for (var row = 0; row < input.Length; row++)
        {
            var (extractedNumbers, extractedSymbols) = ExtractNumbersAndSymbols(input[row], row);

            numbers.AddRange(extractedNumbers);
            symbols.AddRange(extractedSymbols);
        }

        var matchingPairs = numbers.Where(number =>
            symbols.Any(symbol =>
                Math.Abs((decimal)symbol.Pos.r - number.Start.r) <= 1 &&
                symbol.Pos.c >= number.Start.c - 1 &&
                symbol.Pos.c <= number.End.c + 1));

        result = matchingPairs
            .ToList()
            .Sum(pair => pair.Val);


        InputOutputHelper.WriteOutput(isTest, result);
    }

    public static void PartTwo(bool isTest, string[] input)
    {
        var result = 0;

        var numbers = new List<Num>();
        var symbols = new List<Symbol>();

        for (var row = 0; row < input.Length; row++)
        {
            var (extractedNumbers, extractedSymbols) = ExtractNumbersAndSymbols(input[row], row);

            numbers.AddRange(extractedNumbers);
            symbols.AddRange(extractedSymbols);
        }

        result = symbols
            .Where(symbol => symbol.Val == '*')
            .Select(symbol =>
            {
                var matchingNumbers = numbers
                    .Where(number =>
                        Math.Abs(symbol.Pos.r - number.Start.r) <= 1 &&
                        symbol.Pos.c >= number.Start.c - 1 &&
                        symbol.Pos.c <= number.End.c + 1)
                    .ToArray();

                return new { Symbol = symbol, MatchingNumbers = matchingNumbers };
            })
            .Where(pair => pair.MatchingNumbers.Length == 2)
            .Sum(pair => pair.MatchingNumbers[0].Val * pair.MatchingNumbers[1].Val);


        InputOutputHelper.WriteOutput(isTest, result);
    }

    public static (List<Num> extractedNumbers, List<Symbol> extractedSymbols) ExtractNumbersAndSymbols(string rowData,
        int row)
    {
        var numbers = new List<Num>();
        var symbols = new List<Symbol>();
        var digits = new List<int>();
        var currentNumber = new Num();

        for (var col = 0; col < rowData.Length; col++)
        {
            if (rowData[col] == '.') continue;

            if (int.TryParse(rowData[col].ToString(), out var digit))
            {
                digits.Add(digit);

                if (digits.Count == 1) currentNumber.Start = (row, col);

                while (col < rowData.Length - 1 && int.TryParse(rowData[col + 1].ToString(), out digit))
                {
                    digits.Add(digit);
                    col++;
                }

                currentNumber.End = (row, col);
                currentNumber.Val = int.Parse(string.Join("", digits));

                numbers.Add(currentNumber);
                currentNumber = new Num();
                digits.Clear();
            }
            else
            {
                symbols.Add(new Symbol
                {
                    Val = rowData[col],
                    Pos = (row, col)
                });
            }
        }

        return (numbers, symbols);
    }
}