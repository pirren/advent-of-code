using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;

namespace advent_of_code_2022.days
{
    [ProblemInfo(6, "Tuning Trouble")]
    public class Day06 : SolverBase
    {
        public override object PartOne(string[] data)
        {
            // Part 1: How many characters need to be processed before the first start-of-packet marker is detected?
            return GetMarker(data[0], 4);
        }

        public override object PartTwo(string[] data)
        {
            // Part 2: How many characters need to be processed before the first start-of-message marker is detected
            return GetMarker(data[0], 14);
        }

        private int GetMarker(string buffer, int type) 
            => Enumerable.Range(0, buffer.Length - type - 1).First(i => buffer[i..(i + type)].Distinct().Count() == type) + type;
    }
}
