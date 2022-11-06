using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;

namespace advent_of_code_2021.days
{
    [ProblemInfo(1, "Sonar Sweep")]
    public class Day01 : SolverBase
    {
        public override object PartOne(string[] data)
        {
            var parsedData = data.Select(int.Parse).ToArray();
            return Enumerable.Range(1, data.Length - 1)
            .Count(i =>
                parsedData.ElementAt(i) > parsedData.ElementAt(i - 1)
            );
        }

        public override object PartTwo(string[] data)
        {
            var parsedData = data.Select(int.Parse).ToArray();
            return Enumerable.Range(1, data.Length - 1)
            .Count(i =>
                parsedData.Skip(i).Take(3).Sum() > parsedData.Skip(i - 1).Take(3).Sum()
            );
        }
    }
}
