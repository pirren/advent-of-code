using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;

namespace advent_of_code_2022.days
{
    [ProblemInfo(12, "Hill Climbing Algorithm")]
    public class Day12 : SolverBase
    {
        public override object PartOne(string[] data)
        {
            // Part 1: What is the fewest steps required to move from your current position
            // to the location that should get the best signal?
            var graph = GetGraph(data);
            var map = new Map
            {
                Width = data[0].Length,
                Height = data.Length,
                Graph = graph,
                StartCondition = x => x.Id == 'S',
                TargetCondition = x => x.Id == 'E'
            };
            return map.DistanceToEnd(reverse: false);
        }

        public override object PartTwo(string[] data)
        {
            // Part 2: What is the fewest steps required to move starting from any square with
            // elevation a to the location that should get the best signal?
            var graph = GetGraph(data);
            var map = new Map
            {
                Width = data[0].Length,
                Height = data.Length,
                Graph = graph,
                StartCondition = x => x.Id == 'E',
                TargetCondition = x => x.Id == 'a' || x.Id == 'S'
            };
            return map.DistanceToEnd(reverse: true);
        }

        public record Node(int Y, int X, byte Level, char Id) { }

        internal class Map
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public Func<Node, bool> TargetCondition { get; set; }
            public Func<Node, bool> StartCondition { get; set; }
            public List<Node> Graph { get; set; }

            private bool InGrid(Node n) => n.X < Width && n.X >= 0 && n.Y < Height && n.Y >= 0;

            IEnumerable<Node> Edges(Node node, bool reverse)
            {
                var edges = Graph.Where(n => (n.Y - 1 == node.Y && n.X == node.X) || (n.Y + 1 == node.Y && n.X == node.X)
                        || (n.Y == node.Y && n.X - 1 == node.X) || (n.Y == node.Y && n.X + 1 == node.X))
                .Where(InGrid);

                return reverse 
                    ? edges.Where(x => x.Level >= node.Level || node.Level - 1 == x.Level)
                    : edges.Where(x => x.Level <= node.Level || node.Level + 1 == x.Level);
            }

            public int DistanceToEnd(bool reverse)
            {
                var startNode = Graph.First(StartCondition);
                var targetId = Graph.First(TargetCondition).Id;

                var frontier = new Queue<Node>(new [] {startNode});
                frontier.Enqueue(startNode);
                frontier.Enqueue(null); // use null to count steps

                var visited = new Dictionary<Node, Node> { [startNode] = null };

                int stepsTaken = 0;
                while (frontier.Count > 0)
                {
                    var candidate = frontier.Dequeue();
                    if (candidate == null)
                    {
                        stepsTaken++;
                        frontier.Enqueue(null!);
                        if (frontier.Peek() == null) throw new Exception();
                        continue;
                    }
                    foreach (var edge in Edges(candidate, reverse))
                    {
                        if(candidate.Id == targetId) return stepsTaken;
                        if (!visited.ContainsKey(edge))
                        {
                            frontier.Enqueue(edge);
                            visited[edge] = candidate;
                        }
                    }
                }
                throw new Exception();
            }
        }

        private List<Node> GetGraph(string[] data)
        {
            var graph = new List<Node>();
            byte elevation;

            foreach (var (row, i) in data.Select((row, idx) => (row, idx)))
            {
                foreach (var (col, j) in row.Trim().Select((col, idx) => (col, idx)))
                {
                    var node = col switch
                    {
                        'S' => new Node(i, j, 0, col),
                        'E' => new Node(i, j, 25, col),
                        _ => new Node(i, j, (byte)(col - 'a'), col)
                    };
                    graph.Add(node);
                }
            }
            return graph;
        }
    }
}
