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
            // 564 incorrect
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


        public sealed record Node(int Y, int X, byte Level) : IComparable<Node>
        {
            public (int y, int x) Point => (Y, X);
            public override string ToString() => $"({X},{Y})";
            public bool Equals(Node other) => X == other.X && Y == other.Y;

            public int CompareTo(Node? other) => Level < other.Level ? -1 : Level == other.Level ? 0 : 1;
        }

        internal class Map
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public Node Target { get; set; }
            public Node Start { get; set; }

            public List<Node> Graph { get; set; }

            private bool InGrid(Node n) => n.X < Width && n.X >= 0 && n.Y < Height && n.Y >= 0;

            IEnumerable<Node> Edges(Node node)
                => Graph.Where(n => 
                (n.Y - 1 == node.Y && n.X == node.X)
                || (n.Y + 1 == node.Y && n.X == node.X)
                || (n.Y == node.Y && n.X - 1 == node.X)
                || (n.Y == node.Y && n.X + 1 == node.X)
                )
                .Where(InGrid)
                .Where(n => !n.Equals(node))
                .Where(x => x.Level <= node.Level || node.Level + 1 == x.Level);

            public int DistanceToEnd()
            {
                var visited = new HashSet<(int, int)> { Start.Point };
                var queue = new SortedSet<(Node n, int manhattan)>()
                {
                    { (Start, Start.Point.Manhattan(Target.Point)) }
                };

                var came_from = new Dictionary<Node, List<Node>>()
                {
                    //{ (Start.Manhattan(Target), 0, Start.y, Start.x, 0), new() }
                };

                while (queue.Count > 0)
                {
                    var current = queue.First();
                    queue.Remove(current);
                    //var currentSpace = (currentNode.y, currentNode.x);

                    var debug = Edges(current.n).ToArray();
                    foreach (var edge in Edges(current.n))//.Where(x => !visited.Contains(x)))
                    {
                        if (came_from.ContainsKey(edge)) continue;

                        if (edge == Target)
                        {
                            Print(visited);
                            return came_from[current.n].Count;
                        }
                        else
                        {
                            //visited.Add(edge);
                            if(came_from.ContainsKey(edge))
                            {
                                came_from[edge].Add(current.n);
                            }
                            else
                            {
                                came_from.Add(edge, new() { current.n });
                            }
                            queue.Add((edge, edge.Point.Manhattan(Target.Point)));
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

        private (List<Node> graph, Node start, Node target) GetGraph(string[] data)
        {
            var graph = new List<Node>();

            Node start = null, target = null;
            byte elevation;

            foreach (var (row, i) in data.Select((row, idx) => (row, idx)))
            {
                foreach (var (col, j) in row.Trim().Select((col, idx) => (col, idx)))
                {
                    var node = col switch
                    {
                        'S' => new Node(i, j, 0),
                        'E' => new Node(i, j, 25),
                        _ => new Node(i, j, (byte)(col - 'a'))
                    };
                    if (col == 'S') start = node;
                    if (col == 'E') target = node;
                    graph.Add(node);
                }
            }
            return (graph, start, target);
        }
    }
}
