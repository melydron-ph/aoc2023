using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace AoC
{
    internal class Stardust
    {
        static void Main(string[] args)
        {
            Star1();
            Star2();
            ExitConsole();
        }

        // ------ Star 1 ------
        private static void Star1()
        {
            var textFile = @"C:\aoc\star1\input.txt";
            string[] lines = File.ReadAllLines(textFile);
            int totalSum = 0;
            foreach (string line in lines)
            {
                int firstDigit = getFirstDigitFromString(line);
                int secondDigit = getFirstDigitFromString(reverseString(line));
                int lineSum = int.Parse(firstDigit.ToString() + secondDigit.ToString());
                totalSum += lineSum;
            }
            Console.WriteLine("*1 -- Total Sum: " + totalSum);
        }
        private static string reverseString(string str)
        {
            char[] charArray = str.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        private static int getFirstDigitFromString(string str)
        {
            string firstDigit = new string(str.SkipWhile(c => !char.IsDigit(c)).Take(1).ToArray());
            if (firstDigit.Length == 0)
                return -1;
            else
                return Convert.ToInt32(firstDigit);
        }

        // ------ Star 2 ------
        private static void Star2()
        {
            //var textFile = @"C:\aoc\star2\test.txt";
            var textFile = @"C:\aoc\star1\input.txt";
            string[] lines = File.ReadAllLines(textFile);
            int totalSum = 0;
            foreach (string line in lines)
            {
                string newLine = replaceWordsToDigits(line);
                int firstDigit = getFirstDigitFromString(newLine);
                int secondDigit = getFirstDigitFromString(reverseString(newLine));
                int lineSum = 0;
                lineSum = int.Parse(firstDigit.ToString() + secondDigit.ToString());
                totalSum += lineSum;
            }
            Console.WriteLine("*2 -- Total Sum: " + totalSum);
        }

        private static string replaceWordsToDigits(string str)
        {
            var wordToDigit = new Dictionary<string, string>
            {
                { "one", "1"},
                { "two", "2"},
                { "three", "3"},
                { "four", "4"},
                { "five", "5"},
                { "six", "6"},
                { "seven", "7"},
                { "eight", "8"},
                { "nine", "9"}
            };

            var word = new StringBuilder();
            foreach (char c in str)
            {
                word.Append(c);
                foreach (var kvp in wordToDigit)
                {
                    if (word.ToString().Contains(kvp.Key))
                    {
                        word.Replace(kvp.Key, kvp.Key[0] + kvp.Value + kvp.Key[kvp.Key.Length - 1]);
                    }
                }
            }
            return word.ToString();
        }

        private static void ExitConsole()
        {
            Console.WriteLine("\n\nPress any key to close console.");
            Console.ReadKey();
        }
    }
}
