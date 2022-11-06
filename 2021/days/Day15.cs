using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;

namespace advent_of_code_2021.days
{
    [ProblemInfo(15, "Chiton")]
    public class Day15 : SolverBase
    {
        public override object PartOne(string[] data)
        {
            var graph = GetGraph(data);
            var map = new Map
            {
                Graph = graph,
                Width = data[0].Length,
                Height = data.Length
            };

            return map.LowestRisk();
        }

        public override object PartTwo(string[] data)
        {
            var graph = GetGraph(data);
            var chunkWidth = data[0].Length;
            var chunkHeight = data.Length;
            var width = chunkWidth * 5;
            var height = chunkHeight * 5;

            foreach (var x in Enumerable.Range(0, width))
            {
                foreach (var y in Enumerable.Range(0, height))
                {
                    var value = graph[(x % chunkWidth, y % chunkHeight)] + x / chunkWidth + y / chunkHeight;
                    while (value > 9) value -= 9;
                    graph[(x, y)] = value;
                }
            }

            var map = new Map
            {
                Graph = graph,
                Width = width,
                Height = height
            };

            return map.LowestRisk();
        }

        Dictionary<(int, int), int> GetGraph(string[] indata)
        {
            var graph = new Dictionary<(int, int), int>();

            foreach (var (row, i) in indata.Select((row, idx) => (row, idx)))
            {
                foreach (var (col, j) in row.Trim().Select((col, idx) => (col, idx)))
                {
                    graph[(i, j)] = (int)char.GetNumericValue(col);
                }
            }
            return graph;
        }

        internal class Map
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public Dictionary<(int, int), int> Graph = new();

            bool InGrid((int x, int y) p) => p.x >= 0 && p.x < Width && p.y >= 0 && p.y < Height;
            int Manhattan((int x, int y) p) => Width - 1 - p.x + Width - 1 - p.y;

            IEnumerable<(int, int)> Edges((int x, int y) p)
                => new List<(int, int)>
                {
                    (p.x - 1, p.y),
                    (p.x + 1, p.y),
                    (p.x, p.y - 1),
                    (p.x, p.y + 1),
                }.Where(InGrid);

            public int LowestRisk()
            {
                var target = (Width - 1, Height - 1);
                var queue = new SortedSet<(int risk, int manhattan, int x, int y)>()
                {
                    (0, Manhattan((0, 0)), 0, 0)
                };
                var visited = new HashSet<(int, int)> { (0, 0) };

                while (queue.Any())
                {
                    var currentCandidate = queue.First();
                    queue.Remove(currentCandidate);
                    var currentSpace = (currentCandidate.x, currentCandidate.y);

                    foreach (var edge in Edges(currentSpace))
                    {
                        if (edge == target)
                        {
                            return currentCandidate.risk + Graph[target];
                        }
                        else if (!visited.Contains(edge))
                        {
                            visited.Add(edge);
                            var next = (Graph[edge] + currentCandidate.risk, Manhattan(edge), edge.Item1, edge.Item2);
                            queue.Add(next);
                        }
                    }

                }
                throw new Exception("No valid paths");
            }
        }
    }
}
