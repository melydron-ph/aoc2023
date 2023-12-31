﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;
using static advent2023.Helper;

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

        internal static int FindSequenceNextValue(int[] numbers)
        {
            List<int[]> sequences = new List<int[]>
            {
                numbers
            };
            int[] nextSequence = FindNextSequence(numbers);
            sequences.Add(nextSequence);
            while (!allNumsZero(nextSequence))
            {
                int[] newSequence = FindNextSequence(nextSequence);
                sequences.Add(newSequence);
                nextSequence = newSequence;
            }
            int b = 0;
            int a = 0;
            for (int i = sequences.Count() - 1; i >= 0; i--)
            {
                int[] sequence = sequences[i];
                a = sequence[sequence.Length - 1] + b;
                b = a;
            }
            return a;
        }

        internal static int FindSequencePrevValue(int[] numbers)
        {
            List<int[]> sequences = new List<int[]>
            {
                numbers
            };
            int[] nextSequence = FindNextSequence(numbers);
            sequences.Add(nextSequence);
            while (!allNumsZero(nextSequence))
            {
                int[] newSequence = FindNextSequence(nextSequence);
                sequences.Add(newSequence);
                nextSequence = newSequence;
            }
            int b = 0;
            int a = 0;
            for (int i = sequences.Count() - 1; i >= 0; i--)
            {
                int[] sequence = sequences[i];
                a = sequence[0] - b;
                b = a;
            }
            return a;
        }

        private static int[] FindNextSequence(int[] numbers)
        {
            int[] sequence = new int[numbers.Length - 1];
            for (int i = 0; i < sequence.Length; i++)
            {
                sequence[i] = numbers[i + 1] - numbers[i];
            }
            return sequence;
        }

        private static bool allNumsZero(int[] numbers)
        {
            foreach (int num in numbers)
                if (num != 0)
                    return false;
            return true;
        }

        internal static ConsoleColor ClosestConsoleColor(double r, double g, double b)
        {
            ConsoleColor ret = 0;
            double delta = double.MaxValue;

            foreach (ConsoleColor cc in Enum.GetValues(typeof(ConsoleColor)))
            {
                var n = Enum.GetName(typeof(ConsoleColor), cc);
                var c = System.Drawing.Color.FromName(n == "DarkYellow" ? "Orange" : n); // bug fix
                var t = Math.Pow(c.R - r, 2.0) + Math.Pow(c.G - g, 2.0) + Math.Pow(c.B - b, 2.0);
                if (t == 0.0)
                    return cc;
                if (t < delta)
                {
                    delta = t;
                    ret = cc;
                }
            }
            return ret;
        }

        internal static void PrintTrench(int[,] trenches, Dictionary<Tuple<int, int>, string> trenchLocations, int offsetX, int offsetY)
        {
            int arrayX = trenches.GetLength(0);
            int arrayY = trenches.GetLength(1);
            for (int i = 0; i < arrayX; i++)
            {
                for (int j = 0; j < arrayY; j++)
                {
                    if (trenches[i, j] == 1)
                    {
                        Tuple<int, int> location = new Tuple<int, int>(i - offsetX, j - offsetY);
                        if (trenchLocations.ContainsKey(location))
                        {
                            string consoleColor = trenchLocations[location].Trim('#');
                            double r = int.Parse(consoleColor.Substring(0, 2), NumberStyles.HexNumber);
                            double g = int.Parse(consoleColor.Substring(2, 2), NumberStyles.HexNumber);
                            double b = int.Parse(consoleColor.Substring(4, 2), NumberStyles.HexNumber);
                            ConsoleColor colour = Helper.ClosestConsoleColor(r, g, b);
                            Console.ForegroundColor = colour;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                        }
                        Console.Write('#');
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write('.');
                    }
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write('\n');
            }
        }

        internal static void FloodFill(int[,] array, int start_x, int start_y, int fillValue)
        {
            Stack<(int x, int y)> stack = new Stack<(int x, int y)>();
            stack.Push((start_x, start_y));

            while (stack.Count > 0)
            {
                var (x, y) = stack.Pop();

                if (x >= 0 && x < array.GetLength(0) && y >= 0 && y < array.GetLength(1) && array[x, y] == 0)
                {
                    array[x, y] = fillValue;

                    stack.Push((x + 1, y));
                    stack.Push((x - 1, y));
                    stack.Push((x, y + 1));
                    stack.Push((x, y - 1));
                }
            }
        }

        // https://rosettacode.org/wiki/Shoelace_formula_for_polygonal_area#C#
        internal static double CalculateArea(List<PointF> points)
        {
            int n = points.Count;
            double result = 0.0;
            for (int i = 0; i < n - 1; i++)
            {
                result += points[i].X * points[i + 1].Y - points[i + 1].X * points[i].Y;
            }

            result = Math.Abs(result + points[n - 1].X * points[0].Y - points[0].X * points[n - 1].Y) / 2.0;
            return result;
        }

        public class Workflow
        {
            public string Name { get; set; }
            public List<WorkflowRule> WorkflowRules { get; set; }

            public Workflow(string input)
            {
                int braceOpenIndex = input.IndexOf('{');
                int braceCloseIndex = input.IndexOf('}');

                Name = input.Substring(0, braceOpenIndex).Trim();

                WorkflowRules = new List<WorkflowRule>();
                string rulesStr = input.Substring(braceOpenIndex + 1, braceCloseIndex - braceOpenIndex - 1);

                string[] ruleTokens = rulesStr.Split(',');

                foreach (string ruleToken in ruleTokens)
                {
                    string[] ruleParts = ruleToken.Split(':');
                    string condition = string.Empty;
                    string destination = ruleParts[0].Trim();
                    if (ruleParts.Length > 1)
                    {
                        condition = ruleParts[0].Trim();
                        destination = ruleParts[1].Trim();
                    }

                    WorkflowRules.Add(new WorkflowRule { Condition = condition, Destination = destination });
                }
            }

        }

        public class WorkflowRule
        {
            public string Condition { get; set; }
            public string Destination { get; set; }
        }

        public class MachinePart
        {
            public int x { get; set; }
            public int m { get; set; }
            public int a { get; set; }
            public int s { get; set; }

            public MachinePart(string input)
            {
                if (input == string.Empty)
                {
                    x = 0; m = 0; a = 0; s = 0;
                }
                else
                {
                    string[] characteristics = input.Trim('{', '}').Split(',');
                    foreach (string characteristic in characteristics)
                    {
                        char c = characteristic[0];
                        int value = int.Parse(characteristic.Substring(2, characteristic.Length - 2));
                        if (c == 'x')
                            x = value;
                        else if (c == 'm')
                            m = value;
                        else if (c == 'a')
                            a = value;
                        else if (c == 's')
                            s = value;
                    }
                }
            }
        }

        internal static bool CompareToWorkflowRule(string condition, MachinePart machinePart)
        {
            if (condition == string.Empty)
            {
                return true;
            }
            char c = condition[0];
            int characteristicValue = -1;
            if (c == 'x')
            {
                characteristicValue = machinePart.x;
            }
            else if (c == 'm')
            {
                characteristicValue = machinePart.m;
            }
            else if (c == 'a')
            {
                characteristicValue = machinePart.a;
            }
            else
            {
                characteristicValue = machinePart.s;
            }
            char o = condition[1];
            int value = int.Parse(condition.Substring(2, condition.Length - 2));
            if (o == '>')
            {
                if (characteristicValue > value)
                    return true;
                else
                    return false;
            }
            else
            {
                if (characteristicValue < value)
                    return true;
                else
                    return false;

            }
        }

        public class Range
        {
            public int Min { get; set; }
            public int Max { get; set; }

            public Range(int min, int max)
            {
                Min = min;
                Max = max;
            }
        }

        public class MachinePartRanges
        {
            public Range x { get; set; }
            public Range m { get; set; }
            public Range a { get; set; }
            public Range s { get; set; }

            public MachinePartRanges()
            {
            }
        }

        internal static List<List<string>> FindAcceptPaths(Dictionary<string, List<WorkflowRule>> workflows)
        {
            var allAcceptPaths = new List<List<string>>();
            string startNode = "in";
            string startCondition = "";
            FindAcceptPathsFromNode(startNode, startCondition, workflows, new List<string>(), allAcceptPaths);
            return allAcceptPaths;
        }

        internal static void FindAcceptPathsFromNode(string currentNode, string startCondition, Dictionary<string, List<WorkflowRule>> graph, List<string> currentPath, List<List<string>> allAcceptPaths)
        {
            currentPath.Add(startCondition);

            if (!graph.ContainsKey(currentNode) || graph[currentNode].Count == 0)
            {
                // Accept path found, add the current path to the list of Accept paths
                allAcceptPaths.Add(new List<string>(currentPath));
                currentPath.RemoveAt(currentPath.Count - 1); // Backtrack
                return;
            }
            foreach (var rule in graph[currentNode])
            {
                if (rule.Destination == "A")
                {

                    string condition = string.Empty;
                    List<WorkflowRule> rules = graph[currentNode];
                    for (int i = 0; i < rules.Count; i++)
                    {
                        if (rules[i].Condition != rule.Condition)
                        {
                            condition += ";!" + rules[i].Condition;
                        }
                        else if (rules[i].Condition == rule.Condition)
                        {
                            condition += ";" + rule.Condition;
                            break;
                        }
                    }
                    currentPath.Add(condition);

                    // Accept path found, add the current path to the list of Accept paths
                    allAcceptPaths.Add(new List<string>(currentPath));
                    currentPath.RemoveAt(currentPath.Count - 1);
                }
                else if (rule.Destination == "R")
                {
                    // do nothing
                }
                else if (rule.Destination != null && !currentPath.Contains(rule.Destination))
                {
                    // Continue traversing if the rule leads to another node and it's not a cycle
                    string condition = string.Empty;
                    List<WorkflowRule> rules = graph[currentNode];
                    for (int i = 0; i < rules.Count; i++)
                    {
                        if (rules[i].Condition != rule.Condition)
                        {
                            condition += ";!" + rules[i].Condition;
                        }
                        else if (rules[i].Condition == rule.Condition)
                        {
                            condition += ";" + rule.Condition;
                            break;
                        }
                    }
                    FindAcceptPathsFromNode(rule.Destination, condition, graph, currentPath, allAcceptPaths);
                }
            }

            currentPath.RemoveAt(currentPath.Count - 1); // Backtrack
        }

        internal static List<MachinePartRanges> FindAcceptRanges(List<List<string>> acceptPaths)
        {
            List<MachinePartRanges> mpRanges = new List<MachinePartRanges>();
            foreach (List<string> path in acceptPaths)
            {
                MachinePartRanges mpRange = new MachinePartRanges();
                List<Range> xRanges = new List<Range>();
                List<Range> mRanges = new List<Range>();
                List<Range> aRanges = new List<Range>();
                List<Range> sRanges = new List<Range>();
                foreach (string acceptPath in path)
                {
                    string replacedPath = acceptPath.Replace(";;", ";");
                    if (replacedPath != "")
                    {
                        string[] rules = replacedPath.Trim(';').Trim().Split(';');
                        char c;
                        Range r;
                        foreach (string rule in rules)
                        {
                            string checkRule = rule.Trim(';');
                            if (checkRule[0] != '!')
                            {
                                c = checkRule[0];
                                r = GetRangeFromAcceptPath(checkRule.Substring(1, checkRule.Length - 1));
                            }
                            else
                            {
                                c = checkRule[1];
                                r = GetRangeFromAcceptPath(checkRule[0] + checkRule.Substring(2, checkRule.Length - 2));
                            }
                            if (c == 'x')
                                xRanges.Add(r);
                            else if (c == 'm')
                                mRanges.Add(r);
                            else if (c == 'a')
                                aRanges.Add(r);
                            else // c == 's'
                                sRanges.Add(r);
                        }

                    }
                }
                int rangeMin = 1;
                int rangeMax = 4000;
                foreach (Range r in xRanges)
                {
                    if (r.Min > rangeMin)
                        rangeMin = r.Min;
                    if (r.Max < rangeMax)
                        rangeMax = r.Max;
                }
                mpRange.x = new Range(rangeMin, rangeMax);
                rangeMin = 1;
                rangeMax = 4000;
                foreach (Range r in mRanges)
                {
                    if (r.Min > rangeMin)
                        rangeMin = r.Min;
                    if (r.Max < rangeMax)
                        rangeMax = r.Max;
                }
                mpRange.m = new Range(rangeMin, rangeMax);
                rangeMin = 1;
                rangeMax = 4000;
                foreach (Range r in aRanges)
                {
                    if (r.Min > rangeMin)
                        rangeMin = r.Min;
                    if (r.Max < rangeMax)
                        rangeMax = r.Max;
                }
                mpRange.a = new Range(rangeMin, rangeMax);
                rangeMin = 1;
                rangeMax = 4000;
                foreach (Range r in sRanges)
                {
                    if (r.Min > rangeMin)
                        rangeMin = r.Min;
                    if (r.Max < rangeMax)
                        rangeMax = r.Max;
                }
                mpRange.s = new Range(rangeMin, rangeMax);
                mpRanges.Add(mpRange);
            }
            return mpRanges;
        }

        private static Range GetRangeFromAcceptPath(string acceptPath)
        {
            char o = acceptPath[0];
            if (o == '!')
            {
                o = acceptPath[1];
                int value = int.Parse(acceptPath.Substring(2, acceptPath.Length - 2));
                if (o == '>') // ! x > value (x <= value )
                {
                    Range r = new Range(1, value);
                    return r;
                }
                else // (o == '<') // ! x < value ( x>= value )
                {
                    Range r = new Range(value, 4000);
                    return r;
                }
            }
            else
            {
                int value = int.Parse(acceptPath.Substring(1, acceptPath.Length - 1));
                if (o == '>') // x > value
                {
                    Range r = new Range(value + 1, 4000);
                    return r;
                }
                else // (o == '<') // x < value
                {
                    Range r = new Range(1, value - 1);
                    return r;
                }

            }

        }


        public class Module
        {
            public string Name { get; set; }
            public int Pulse { get; set; }
            public virtual int SendPulse()
            {
                return 0;
            }
            public virtual bool ReceivePulse(int pulse, string sender)
            {
                return false;
            }
            public List<string> Modules { get; } = new List<string>();

            public Module(string name)
            {
                Name = name;
            }
        }

        public class FlipFlop : Module
        {
            public bool state = false;
            public FlipFlop(string name) : base(name) { }
            public override int SendPulse()
            {
                return state ? 0 : 1;
            }

            public override bool ReceivePulse(int pulse, string sender)
            {
                //Console.WriteLine("\t\t" + Name + " <- " + pulse + " :: " + " -> " + Pulse);
                Pulse = SendPulse();
                if (pulse == 0)
                {
                    state = !state;
                    return true;
                }
                return false;
            }
        }

        public class Conjunction : Module
        {
            public Conjunction(string name) : base(name) { }
            private Dictionary<string, int> inputModules = new Dictionary<string, int>();

            public override int SendPulse()
            {
                foreach (KeyValuePair<string, int> kvp in inputModules)
                {
                    if (kvp.Value != 1)
                    {
                        return 1;
                    }
                }
                return 0;
            }

            public override bool ReceivePulse(int pulse, string sender)
            {
                //Console.WriteLine("\t\t" + Name + " <- " + pulse + " :: " + " -> " + Pulse);
                if (!inputModules.ContainsKey(sender))
                {
                    inputModules.Add(sender, pulse);
                }
                else
                {
                    inputModules[sender] = pulse;
                }
                Pulse = SendPulse();
                return true;
            }
        }

        internal static void PrintMap(char[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i, j]);
                }
                Console.WriteLine();
            }
        }


        internal static HashSet<(int, int)> GetVisitedPositions(int currentX, int currentY, int stepsLeft, ref Dictionary<(int, int, int), HashSet<(int, int)>> memo, char[,] map)
        {
            var stack = new Stack<(int x, int y, int steps)>();
            var visited = new HashSet<(int, int)>();
            stack.Push((currentX, currentY, stepsLeft));

            while (stack.Count > 0)
            {
                var (x, y, steps) = stack.Pop();

                if (steps == 0)
                {
                    visited.Add((x, y));
                    continue;
                }

                if (memo.TryGetValue((x, y, steps), out var cachedResult))
                {
                    visited.UnionWith(cachedResult);
                    continue;
                }

                var uniqueFields = new HashSet<(int, int)>();

                // Move Up
                if (x - 1 >= 0 && map[x - 1, y] == '.')
                {
                    stack.Push((x - 1, y, steps - 1));
                }

                // Move Down
                if (x + 1 < map.GetLength(0) && map[x + 1, y] == '.')
                {
                    stack.Push((x + 1, y, steps - 1));
                }

                // Move Left
                if (y - 1 >= 0 && map[x, y - 1] == '.')
                {
                    stack.Push((x, y - 1, steps - 1));
                }

                // Move Right
                if (y + 1 < map.GetLength(1) && map[x, y + 1] == '.')
                {
                    stack.Push((x, y + 1, steps - 1));
                }

                memo[(x, y, steps)] = uniqueFields;
                visited.UnionWith(uniqueFields);
            }

            return visited;
        }

        internal static char[,] Create3x3Map(char[,] inputMap)
        {
            int inputX = inputMap.GetLength(0);
            int inputY = inputMap.GetLength(1);
            int newX = inputX * 3;
            int newY = inputY * 3;

            char[,] newMap = new char[newX, newY];

            for (int i = 0; i < newX; i++)
            {
                for (int j = 0; j < newY; j++)
                {
                    // Calculate the corresponding position in the original map
                    int inputRow = i % inputX;
                    int inputCol = j % inputY;

                    // Copy the value from the original map, replacing 'S' if necessary
                    if (inputMap[inputRow, inputCol] == 'S')
                    {
                        // Place 'S' in the middle grid, '.' in other grids
                        newMap[i, j] = (i / inputX == 1 && j / inputY == 1) ? 'S' : '.';
                    }
                    else
                    {
                        newMap[i, j] = inputMap[inputRow, inputCol];
                    }
                }
            }

            return newMap;
        }

        public enum Direction
        {
            Up,
            Down,
            Left,
            Right,
            Invalid
        }
        internal static Direction CanEnter(Direction comingFrom, char candidate)
        {
            // │ └ ┐┌ ┘ ─;
            // 0 down, 1 up, 2 left, 3 right

            if (comingFrom == Direction.Down)
            {
                if (candidate == '│')
                {
                    return Direction.Up;
                }
                else if (candidate == '┐')
                {
                    return Direction.Left;
                }
                else if (candidate == '┌')
                {
                    return Direction.Right;
                }
                else
                {
                    return Direction.Invalid;
                }

            }
            else if (comingFrom == Direction.Up)
            {
                if (candidate == '│')
                {
                    return Direction.Down;
                }
                else if (candidate == '┘')
                {
                    return Direction.Left;
                }
                else if (candidate == '└')
                {
                    return Direction.Right;
                }
                else
                {
                    return Direction.Invalid;
                }
            }
            else if (comingFrom == Direction.Left)
            {
                if (candidate == '┘')
                {
                    return Direction.Up;
                }
                else if (candidate == '┐')
                {
                    return Direction.Down;
                }
                else if (candidate == '─')
                {
                    return Direction.Right;
                }
                else
                {
                    return Direction.Invalid;
                }
            }
            else if (comingFrom == Direction.Right)
            {
                if (candidate == '└')
                {
                    return Direction.Up;
                }
                else if (candidate == '┌')
                {
                    return Direction.Down;
                }
                else if (candidate == '─')
                {
                    return Direction.Left;
                }
                else
                {
                    return Direction.Invalid;
                }
            }
            return Direction.Invalid;
        }
        internal static (int x, int y, int maxDistance) FindFurthestPointFromStart(int startX, int startY, char[,] map)
        {

            int mapX = map.GetLength(0);
            int mapY = map.GetLength(1);
            bool[,] visited = new bool[mapX, mapY];
            Queue<(int x, int y, int distance)> queue = new Queue<(int, int, int)>();
            queue.Enqueue((startX, startY, 0));
            visited[startX, startY] = true;
            (int x, int y) furthestPoint = (startX, startY);
            int maxDistance = 0;
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                int x = current.Item1;
                int y = current.Item2;
                int distance = current.Item3;

                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    furthestPoint = (x, y);
                }
                // Explore all four directions
                CheckAndEnqueue(x - 1, y, Direction.Down, distance + 1, map, visited, queue); // Up
                CheckAndEnqueue(x + 1, y, Direction.Up, distance + 1, map, visited, queue);   // Down
                CheckAndEnqueue(x, y - 1, Direction.Right, distance + 1, map, visited, queue); // Left
                CheckAndEnqueue(x, y + 1, Direction.Left, distance + 1, map, visited, queue);  // Right
            }
            return (furthestPoint.x, furthestPoint.y, maxDistance);
        }

        private static void CheckAndEnqueue(int x, int y, Direction comingFrom, int distance, char[,] map, bool[,] visited, Queue<(int, int, int)> queue)
        {
            int mapX = map.GetLength(0);
            int mapY = map.GetLength(1);

            if (x >= 0 && x < mapX && y >= 0 && y < mapY && !visited[x, y])
            {
                char candidate = map[x, y];
                Direction move = CanEnter(comingFrom, candidate);

                if (move != Direction.Invalid)
                {
                    queue.Enqueue((x, y, distance));
                    visited[x, y] = true;
                }
            }
        }

        private static Direction FindFirstMove(int startX, int startY, char[,] map)
        {
            char candidate;
            Direction firstMove = Direction.Invalid;
            int mapX = map.GetLength(0);
            int mapY = map.GetLength(1);
            if (startX > 0)
            {
                //check Up
                candidate = map[startX - 1, startY];
                firstMove = CanEnter(Direction.Down, candidate);
                if (firstMove != Direction.Invalid)
                    return firstMove;
            }
            if (startY < mapY)
            {
                //check Right
                candidate = map[startX, startY + 1];
                firstMove = CanEnter(Direction.Left, candidate);
                if (firstMove != Direction.Invalid)
                    return firstMove;
            }

            if (startY > 0)
            {
                //check Left
                candidate = map[startX, startY - 1];
                firstMove = CanEnter(Direction.Right, candidate);
                if (firstMove != Direction.Invalid)
                    return firstMove;
            }

            if (startX < mapX)
            {
                //check Down
                candidate = map[startX + 1, startY];
                firstMove = CanEnter(Direction.Right, candidate);
                if (firstMove != Direction.Invalid)
                    return firstMove;
            }

            return firstMove;
        }

        public static List<PointF> TraversePath(char[,] map, int startX, int startY)
        {
            List<PointF> path = new List<PointF>();
            int currentX = startX, currentY = startY;
            Direction currentDirection = FindFirstMove(startX, startY, map);

            while (true)
            {
                path.Add(new PointF(currentX, currentY));

                // Update position based on current direction
                switch (currentDirection)
                {
                    case Direction.Up:
                        currentDirection = Direction.Down;
                        currentX--;
                        break;
                    case Direction.Down:
                        currentDirection = Direction.Up;
                        currentX++;
                        break;
                    case Direction.Left:
                        currentDirection = Direction.Right;
                        currentY--;
                        break;
                    case Direction.Right:
                        currentDirection = Direction.Left;
                        currentY++;
                        break;
                    default:
                        return path; // Exit if direction is invalid
                }

                // Check if returned to the starting position
                if (currentX == startX && currentY == startY)
                    break;

                // Update direction for the next move
                char candidate = map[currentX, currentY];
                currentDirection = CanEnter(currentDirection, candidate);
            }

            return path;

        }


        public class Point3D
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }

            public Point3D(int x, int y, int z)
            {
                X = x;
                Y = y;
                Z = z;
            }
            public override string ToString()
            {
                return $"({X}, {Y}, {Z})";
            }
        }

        public class Brick
        {
            public string Snapshot { get; set; }
            public Point3D CurrentStart { get; set; }
            public Point3D CurrentEnd { get; set; }

            public bool Falling { get; set; }

            public List<Brick> SupportingBricks { get; set; }
            public List<Brick> SupportedByBricks { get; set; }

            public Brick(string snapshot)
            {
                Snapshot = snapshot;
                string[] brickPositions = Snapshot.Split('~');
                string[] brickPosition1 = brickPositions[0].Split(',');
                string[] brickPosition2 = brickPositions[1].Split(',');
                CurrentStart = new Point3D(int.Parse(brickPosition1[0]), int.Parse(brickPosition1[1]), int.Parse(brickPosition1[2]));
                CurrentEnd = new Point3D(int.Parse(brickPosition2[0]), int.Parse(brickPosition2[1]), int.Parse(brickPosition2[2]));
                SupportingBricks = new List<Brick>();
                SupportedByBricks = new List<Brick>();
                Falling = true;
            }

            public void DropBrick(List<Brick> placedBricks)
            {
                foreach (var brick in placedBricks)
                {
                    int brickBotZ = CurrentStart.Z;
                    int candidateBotZ = brick.CurrentEnd.Z;

                    if (brickBotZ == candidateBotZ)
                    {
                        int brickTopZ = BricksCollide(this, brick);
                        if (brickTopZ > 0)
                        {
                            Falling = false;
                            int height = CurrentEnd.Z - CurrentStart.Z;
                            CurrentStart.Z = brickTopZ + 1;
                            CurrentEnd.Z = height + brickTopZ + 1;
                            placedBricks.Add(this);
                            return;
                        }
                    }
                }
                if (Falling)
                {
                    CurrentStart.Z -= 1;
                    CurrentEnd.Z -= 1;
                }
                int minZ = CurrentStart.Z;
                if (minZ == 0)
                {
                    CurrentStart.Z += 1;
                    CurrentEnd.Z += 1;
                    Falling = false;
                    placedBricks.Add(this);
                    return;
                }
            }

            private int BricksCollide(Brick brick1, Brick brick2)
            {
                bool overlapsInX = brick1.CurrentStart.X <= brick2.CurrentEnd.X && brick1.CurrentEnd.X >= brick2.CurrentStart.X;
                bool overlapsInY = brick1.CurrentStart.Y <= brick2.CurrentEnd.Y && brick1.CurrentEnd.Y >= brick2.CurrentStart.Y;
                int brick2TopZ = brick2.CurrentEnd.Z;
                return overlapsInX && overlapsInY ? brick2TopZ : -1;
            }

            public override string ToString()
            {
                string status = Falling ? "Falling" : "Landed";
                return $"{Snapshot} => [Start: {CurrentStart}] , [End: {CurrentEnd}] -- {status}";
            }

            internal void AddSupportingBricks(List<Brick> placedBricks)
            {
                int topZ = CurrentEnd.Z;
                foreach (Brick b in placedBricks)
                {
                    if (b == this) continue;

                    int brickBotZ = b.CurrentStart.Z;
                    if (brickBotZ == topZ + 1)
                    {
                        if (BricksCollide(this, b) > 0)
                        {
                            SupportingBricks.Add(b);
                            b.SupportedByBricks.Add(this);
                        }
                    }
                }
            }

            public string PrintSupportingBricks()
            {
                if (SupportingBricks.Count == 0)
                {
                    return $"{Snapshot} is supporting: None";
                }
                var supportingSnapshots = SupportingBricks.Select(brick => brick.Snapshot);
                return $"{Snapshot} is supporting: {string.Join(", ", supportingSnapshots)}";
            }

            public string PrintSupportedByBricks()
            {
                if (SupportedByBricks.Count == 0)
                {
                    return $"{Snapshot} is supported by: None";
                }
                var supportedBySnapshots = SupportedByBricks.Select(brick => brick.Snapshot);
                return $"{Snapshot} is supportedBy: {string.Join(", ", supportedBySnapshots)}";
            }

            private void CountFallingBricks(Brick brick, HashSet<Brick> fallingBricks)
            {
                foreach (var supportedBrick in brick.SupportingBricks)
                {
                    if (supportedBrick.SupportedByBricks.Count() == 1)
                    {
                        supportedBrick.Falling = true;
                        if (fallingBricks.Add(supportedBrick))
                        {
                            CountFallingBricks(supportedBrick, fallingBricks);
                        }
                    }
                    else
                    {
                        int falling = 0;
                        foreach (Brick supporterBrick in supportedBrick.SupportedByBricks)
                        {
                            if (supporterBrick.Falling)
                            {
                                falling++;
                            }
                        }
                        if (falling == supportedBrick.SupportedByBricks.Count())
                        {
                            supportedBrick.Falling = true;
                            if (fallingBricks.Add(supportedBrick))
                            {
                                CountFallingBricks(supportedBrick, fallingBricks);
                            }
                        }
                    }
                }
            }

            public int CalculateFallingBricks()
            {
                Falling = true;
                HashSet<Brick> fallingBricks = new HashSet<Brick>();
                CountFallingBricks(this, fallingBricks);
                return fallingBricks.Count;
            }

        }

        internal static int HashString(string input)
        {
            int currentValue = 0;
            foreach (char c in input)
            {
                int asciiCode = (int)c;
                currentValue += asciiCode;
                currentValue *= 17;
                currentValue %= 256;
            }
            return currentValue;
        }

        internal static List<Tuple<Point, Point>> GeneratePairs(List<Point> points)
        {
            var pairs = new List<Tuple<Point, Point>>();
            for (int i = 0; i < points.Count; i++)
            {
                for (int j = i + 1; j < points.Count; j++)
                {
                    pairs.Add(new Tuple<Point, Point>(points[i], points[j]));
                }
            }
            return pairs;
        }

        internal static int FindShortestPath(Point start, Point end, (int, int) extraDistance)
        {
            int xDistance = Math.Abs(end.X - start.X) + extraDistance.Item1;
            int yDistance = Math.Abs(end.Y - start.Y) + extraDistance.Item2;
            int totalDistance = xDistance + yDistance;
            return totalDistance;
        }

        internal static (int, int) FindExtraDistance(Point start, Point end, List<int> rowsWithoutGalaxy, List<int> colsWithoutGalaxy)
        {
            int extraYDistance = 0;
            if (start.X < end.X)
            {
                for (int i = start.X + 1; i < end.X; i++)
                {
                    if (rowsWithoutGalaxy.Contains(i))
                    {
                        extraYDistance++;
                    }
                }
            }
            else if (start.X > end.X)
            {
                for (int i = end.X + 1; i < start.X; i++)
                {
                    if (rowsWithoutGalaxy.Contains(i))
                    {
                        extraYDistance++;
                    }
                }
            }

            int extraXDistance = 0;
            if (start.Y < end.Y)
            {
                for (int i = start.Y + 1; i < end.Y; i++)
                {
                    if (colsWithoutGalaxy.Contains(i))
                    {
                        extraXDistance++;
                    }
                }
            }
            else if (start.Y > end.Y)
            {
                for (int i = end.Y + 1; i < start.Y; i++)
                {
                    if (colsWithoutGalaxy.Contains(i))
                    {
                        extraXDistance++;
                    }
                }
            }

            return (extraXDistance, extraYDistance);
        }
        internal static long FindShortestPathMultiple(Point start, Point end, (long, long) extraDistance)
        {
            long xDistance = Math.Abs(end.X - start.X) + extraDistance.Item1;
            long yDistance = Math.Abs(end.Y - start.Y) + extraDistance.Item2;
            long totalDistance = xDistance + yDistance;
            return totalDistance;
        }
        internal static (long, long) FindExtraDistanceMultiple(Point start, Point end, List<int> rowsWithoutGalaxy, List<int> colsWithoutGalaxy, long multiple = 999999)
        {
            long extraYDistance = 0;
            if (start.X < end.X)
            {
                for (int i = start.X + 1; i < end.X; i++)
                {
                    if (rowsWithoutGalaxy.Contains(i))
                    {
                        extraYDistance = extraYDistance + multiple;
                    }
                }
            }
            else if (start.X > end.X)
            {
                for (int i = end.X + 1; i < start.X; i++)
                {
                    if (rowsWithoutGalaxy.Contains(i))
                    {
                        extraYDistance = extraYDistance + multiple;
                    }
                }
            }

            long extraXDistance = 0;
            if (start.Y < end.Y)
            {
                for (int i = start.Y + 1; i < end.Y; i++)
                {
                    if (colsWithoutGalaxy.Contains(i))
                    {
                        extraXDistance = extraXDistance + multiple;
                    }
                }
            }
            else if (start.Y > end.Y)
            {
                for (int i = end.Y + 1; i < start.Y; i++)
                {
                    if (colsWithoutGalaxy.Contains(i))
                    {
                        extraXDistance = extraXDistance + multiple;
                    }
                }
            }

            return (extraXDistance, extraYDistance);
        }


        public class PathNode
        {
            public Point Start { get; set; }
            public Point End { get; set; }

            public int Length { get; set; }

            public Point StartConjuction { get; set; }
            public Point EndConjuction { get; set; }

            public PathNode(Point start, Point end, int length)
            {
                Start = start;
                End = end;
                Length = length;
            }

            public override string ToString()
            {
                return $"{Start} : [{Length}] : ({StartConjuction} - {EndConjuction}]";
            }

        }

        public class PathConjuction
        {
            public Point ConjuctionPoint { get; set; }
            public bool HasBelow;
            public bool HasRight;

            public PathNode AbovePathNode { get; set; }
            public PathNode BelowPathNode { get; set; }
            public PathNode LeftPathNode { get; set; }
            public PathNode RightPathNode { get; set; }

            public PathConjuction(Point c)
            {
                ConjuctionPoint = c;
            }
        }

        internal static void BuildGraph(char[,] map, Point startP, out Dictionary<Point, PathNode> pathNodes, out Dictionary<Point, PathConjuction> pathConjuctions)
        {
            int rows = map.GetLength(0);
            int cols = map.GetLength(1);
            Direction d = Direction.Down;
            pathNodes = new Dictionary<Point, PathNode>();
            pathConjuctions = new Dictionary<Point, PathConjuction>();
            AddPointAndConjuction(map, startP, pathNodes, pathConjuctions);
        }


        internal static void AddPointAndConjuction(char[,] map, Point startP, Dictionary<Point, PathNode> pathNodes, Dictionary<Point, PathConjuction> pathConjuctions)
        {
            Point end = FindNodeEndPoint(map, startP, out int length);
            PathNode pn = new PathNode(startP, end, length);
            Point cp = GetPathConjuction(map, pn, pathConjuctions);
            pn.EndConjuction = cp;
            if (!pathNodes.ContainsKey(startP))
            {
                pathNodes.Add(startP, pn);
            }
            if (cp.X > -1)
            {
                if (pathConjuctions[cp].HasRight)
                {
                    Point new_startP = new Point(cp.X, cp.Y + 2);
                    AddPointAndConjuction(map, new_startP, pathNodes, pathConjuctions);
                    PathNode toAdd = pathNodes[new_startP];
                    pathConjuctions[cp].RightPathNode = toAdd;
                    pathNodes[new_startP].StartConjuction = cp;
                }
                if (pathConjuctions[cp].HasBelow)
                {
                    Point new_startP = new Point(cp.X + 2, cp.Y);
                    AddPointAndConjuction(map, new_startP, pathNodes, pathConjuctions);
                    PathNode toAdd = pathNodes[new_startP];
                    pathConjuctions[cp].BelowPathNode = toAdd;
                    pathNodes[new_startP].StartConjuction = cp;

                }
            }
        }
        private static Point GetPathConjuction(char[,] map, PathNode pn, Dictionary<Point, PathConjuction> pathConjuctions)
        {
            int i = pn.End.X;
            if (i == map.GetLength(0) - 1)
            {
                return new Point(-1, -1);
            }
            int j = pn.End.Y;
            char c = map[i, j];
            Point mid = new Point();
            bool fromLeft = false;
            if (c == '>')
            {
                j = j + 1;
                fromLeft = true;
            }
            else if (c == 'v')
            {
                i = i + 1;
            }
            mid.X = i;
            mid.Y = j;

            if (!pathConjuctions.ContainsKey(mid))
            {
                pathConjuctions[mid] = new PathConjuction(mid);
                int rows = map.GetLength(0);
                int cols = map.GetLength(1);
                if (i + 1 < rows && (map[i + 1, j] == 'v'))
                {
                    pathConjuctions[mid].HasBelow = true;
                }
                if (j + 1 < cols && (map[i, j + 1] == '>'))
                {
                    pathConjuctions[mid].HasRight = true;
                }
            }
            if (fromLeft)
            {
                pathConjuctions[mid].LeftPathNode = pn;
            }
            else
            {
                pathConjuctions[mid].AbovePathNode = pn;
            }

            return mid;
        }

        internal static Point FindNodeEndPoint(char[,] map, Point start, out int length)
        {
            int i = start.X;
            int j = start.Y;
            char c = map[i, j];
            int steps = 0;

            int mapX = map.GetLength(0);
            int mapY = map.GetLength(1);

            bool[,] visited = new bool[mapX, mapY];
            visited[i, j] = true;
            while (true)
            {
                if (i + 1 < mapX && (map[i + 1, j] == '.' || map[i + 1, j] == 'v' || map[i + 1, j] == '>') && !visited[i + 1, j]) // down
                {
                    i = i + 1;
                    visited[i, j] = true;
                    steps++;
                }
                if (j + 1 < mapY && (map[i, j + 1] == '.' || map[i, j + 1] == 'v' || map[i, j + 1] == '>') && !visited[i, j + 1]) // right
                {
                    j = j + 1;
                    visited[i, j] = true;
                    steps++;
                }
                if (i > 0 && (map[i - 1, j] == '.' || map[i - 1, j] == 'v' || map[i - 1, j] == '>') && !visited[i - 1, j]) // up
                {
                    if (!(map[i - 1, j] == 'v' && steps == 0))
                    {
                        i = i - 1;
                        visited[i, j] = true;
                        steps++;
                    }
                }
                if (j > 0 && (map[i, j - 1] == '.' || map[i, j - 1] == 'v' || map[i, j - 1] == '>') && !visited[i, j - 1]) // left
                {
                    if (!(map[i, j - 1] == '>' && steps == 0))
                    {
                        j = j - 1;
                        visited[i, j] = true;
                        steps++;
                    }
                }

                if (map[i, j] == '>' || map[i, j] == 'v' || i == mapX - 1)
                {
                    if (i == mapX - 1)
                        steps++;
                    break;
                }
            }
            length = steps;
            return new Point(i, j);
        }


        internal static List<List<PathNode>> FindAllPaths(Dictionary<Point, PathNode> pathNodes, Dictionary<Point, PathConjuction> pathConjuctions, Point start, Point end, bool slipperySlopes)
        {
            var visited = new HashSet<PathConjuction>();
            List<PathNode> currentPath = new List<PathNode>();
            PathNode pn = pathNodes[start];
            Point lastPathStart = new Point();
            foreach (KeyValuePair<Point, PathNode> p in pathNodes)
            {
                if (p.Value.EndConjuction.X == -1)
                {
                    lastPathStart = p.Value.Start;
                }
            }
            PathNode pe = pathNodes[lastPathStart];
            PathConjuction lastConjuction = pathConjuctions[pe.StartConjuction];
            List<PathNode> endingNodes = new List<PathNode>();
            if (lastConjuction.AbovePathNode != null)
            {
                endingNodes.Add(pathNodes[lastConjuction.AbovePathNode.Start]);
            }
            if (lastConjuction.LeftPathNode != null)
            {
                endingNodes.Add(pathNodes[lastConjuction.LeftPathNode.Start]);
            }
            currentPath.Add(pn);
            List<List<PathNode>> allPaths = DFSAllPaths(pathNodes, pathConjuctions, start, end, currentPath, visited, slipperySlopes, Direction.Down, endingNodes);
            return allPaths;
        }

        private static List<List<PathNode>> DFSAllPaths(Dictionary<Point, PathNode> pathNodes, Dictionary<Point, PathConjuction> pathConjuctions, Point current, Point end, List<PathNode> currentPath, HashSet<PathConjuction> visited, bool slipperySlopes, Direction direction, List<PathNode> endingNodes)
        {
            PathNode pn = pathNodes[current];

            if (endingNodes.Contains(pn))
            {
                //currentPath.Add(pn);
                return new List<List<PathNode>> { new List<PathNode>(currentPath) };
            }
            PathConjuction pc = pathConjuctions[pathNodes[current].EndConjuction];
            if (direction == Direction.Left || direction == Direction.Up)
            {
                if (! (pathNodes[current].StartConjuction.X == 0)) // start
                    pc = pathConjuctions[pathNodes[current].StartConjuction];
            }
            if (visited.Contains(pc))
            {
                return new List<List<PathNode>>();
            }

            visited.Add(pc);
            var allPaths = new List<List<PathNode>>();



            if (pc.RightPathNode != null)
            {
                currentPath.Add(pc.RightPathNode);
                allPaths.AddRange(DFSAllPaths(pathNodes, pathConjuctions, pc.RightPathNode.Start, end, currentPath, visited, slipperySlopes, Direction.Right, endingNodes));
                currentPath.RemoveAt(currentPath.Count - 1); // Backtrack
            }
            if (pc.BelowPathNode != null)
            {
                currentPath.Add(pc.BelowPathNode);
                allPaths.AddRange(DFSAllPaths(pathNodes, pathConjuctions, pc.BelowPathNode.Start, end, currentPath, visited, slipperySlopes, Direction.Down, endingNodes));
                currentPath.RemoveAt(currentPath.Count - 1); // Backtrack
            }
            if (slipperySlopes && pc.LeftPathNode != null)
            {
                currentPath.Add(pc.LeftPathNode);
                allPaths.AddRange(DFSAllPaths(pathNodes, pathConjuctions, pc.LeftPathNode.Start, end, currentPath, visited, slipperySlopes, Direction.Left, endingNodes));
                currentPath.RemoveAt(currentPath.Count - 1); // Backtrack
            }
            if (slipperySlopes && pc.AbovePathNode != null)
            {
                currentPath.Add(pc.AbovePathNode);
                allPaths.AddRange(DFSAllPaths(pathNodes, pathConjuctions, pc.AbovePathNode.Start, end, currentPath, visited, slipperySlopes, Direction.Up, endingNodes));
                currentPath.RemoveAt(currentPath.Count - 1); // Backtrack
            }

            visited.Remove(pc);
            return allPaths;
        }
    }
}