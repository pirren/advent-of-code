using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;

namespace advent_of_code_2022.days
{
    [ProblemInfo(1, "Calorie Counting")]
    public class Day01 : SolverBase
    {
        public override object PartOne(string[] data)
        {
            // Part 1: Find the Elf carrying the most Calories. How many total Calories is that Elf carrying?
            return GetAllData(Folder).Split("\r\n\r\n").Select(x => x.Split("\r\n").Select(int.Parse).Sum()).Max();
            //return GetAllData(Folder).Split("\r\n\r\n").Select(x => x.Split("\r\n").Select(int.Parse).Sum()).Max();
        }

        public override object PartTwo(string[] data)
        {
            // Part 2: Find the top three Elves carrying the most Calories. How many Calories are those Elves carrying in total?
            return GetAllData(Folder)
                .Split("\r\n\r\n").Select(x => x.Split("\r\n").Select(int.Parse).Sum())
                .OrderByDescending(x => x)
                .Take(3)
                .Sum();
        }
    }
}
