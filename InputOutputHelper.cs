using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    public static class InputOutputHelper
    {
        public static string[] GetInput(bool isTest, string day /*e.g.: "09"*/)
        {
            if(isTest)
            {
                return File.ReadAllLines(Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net6.0", "") + "\\" + day + "\\testInput.txt");
            }
            else
            {
                return File.ReadAllLines(Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net6.0", "") + "\\" + day + "\\input.txt");
            }
        }

        public static void WriteOutput<T>(bool isTest, T output)
        {
            if(isTest)
            {
                Console.WriteLine("Test:");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(output.ToString());
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Result:");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(output.ToString());
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
            }
        }
    }
}
