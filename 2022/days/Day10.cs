using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;
using System.Text;

namespace advent_of_code_2022.days
{
    [ProblemInfo(10, "Cathode-Ray Tube")]
    public class Day10 : SolverBase
    {
        private readonly List<int> SignalStrengthCycles = new() { 20, 60, 100, 140, 180, 220 };
        private readonly string addx = nameof(addx);

        public override object PartOne(string[] data)
        {
            // Part 1: What is the sum of these six signal strengths?
            return MeasureScreen(data, 1);
        }

        public override object PartTwo(string[] data)
        {
            // Part 2: Render the image given by your program. What eight capital letters appear on your CRT?
            return MeasureScreen(data, 2);
        }

        private object MeasureScreen(string[] data, int part) 
        {
            var X = 1;
            var cycles = 0;
            int row = 40;
            int signalStrengthSums = 0;

            StringBuilder CRTRow = new();

            foreach (var line in data)
            {
                int val = 0;

                if (line.StartsWith(addx)) val = int.Parse(line.Split()[^1]);

                for (int i = 0; i < (val == 0 ? 1 : 2); i++)
                {
                    cycles++;
                    if (part == 1 && SignalStrengthCycles.Contains(cycles)) signalStrengthSums += cycles * X;
                    else if(part == 2)
                    {
                        CRTRow.Append(Enumerable.Range(X - 1, 3).Contains(cycles - 1) ? "#" : ' ');

                        if (cycles != row) continue;
                        
                        Console.WriteLine(CRTRow.ToString());
                        CRTRow = CRTRow.Clear();
                        cycles = 0;
                    }
                }
                X += val != 0 ? val : 0;
            }
            return part == 1 ? signalStrengthSums : "ERCREPCJ";
        }
    }
}
