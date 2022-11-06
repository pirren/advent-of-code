using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;

namespace advent_of_code_2021.days
{
    [ProblemInfo(6, "Lanternfish")]
    public class Day06 : SolverBase
    {
        public override object PartOne(string[] data)
             => SimulateSpawnRate(data[0], 80);

        public override object PartTwo(string[] data)
             => SimulateSpawnRate(data[0], 256);

        public long SimulateSpawnRate(string indata, int days)
        {
            var data = indata.Trim().Split(',').Select(long.Parse).ToArray();
            long[] fish = new long[9];

            for (int idx = 0; idx < fish.Length; idx++)
                fish[idx] = data.Count(f => f == idx);

            for (int day = 0; day < days; day++)
            {
                long[] copy = new long[9];
                long spawns = 0;
                for (int cycle = 0; cycle < fish.Length; cycle++)
                {
                    if (cycle > 0) copy[cycle - 1] += copy[cycle - 1] + fish[cycle];
                    else
                    {
                        spawns = fish[cycle];
                        copy[8] = spawns;
                        copy[6] = spawns;
                    }
                }
                Array.Copy(copy, fish, fish.Length);
                fish[6] -= spawns;
            }

            return fish.Sum();
        }
    }
}
