using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;
using advent_of_code_lib.extensions;
using System.Drawing;

namespace advent_of_code_2021.days
{
    [ProblemInfo(9, "Smoke Basin")]
    public class Day09 : SolverBase
    {
        protected List<int[]> Heightmap { get; private set; } = new();
        public override object PartOne(string[] data)
        {
            Heightmap = data.Select(s => s.Select(ch => ch.ToInt()).ToArray()).ToList();
            return GetLowPoints().Select(point => Heightmap[point.Y][point.X] + 1).Sum();
        }

        public override object PartTwo(string[] data)
        {
            Heightmap = data.Select(s => s.Select(ch => ch.ToInt()).ToArray()).ToList();

            return GetLowPoints().Select(lowpoint => CalculateBasin(lowpoint))
                .OrderByDescending(s => s)
                .Take(3)
                .Aggregate((a, b) => a * b);
        }

        private long CalculateBasin(Point point)
            => CalculateBasin(point, 0, new());

        IEnumerable<Point> Neighbors(Point point)
            => new List<Point> {
                    new Point { Y = point.Y - 1, X = point.X },
                    new Point { Y = point.Y + 1, X = point.X },
                    new Point { Y = point.Y, X = point.X - 1 },
                    new Point { Y = point.Y, X = point.X + 1 },
            }.Where(InBounds);

        bool InBounds(Point p) => p.X >= 0 && p.X < Heightmap.First().Length && p.Y >= 0 && p.Y < Heightmap.Count;

        public string Segment(int length, Func<string, bool> exp, ref List<string> data)
        {
            var segment = data.Where(s => s.Length == length).Where(exp).Distinct().SingleOrDefault() ?? "";
            if (!string.IsNullOrEmpty(segment)) data.RemoveAll(s => s == segment);
            return segment;
        }

        public string Segment(int length, ref List<string> data)
            => Segment(length, s => true, ref data);

        private long CalculateBasin(Point point, int previous, List<Point> checkedPoints)
        {
            var currentvalue = Heightmap[point.Y][point.X];
            if (currentvalue == 9 || checkedPoints.Any(p => p.Equals(point)) || currentvalue < previous) return 0;

            checkedPoints.Add(point);

            var debug = Neighbors(point).ToList();

            foreach (var neighbor in Neighbors(point))
                CalculateBasin(neighbor, currentvalue, checkedPoints);

            return previous == 0 ? checkedPoints.Count : 0;
        }

        private List<Point> GetLowPoints()
        {
            List<Point> lowpoints = new();

            for (int row = 0; row < Heightmap.Count; row++)
            {
                for (int col = 0; col < Heightmap.First().Length; col++)
                {
                    var debug = Neighbors(new Point { X = col, Y = row }).ToList();

                    List<int> controlvalues = new();
                    controlvalues.AddRange(Neighbors(new Point { X = col, Y = row }).Select(s => Heightmap[s.Y][s.X]));

                    if (controlvalues.All(val => val > Heightmap[row][col]))
                        lowpoints.Add(new Point { X = col, Y = row });
                }
            }

            return lowpoints;
        }
    }
}
