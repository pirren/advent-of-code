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
            var rootNode = HydrateNodeTree(new Node(), new(data.Skip(1)));
            var folderSizes = FolderSizes(new(rootNode.FlattenNode()));

            return folderSizes.Where(x => x <= 100000).Sum();
        }

        public override object PartTwo(string[] data)
        {
            // Part 2: Find the smallest directory that, if deleted, would free up enough space on the filesystem to run the update.
            // What is the total size of that directory?
            var rootNode = HydrateNodeTree(new Node(), new(data.Skip(1)));
            var folderSizes = FolderSizes(new(rootNode.FlattenNode()));

            return folderSizes.OrderBy(x => x).First(x => x >= folderSizes.Max() - 40000000);
        }

        private IEnumerable<int> FolderSizes(Stack<Node> stack)
        {
            if (!stack.Any()) return new List<int>();
            var next = stack.Pop();

            int folderSize = new[] { next }.Flatten().Select(x => x.Size).Sum();
            return FolderSizes(stack).Concat(new[] { folderSize });
        }

        private Node HydrateNodeTree(Node currentNode, Queue<string> cmds)
        {
            if (!cmds.Any()) return currentNode;

            if (cmds.Peek().EndsWith("ls")) cmds.Dequeue(); // skip the ls
            currentNode.Size += GetFileSizes(cmds).Sum();

            if (!cmds.Any()) return currentNode;

            var next = cmds.Dequeue().Split();
            if (next[^1] == "..") return currentNode;

            if (next[1] == "cd")
            {
                var childNode = new Node();
                currentNode.SubNodes.Add(HydrateNodeTree(childNode, cmds));
            }

            return HydrateNodeTree(currentNode, cmds);
        }

        private IEnumerable<int> GetFileSizes(Queue<string> cmds)
        {
            while (cmds.Any() && !cmds.Peek().StartsWith("$")) // take until next command
            {
                var item = cmds.Dequeue();
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
