using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;
using advent_of_code_lib.extensions;

namespace advent_of_code_2021.days
{
    [ProblemInfo(2, "Dive!")]
    public class Day02 : SolverBase
    {
        public override object PartOne(string[] data)
        {
            var instructions = ParseData(data)
                .GroupBy(s => s[0])
                .ToDictionary(k => k.Key, v => v.Select(s => int.Parse(s[1])));

            return (instructions["down"].Sum() - instructions["up"].Sum()) * instructions["forward"].Sum();
        }

        public override object PartTwo(string[] data)
        {
            var instructions = ParseData(data);
            int aim = 0, depth = 0, pos = 0;

            foreach (var instruction in instructions)
            {
                var value = instruction[1].ToInt();
                if (instruction[0] == "down") aim += value;
                if (instruction[0] == "up") aim -= value;
                if (instruction[0] == "forward")
                {
                    pos += value;
                    depth += aim * value;
                }
            }
            return pos * depth;
        }
        private List<string[]> ParseData(string[] indata)
            => indata.Select(s => s.Split(' ')).ToList();
    }
}
