using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;
using advent_of_code_lib.extensions;

namespace advent_of_code_2021.days
{
    [ProblemInfo(12, "Passage Pathing")]
    public class Day12 : SolverBase
    {
        protected Dictionary<string, Node> NodeTree { get; set; } = new();
        public override object PartOne(string[] data)
        {
            ParseInput(data);
            Node startnode = NodeTree["start"];

            return CalculatePath(new(new[] { startnode })).Count;
        }

        public override object PartTwo(string[] data)
        {
            ParseInput(data);
            Node startnode = NodeTree["start"];

            return CalculatePath(new(new[] { startnode }), true).Count;
        }

        private List<List<Node>> CalculatePath(List<Node> currentPath, bool revisit = false)
        {
            var last = currentPath.Last();

            if (last.IsEnd) return new List<List<Node>> { currentPath };

            var validNewNodes = revisit ? ValidNextNodesRevisit(currentPath, last) : ValidNextNodes(currentPath, last);
            if (!validNewNodes.Any())
                return new List<List<Node>>();

            return validNewNodes
                .Select(nextEdge => new List<List<Node>> { currentPath, new List<Node> { nextEdge } }.SelectMany(s => s).ToList())
                .SelectMany(path => CalculatePath(path, revisit)).ToList();
        }

        private List<Node> ValidNextNodes(List<Node> currentPath, Node node)
            => node.Links
                    .Select(x => NodeTree[x])
                    .Where(s => !s.IsStart && (!currentPath.Contains(s) || s.IsBig))
                    .ToList();

        private List<Node> ValidNextNodesRevisit(List<Node> currentPath, Node node)
            => node.Links
                    .Select(s => NodeTree[s])
                    .Where(node => !node.IsStart)
                    .Where(node => node.IsBig || !currentPath.Contains(node) ||
                                   currentPath.Where(node => node.IsSmall).Count() == currentPath.Where(node => node.IsSmall).Distinct().Count())
                    .ToList();

        public void ParseInput(string[] indata)
        {
            var nodes = indata.SelectMany(row => row.Split('-')).Distinct().Select(id => new Node(id)).ToArray();

            NodeTree = nodes.ToDictionary(key => key.Id);

            indata.ForEach(row =>
            {
                var valOne = row.Split('-')[0];
                var valTwo = row.Split('-')[1];

                NodeTree[valOne].Links.Add(valTwo);
                NodeTree[valTwo].Links.Add(valOne);
            });
        }

        public class Node
        {
            public string Id { get; set; }
            public HashSet<string> Links { get; private set; }
            public bool IsSmall => Id.ToLower() == Id && !Id.Equals("start") && !Id.Equals("end");
            public bool IsBig => Id.ToUpper() == Id && !Id.Equals("start") && !Id.Equals("end");
            public bool IsStart => Id.Equals("start");
            public bool IsEnd => Id.Equals("end");
            public Node(string Id)
            {
                this.Id = Id;
                Links = new();
            }
        }
    }
}
