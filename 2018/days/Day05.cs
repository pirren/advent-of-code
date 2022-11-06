using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;
using advent_of_code_lib.extensions;

namespace advent_of_code_2018.days
{
    [ProblemInfo(5, "Alchemical Reduction")]
    public class Day05 : SolverBase
    {
        public override object PartOne(string[] data)
        {
            // Part 1: How many units remain after fully reacting the polymer you scanned?
            var list = ParseData(data[0]);
            return ReactedPolymerLength(list);
        }

        public override object PartTwo(string[] data)
        {
            // Part 2: What is the length of the shortest polymer you can produce
            var list = ParseData(data[0]);
            return 'A'.Range('Z')
                .Select(polarity => FilteredPolymer(new(list), polarity))
                .Where(x => x.Count != 0)
                .Min(poly => ReactedPolymerLength(poly));
        }

        private int ReactedPolymerLength(LinkedList<char> linkedTree)
        {
            var node = linkedTree.First;

            while (node != null && node.Next != null)
            {
                if (Math.Abs(node.Value - node.Next.Value) != 32)
                {
                    node = node.Next;
                    continue;
                }

                if (node.Previous == null)
                {
                    linkedTree.Remove(node.Next);
                    linkedTree.Remove(node);
                    node = linkedTree.First;
                }
                else
                {
                    node = node.Previous;
                    linkedTree.Remove(node.Next);
                    linkedTree.Remove(node.Next);
                }
            }

            return linkedTree.Count;
        }

        private LinkedList<char> FilteredPolymer(LinkedList<char> linkedTree, char polarity)
        {
            var node = linkedTree.First;
            var (p1, p2) = (polarity, (char)(polarity + 32));

            if (linkedTree.All(x => x != p1 && x != p2))
                return new();

            while (node != null && node.Next != null)
            {
                if (node.Value != p1 && node.Value != p2)
                {
                    node = node.Next;
                    continue;
                }

                if (node.Previous == null)
                {
                    linkedTree.Remove(node);
                    node = linkedTree.First;
                }
                else
                {
                    node = node.Previous;
                    linkedTree.Remove(node.Next);
                }
            }

            return linkedTree;
        }

        private LinkedList<char> ParseData(string data)
        {
            var ll = new LinkedList<char>();
            for (int i = 0; i < data.Length; i++)
            {
                ll.AddLast(data[i]);
            }
            return ll;
        }
    }
}
