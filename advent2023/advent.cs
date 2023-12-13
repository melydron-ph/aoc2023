using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
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
            var textFile = @"C:\aoc\2023\day5\test.txt";
            //var textFile = @"C:\aoc\2023\day5\input.txt";
            string[] lines = File.ReadAllLines(textFile);
            // Parse first line to get seeds
            int[] seeds = Array.ConvertAll(lines[0].Split(':')[1].Trim().Split(' ').ToArray(), int.Parse);
            foreach (string line in lines.Skip(1))
            {

            }

            // Find first map
            // Get first transformation rule range
            // For each seed
            //      if in range
            //          update seed    

            Console.WriteLine("5*1 -- ");
        }

        private static void ExitConsole()
        {
            Console.WriteLine("\n\nPress any key to close console.");
            Console.ReadKey();
        }
    }
}
