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


        public class Node
        {
            public Node(int y, int x, byte level)
            {
                Y = y;
                X = x;
                Level = level;
            }
            public int Y { get; set; }
            public int X { get; set; }
            public byte Level { get; set; }
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
                => Graph.Where(n => (n.Y - 1 == node.Y && n.X == node.X) || (n.Y + 1 == node.Y && n.X == node.X)
                        || (n.Y == node.Y && n.X - 1 == node.X) || (n.Y == node.Y && n.X + 1 == node.X))
                .Where(InGrid)
                .Where(x => x.Level <= node.Level || node.Level + 1 == x.Level);

            public int DistanceToEnd()
            {
                var frontier = new Queue<Node>(new[] { Start });

                var came_from = new Dictionary<Node, Node>
                {
                    [Start] = null
                };

                while (frontier.Count > 0)
                {
                    var candidate = frontier.Dequeue();
                    foreach(var edge in Edges(candidate))
                    {
                        if (!came_from.ContainsKey(edge))
                        {
                            frontier.Enqueue(edge);
                            came_from[edge] = candidate;
                        }
                    }
                }

                var current = Target;
                var path = new List<Node>();

                while(current != Start)
                {
                    path.Add(current);
                    current = came_from[current];
                }

                return path.Count;
                //path.Reverse();

                //throw new Exception("No valid paths");
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
