using Microsoft.SqlServer.Server;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.ExceptionServices;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Versioning;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static advent2023.Helper;

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
            //Day5_Star1();
            //Day5_Star2();
            //Day6_Star1();
            //Day6_Star2();
            //Day7_Star1();
            //Day7_Star2();
            //Day8_Star1();
            //Day8_Star2();
            //Day9_Star1();
            //Day9_Star2();
            //Day10_Star1();
            //Day10_Star2();
            Day11_Star1();
            Day11_Star2();
            //Day15_Star1();
            //Day15_Star2();
            //Day18_Star1();
            //Day18_Star2();
            //Day19_Star1();
            //Day19_Star2();
            //Day20_Star1();
            //Day20_Star2();
            //Day21_Star1();
            //Day21_Star2();
            //Day22_Star1();
            //Day22_Star2();
            ExitConsole();
        }

        private static void Day1_Star1()
        {
            var textFile = @"C:\aoc\2023\day1\input.txt";
            string[] lines = File.ReadAllLines(textFile);
            int totalSum = 0;
            foreach (string line in lines)
            {
                int firstDigit = GetFirstDigitFromString(line);
                int secondDigit = GetFirstDigitFromString(ReverseString(line));
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
                string newLine = ReplaceWordsToDigits(line);
                int firstDigit = GetFirstDigitFromString(newLine);
                int secondDigit = GetFirstDigitFromString(ReverseString(newLine));
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
                        if (NumberIsGood(i, numberStart, numberEnd, lines))
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
                        List<int> gearNumbers = FindGearNumbers(i, j, lines);
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

        private static void Day6_Star1()
        {
            //var textFile = @"C:\aoc\2023\day6\test.txt";
            var textFile = @"C:\aoc\2023\day6\input.txt";
            string[] lines = File.ReadAllLines(textFile);
            string timeLine = string.Join(" ", lines[0].Split(':')[1].Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            string distanceLine = string.Join(" ", lines[1].Split(':')[1].Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            int[] time = Array.ConvertAll(timeLine.Split(' ').ToArray(), int.Parse);
            int[] distance = Array.ConvertAll(distanceLine.Split(' ').ToArray(), int.Parse);
            int total = 1;
            for (int i = 0; i < time.Length; i++) // for each race duration
            {
                int waysToWin = 0;
                for (int j = 0; j < time[i]; j++)
                {
                    int speed = j;
                    int duration = time[i] - speed;
                    int d = duration * speed;
                    if (d > distance[i])
                    {
                        waysToWin++;
                    }
                }
                total = total * waysToWin;
            }
            Console.WriteLine("6*1 -- " + total);
        }

        private static void Day6_Star2()
        {
            //var textFile = @"C:\aoc\2023\day6\test.txt";
            var textFile = @"C:\aoc\2023\day6\input.txt";
            string[] lines = File.ReadAllLines(textFile);
            string timeLine = string.Join(" ", lines[0].Split(':')[1].Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            string distanceLine = string.Join(" ", lines[1].Split(':')[1].Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            int[] time = Array.ConvertAll(timeLine.Split(' ').ToArray(), int.Parse);
            int[] distance = Array.ConvertAll(distanceLine.Split(' ').ToArray(), int.Parse);
            string timeNumbers = string.Empty;
            foreach (int t in time)
            {
                timeNumbers += t.ToString();
            }
            long raceTime = long.Parse(timeNumbers);

            string distanceNumbers = string.Empty;
            foreach (int d in distance)
            {
                distanceNumbers += d.ToString();
            }
            long raceDistance = long.Parse(distanceNumbers);

            int waysToWin = 0;

            for (int j = 0; j < raceTime; j++)
            {
                int speed = j;
                long duration = raceTime - speed;
                long d = duration * speed;
                if (d > raceDistance)
                {
                    waysToWin++;
                }
            }
            Console.WriteLine("6*2 -- " + waysToWin);

        }

        private static void Day7_Star1()
        {
            //var textFile = @"C:\aoc\2023\day7\test.txt";
            var textFile = @"C:\aoc\2023\day7\input.txt";
            string[] lines = File.ReadAllLines(textFile);
            List<(string hand, int bid)> handsAndBids = new List<(string, int)>();
            foreach (string line in lines)
            {
                string[] lineParts = line.Split(' ');
                string hand = lineParts[0].Trim();
                string bid = lineParts[1].Trim();
                (string hand, int bid) handAndBid = (lineParts[0].Trim(), int.Parse(lineParts[1]));
                handsAndBids.Add(handAndBid);
            }
            handsAndBids.Sort(delegate ((string hand, int bid) h1, (string hand, int bid) h2)
            {
                int type1 = FindHandType(h1.hand);
                int type2 = FindHandType(h2.hand);
                int compareTo = type2.CompareTo(type1);
                if (compareTo == 0)
                {
                    compareTo = SolveDraw(h1.hand, h2.hand);

                }
                return compareTo;
            });

            int j = handsAndBids.Count();
            int totalWinnings = 0;
            for (int i = 0; i < handsAndBids.Count(); i++)
            {
                totalWinnings += j-- * handsAndBids[i].bid;
            }
            Console.WriteLine("7*1 -- " + totalWinnings);
        }

        private static void Day7_Star2()
        {
            //var textFile = @"C:\aoc\2023\day7\test.txt";
            var textFile = @"C:\aoc\2023\day7\input.txt";
            string[] lines = File.ReadAllLines(textFile);
            List<(string hand, int bid)> handsAndBids = new List<(string, int)>();
            foreach (string line in lines)
            {
                string[] lineParts = line.Split(' ');
                string hand = lineParts[0].Trim();
                string bid = lineParts[1].Trim();
                (string hand, int bid) handAndBid = (lineParts[0].Trim(), int.Parse(lineParts[1]));
                handsAndBids.Add(handAndBid);
            }
            handsAndBids.Sort(delegate ((string hand, int bid) h1, (string hand, int bid) h2)
            {
                int type1 = FindHandType2(h1.hand);
                int type2 = FindHandType2(h2.hand);
                int compareTo = type2.CompareTo(type1);
                if (compareTo == 0)
                {
                    compareTo = SolveDraw2(h1.hand, h2.hand);
                }
                return compareTo;
            });

            int j = handsAndBids.Count();
            int totalWinnings = 0;
            for (int i = 0; i < handsAndBids.Count(); i++)
            {
                totalWinnings += j-- * handsAndBids[i].bid;
            }
            Console.WriteLine("7*2 -- " + totalWinnings);
        }

        private static void Day8_Star1()
        {
            //var textFile = @"C:\aoc\2023\day8\test1.txt";
            //var textFile = @"C:\aoc\2023\day8\test2.txt";
            var textFile = @"C:\aoc\2023\day8\input.txt";
            string[] lines = File.ReadAllLines(textFile);
            string steps = lines[0];
            Dictionary<string, Tuple<string, string>> nodes = new Dictionary<string, Tuple<string, string>>();
            foreach (string line in lines.Skip(2))
            {
                string[] lineParts = line.Split('=');
                string nodeName = lineParts[0].Trim();
                string[] values = lineParts[1].Trim().Trim('(', ')').Split(',');
                string nodeLeft = values[0].Trim();
                string nodeRight = values[1].Trim();
                Tuple<string, string> nodesToGo = new Tuple<string, string>(nodeLeft, nodeRight);
                nodes.Add(nodeName, nodesToGo);
            }
            string currentNode = "AAA";
            int currentStep = 0;
            int totalSteps = 0;
            while (currentNode != "ZZZ")
            {
                if (currentStep == steps.Length)
                {
                    totalSteps += currentStep;
                    currentStep = 0;
                }
                char direction = steps[currentStep++];
                if (direction == 'L')
                {
                    currentNode = nodes[currentNode].Item1;
                }
                else
                {
                    currentNode = nodes[currentNode].Item2;
                }
            }
            totalSteps += currentStep;
            Console.WriteLine("8*1 -- " + totalSteps);
        }

        private static void Day8_Star2()
        {
            //var textFile = @"C:\aoc\2023\day8\test3.txt";
            var textFile = @"C:\aoc\2023\day8\input.txt";
            string[] lines = File.ReadAllLines(textFile);
            string steps = lines[0];
            Dictionary<string, Tuple<string, string>> nodes = new Dictionary<string, Tuple<string, string>>();
            List<string> startingNodes = new List<string>();
            foreach (string line in lines.Skip(2))
            {
                string[] lineParts = line.Split('=');
                string nodeName = lineParts[0].Trim();
                if (nodeName[2] == 'A')
                {
                    startingNodes.Add(nodeName);
                }
                string[] values = lineParts[1].Trim().Trim('(', ')').Split(',');
                string nodeLeft = values[0].Trim();
                string nodeRight = values[1].Trim();
                Tuple<string, string> nodesToGo = new Tuple<string, string>(nodeLeft, nodeRight);
                nodes.Add(nodeName, nodesToGo);
            }
            int currentStep = 0;
            int totalSteps = 0;
            List<long> nodeSteps = new List<long>();
            int nodesFinished = 0;
            while (nodesFinished < 6)
            {
                if (currentStep == steps.Length)
                {
                    totalSteps += currentStep;
                    currentStep = 0;
                }
                for (int i = 0; i < startingNodes.Count(); i++)
                {
                    string currentNode = startingNodes[i];
                    char direction = steps[currentStep];
                    if (direction == 'L')
                    {
                        currentNode = nodes[currentNode].Item1;
                    }
                    else
                    {
                        currentNode = nodes[currentNode].Item2;
                    }
                    startingNodes[i] = currentNode;
                    if (currentNode[2] == 'Z')
                    {
                        nodesFinished++;
                        nodeSteps.Add(totalSteps + currentStep + 1);
                    }
                }
                currentStep++;

            }
            Console.WriteLine("8*2 -- " + LCMOfList(nodeSteps));
        }

        private static void Day9_Star1()
        {
            //var textFile = @"C:\aoc\2023\day9\test.txt";
            var textFile = @"C:\aoc\2023\day9\input.txt";
            string[] lines = File.ReadAllLines(textFile);
            int total = 0;
            foreach (string line in lines)
            {
                int[] numbers = Array.ConvertAll(line.Split(' ').ToArray(), int.Parse);
                int nextValue = FindSequenceNextValue(numbers);
                total += nextValue;
            }
            Console.WriteLine("9*1 -- " + total);
        }

        private static void Day9_Star2()
        {
            //var textFile = @"C:\aoc\2023\day9\test.txt";
            var textFile = @"C:\aoc\2023\day9\input.txt";
            string[] lines = File.ReadAllLines(textFile);
            int total = 0;
            foreach (string line in lines)
            {
                int[] numbers = Array.ConvertAll(line.Split(' ').ToArray(), int.Parse);
                int nextValue = FindSequencePrevValue(numbers);
                total += nextValue;
            }
            Console.WriteLine("9*2 -- " + total);
        }

        private static void Day10_Star1()
        {
            //var textFile = @"C:\aoc\2023\day10\test1.txt";
            //var textFile = @"C:\aoc\2023\day10\test2.txt";
            var textFile = @"C:\aoc\2023\day10\input.txt";
            string file = File.ReadAllText(textFile);
            file = file.Replace('7', '┐').Replace('|', '│').Replace('L', '└').Replace('F', '┌').Replace('J', '┘').Replace('-', '─').Trim();
            //file = file.Trim();
            string[] lines = file.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            int mapX = lines[0].Length;
            int mapY = lines.Count();
            char[,] map = new char[mapX, mapY];
            int startX = 0;
            int startY = 0;
            for (int i = 0; i < mapY; i++)
            {
                string line = lines[i];
                for (int j = 0; j < mapX; j++)
                {
                    map[i, j] = line[j];
                    if (map[i, j] == 'S')
                    {
                        startX = i;
                        startY = j;
                    }
                }
            }
            //PrintMap(map);
            Console.WriteLine();
            (int, int, int) furthestPoint = FindFurthestPointFromStart(startX, startY, map);

            Console.WriteLine("10*1 -- " + furthestPoint.Item3);
        }


        private static void Day10_Star2()
        {
            //var textFile = @"C:\aoc\2023\day10\test1.txt";
            //var textFile = @"C:\aoc\2023\day10\test2.txt";
            var textFile = @"C:\aoc\2023\day10\input.txt";
            string file = File.ReadAllText(textFile);
            file = file.Replace('7', '┐').Replace('|', '│').Replace('L', '└').Replace('F', '┌').Replace('J', '┘').Replace('-', '─').Trim();
            //file = file.Trim();
            string[] lines = file.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            int mapX = lines[0].Length;
            int mapY = lines.Count();
            char[,] map = new char[mapX, mapY];
            int startX = 0;
            int startY = 0;
            for (int i = 0; i < mapY; i++)
            {
                string line = lines[i];
                for (int j = 0; j < mapX; j++)
                {
                    map[i, j] = line[j];
                    if (map[i, j] == 'S')
                    {
                        startX = i;
                        startY = j;
                    }
                }
            }
            //PrintMap(map);
            List<PointF> mapPath = TraversePath(map, startX, startY);

            double polygonArea = CalculateArea(mapPath);

            double result = polygonArea + mapPath.Count() / 2 + 1 - mapPath.Count();

            Console.WriteLine("10*2 -- " + result);

        }

        private static void Day11_Star1()
        {
            //var textFile = @"C:\aoc\2023\day11\test.txt";
            var textFile = @"C:\aoc\2023\day11\input.txt";
            string[] lines = File.ReadAllLines(textFile);
            List<Point> galaxies = new List<Point>();
            int mapX = lines[0].Length;
            int mapY = lines.Count();
            char[,] map = new char[mapX, mapY];
            List<int> rowsWithoutGalaxy = new List<int>();
            List<int> colsWithoutGalaxy = new List<int>();
            int galaxyNum = 1;
            Dictionary<Point, int> galaxiesDictionary = new Dictionary<Point, int>();
            for (int i = 0; i < mapY; i++)
            {
                string line = lines[i];
                if (!line.Contains('#'))
                    rowsWithoutGalaxy.Add(i);
                for (int j = 0; j < mapX; j++)
                {
                    map[i, j] = line[j];
                    if (map[i, j] == '#')
                    {
                        Point galaxy = new Point(i, j);
                        galaxies.Add(galaxy);
                        galaxiesDictionary.Add(galaxy, galaxyNum++);
                    }
                }
            }

            for (int j = 0; j < mapX; j++)
            {
                bool galaxyFound = false;
                for (int i = 0; i < mapY; i++)
                {
                    if (map[i, j] == '#')
                    {
                        galaxyFound = true;
                        break;
                    }
                }
                if (!galaxyFound)
                {
                    colsWithoutGalaxy.Add(j);
                }
            }
            List<Tuple<Point, Point>> pairs = GeneratePairs(galaxies);
            int total = 0;
            foreach (Tuple<Point, Point> pair in pairs)
            {

                (int, int) extraDistance = FindExtraDistance(pair.Item1, pair.Item2, rowsWithoutGalaxy, colsWithoutGalaxy);
                int distance = FindShortestPath(pair.Item1, pair.Item2, extraDistance);
                total += distance;
                //Console.WriteLine($"Distance between {galaxiesDictionary[pair.Item1]} - {galaxiesDictionary[pair.Item2]} : {distance}");
            }

            Console.WriteLine("11*1 -- " + total);
        }

        private static void Day11_Star2()
        {
            //var textFile = @"C:\aoc\2023\day11\test.txt";
            var textFile = @"C:\aoc\2023\day11\input.txt";
            string[] lines = File.ReadAllLines(textFile);
            List<Point> galaxies = new List<Point>();
            int mapX = lines[0].Length;
            int mapY = lines.Count();
            char[,] map = new char[mapX, mapY];
            List<int> rowsWithoutGalaxy = new List<int>();
            List<int> colsWithoutGalaxy = new List<int>();
            int galaxyNum = 1;
            Dictionary<Point, int> galaxiesDictionary = new Dictionary<Point, int>();
            for (int i = 0; i < mapY; i++)
            {
                string line = lines[i];
                if (!line.Contains('#'))
                    rowsWithoutGalaxy.Add(i);
                for (int j = 0; j < mapX; j++)
                {
                    map[i, j] = line[j];
                    if (map[i, j] == '#')
                    {
                        Point galaxy = new Point(i, j);
                        galaxies.Add(galaxy);
                        galaxiesDictionary.Add(galaxy, galaxyNum++);
                    }
                }
            }

            for (int j = 0; j < mapX; j++)
            {
                bool galaxyFound = false;
                for (int i = 0; i < mapY; i++)
                {
                    if (map[i, j] == '#')
                    {
                        galaxyFound = true;
                        break;
                    }
                }
                if (!galaxyFound)
                {
                    colsWithoutGalaxy.Add(j);
                }
            }
            List<Tuple<Point, Point>> pairs = GeneratePairs(galaxies);
            long total = 0;
            foreach (Tuple<Point, Point> pair in pairs)
            {

                (long, long) extraDistance = FindExtraDistanceMultiple(pair.Item1, pair.Item2, rowsWithoutGalaxy, colsWithoutGalaxy);
                long distance = FindShortestPathMultiple(pair.Item1, pair.Item2, extraDistance);
                total += distance;
                //Console.WriteLine($"Distance between {galaxiesDictionary[pair.Item1]} - {galaxiesDictionary[pair.Item2]} : {distance}");
            }

            Console.WriteLine($"int max value: {int.MaxValue}");
            Console.WriteLine("11*2 -- " + total);
        }

        private static void Day15_Star1()
        {
            //var textFile = @"C:\aoc\2023\day15\test.txt";
            var textFile = @"C:\aoc\2023\day15\input.txt";
            string file = File.ReadAllText(textFile).Trim();
            string[] steps = file.Split(',');
            int total = 0;
            int[] results = new int[steps.Length];
            int i = 0;
            foreach (string step in steps)
            {
                int stepHashed = HashString(step);
                results[i++] = stepHashed;
            }
            Array.ForEach(results, value => total += value);
            Console.WriteLine("15*1 -- " + total);
        }


        private static void Day15_Star2()
        {
            //var textFile = @"C:\aoc\2023\day15\test.txt";
            var textFile = @"C:\aoc\2023\day15\input.txt";
            string file = File.ReadAllText(textFile);
            string[] steps = file.Split(',');
            List<(string, char, int)> operations = new List<(string, char, int)>();
            Dictionary<int, List<(string, int)>> boxes = new Dictionary<int, List<(string, int)>>();
            for (int i = 0; i < 256; i++)
            {
                boxes[i] = new List<(string, int)>();
            }
            foreach (string step in steps)
            {
                if (step.Contains('='))
                {
                    string[] stepParts = step.Split('=');
                    string label = stepParts[0];
                    int value = int.Parse(stepParts[1]);
                    int boxNum = HashString(label);
                    bool lenseFound = false;
                    List<(string, int)> lenseBox = boxes[boxNum];
                    for (int i = 0; i < lenseBox.Count(); i++)
                    {
                        if (lenseBox[i].Item1 == label)
                        {
                            lenseBox[i] = (label, value);
                            lenseFound = true;
                        }
                    }
                    if (!lenseFound)
                    {
                        lenseBox.Add((label, value));
                    }
                }
                else
                {
                    string[] stepParts = step.Split('-');
                    string label = stepParts[0];
                    int boxNum = HashString(label);
                    List<(string, int)> lenseBox = boxes[boxNum];
                    lenseBox.RemoveAll(item => item.Item1 == label);
                }
            }

            int total = 0;
            for (int i = 0; i < 256; i++)
            {
                List<(string, int)> lenseBox = boxes[i];
                int boxSum = 0;
                for (int j = 0; j < lenseBox.Count(); j++)
                {
                    int focalLength = lenseBox[j].Item2;
                    boxSum += focalLength * (j + 1);
                }
                total += boxSum * (i + 1);
            }
            Console.WriteLine("15*2 -- " + total);

        }
        private static void Day18_Star1()
        {
            //var textFile = @"C:\aoc\2023\day18\test.txt";
            var textFile = @"C:\aoc\2023\day18\input.txt";
            string[] lines = File.ReadAllLines(textFile);
            int total = 0;
            Dictionary<Tuple<int, int>, string> trenchLocations = new Dictionary<Tuple<int, int>, string>();
            int locX = 0, locY = 0, maxX = 0, maxY = 0, minX = 0, minY = 0;
            foreach (string line in lines)
            {
                string[] lineParts = line.Split(' ');
                char direction = lineParts[0].Trim()[0];
                int steps = int.Parse(lineParts[1].Trim());
                string color = lineParts[2].Trim().Trim('(', ')');

                if (direction == 'R')
                {
                    for (int i = 0; i < steps; i++)
                    {
                        trenchLocations.Add(new Tuple<int, int>(locX, locY), color);
                        if (++locY > maxY)
                            maxY = locY;
                    }
                }
                else if (direction == 'D')
                {
                    for (int i = 0; i < steps; i++)
                    {
                        trenchLocations.Add(new Tuple<int, int>(locX, locY), color);
                        if (++locX > maxX)
                            maxX = locX;
                    }
                }
                else if (direction == 'L')
                {
                    for (int i = 0; i < steps; i++)
                    {
                        trenchLocations.Add(new Tuple<int, int>(locX, locY), color);
                        if (--locY < minY)
                            minY = locY;
                    }
                }
                else if (direction == 'U')
                {
                    for (int i = 0; i < steps; i++)
                    {
                        trenchLocations.Add(new Tuple<int, int>(locX, locY), color);
                        if (--locX < minX)
                            minX = locX;

                    }
                }

            }
            int arrayX = Math.Abs(minX) + Math.Abs(maxX) + 1;
            int arrayY = Math.Abs(minY) + Math.Abs(maxY) + 1;
            int[,] trenches = new int[arrayX, arrayY];
            int offsetX = Math.Abs(minX);
            int offsetY = Math.Abs(minY);
            foreach (var location in trenchLocations)
            {
                int x = location.Key.Item1 + offsetX;
                int y = location.Key.Item2 + offsetY;
                trenches[x, y] = 1;
                //Console.WriteLine("Location: " + location + " --- [" + x + "," + y + "]");

            }
            Console.ReadKey();
            PrintTrench(trenches, trenchLocations, offsetX, offsetY);


            int digStop = -1;
            for (int i = arrayX - 1; i >= 0; i--)
            {
                int digStart = -1;
                for (int j = 0; j < arrayY; j++)
                {
                    if (trenches[i, j] == 1 && digStart < 0)
                    {
                        digStart = j;
                        while (j < arrayY && trenches[i, j] == 1)
                        {
                            j++;
                            digStart = j;
                        }
                        while (j < arrayY && trenches[i, j] == 0)
                        {
                            j++;
                        }
                        if (j < arrayY && trenches[i, j] == 1)
                        {
                            digStop = j;
                            FloodFill(trenches, i, digStop - 1, 1);
                            break;
                        }

                    }
                }
                if (digStop > 0) { break; }
            }

            Console.ReadKey();
            PrintTrench(trenches, trenchLocations, offsetX, offsetY);

            for (int i = 0; i < arrayX; i++)
            {
                for (int j = 0; j < arrayY; j++)
                {
                    if (trenches[i, j] == 1)
                    {
                        total++;
                    }
                }
            }
            Console.WriteLine("18*1 -- " + total);
        }

        private static void Day18_Star2()
        {
            //var textFile = @"C:\aoc\2023\day18\test.txt";
            var textFile = @"C:\aoc\2023\day18\input.txt";
            string[] lines = File.ReadAllLines(textFile);
            List<PointF> points = new List<PointF>();

            int locX = 0, locY = 0;
            int totalSteps = 0;
            foreach (string line in lines)
            {
                PointF p = new Point(locX, locY);
                points.Add(p);
                string[] lineParts = line.Split(' ');
                string hexCode = lineParts[2].Trim().Trim('(', ')').Trim('#');
                int steps = int.Parse(hexCode.Substring(0, 5), NumberStyles.HexNumber);
                int direction = int.Parse(hexCode.Substring(5, 1), NumberStyles.HexNumber);
                if (direction == 0) // R
                    locY += steps;
                else if (direction == 1) // D
                    locX += steps;
                else if (direction == 2) // L
                    locY -= steps;
                else if (direction == 3) // U
                    locX -= steps;
                totalSteps += steps;
            }
            double polygonArea = CalculateArea(points);

            double result = polygonArea + totalSteps / 2 + 1;

            Console.WriteLine("18*2 -- " + result);
        }

        private static void Day19_Star1()
        {
            //var textFile = @"C:\aoc\2023\day19\test.txt";
            var textFile = @"C:\aoc\2023\day19\input.txt";
            string file = File.ReadAllText(textFile);
            string[] fileBlocks = file.Split(new string[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.None);
            string[] workflowsBlock = fileBlocks[0].Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            string[] partsBlock = fileBlocks[1].Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            Dictionary<string, List<WorkflowRule>> workflows = new Dictionary<string, List<WorkflowRule>>();
            List<MachinePart> machineParts = new List<MachinePart>();
            foreach (string workflowLine in workflowsBlock)
            {
                if (workflowLine != "")
                {
                    Workflow wf = new Workflow(workflowLine);
                    workflows.Add(wf.Name, wf.WorkflowRules);
                }

            }
            foreach (string partLine in partsBlock)
            {
                if (partLine != "")
                {
                    machineParts.Add(new MachinePart(partLine));
                }
            }
            List<WorkflowRule> currentRules = workflows["in"];
            int i = 0;
            int total = 0;
            foreach (MachinePart machinePart in machineParts)
            {
                bool processing = true;
                while (processing)
                {
                    foreach (WorkflowRule wfRule in currentRules)
                    {
                        if (CompareToWorkflowRule(wfRule.Condition, machinePart))
                        {
                            if (wfRule.Destination == "A")
                            {
                                int mpSum = machinePart.x + machinePart.m + machinePart.a + machinePart.s;
                                total += mpSum;
                                processing = false;
                                break;
                            }
                            else if (wfRule.Destination == "R")
                            {
                                processing = false;
                                break;
                            }
                            else
                            {
                                currentRules = workflows[wfRule.Destination];
                                break;
                            }
                        }
                    }
                }
                currentRules = workflows["in"];
            }

            Console.WriteLine("19*1 -- " + total);
        }

        private static void Day19_Star2()
        {
            //var textFile = @"C:\aoc\2023\day19\test.txt";
            var textFile = @"C:\aoc\2023\day19\input.txt";
            string file = File.ReadAllText(textFile);
            string[] fileBlocks = file.Split(new string[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.None);
            string[] workflowsBlock = fileBlocks[0].Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            Dictionary<string, List<WorkflowRule>> workflows = new Dictionary<string, List<WorkflowRule>>();
            foreach (string workflowLine in workflowsBlock)
            {
                if (workflowLine != "")
                {
                    Workflow wf = new Workflow(workflowLine);
                    workflows.Add(wf.Name, wf.WorkflowRules);
                }
            }

            List<List<string>> acceptPaths = FindAcceptPaths(workflows);

            List<MachinePartRanges> acceptRanges = FindAcceptRanges(acceptPaths);
            long xRange = 0;
            long mRange = 0;
            long aRange = 0;
            long sRange = 0;
            long sum = 0;
            foreach (MachinePartRanges acceptRange in acceptRanges)
            {
                xRange = acceptRange.x.Max - acceptRange.x.Min + 1;
                mRange = acceptRange.m.Max - acceptRange.m.Min + 1;
                aRange = acceptRange.a.Max - acceptRange.a.Min + 1;
                sRange = acceptRange.s.Max - acceptRange.s.Min + 1;
                long isum = xRange * mRange * aRange * sRange;
                sum += isum;
            }

            Console.WriteLine("19*2 -- " + sum);
        }

        private static void Day20_Star1()
        {
            //var textFile = @"C:\aoc\2023\day20\test1.txt";
            //var textFile = @"C:\aoc\2023\day20\test2.txt";
            var textFile = @"C:\aoc\2023\day20\input.txt";
            string[] lines = File.ReadAllLines(textFile);
            int total = 0;

            Dictionary<string, Module> modulesDictionary = new Dictionary<string, Module>();

            foreach (string line in lines)
            {
                string modulesPart = line.Split('>')[1].Trim();
                string[] modulesToAdd = modulesPart.Split(',');
                if (line[0] == '%')
                {
                    string namePart = line.Split(' ')[0];
                    string name = namePart.Substring(1, namePart.Length - 1);
                    FlipFlop f = new FlipFlop(name);

                    foreach (string m in modulesToAdd)
                    {
                        f.Modules.Add(m.Trim());
                    }
                    modulesDictionary.Add(f.Name, f);
                }
                else if (line[0] == '&')
                {
                    string namePart = line.Split(' ')[0];
                    string name = namePart.Substring(1, namePart.Length - 1);
                    Conjunction c = new Conjunction(name);
                    foreach (string m in modulesToAdd)
                    {
                        c.Modules.Add(m.Trim());
                    }
                    modulesDictionary.Add(c.Name, c);
                }
                else // broadcaster
                {
                    Module bc = new Module("bc"); ;
                    foreach (string m in modulesToAdd)
                    {
                        bc.Modules.Add(m.Trim());
                    }
                    modulesDictionary.Add("bc", bc);
                }
            }

            // initialize Conjuction connected inputs to 0
            foreach (KeyValuePair<string, Module> kvp in modulesDictionary)
            {
                Module m = kvp.Value;
                foreach (string recipient in m.Modules)
                {
                    if (modulesDictionary.ContainsKey(recipient))
                    {
                        var r = modulesDictionary[recipient];
                        if (r.GetType() == typeof(Conjunction))
                        {
                            r.ReceivePulse(0, m.Name);
                        }
                    }
                }
            }

            int low = 1; // button -low-> broadcaster
            int high = 0;
            Queue<(string, int)> responseQ = new Queue<(string, int)>();
            int buttonPress = 1;
            foreach (string moduleName in modulesDictionary["bc"].Modules)
            {
                low++;
                if (modulesDictionary[moduleName].ReceivePulse(0, "bc"))
                {
                    Module m = modulesDictionary[moduleName];
                    responseQ.Enqueue((moduleName, m.Pulse));
                }
            }
            bool initStateReached = false;
            while (!initStateReached)
            {
                while (responseQ.Count > 0)
                {
                    (string, int) moduleNameAndPulse = responseQ.Dequeue();
                    Module m = modulesDictionary[moduleNameAndPulse.Item1];
                    int p = moduleNameAndPulse.Item2;
                    foreach (string recipient in m.Modules)
                    {
                        if (p == 0)
                        {
                            low++;
                        }
                        else
                        {
                            high++;
                        }
                        if (modulesDictionary.ContainsKey(recipient))
                        {
                            if (modulesDictionary[recipient].ReceivePulse(p, m.Name))
                            {
                                Module r = modulesDictionary[recipient];
                                responseQ.Enqueue((recipient, r.Pulse));
                            }
                        }
                    }
                }
                if (responseQ.Count == 0)
                {
                    initStateReached = true;
                    foreach (KeyValuePair<string, Module> kvp in modulesDictionary)
                    {
                        var m = kvp.Value;
                        if (m.GetType() == typeof(FlipFlop) && m.SendPulse() == 0) // State ON
                        {
                            initStateReached = false;
                            break;
                        }
                    }
                    if (!initStateReached)
                    {
                        ++buttonPress;
                        if (buttonPress > 1000)
                        {
                            buttonPress--;
                            break;
                        }
                        low++;
                        foreach (string moduleName in modulesDictionary["bc"].Modules)
                        {
                            low++;
                            if (modulesDictionary[moduleName].ReceivePulse(0, "bc"))
                            {
                                Module m = modulesDictionary[moduleName];
                                responseQ.Enqueue((moduleName, m.Pulse));
                            }
                        }
                    }
                }
            }
            int cycles = 1000 / buttonPress;
            total = low * high * cycles * cycles;
            Console.WriteLine("20*1 -- " + total);
        }

        private static void Day20_Star2()
        {
            //var textFile = @"C:\aoc\2023\day20\test1.txt";
            //var textFile = @"C:\aoc\2023\day20\test2.txt";
            var textFile = @"C:\aoc\2023\day20\input.txt";
            string[] lines = File.ReadAllLines(textFile);
            int total = 0;

            Dictionary<string, Module> modulesDictionary = new Dictionary<string, Module>();

            foreach (string line in lines)
            {
                string modulesPart = line.Split('>')[1].Trim();
                string[] modulesToAdd = modulesPart.Split(',');
                if (line[0] == '%')
                {
                    string namePart = line.Split(' ')[0];
                    string name = namePart.Substring(1, namePart.Length - 1);
                    FlipFlop f = new FlipFlop(name);

                    foreach (string m in modulesToAdd)
                    {
                        f.Modules.Add(m.Trim());
                    }
                    modulesDictionary.Add(f.Name, f);
                }
                else if (line[0] == '&')
                {
                    string namePart = line.Split(' ')[0];
                    string name = namePart.Substring(1, namePart.Length - 1);
                    Conjunction c = new Conjunction(name);
                    foreach (string m in modulesToAdd)
                    {
                        c.Modules.Add(m.Trim());
                    }
                    modulesDictionary.Add(c.Name, c);
                }
                else // broadcaster
                {
                    Module bc = new Module("bc"); ;
                    foreach (string m in modulesToAdd)
                    {
                        bc.Modules.Add(m.Trim());
                    }
                    modulesDictionary.Add("bc", bc);
                }
            }

            Conjunction conToRX = new Conjunction("");
            // initialize Conjuction connected inputs to 0
            foreach (KeyValuePair<string, Module> kvp in modulesDictionary)
            {
                Module m = kvp.Value;
                foreach (string recipient in m.Modules)
                {
                    if (modulesDictionary.ContainsKey(recipient))
                    {
                        var r = modulesDictionary[recipient];
                        if (r.GetType() == typeof(Conjunction))
                        {
                            r.ReceivePulse(0, m.Name);
                        }
                    }
                    if (recipient == "rx")
                    {
                        conToRX = (Conjunction)m;
                    }
                }
            }

            Dictionary<string, int> modulesToConToRX = new Dictionary<string, int>();
            foreach (KeyValuePair<string, Module> kvp in modulesDictionary)
            {
                Module m = kvp.Value;
                foreach (string recipient in m.Modules)
                {
                    if (modulesDictionary.ContainsKey(recipient))
                    {
                        var r = modulesDictionary[recipient];
                        if (r.GetType() == typeof(Conjunction))
                        {
                            r.ReceivePulse(0, m.Name);
                        }
                    }
                    if (recipient == conToRX.Name)
                    {
                        modulesToConToRX.Add(m.Name, -1);
                    }
                }
            }

            Queue<(string, int)> responseQ = new Queue<(string, int)>();
            int buttonPress = 1;
            foreach (string moduleName in modulesDictionary["bc"].Modules)
            {
                if (modulesDictionary[moduleName].ReceivePulse(0, "bc"))
                {
                    Module m = modulesDictionary[moduleName];
                    responseQ.Enqueue((moduleName, m.Pulse));
                }
            }
            bool allStepsFound = false;
            while (!allStepsFound)
            {
                while (responseQ.Count > 0)
                {
                    (string, int) moduleNameAndPulse = responseQ.Dequeue();
                    Module m = modulesDictionary[moduleNameAndPulse.Item1];
                    int p = moduleNameAndPulse.Item2;
                    if (modulesToConToRX.ContainsKey(m.Name) && p == 1)
                    {
                        if (modulesToConToRX[m.Name] < 0)
                            modulesToConToRX[m.Name] = buttonPress;
                    }
                    bool finished = true;
                    foreach (KeyValuePair<string, int> kvp in modulesToConToRX)
                    {
                        int step = kvp.Value;
                        if (step < 0)
                        {
                            finished = false;
                            break;
                        }
                    }
                    if (finished)
                    {
                        allStepsFound = true;
                        break;
                    }
                    foreach (string recipient in m.Modules)
                    {
                        if (modulesDictionary.ContainsKey(recipient))
                        {
                            if (modulesDictionary[recipient].ReceivePulse(p, m.Name))
                            {
                                Module r = modulesDictionary[recipient];
                                responseQ.Enqueue((recipient, r.Pulse));
                            }
                        }
                    }
                }
                if (responseQ.Count == 0)
                {
                    ++buttonPress;
                    foreach (string moduleName in modulesDictionary["bc"].Modules)
                    {
                        if (modulesDictionary[moduleName].ReceivePulse(0, "bc"))
                        {
                            Module m = modulesDictionary[moduleName];
                            responseQ.Enqueue((moduleName, m.Pulse));
                        }
                    }
                }
            }

            List<long> numbers = new List<long>();
            foreach (KeyValuePair<string, int> kvp in modulesToConToRX)
            {
                int step = kvp.Value;
                numbers.Add(step);
            }

            Console.WriteLine("20*2 -- " + LCMOfList(numbers));
        }

        private static void Day21_Star1()
        {
            //int totalSteps = 6;
            //var textFile = @"C:\aoc\2023\day21\test.txt";
            int totalSteps = 64;
            var textFile = @"C:\aoc\2023\day21\input.txt";
            string[] lines = File.ReadAllLines(textFile);
            int mapX = lines[0].Length;
            int mapY = lines.Count();
            char[,] map = new char[mapX, mapY];
            int startX = 0;
            int startY = 0;
            for (int i = 0; i < mapY; i++)
            {
                string line = lines[i];
                for (int j = 0; j < mapX; j++)
                {
                    map[i, j] = line[j];
                    if (map[i, j] == 'S')
                    {
                        startX = i;
                        startY = j;
                        map[i, j] = '.';
                    }
                }
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Dictionary<(int, int, int), HashSet<(int, int)>> memo = new Dictionary<(int, int, int), HashSet<(int, int)>>();
            HashSet<(int, int)> visitedPositions = GetVisitedPositions(startX, startY, totalSteps, ref memo, map);
            stopwatch.Stop();

            Console.WriteLine("21*1 -- " + visitedPositions.Count + " -- " + stopwatch.Elapsed.TotalSeconds.ToString(".0#"));
        }

        private static void Day21_Star2()
        {

            //https://www.reddit.com/r/adventofcode/comments/18nevo3/comment/keaiiq7/
            // The main thing to notice for part 2 is that the grid is a square, and there are no obstacles in the same row/col of the starting point.
            // Let f(n) be the number of spaces you can reach after n steps.Let X be the length of your input grid.
            // f(n), f(n + X), f(n + 2X), ...., is a quadratic, so you can find it by finding the first 3 values,
            // then use that to interpolate the final answer.

            var textFile = @"C:\aoc\2023\day21\input.txt";
            string[] lines = File.ReadAllLines(textFile);
            int mapX = lines[0].Length;
            int mapY = lines.Count();
            char[,] map = new char[mapX, mapY];
            int startX = 0;
            int startY = 0;
            for (int i = 0; i < mapY; i++)
            {
                string line = lines[i];
                for (int j = 0; j < mapX; j++)
                {
                    map[i, j] = line[j];
                    if (map[i, j] == 'S')
                    {
                        startX = i;
                        startY = j;
                        map[i, j] = '.';
                    }
                }
            }

            int grids = 26501365 / mapX;
            int rem = 26501365 % mapY;

            List<int> sequence = new List<int>();

            //get f(n), f(n + X) and f(n + 2X)
            for (int n = 0; n < 3; n++)
            {
                int totalSteps = n * lines[0].Length + rem;
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                Dictionary<(int, int, int), HashSet<(int, int)>> memo = new Dictionary<(int, int, int), HashSet<(int, int)>>();
                HashSet<(int, int)> visitedPositions = GetVisitedPositions(startX, startY, totalSteps, ref memo, map);
                stopwatch.Stop();
                Console.WriteLine("22*2 -" + n + "- " + visitedPositions.Count + " -- " + stopwatch.Elapsed.TotalSeconds.ToString(".0#"));
                sequence.Add(visitedPositions.Count());

                map[startX, startY] = 'S';
                map = Create3x3Map(map);
                mapX = map.GetLength(0);
                mapY = map.GetLength(1);
                for (int i = 0; i < mapY; i++)
                {
                    for (int j = 0; j < mapX; j++)
                    {
                        if (map[i, j] == 'S')
                        {
                            startX = i;
                            startY = j;
                            map[i, j] = '.';
                        }
                    }
                }

            }

            // 22 * 2 - 0 - 3941-- .07
            // 22 * 2 - 1 - 35259-- 4.27
            // 22 * 2 - 2 - 97807-- 41.33

            //https://github.com/jmg48/AdventOfCode2023/blob/master/AdventOfCode2023/Day21.cs
            // Solve for the quadratic coefficients
            var c = sequence[0];
            var aPlusB = sequence[1] - c;
            var fourAPlusTwoB = sequence[2] - c;
            var twoA = fourAPlusTwoB - (2 * aPlusB);
            var a = twoA / 2;
            var b = aPlusB - a;

            long F(long n)
            {
                return a * (n * n) + b * n + c;
            }

            for (var i = 0; i < sequence.Count; i++)
            {
                Console.WriteLine($"{sequence[i]} : {F(i)}");
            }
            Console.WriteLine(F(grids));
        }

        private static void Day22_Star1()
        {
            //var textFile = @"C:\aoc\2023\day22\test.txt";
            var textFile = @"C:\aoc\2023\day22\input.txt";
            string[] lines = File.ReadAllLines(textFile);
            List<Brick> bricks = new List<Brick>();

            foreach (string line in lines)
            {
                Brick b = new Brick(line);
                bricks.Add(b);
            }

            var sortedBricks = bricks.OrderBy(brick => Math.Min(brick.CurrentStart.Z, brick.CurrentEnd.Z)).ToList();

            int time = 0;
            List<Brick> placedBricks = new List<Brick>();
            while (true)
            {
                placedBricks.OrderBy(b => Math.Max(b.CurrentStart.Z, b.CurrentEnd.Z)).ToList();
                foreach (Brick b in sortedBricks)
                {
                    if (b.Falling)
                    {
                        b.DropBrick(placedBricks);
                    }
                }
                if (sortedBricks.Count() == placedBricks.Count())
                {
                    break;
                }
            }
            foreach (Brick b in placedBricks)
            {
                b.AddSupportingBricks(placedBricks);
            }

            int total = 0;
            foreach (Brick b in placedBricks)
            {
                bool safeToDelete = true;
                foreach (Brick supportingBrick in b.SupportingBricks)
                {
                    if (supportingBrick.SupportedByBricks.Count() == 1)
                    {
                        safeToDelete = false;
                        break;
                    }
                }
                if (safeToDelete)
                {
                    total++;
                }
            }

            Console.WriteLine($"22*1 -- {total}");
        }

        private static void Day22_Star2()
        {
            //var textFile = @"C:\aoc\2023\day22\test.txt";
            var textFile = @"C:\aoc\2023\day22\input.txt";
            string[] lines = File.ReadAllLines(textFile);
            List<Brick> bricks = new List<Brick>();

            foreach (string line in lines)
            {
                Brick b = new Brick(line);
                bricks.Add(b);
            }

            var sortedBricks = bricks.OrderBy(brick => Math.Min(brick.CurrentStart.Z, brick.CurrentEnd.Z)).ToList();

            int time = 0;
            List<Brick> placedBricks = new List<Brick>();
            while (true)
            {
                placedBricks.OrderBy(b => Math.Max(b.CurrentStart.Z, b.CurrentEnd.Z)).ToList();
                foreach (Brick b in sortedBricks)
                {
                    if (b.Falling)
                    {
                        b.DropBrick(placedBricks);
                    }
                }
                if (sortedBricks.Count() == placedBricks.Count())
                {
                    break;
                }
            }
            foreach (Brick b in placedBricks)
            {
                b.AddSupportingBricks(placedBricks);
            }

            int total = 0;
            List<Brick> unsafeToRemoveBricks = new List<Brick>();
            foreach (Brick b in placedBricks)
            {
                bool safeToRemove = true;
                foreach (Brick supportingBrick in b.SupportingBricks)
                {
                    if (supportingBrick.SupportedByBricks.Count() == 1)
                    {
                        safeToRemove = false;
                        unsafeToRemoveBricks.Add(b);
                        break;
                    }
                }
                if (safeToRemove)
                {
                    total++;
                }
            }

            total = 0;
            foreach (Brick b in unsafeToRemoveBricks)
            {
                int fallingBricks = b.CalculateFallingBricks();
                total += fallingBricks;
                foreach (Brick br in placedBricks)
                {
                    br.Falling = false;
                }
            }

            Console.WriteLine($"22*2 -- {total}");
        }

        private static void ExitConsole()
        {
            Console.WriteLine("\n\nPress any key to close console.");
            Console.ReadKey();
        }
    }
}
