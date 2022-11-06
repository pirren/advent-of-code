using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;

namespace advent_of_code_2018.days
{
    [ProblemInfo(3, "No Matter How You Slice It")]
    public class Day03 : SolverBase
    {
        public override object PartOne(string[] data)
        {
            // Part 1: How many square inches of fabric are within two or more claims?
            var claims = ParseClaims(data).Select(x => (x.pos, x.size));
            Dictionary<(int, int), int> overlaps = new();

            foreach (var (pos, size) in claims)
            {
                for (int y = 0; y < size.y; y++)
                {
                    for (int x = 0; x < size.x; x++)
                    {
                        var key = (pos.y + y, pos.x + x);
                        if (overlaps.ContainsKey(key))
                            overlaps[key]++;
                        else
                            overlaps.Add(key, 1);
                    }
                }
            }

            return overlaps.Count(x => x.Value > 1);
        }

        public override object PartTwo(string[] data)
        {
            // Part 2: What is the ID of the only claim that doesn't overlap?
            var claims = ParseClaims(data);
            Dictionary<(int y, int x), (int count, List<int> IDs)> overlaps = new();
            foreach (var (id, pos, size) in claims)
            {
                for (int y = 0; y < size.y; y++)
                {
                    for (int x = 0; x < size.x; x++)
                    {
                        var key = (pos.y + y, pos.x + x);
                        if (overlaps.ContainsKey(key))
                        {
                            // here we have to re-make the list..
                            var list = overlaps[key].IDs;
                            list.Add(id);
                            overlaps[key] = (overlaps[key].count + 1, list);
                        }
                        else overlaps.Add(key, (1, new() { id }));
                    }
                }
            }

            var candidates = overlaps.Where(x => x.Value.count == 1).SelectMany(x => x.Value.IDs).Distinct().OrderBy(x => x).ToList();
            var bad = overlaps.Where(x => x.Value.count > 1).SelectMany(x => x.Value.IDs).Distinct().OrderBy(x => x).ToList();
            return candidates.Except(bad).Single();
        }

        public List<(int id, (int x, int y) pos, (int x, int y) size)> ParseClaims(string[] data)
            => data
                .Select(c => c.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                .Select(x =>
                {
                    var id = int.Parse(x[0].TrimStart('#'));
                    var prepPos = x[2].Split(',').Select(x => x.TrimEnd(':')).ToArray();
                    var pos = (int.Parse(prepPos[0]), int.Parse(prepPos[1]));
                    var prepSize = x[3].Split('x').ToArray();
                    var size = (int.Parse(prepSize[0]), int.Parse(prepSize[1]));
                    return (id, pos, size);
                }).ToList();
    }
}
