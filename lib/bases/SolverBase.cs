using advent_of_code.lib.helpers;
using advent_of_code_lib.extensions;
using advent_of_code_lib.interfaces;
using System.Diagnostics;

namespace advent_of_code_lib.bases
{
    public abstract class SolverBase : ISolver
    {
        public string Folder { get; set; } = null!;

        public string[] GetAllLines(string folder)
            => File.ReadAllLines(Path.Combine(folder, $"{this.GetProblemInfo().Day}.in"));

        public string GetAllData(string folder)
            => File.ReadAllText(Path.Combine(folder, $"{this.GetProblemInfo().Day}.in"));

        protected static Dictionary<int, ConsoleColor> BenchmarkColors => new()
        {
            { 50, ConsoleColor.Green },
            { 120, ConsoleColor.DarkGreen },
            { 180, ConsoleColor.Yellow },
            { 300, ConsoleColor.DarkYellow },
            { 500, ConsoleColor.Red },
            { 750, ConsoleColor.DarkRed }
        };

        private void PrintSolutionPart(object? result, long benchmarkTime, int part)
        {
            Console.Write($"Part {part}, solved in ");
            PrintTime(benchmarkTime);
            Console.Write($" ms".PadRight(15 - benchmarkTime.ToString().Length) + ": ");
            using (ColorScope.CreateScope(ConsoleColor.Yellow))
            {
                Console.Write($"{(result?.ToString() ?? "")} \r\n");
            };
        }

        public void PrintSolutionHeader()
        {
            var info = this.GetProblemInfo();

            Console.Write("[");
            using (ColorScope.CreateScope(ConsoleColor.Yellow))
            {
                Console.Write($"Day {info.Day}: {info.ProblemName}");
            };
            Console.Write("]\r\n\r\n");
        }

        public void Solve(string folder)
        {
            Folder = folder;
            PrintSolutionHeader();

            if (this.IsSlow())
            {
                using(ColorScope.CreateScope(ConsoleColor.DarkRed))
                {
                    Console.WriteLine($"Skipping Day {this.GetProblemInfo().Day}, solution marked as slow.");
                }
                return;
            }

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            PrintSolutionPart(PartOne(GetAllLines(folder)), stopWatch.ElapsedMilliseconds, 1);

            stopWatch.Restart();
            PrintSolutionPart(PartTwo(GetAllLines(folder)), stopWatch.ElapsedMilliseconds, 2);
            stopWatch.Stop();
        }

        private void PrintTime(long time)
        {
            using (ColorScope.CreateScope(BenchmarkColors[BenchmarkColors.Keys.Aggregate((x, y) => Math.Abs(x - time) < Math.Abs(y - time) ? x : y)]))
            {
                Console.Write(time);
            };
        }

        public abstract object PartOne(string[] data);

        public abstract object PartTwo(string[] data);
    }
}
