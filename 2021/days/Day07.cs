using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;

namespace advent_of_code_2021.days
{
    [ProblemInfo(7, "The Treachery of Whales")]
    public class Day07 : SolverBase
    {
        public override object PartOne(string[] data)
             => FuelSpent(fuel => fuel, data[0]);

        public override object PartTwo(string[] data)
             => FuelSpent(fuel => fuel * (1 + fuel) / 2, data[0]);

        public object FuelSpent(Func<int, int> cost, string indata)
        {
            int[] crabpos = indata.Trim().Split(',').Select(int.Parse).ToArray();
            var low = crabpos.Min();
            var max = crabpos.Max();

            return Enumerable.Range(low, max - low + 1).Min(i => crabpos.Select(pos => Math.Abs(pos - i)).Select(cost).Sum());
        }
    }
}
