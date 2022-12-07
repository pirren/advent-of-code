using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;

namespace advent_of_code_2022.days
{
    [ProblemInfo(7, "No Space Left On Device")]
    public class Day07 : SolverBase
    {
        public override object PartOne(string[] data)
        {
            // Part 1: Find all of the directories with a total size of at most 100000.
            // What is the sum of the total sizes of those directories
            var rootNode = HydrateNodeTree(new Node(), new (data.Skip(1)));
            var folderSizes = FolderSizes(new(rootNode.FlattenNode())); // 
            return folderSizes.Where(x => x <= 100000).Sum();
        }

        public override object PartTwo(string[] data)
        {
            // Part 2: Find the smallest directory that, if deleted, would free up enough space on the filesystem to run the update.
            // What is the total size of that directory?
            var rootNode = HydrateNodeTree(new Node(), new (data.Skip(1)));
            var folderSizes = FolderSizes(new(rootNode.FlattenNode())).OrderBy(x => x);
            return folderSizes.First(x => x >= folderSizes.Max() - 40000000);
        }

        private IEnumerable<int> FolderSizes(Stack<Node> stack)
        {
            if (!stack.Any()) return new List<int>();
            var next = stack.Pop();

            int folderSize = new[] { next }.Flatten().Select(x => x.Size).Sum();
            return FolderSizes(stack).Concat(new[] { folderSize });
        }

        private Node HydrateNodeTree(Node currentNode, Queue<string> q)
        {
            if (!q.Any()) return currentNode;

            if (q.Peek().EndsWith("ls")) q.Dequeue(); // skip the ls
            currentNode.Size += GetFileSizes(q).Sum();

            if (!q.Any()) return currentNode;

            var next = q.Dequeue().Split();
            if (next[^1] == "..") return currentNode;

            if (next[1] == "cd")
            {
                var childNode = new Node();
                currentNode.SubNodes.Add(HydrateNodeTree(childNode, q));
            }

            return HydrateNodeTree(currentNode, q);
        }

        private IEnumerable<int> GetFileSizes(Queue<string> q)
        {
            while (q.Count > 0 && !q.Peek().StartsWith("$")) // take until next command
            {
                var item = q.Dequeue();
                if (item.StartsWith("dir")) continue; // we dont care about dir
                yield return int.Parse(item.Split()[0]);
            }
        }
    }

    internal class Node
    {
        public int Size { get; set; }
        public List<Node> SubNodes { get; set; } = new();
    }

    internal static class Ext07
    {
        public static IEnumerable<Node> Flatten(this IEnumerable<Node> e) =>
            e.SelectMany(c => c.SubNodes.Flatten()).Concat(e);

        public static IEnumerable<Node> FlattenNode(this Node n) => new[] { n }.Flatten(); 
    }
}
