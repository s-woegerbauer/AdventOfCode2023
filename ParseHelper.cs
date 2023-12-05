using System.ComponentModel;

namespace AdventOfCode2023;

public static class ParseHelper
{
    public static class Tuples
    {
        /// <summary>
        ///     Parses a string into a Tuple with two items
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="input"></param>
        /// <param name="splitter"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static Tuple<T1, T2> ParseTuple<T1, T2>(string input, string splitter)
        {
            var parts = input.Split(splitter);
            if (parts.Length != 2)
                throw new ArgumentException("Input string does not match the expected format.");

            return new Tuple<T1, T2>(Help.ConvertValue<T1>(parts[0]), Help.ConvertValue<T2>(parts[1]));
        }

        /// <summary>
        ///     Parses a string into a Tuple with three items
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static Tuple<T1, T2, T3> ParseTuple<T1, T2, T3>(string input, string splitter)
        {
            var parts = input.Split(splitter);
            if (parts.Length != 3)
                throw new ArgumentException("Input string does not match the expected format.");

            return new Tuple<T1, T2, T3>(Help.ConvertValue<T1>(parts[0]), Help.ConvertValue<T2>(parts[1]),
                Help.ConvertValue<T3>(parts[2]));
        }
    }

    public static class Advanced
    {
    }

    public static class Simple
    {
        /// <summary>
        ///     Parses a line to a list
        ///     One split is one item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="line"></param>
        /// <returns name="result"></returns>
        public static List<T> LineToListWithSplit<T>(string line, string splitter)
        {
            List<T> result = new();

            for (var i = 0; i < line.Split(splitter).Length; i++)
                result.Add(Help.ParseLine<T>(line.Split(splitter)[i]));

            return result;
        }

        /// <summary>
        ///     Parses a line to a list
        ///     One char is one item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="line"></param>
        /// <returns name="result"></returns>
        public static List<T> LineToList<T>(string line)
        {
            List<T> result = new();

            for (var i = 0; i < line.Length; i++) result.Add(Help.ParseLine<T>(line[i].ToString()));

            return result;
        }

        /// <summary>
        ///     Parses a line to an one dimensional Array
        ///     One char is one item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="line"></param>
        /// <returns name="result"></returns>
        public static T[] LineToOneDimensionalArray<T>(string line)
        {
            var result = new T[line.Length];

            for (var i = 0; i < line.Length; i++) result[i] = Help.ParseLine<T>(line[i].ToString());

            return result;
        }

        /// <summary>
        ///     Parses a line to an one dimensional Array
        ///     One split is one item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="line"></param>
        /// <returns name="result"></returns>
        public static T[] LineToOneDimensionalArrayWithSplit<T>(string line, string splitter)
        {
            var result = new T[line.Length];

            for (var i = 0; i < line.Split(splitter).Length; i++)
                result[i] = Help.ParseLine<T>(line.Split(splitter)[i]);

            return result;
        }

        /// <summary>
        ///     Converts a file to an one dimensional Array
        ///     Each line is one item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lines"></param>
        /// <returns name="result"></returns>
        public static T[] LinesToOneDimensionalArray<T>(string[] lines)
        {
            var result = new T[lines.Length];

            for (var i = 0; i < lines.Length; i++) result[i] = Help.ParseLine<T>(lines[i]);

            return result;
        }

        /// <summary>
        ///     Converts a file to an two dimensional Array
        ///     Each line is one row
        ///     Each split in a row is an item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lines"></param>
        /// <param name="splitter"></param>
        /// <returns name="result"></returns>
        public static T[,] LinesToTwoDimensionalArrayWithSplitter<T>(string[] lines, string splitter)
        {
            var maxLength = Help.GetMaxLengthOfSplit(lines, splitter);
            var result = new T[lines.Length, maxLength];

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];

                for (var j = 0; j < maxLength; j++)
                    if (line.Split(splitter).Length > j)
                        result[i, j] = Help.ParseLine<T>(lines[i].Split(splitter)[j]);
                    else
                        result[i, j] = default;
            }

            return result;
        }

        /// <summary>
        ///     Converts a file to an two dimensional Array
        ///     Each line is one row
        ///     Each char in a row is an item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lines"></param>
        /// <returns name="result"></returns>
        public static T[,] LinesToTwoDimensionalArray<T>(string[] lines)
        {
            var maxLength = Help.GetMaxLengthOfLine(lines);
            var result = new T[lines.Length, maxLength];

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];

                for (var j = 0; j < maxLength; j++)
                    if (line.Length > j)
                        result[i, j] = Help.ParseLine<T>(lines[i][j].ToString());
                    else
                        result[i, j] = default;
            }

            return result;
        }
    }

    internal static class Help
    {
        /// <summary>
        ///     Parses one line into a value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="line"></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException"></exception>
        internal static T ParseLine<T>(string line)
        {
            if (typeof(T) == typeof(string)) return (T)(object)line;

            var converter = TypeDescriptor.GetConverter(typeof(T));

            if (converter.CanConvertFrom(typeof(string)))
                return (T)converter.ConvertFrom(line);
            throw new InvalidCastException($"Cannot parse '{line}' to type {typeof(T)}.");
        }

        /// <summary>
        ///     Gets the longest length of multiple line.Split()
        /// </summary>
        /// <param name="input"></param>
        /// <param name="splitter"></param>
        /// <returns name="max"></returns>
        internal static int GetMaxLengthOfSplit(string[] input, string splitter)
        {
            var max = 0;

            foreach (var line in input)
                if (line.Split(splitter).Length >= max)
                    max = line.Split(splitter).Length;

            return max;
        }

        /// <summary>
        ///     Gets the longest length of lines
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        internal static int GetMaxLengthOfLine(string[] input)
        {
            var max = 0;

            foreach (var line in input)
                if (line.Length >= max)
                    max = line.Length;

            return max;
        }

        /// <summary>
        ///     Converts a value into the correct type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static T ConvertValue<T>(string value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}