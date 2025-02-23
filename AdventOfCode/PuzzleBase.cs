﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AdventOfCode
{
    public class PuzzleBase
    {
        public int Day = 0;
        public bool IsTesting = false;
        public Dictionary<string, long> Timers = new Dictionary<string, long>();
        public Dictionary<string, long> IterationCounters = new Dictionary<string, long>();
        private Dictionary<string, Stopwatch> Stopwatches = new Dictionary<string, Stopwatch>();

        public void Init(int day, bool isTesting)
        {
            this.Day = day;
            this.IsTesting = isTesting;
        }

        public void StartTimer(string key)
        {
            if (!Timers.ContainsKey(key))
            {
                Timers[key] = 0;
                Stopwatches[key] = new Stopwatch();
            }

            Stopwatches[key].Reset();
            Stopwatches[key].Start();
        }
        
        public void AddIteration(string key)
        {
            if (!IterationCounters.ContainsKey(key))
            {
                IterationCounters[key] = 0;
            }

            IterationCounters[key]++;
        }

        public void EndTimer(string key)
        {
            Timers[key] += Stopwatches[key].ElapsedMilliseconds;
            Stopwatches[key].Reset();
            Stopwatches[key].Stop();
        }

        public void PrintTimers()
        {
            foreach (var pair in Timers)
            {
                Console.WriteLine($"{pair.Key}: {pair.Value}ms");
            }
        }
        
        public void PrintIterations()
        {
            foreach (var pair in IterationCounters)
            {
                Console.WriteLine($"{pair.Key}: {pair.Value} iterations");
            }
        }
        
        public string GetPuzzleText()
        {
            return File.ReadAllText(GetPuzzleFilename());
        }

        public string[] GetPuzzleLines()
        {
            return File.ReadAllLines(GetPuzzleFilename());
        }

        public void TestExpected<TType>(TType actual, TType expected)
        {
            var actualString = actual.ToString();
            var expectedString = expected.ToString();
            var color = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("EXPECTED: ");
            Console.Write(expected);
            Console.Write("\n");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("ACTUAL  : ");
            Console.ForegroundColor = ConsoleColor.Red;
            if (actualString == expectedString)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            Console.Write(actual);
            Console.Write("\n");
            Console.ForegroundColor = color;
            Console.WriteLine("---------------------------------");
        }
        
        public void TestNotExpected(string actual, List<string> notExpected)
        {
            var color = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("ACTUAL  : ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (notExpected.Contains(actual))
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.Write(actual);
            Console.Write("\n");
            Console.ForegroundColor = color;
            Console.WriteLine("---------------------------------");
        }

        public List<string[]> LinesToArray(IEnumerable<string> lines, string separator)
        {
            if (separator == "")
                return lines.Select(x => x.ToArray().Select(x => x.ToString()).ToArray()).ToList();
                
            return lines.Select(x => x.Split(separator)).ToList();
        }

        public Dictionary<string, string> ToDictionary(List<string[]> lines)
        {
            return lines.ToDictionary(x => x[0], x => x[1]);
        }

        public int[] LinesToInts(IEnumerable<string> lines)
        {
            return lines.Select(x => Convert.ToInt32(x)).ToArray();
        }

        public List<int[]> ArrayLinesToInts(IEnumerable<string[]> lines)
        {
            return lines.Select(x => x.Select(y => Convert.ToInt32(y)).ToArray()).ToList();
        }
        
        public string GetPuzzleFilename()
        {
            var testPrefix = IsTesting ? "test" : "";
            
            return $"Year{Program.Year}\\Day{Day}\\{testPrefix}input.txt";
        }
        
        public string GetPuzzlePath()
        {
            return $"Year{Program.Year}\\Day{Day}\\";
        }
    }
}