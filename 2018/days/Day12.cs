using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;
using System.Text;

namespace advent_of_code_2018.days
{
    [ProblemInfo(12, "Subterranean Sustainability")]
    public class Day12 : SolverBase
    {
        public override object PartOne(string[] data)
        {
            // Part 1: After 20 generations, what is the sum of the numbers of all pots which contain a plant?
            var state = new LinkedList<char>(data[0].Select(ch => ch));
            var rules = GetRules(data.Skip(2).ToArray());
            var graph = new Graph()
            {
                State = state,
                Rules = rules,
                RemainingIterations = 20
            };
            return graph.Grow();
        }

        public override object PartTwo(string[] data)
        {
            // Part 2: After fifty billion (50000000000) generations, what is the sum of the numbers of all pots which contain a plant?
            // 3150000000905
            var state = new LinkedList<char>(data[0].Select(ch => ch));
            var rules = GetRules(data.Skip(2).ToArray());
            var graph = new Graph()
            {
                State = state,
                Rules = rules,
                RemainingIterations = 50000000000
            };
            return graph.Grow();
        }

        internal class Graph
        {
            public LinkedList<char> State { get; set; }
            public Dictionary<string, char> Rules { get; set; }
            public long RemainingIterations { get; set; } = 0;
            public int CurrentIteration { get; set; } = 0;
            protected char Plant { get; } = '#';
            protected char Empty { get; } = '.';
            protected int LeftOffset { get; set; } = 0;

            char LeftPot()
            {
                var first = State.First;
                if(!Rules.TryGetValue(String.Format("{0}{0}{0}{1}{2}", Empty, first.Value, first.Next.Value), out var pot))
                    return Empty;

                if (pot == Plant) 
                    LeftOffset--;
                return pot;
            }

            char RightPot()
            {
                var last = State.Last;
                return Rules.TryGetValue(String.Format("{0}{1}{2}{2}{2}", last.Previous.Value, last.Value, Empty), out var pot)
                    ? pot : Empty;
            }
            
            long SumPlants()
            {
                long score = 0;
                long idx = LeftOffset;
                
                var node = State.First;
                while (node != null)
                {
                    score += node.Value == Plant ? idx + RemainingIterations : 0;
                    idx++;
                    node = node.Next;
                }
                return score;
            }
            
            public char ValueAt(LinkedListNode<char>? node) => node?.Value ?? Empty;

            public char[] PotPattern(LinkedListNode<char> node)
                => new char[] {
                    ValueAt(node?.Previous?.Previous),
                    ValueAt(node?.Previous),
                    ValueAt(node),
                    ValueAt(node?.Next),
                    ValueAt(node?.Next?.Next)
                };

            public long Grow()
            {
                // after 124 (0-123) iterations the pattern repeats itself by adding 1 position offset to left (> 0)
                if (RemainingIterations == 0 || CurrentIteration == 124) 
                    return SumPlants();

                var newstate = new LinkedList<char>();
                var pot = State.First;

                if (LeftPot() == Plant) newstate.AddFirst(Plant); // add left plant if any

                while (pot != null)
                {
                    var rule = new string(PotPattern(pot));

                    char newnode = Rules.TryGetValue(rule, out var potstate) ? potstate : Empty;

                    newstate.AddLast(newnode);
                    pot = pot.Next;
                }

                CurrentIteration++;
                RemainingIterations--;

                if (RightPot() == Plant) newstate.AddLast(Plant); // add right plant if any

                State = newstate;

                return Grow();
            }
        }

        Dictionary<string, char> GetRules(string[] data)
            => data
                .Select(line => line.Split())
                .ToDictionary(x => x[0], v => v[1][0]);
    }
}
