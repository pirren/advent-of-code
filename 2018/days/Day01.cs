using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;
using advent_of_code_lib.extensions;

namespace advent_of_code_2018.days
{
    [ProblemInfo(1, "Chronal Calibration")]
    public class Day01 : SolverBase
    {
        public override object PartOne(string[] data)
            => ParseChangeList(data).Sum();

        public override object PartTwo(string[] data)
        {
            int currentFrequency = 0;
            HashSet<int> frequencies = new() { currentFrequency };
            while (true)
            {
                var changes = ParseChangeList(data);
                while(changes.Count > 0)
                {
                    currentFrequency += changes.Dequeue();

                    if (frequencies.Contains(currentFrequency))
                        return currentFrequency;
                    else frequencies.Add(currentFrequency);
                }
            }
        }

        private Queue<int> ParseChangeList(string[] indata)
           => new(indata.Select(x => x[0] == '+' ? x[1..].ToInt() : x[1..].ToInt() * -1));
    }
}
