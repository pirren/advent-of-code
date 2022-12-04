using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;
using static advent_of_code_2022.days.Day03;

namespace advent_of_code_2022.days
{
    [ProblemInfo(3, "Rucksack Reorganization")]
    public class Day03 : SolverBase
    {
        internal record ItemType(char Id, int Count) { }
        public override object PartOne(string[] data)
        {
            // Part 1: Find the item type that appears in both compartments of each rucksack. What is the sum of the priorities of those item types?
            return data.SelectMany(x => x.Take(x.Length / 2).ItemTypes().Join(
                x.Skip(x.Length / 2).ItemTypes(),
                l => l.Id,
                r => r.Id,
                (l, _) => l.Id.PriorityScore())
            ).Sum();
        }

        public override object PartTwo(string[] data)
        {
            // Part 2: Find the item type that corresponds to the badges of each three-Elf group. What is the sum of the priorities of those item types?
            return data.Chunk(3)
                .Sum(batch => batch
                    .SelectMany(x => x.Select(y => y).Distinct())
                    .GroupBy(ch => ch)
                    .Where(x => x.Count() >= 3)
                    .Select(x => x.Key).FirstOrDefault().PriorityScore()
                ); 
        }
    }

    internal static class Ext03
    {
        public static ItemType[] ItemTypes(this IEnumerable<char> d)
            => d.GroupBy(x => x).Select(x => new ItemType(x.Key, x.Count())).ToArray();

        public static int PriorityScore(this char ch)
            => char.IsUpper(ch) ? ch - 38 : ch - 96;
    }
}
