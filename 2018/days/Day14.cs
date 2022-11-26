using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;
using advent_of_code_lib.extensions;

namespace advent_of_code_2018.days
{
    [ProblemInfo(14, "Chocolate Charts")]
    public class Day14 : SolverBase
    {
        public override object PartOne(string[] data)
        {
            // Part 1: What are the scores of the ten recipes immediately after the number of recipes in your puzzle input?
            int target = data[0].ToInt();
            List<int> nodes = new() { 3, 7 };

            int first = 0;
            int second = 1;

            while (nodes.Count < target + 10)
            {
                var nextsum = nodes[first] + nodes[second];
                if (nextsum > 9) nodes.Add(1);
                nodes.Add(nextsum % 10);

                first = (first + nodes[first] + 1) % nodes.Count;
                second = (second + nodes[second] + 1) % nodes.Count;
            }

            return string.Join(string.Empty, nodes.GetRange(target, 10).Select(x => x.ToString()).ToArray());
        }

        public override object PartTwo(string[] data)
        {
            // Part 2: How many recipes appear on the scoreboard to the left of the score sequence in your puzzle input?
            int target = data[0].ToInt();
            List<int> targetSequence = new();
            for(int i = target.CountDigits(); i > 0; i--) targetSequence.Add(target.NumberAt(i - 1));
            
            List<int> nodes = new() { 3, 7 };

            int first = 0;
            int second = 1;

            while (!nodes.TakeLast(targetSequence.Count).SequenceEqual(targetSequence))
            {
                var nextsum = nodes[first] + nodes[second];
                if (nextsum > 9)
                {
                    nodes.Add(1);
                    if (nodes.TakeLast(targetSequence.Count).SequenceEqual(targetSequence)) break;
                }
                nodes.Add(nextsum % 10);

                first = (first + nodes[first] + 1) % nodes.Count;
                second = (second + nodes[second] + 1) % nodes.Count;
            }

            return nodes.SkipLast(targetSequence.Count).Count();
        }
    }
}
