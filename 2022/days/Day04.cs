using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;

namespace advent_of_code_2022.days
{
    [ProblemInfo(4, "Camp Cleanup")]
    public class Day04 : SolverBase
    {
        public override object PartOne(string[] data)
        {
            // Part 1: In how many assignment pairs does one range fully contain the other?
            return AssignmentPairs(data).Count(x => x[0].Containing(x[1]) || x[1].Containing(x[0]));
        }

        public override object PartTwo(string[] data)
        {
            // Part 2: In how many assignment pairs do the ranges overlap?
            return AssignmentPairs(data).Count(x => x[0].Overlapping(x[1]));
        }

        internal IEnumerable<(int min, int max)[]> AssignmentPairs(string[] data)
                => data.Select(x => x.Split(',').Select(elf =>
                {
                    var minmax = elf.Split('-').Select(int.Parse).ToArray();
                    return (minmax[0], minmax[1]);
                }).ToArray()
            );
    }

    internal static class Ext04
    {
        public static bool Overlapping(this (int min, int max) section, (int min, int max) otherSection)
            => Math.Max(0, Math.Min(section.max, otherSection.max) - Math.Max(section.min, otherSection.min) + 1) > 0;

        public static bool Containing(this (int min, int max) section, (int min, int max) otherSection)
            => section.min >= otherSection.min && section.max <= otherSection.max;
    }
}
