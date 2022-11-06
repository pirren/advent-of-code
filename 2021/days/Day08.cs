using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;
using advent_of_code_lib.extensions;

namespace advent_of_code_2021.days
{
    [ProblemInfo(8, "Seven Segment Search")]
    public class Day08 : SolverBase
    {
        public override object PartOne(string[] data)
            => data
                .Select(s => s.Split(" | ")[1])
                .SelectMany(s => s.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries))
                .Count(digit => new[] { 2, 4, 3, 7 }.Contains(digit.Length));

        public override object PartTwo(string[] data)
        {
            List<int> displayvalues = new();
            foreach (var line in data)
            {
                var split = line.Split(" | ");
                var numbers = split[1].Split(' ').ToArray();
                var d = split[0].Split(' ').Distinct().ToList();
                Dictionary<int, string> displaymap = new();

                displaymap[1] = Segment(2, ref d);
                displaymap[4] = Segment(4, ref d);
                displaymap[7] = Segment(3, ref d);
                displaymap[8] = Segment(7, ref d);
                displaymap[9] = Segment(6, s => s.Trim(displaymap[1].Concat(displaymap[4]).Concat(displaymap[7]).Distinct().ToArray()).Length == 1, ref d);
                displaymap[2] = Segment(5, s => s.Trim(displaymap[9].ToArray()).Length == 1, ref d);
                displaymap[3] = Segment(5, s => s.Trim(displaymap[2].ToArray()).Length == 1, ref d);
                displaymap[5] = Segment(5, s => s.Trim(displaymap[3].ToArray()).Length == 1, ref d);
                displaymap[6] = Segment(6, s => s.Trim(displaymap[5].ToArray()).Length == 1, ref d);
                displaymap[0] = Segment(6, ref d);

                var valuetoadd = string.Join("",
                    numbers.Select(num =>
                        displaymap.Where(s => s.Value.Length == num.Length && s.Value.Trim(num.ToArray()).Length == 0)
                            .Select(s => s.Key)
                            .Sum()
                            .ToString()
                        ))
                    .ToInt();

                displayvalues.Add(valuetoadd);
            }
            return displayvalues.Aggregate((a, b) => a + b);
        }

        public string Segment(int length, Func<string, bool> exp, ref List<string> data)
        {
            var segment = data.Where(s => s.Length == length).Where(exp).Distinct().SingleOrDefault() ?? "";
            if (!string.IsNullOrEmpty(segment)) data.RemoveAll(s => s == segment);
            return segment;
        }

        public string Segment(int length, ref List<string> data)
            => Segment(length, s => true, ref data);
    }
}
