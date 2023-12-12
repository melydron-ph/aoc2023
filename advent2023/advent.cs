using System;
using System.Collections.Generic;
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
            Day1_Star1();
            Day1_Star2();
            Day2_Star1();
            Day2_Star2();
            Day3_Star1();
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
            Console.WriteLine("1*1 -- Total Sum: " + totalSum);
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
            Console.WriteLine("1*2 -- Total Sum: " + totalSum);
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
            Console.WriteLine("2*1 -- Total Sum: " + totalSum);
        }

        private static void Day2_Star2()
        {
            //var textFile = @"C:\aoc\2023\day2\test.txt";
            var textFile = @"C:\aoc\2023\day2\input.txt";
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
            Console.WriteLine("2*2 -- Total Sum: " + totalSum);
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
            Console.WriteLine("3*1 -- Total Sum: " + totalSum);
        }

        private static void ExitConsole()
        {
            Console.WriteLine("\n\nPress any key to close console.");
            Console.ReadKey();
        }
    }
}
