using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
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
                c = line[i];
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

        internal static int FindHandType(string hand)
        {
            Dictionary<char, int> occurences = new Dictionary<char, int>()
            {
                { 'A', 0 },
                { 'K', 0 },
                { 'Q', 0 },
                { 'J', 0 },
                { 'T', 0 },
                { '1', 0 },
                { '2', 0 },
                { '3', 0 },
                { '4', 0 },
                { '5', 0 },
                { '6', 0 },
                { '7', 0 },
                { '8', 0 },
                { '9', 0 },
            };
            int max1 = 0;
            int max2 = 0;
            foreach (char c in hand)
            {
                occurences[c]++;

            }
            foreach (int value in occurences.Values)
            {
                if (value > max1)
                {
                    max2 = max1;
                    max1 = value;
                }
                else if (value <= max1 && value > max2)
                {
                    max2 = value;
                }
            }
            return int.Parse(max1.ToString() + max2.ToString());

        }

        internal static int SolveDraw(string hand1, string hand2)
        {
            Dictionary<char, int> cardPwer = new Dictionary<char, int>()
            {
                { 'A', 14 },
                { 'K', 13 },
                { 'Q', 12 },
                { 'J', 11 },
                { 'T', 10 },
                { '1', 1 },
                { '2', 2 },
                { '3', 3 },
                { '4', 4 },
                { '5', 5 },
                { '6', 6 },
                { '7', 7 },
                { '8', 8 },
                { '9', 9 },
            };
            for (int i = 0; i < hand1.Length; i++)
            {
                int handPower1 = cardPwer[hand1[i]];
                int handPower2 = cardPwer[hand2[i]];
                if (handPower1 > handPower2)
                {
                    return -1;
                }
                else if (handPower1 < handPower2)
                {
                    return 1;
                }
            }
            return 0;
        }

        internal static int FindHandType2(string hand)
        {
            Dictionary<char, int> occurences = new Dictionary<char, int>()
            {
                { 'A', 0 },
                { 'K', 0 },
                { 'Q', 0 },
                { 'J', 0 },
                { 'T', 0 },
                { '1', 0 },
                { '2', 0 },
                { '3', 0 },
                { '4', 0 },
                { '5', 0 },
                { '6', 0 },
                { '7', 0 },
                { '8', 0 },
                { '9', 0 },
            };
            int max1 = 0;
            int max2 = 0;
            foreach (char c in hand)
            {
                occurences[c]++;
            }
            foreach (var oc in occurences)
            {
                if (oc.Key != 'J')
                {
                    if (oc.Value > max1)
                    {
                        max2 = max1;
                        max1 = oc.Value;
                    }
                    else if (oc.Value <= max1 && oc.Value > max2)
                    {
                        max2 = oc.Value;
                    }
                }
            }
            if (occurences['J'] > 0)
            {
                max1 += occurences['J'];
            }
            return int.Parse(max1.ToString() + max2.ToString());

        }

        internal static int SolveDraw2(string hand1, string hand2)
        {
            Dictionary<char, int> cardPwer = new Dictionary<char, int>()
            {
                { 'A', 14 },
                { 'K', 13 },
                { 'Q', 12 },
                { 'J', 0 },
                { 'T', 10 },
                { '1', 1 },
                { '2', 2 },
                { '3', 3 },
                { '4', 4 },
                { '5', 5 },
                { '6', 6 },
                { '7', 7 },
                { '8', 8 },
                { '9', 9 },
            };
            for (int i = 0; i < hand1.Length; i++)
            {
                int handPower1 = cardPwer[hand1[i]];
                int handPower2 = cardPwer[hand2[i]];
                if (handPower1 > handPower2)
                {
                    return -1;
                }
                else if (handPower1 < handPower2)
                {
                    return 1;
                }
            }
            return 0;
        }

        internal static long LCMOfList(List<long> numbers)
        {
            if (numbers.Count == 0) return 0;
            long lcm = numbers[0];

            for (int i = 1; i < numbers.Count; i++)
            {
                lcm = LCM(lcm, numbers[i]);
            }

            return lcm;
        }

        internal static long LCM(long a, long b)
        {
            return (a * b) / GCD(a, b);
        }

        internal static long GCD(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }
    }
}