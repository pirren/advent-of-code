using advent_of_code.lib.helpers;
using advent_of_code_lib.extensions;
using advent_of_code_lib.interfaces;
using System.Diagnostics;

namespace advent_of_code_lib.bases
{
    public abstract class SolverBase : ISolver
    {
        private string[] GetIndata(string folder)
            => File.ReadAllLines(Path.Combine(folder, $"{this.GetProblemInfo().Day}.in"));

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
            using (ColorScope.CreateScope(ConsoleColor.Yellow))
            {
                Console.Write($"\r\n{(result?.ToString() ?? "")} \r\n");
            };

            Console.Write($"Part {part}, solved in ");
            PrintTime(benchmarkTime);
            Console.Write($" ms\r\n");
        }

        public void PrintSolutionHeader()
        {
            var info = this.GetProblemInfo();

            Console.Write("[");
            using (ColorScope.CreateScope(ConsoleColor.Yellow))
            {
                Console.Write($"Day {info.Day}: {info.ProblemName}");
            };
            Console.Write("]\r\n");
        }

        public void Solve(string folder)
        {
            PrintSolutionHeader();

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            PrintSolutionPart(PartOne(GetIndata(folder)), stopWatch.ElapsedMilliseconds, 1);

            stopWatch.Restart();
            PrintSolutionPart(PartTwo(GetIndata(folder)), stopWatch.ElapsedMilliseconds, 1);
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
