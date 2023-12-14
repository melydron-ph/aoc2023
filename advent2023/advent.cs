using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace advent2023
{
    internal class Advent
    {
        static void Main(string[] args)
        {
            //Day1_Star1();
            //Day1_Star2();
            //Day2_Star1();
            //Day2_Star2();
            //Day3_Star1();
            //Day3_Star2();
            //Day4_Star1();
            //Day4_Star2();
            Day5_Star1();
            Day5_Star2();
            ExitConsole();
        }

        private static void Day1_Star1()
        {
            var textFile = @"C:\aoc\2023\day1\input.txt";
            string[] lines = File.ReadAllLines(textFile);
            int totalSum = 0;
            foreach (string line in lines)
            {
                int firstDigit = Helper.GetFirstDigitFromString(line);
                int secondDigit = Helper.GetFirstDigitFromString(Helper.ReverseString(line));
                int lineSum = int.Parse(firstDigit.ToString() + secondDigit.ToString());
                totalSum += lineSum;
            }
            Console.WriteLine("1*1 -- " + totalSum);
        }

        private static void Day1_Star2()
        {
            //var textFile = @"C:\aoc\star2\test.txt";
            var textFile = @"C:\aoc\2023\day1\input.txt";
            string[] lines = File.ReadAllLines(textFile);
            int totalSum = 0;
            foreach (string line in lines)
            {
                string newLine = Helper.ReplaceWordsToDigits(line);
                int firstDigit = Helper.GetFirstDigitFromString(newLine);
                int secondDigit = Helper.GetFirstDigitFromString(Helper.ReverseString(newLine));
                int lineSum = 0;
                lineSum = int.Parse(firstDigit.ToString() + secondDigit.ToString());
                totalSum += lineSum;
            }
            Console.WriteLine("1*2 -- " + totalSum);
        }

        private static void Day2_Star1()
        {
            //var textFile = @"C:\aoc\2023\day2\test.txt";
            var textFile = @"C:\aoc\2023\day2\input.txt";
            string[] lines = File.ReadAllLines(textFile);
            int totalSum = 0;
            int i = 0;
            var limits = new Dictionary<string, int>
            {
                { "red", 12 },
                { "green", 13 },
                { "blue", 14 },
            };
            foreach (string line in lines)
            {
                i++;
                bool badGame = false;
                string[] bags = line.Split(':')[1].Split(';');
                foreach (string bag in bags)
                {
                    string[] balls = bag.Split(',');
                    foreach (string ball in balls)
                    {
                        int numOfBalls = Convert.ToInt32(ball.Trim().Split(' ')[0]);
                        string color = ball.Trim().Split(' ')[1];
                        if (numOfBalls > limits[color])
                        {
                            badGame = true;
                            break;
                        }
                    }
                    if (badGame)
                    {
                        break;
                    }
                }
                if (!badGame)
                    totalSum += i;
            }
            Console.WriteLine("2*1 -- " + totalSum);
        }

        private static void Day2_Star2()
        {
            var textFile = @"C:\aoc\2023\day2\test.txt";
            //var textFile = @"C:\aoc\2023\day2\input.txt";
            string[] lines = File.ReadAllLines(textFile);
            int totalSum = 0;

            foreach (string line in lines)
            {
                var ballMax = new Dictionary<string, int>
                {
                    { "red", 0 },
                    { "green", 0 },
                    { "blue", 0 },
                };
                string[] bags = line.Split(':')[1].Split(';');
                foreach (string bag in bags)
                {
                    string[] balls = bag.Split(',');
                    foreach (string ball in balls)
                    {
                        int numOfBalls = Convert.ToInt32(ball.Trim().Split(' ')[0]);
                        string color = ball.Trim().Split(' ')[1];
                        if (numOfBalls > ballMax[color])
                        {
                            ballMax[color] = numOfBalls;
                        }
                    }
                }
                totalSum += (ballMax["red"] * ballMax["green"] * ballMax["blue"]);
            }
            Console.WriteLine("2*2 -- " + totalSum);
        }

        private static void Day3_Star1()
        {
            //var textFile = @"C:\aoc\2023\day3\test.txt";
            var textFile = @"C:\aoc\2023\day3\input.txt";
            string[] lines = File.ReadAllLines(textFile);
            int lineLength = lines[0].Length;
            int linesCount = lines.Length;
            int totalSum = 0;
            for (int i = 0; i < linesCount; i++)
            {
                for (int j = 0; j < lineLength; j++)
                {
                    int number = 0;
                    int numberStart = -1;
                    int numberEnd = -1;
                    bool numberFound = false;
                    bool endFound = false;
                    char c = lines[i][j];
                    while (char.IsDigit(c))
                    {
                        if (!numberFound)
                        {
                            numberFound = true;
                            numberStart = j;
                        }
                        int numberC = c - '0';
                        number = number * 10 + numberC;
                        if (j + 1 == lineLength)
                        {
                            endFound = true;
                            break;
                        }
                        c = lines[i][++j];
                    }
                    if (numberFound)
                    {
                        if (endFound)
                            numberEnd = j;
                        else
                            numberEnd = j - 1;
                        if (Helper.NumberIsGood(i, numberStart, numberEnd, lines))
                            totalSum += number;
                    }
                }
            }
            Console.WriteLine("3*1 -- " + totalSum);
        }

        private static void Day3_Star2()
        {
            //var textFile = @"C:\aoc\2023\day3\test.txt";
            var textFile = @"C:\aoc\2023\day3\input.txt";
            string[] lines = File.ReadAllLines(textFile);
            int lineLength = lines[0].Length;
            int linesCount = lines.Length;
            int totalSum = 0;
            for (int i = 0; i < linesCount; i++)
            {
                for (int j = 0; j < lineLength; j++)
                {

                    char c = lines[i][j];
                    if (c == '*')
                    {
                        List<int> gearNumbers = Helper.FindGearNumbers(i, j, lines);
                        if (gearNumbers.Count == 2)
                        {
                            totalSum += gearNumbers[0] * gearNumbers[1];
                        }
                    }
                }
            }
            Console.WriteLine("3*2 -- " + totalSum);
        }

        private static void Day4_Star1()
        {
            //var textFile = @"C:\aoc\2023\day4\test.txt";
            var textFile = @"C:\aoc\2023\day4\input.txt";
            string[] lines = File.ReadAllLines(textFile);
            double totalSum = 0;
            int i = 0;
            foreach (string line in lines)
            {

                string[] numbers = line.Split(':')[1].Split('|');
                string winningGroup = string.Join(" ", numbers[0].Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                string candidateGroup = string.Join(" ", numbers[1].Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                int[] winningNumbers = Array.ConvertAll(winningGroup.Split(' ').ToArray(), int.Parse);
                int[] candidateNumbers = Array.ConvertAll(candidateGroup.Split(' ').ToArray(), int.Parse);
                int total = -1;
                foreach (int number in candidateNumbers)
                {
                    if (winningNumbers.Contains(number))
                    {
                        total++;
                    }
                }
                if (total >= 0)
                {
                    totalSum += Math.Pow(2, total);
                }

            }
            Console.WriteLine("4*1 -- " + totalSum);
        }

        private static void Day4_Star2()
        {
            //var textFile = @"C:\aoc\2023\day4\test.txt";
            var textFile = @"C:\aoc\2023\day4\input.txt";
            string[] lines = File.ReadAllLines(textFile);
            int totalCards = 0;
            int cardNumber = 0;
            int[] extraIterations = new int[lines.Length];
            foreach (string line in lines)
            {
                totalCards++;

                string[] numbers = line.Split(':')[1].Split('|');
                string winningGroup = string.Join(" ", numbers[0].Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                string candidateGroup = string.Join(" ", numbers[1].Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                int[] winningNumbers = Array.ConvertAll(winningGroup.Split(' ').ToArray(), int.Parse);
                int[] candidateNumbers = Array.ConvertAll(candidateGroup.Split(' ').ToArray(), int.Parse);
                int nextCardsWon = 0;
                foreach (int number in candidateNumbers)
                {
                    if (winningNumbers.Contains(number))
                    {
                        nextCardsWon++;
                    }
                }
                for (int i = 0; i <= extraIterations[cardNumber]; i++)
                {
                    for (int j = cardNumber + 1; j <= cardNumber + nextCardsWon; j++)
                    {
                        if (j < extraIterations.Length)
                        {
                            extraIterations[j]++;
                        }
                    }
                }
                cardNumber++;

            }
            foreach (int number in extraIterations)
            {
                totalCards += number;
            }
            Console.WriteLine("4*2 -- " + totalCards);
        }

        private static void Day5_Star1()
        {
            //var textFile = @"C:\aoc\2023\day5\test.txt";
            var textFile = @"C:\aoc\2023\day5\input.txt";
            string file = File.ReadAllText(textFile);
            string[] fileBlocks = file.Split(new string[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.None);
            long[] seeds = Array.ConvertAll(fileBlocks[0].Split(':')[1].Trim().Split(' ').ToArray(), long.Parse);
            foreach (string fileBlock in fileBlocks.Skip(1))
            {
                string[] lines = fileBlock.Split('\n');
                for (int i = 0; i < seeds.Length; i++)
                {
                    foreach (string line in lines.Skip(1))
                    {
                        if (line.Length > 0)
                        {
                            long[] transformRule = Array.ConvertAll(line.Split(' ').ToArray(), long.Parse);
                            long dest = transformRule[0];
                            long source = transformRule[1];
                            long range = transformRule[2];
                            if (seeds[i] >= source && seeds[i] < source + range)
                            {
                                long diff = seeds[i] - source;
                                seeds[i] = dest + diff;
                                break;
                            }
                        }
                    }
                }
            }
            Array.Sort(seeds);
            Console.WriteLine("5*1 -- " + seeds[0]);
        }

        private static void Day5_Star2()
        {
            //var textFile = @"C:\aoc\2023\day5\test.txt";
            var textFile = @"C:\aoc\2023\day5\input.txt";
            string file = File.ReadAllText(textFile);
            string[] fileBlocks = file.Split(new string[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.None);
            long[] oldSeeds = Array.ConvertAll(fileBlocks[0].Split(':')[1].Trim().Split(' ').ToArray(), long.Parse);
            List<Tuple<long, long>> seedPairs = new List<Tuple<long, long>>();
            for (int i = 0; i < oldSeeds.Length - 1; i += 2)
            {
                seedPairs.Add(new Tuple<long, long>(oldSeeds[i], oldSeeds[i + 1]));
            }

            List<List<(long source, long dest, long range)>> transformRulesList = new List<List<(long, long, long)>>();
            for (int i = 1; i < fileBlocks.Count(); i++)
            {
                string[] lines = fileBlocks[i].Split('\n');
                List<(long source, long dest, long range)> mapRules = new List<(long, long, long)>();
                foreach (string line in lines.Skip(1))
                {
                    if (line.Length > 0)
                    {
                        long[] transformRule = Array.ConvertAll(line.Split(' ').ToArray(), long.Parse);
                        (long source, long dest, long range) rule = (transformRule[0], transformRule[1], transformRule[2]);
                        mapRules.Add(rule);
                    }
                }
                transformRulesList.Add(mapRules);
            }

            List<(long start, long range)> seedPairsList = seedPairs.Select(p => (p.Item1, p.Item2)).ToList();

            long minLoc = long.MaxValue;
            bool numberFound = false;
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            for (long i = 0; i < long.MaxValue; i++)
            {
                long num = i;
                for (int j = transformRulesList.Count - 1; j >= 0; j--)
                {
                    foreach (var (source, dest, range) in transformRulesList[j])
                    {
                        if (num >= source && num < source + range)
                        {
                            num = dest + (num - source);
                            break;
                        }
                    }
                }

                foreach (var (start, range) in seedPairsList)
                {
                    if (num >= start && num <= start + range)
                    {
                        minLoc = i;
                        numberFound = true;
                        break;
                    }
                }

                if (numberFound)
                    break;
            }
            stopwatch.Stop();
            Console.WriteLine("5*2 -- " + minLoc + " (Time elapsed: " + stopwatch.Elapsed.TotalSeconds.ToString(".0#") + " secs)");

        }
        private static void ExitConsole()
        {
            Console.WriteLine("\n\nPress any key to close console.");
            Console.ReadKey();
        }
    }
}
