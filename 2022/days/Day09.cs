using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;
using advent_of_code_lib.extensions;

namespace advent_of_code_2022.days
{
    [ProblemInfo(9, "Rope Bridge")]
    public class Day09 : SolverBase
    {

        public override object PartOne(string[] data)
        {
            // Part 1: How many positions does the tail of the rope visit at least once?
            return TailPositions(data, 1).Count;
        }

        public override object PartTwo(string[] data)
        {
            // Part 2: Simulate your complete series of motions on a larger rope with ten knots.
            // How many positions does the tail of the rope visit at least once?
            return TailPositions(data, 9).Count;
        }

        private HashSet<(int y, int x)> TailPositions(string[] data, int numtails)
        {
            (int y, int x) start = (0, 0);
            var H = start;

            List<(int y, int x)> T = new(0.Range(numtails).Select(x => start));
            HashSet<(int y, int x)> visited = new() { start };

            foreach (var line in data)
            {
                ((int dy, int dx), int steps) = line.GetMove();

                for (int i = 0; i < steps; i++)
                {
                    H = (H.y + dy, H.x + dx);

                    for (int t = 0; t < T.Count; t++)
                    {
                        var h = t == 0 ? H : T[t - 1];

                        if (T[t].x != h.x && T[t].y != h.y && T[t].Manhattan(h) > 2) 
                        {
                            T[t] = T[t].ClosestPoint(h); 
                        }
                        else if (T[t].Manhattan(h) > 1 && (T[t].x == h.x || T[t].y == h.y)) 
                        {
                            T[t] = T[t].FollowPoint(h);
                        }
                    }
                    visited.Add(T[^1]);
                }
            }
            return visited;
        }
    }

    internal static class Ext09
    {
        private static Dictionary<string, (int dy, int dx)> Directions = new()
        {
            { "U", (-1, 0) },
            { "R", (0, 1) },
            { "D", (1, 0) },
            { "L", (0, -1) }
        };

        public static ((int dy, int dx), int steps) GetMove(this string data)
        {
            var d = data.Split();
            return (Directions[d[0]], int.Parse(d[^1]));
        }

        public static (int, int) ClosestPoint(this (int y, int x) source, (int y, int x) target)
            => source.Neighbors().OrderBy(p => p.Manhattan(target)).First();

        public static (int, int) FollowPoint(this (int y, int x) source, (int y, int x) target)
            => Directions.Values.Select(d => (source.y + d.dy, source.x + d.dx)).OrderBy(p => p.Manhattan(target)).First();

        public static IEnumerable<(int, int)> Neighbors(this (int y, int x) point)
        {
            for(int y = point.y - 1; y <= point.y + 1; y++) 
                for(int x = point.x - 1; x <= point.x + 1; x++)
                    if((y,x) != point)
                        yield return (y, x);
        }
    }
}
