using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Versioning;
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
            Day18_Star1();
            Day18_Star2();
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
                int type1 = Helper.FindHandType(h1.hand);
                int type2 = Helper.FindHandType(h2.hand);
                int compareTo = type2.CompareTo(type1);
                if (compareTo == 0)
                {
                    compareTo = Helper.SolveDraw(h1.hand, h2.hand);

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
                int type1 = Helper.FindHandType2(h1.hand);
                int type2 = Helper.FindHandType2(h2.hand);
                int compareTo = type2.CompareTo(type1);
                if (compareTo == 0)
                {
                    compareTo = Helper.SolveDraw2(h1.hand, h2.hand);
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
            Console.WriteLine("8*2 -- " + Helper.LCMOfList(nodeSteps));
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
                int nextValue = Helper.FindSequenceNextValue(numbers);
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
                int nextValue = Helper.FindSequencePrevValue(numbers);
                total += nextValue;
            }
            Console.WriteLine("9*2 -- " + total);
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
            //Helper.PrintTrench(trenches, trenchLocations, offsetX, offsetY);


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
                            Helper.FloodFill(trenches, i, digStop - 1, 1);
                            break;
                        }

                    }
                }
                if (digStop > 0) { break; }
            }
            //Helper.PrintTrench(trenches, trenchLocations, offsetX, offsetY);

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
            double polygonArea = Helper.CalculateArea(points);

            double result = polygonArea + totalSteps / 2 + 1;

            Console.WriteLine("18*2 -- " + result);
        }

        private static void ExitConsole()
        {
            Console.WriteLine("\n\nPress any key to close console.");
            Console.ReadKey();
        }
    }
}
