using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;
using advent_of_code_lib.extensions;

namespace advent_of_code_2022.days
{
    [ProblemInfo(12, "Hill Climbing Algorithm")]
    public class Day12 : SolverBase
    {

        public override object PartOne(string[] data)
        {
            // Part 1: What is the fewest steps required to move from your current position to the
            // location that should get the best signal?
            // 3934 too high
            // 3700 too high
            // 2355 ??
            // 1866 incorrect
            // 1772 ??
            // 1032 incorrect
            // 558 incorrect
            // 556 incorrect
            var (graph, start, target) = GetGraph(data);
            var map = new Map 
            { 
                Width = data[0].Length, 
                Height = data.Length, 
                Graph = graph, 
                Start = start, 
                Target = target 
            };
            return map.DistanceToEnd();
        }

        public override object PartTwo(string[] data)
        {
            // Part 2: 
            return 0;
        }


        internal record Node(int Y, int X)
        {
            public bool Explored { get; set; } = false;
        }

        internal class Map
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public (int, int) Target { get; set; }
            public (int, int) Start { get; set; }

            public Dictionary<(int y, int x), byte> Graph { get; set; }

            private bool InGrid((int y, int x) p) => p.x < Width && p.x >= 0 && p.y < Height && p.y >= 0;

            IEnumerable<(int y, int x)> Edges((int y, int x) p, int level)
                => new List<(int y, int x)>
                {
                    (p.y - 1, p.x),
                    (p.y + 1, p.x),
                    (p.y, p.x - 1),
                    (p.y, p.x + 1),
                }.Where(InGrid).Where(x => Graph[x] >= level || level - 1 == Graph[x]);//.Where(x => Graph[p] < Graph[x] || Graph[p] == Graph[x] || Graph[p] - 1 == Graph[x]);

            public int DistanceToEnd()
            {
                var visited = new HashSet<(int, int)> { Start };
                var queue = new SortedSet<(int manhattan, int level, int y, int x, int steps)>()
                {
                    { (Start.Manhattan(Target), 0, Start.Item1, Start.Item2, 0) }
                };
                while (queue.Count > 0)
                {
                    var currentNode = queue.Last();
                    queue.Remove(currentNode);
                    var currentSpace = (currentNode.y, currentNode.x);

                    var debug = Edges(currentSpace, currentNode.level).Where(x => !visited.Contains(x)).ToArray();
                    foreach (var edge in Edges(currentSpace, currentNode.level).Where(x => !visited.Contains(x)))
                    {
                        if (edge == Target)
                        {
                            Print(visited);
                            return currentNode.steps + 1;
                        }
                        else
                        {
                            visited.Add(edge);
                            var next = (edge.Manhattan(Target), Graph[edge], edge.y, edge.x, currentNode.steps + 1);
                            queue.Add(next);
                        }
                    }
                }
                throw new Exception("No valid paths");
            }

            void Print(HashSet<(int, int)> visited)
            {
                int count = 0;
                for (var y = 0; y < Height; y++)
                {
                    for (var x = 0; x < Width; x++)
                    {
                        if (visited.Contains((y, x)))
                        {
                            Console.Write("#");
                            count++;
                        }
                        else Console.Write(".");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine(count);
            }
        }

        private (Dictionary<(int y, int x), byte>, (int, int) start, (int, int) target) GetGraph(string[] data)
        {
            var graph = new Dictionary<(int y, int x), byte>();

            (int, int) start = (0, 0);
            (int, int) target = (0, 0);
            byte elevation;

            foreach (var (row, i) in data.Select((row, idx) => (row, idx)))
            {
                foreach (var (col, j) in row.Trim().Select((col, idx) => (col, idx)))
                {
                    switch (col)
                    {
                        case 'S':
                            start = (i, j);
                            elevation = 0;
                            break;
                        case 'E':
                            target = (i, j);
                            elevation = 25;
                            break;
                        default:
                            elevation = (byte)(col - 'a');
                            break;
                    };
                    graph[(i, j)] = elevation;
                }
            }
            return (graph, start, target);
        }
    }
}
