using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace advent2023
{
    internal class Helper
    {
        internal static string ReverseString(string str)
        {
            char[] charArray = str.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        internal static int GetFirstDigitFromString(string str)
        {
            string firstDigit = new string(str.SkipWhile(c => !char.IsDigit(c)).Take(1).ToArray());
            if (firstDigit.Length == 0)
                return -1;
            else
                return Convert.ToInt32(firstDigit);
        }

        internal static string ReplaceWordsToDigits(string str)
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

        internal static bool NumberIsGood(int row, int indexStart, int indexStop, string[] lines)
        {
            int lineLength = lines[0].Length;
            int linesCount = lines.Length;
            bool numberIsGood = false;
            if (row > 0)
            {
                numberIsGood = CheckAboveOrBelow(indexStart, indexStop, lines[row - 1]);
            }
            if (!numberIsGood && indexStart > 0)
            {
                numberIsGood = CheckLeft(indexStart, lines[row]);
            }
            if (!numberIsGood && indexStop < lineLength - 1)
            {
                numberIsGood = CheckRight(indexStop, lines[row]);
            }
            if (!numberIsGood && row < linesCount - 1)
            {
                numberIsGood = CheckAboveOrBelow(indexStart, indexStop, lines[row + 1]);
            }
            return numberIsGood;
        }


        private static bool CheckAboveOrBelow(int indexStart, int indexStop, string line)
        {
            if (indexStart > 0)
                indexStart--;
            if (indexStop < line.Length - 1)
                indexStop++;
            for (int i = indexStart; i <= indexStop; i++)
            {
                char c = line[i];
                if (c != '.')
                    return true;
            }
            return false;
        }
        private static bool CheckLeft(int indexStart, string line)
        {
            char c = line[indexStart - 1];
            if (c != '.')
                return true;
            return false;
        }
        private static bool CheckRight(int indexStop, string line)
        {
            char c = line[indexStop + 1];
            if (c != '.')
                return true;
            return false;
        }

        internal static List<int> FindGearNumbers(int i, int j, string[] lines)
        {

            List<int> adjacentNumbers = new List<int>();

            int lineLength = lines[0].Length;
            int linesCount = lines.Length;
            if (i > 0)
            {
                AddAdjacentNumbersAboveOrBelow(adjacentNumbers, j, lines[i - 1]);
            }
            if (adjacentNumbers.Count() < 3 && j > 0)
            {
                AddAdjacentNumbersLeft(adjacentNumbers, j, lines[i]);
            }
            if (adjacentNumbers.Count() < 3 && j < lineLength - 1)
            {
                AddAdjacentNumbersRight(adjacentNumbers, j, lines[i]);
            }
            if (adjacentNumbers.Count() < 3 && i < linesCount - 1)
            {
                AddAdjacentNumbersAboveOrBelow(adjacentNumbers, j, lines[i + 1]);
            }
            return adjacentNumbers;
        }

        private static void AddAdjacentNumbersAboveOrBelow(List<int> numbers, int index, string line)
        {
            int start = index;
            int stop = index;
            if (index > 0)
                start--;
            if (index < line.Length - 1)
                stop++;
            for (int i = start; i <= stop; i++)
            {
                char c = line[i];
                if (char.IsDigit(c))
                {
                    numbers.Add(ScanNumber(ref i, line));
                }

            }
        }

        private static int ScanNumber(ref int i, string line)
        {
            int start = i;
            char c = line[i];
            bool leftHit = false;
            while (char.IsDigit(c))
            {
                if (start - 1 < 0)
                {
                    leftHit = true;
                    break;
                }
                c = line[--start];

            }
            int number = 0;
            i = start;
            if (!leftHit)
            {
                i++;
                c= line[i];
            }
            while (char.IsDigit(c))
            {
                int numberC = c - '0';
                number = number * 10 + numberC;
                if (i + 1 == line.Length)
                {
                    break;
                }
                c = line[++i];
            }
            return number;
        }

        private static void AddAdjacentNumbersLeft(List<int> numbers, int index, string line)
        {
            index--;
            char c = line[index];
            if (char.IsDigit(c))
                numbers.Add(ScanNumber(ref index, line));
        }
        private static void AddAdjacentNumbersRight(List<int> numbers, int index, string line)
        {
            index++;
            char c = line[index];
            if (char.IsDigit(c))
                numbers.Add(ScanNumber(ref index, line));
        }

        internal static void UpdateExtraIterations(ref int[] extraIterations, int cardNumber, int nextCardsWon)
        {
            for (int i = cardNumber + 1; i <= cardNumber + nextCardsWon; i++)
            {
                if (i < extraIterations.Length)
                {
                    extraIterations[i]++;
                }
            }
        }

        Dictionary<>
    }
}