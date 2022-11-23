using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;
using advent_of_code_lib.extensions;
using static advent_of_code_2018.days.Day09;
using static advent_of_code_2018.days.Day13;

namespace advent_of_code_2018.days
{
    [ProblemInfo(14, "Chocolate Charts")]
    public class Day14 : SolverBase
    {
        public override object PartOne(string[] data)
        {
            // Part 1: What are the scores of the ten recipes immediately after the number of recipes in your puzzle input?
            // 2151055104 not right
            var recipes = new LinkedRecipesList
            {
                TargetCountRecipes = 2018//data[0].ToInt()
            };
            return recipes.MakeChocolate();
        }

        public override object PartTwo(string[] data)
        {
            // Part 2: -
            return 0;
        }

        internal class LinkedRecipesList
        {
            public List<LinkedNode> Nodes { get; set; } = new();

            public Cook FirstElf { get; set; } = null;
            public Cook SecondElf { get; set; } =  null;
            public int CookingElvesCount { get; set; } = 2;
            public int TargetCountRecipes { get; set; }

            public string MakeChocolate()
            {
                var firstnode = new LinkedNode { Value = 3, NodeId = 1 };
                var secondnode = new LinkedNode { Value = 7, NodeId = 2 };

                // connect nodes
                firstnode.Connect(secondnode);
                firstnode.ConnectEdge(secondnode);

                // let elves track their current nodes
                FirstElf = new Cook { Recipe = firstnode };
                SecondElf = new Cook { Recipe = secondnode };

                Nodes.Add(firstnode);
                Nodes.Add(secondnode);

                while (true)
                {
                    for(var i = 0; i < CookingElvesCount; i++)
                    {
                        CreateNewRecipes();

                        if (Nodes.Count < TargetCountRecipes) continue;

                        var relativeNode = Nodes.Count > TargetCountRecipes ? Nodes.Last().Previous : Nodes.Last();

                        // get the next 10
                        for (int n = Nodes.Count - TargetCountRecipes; n < 10; n++)
                            CreateNewRecipes();

                        return string.Join(string.Empty, GetNextTenNodes(relativeNode).Select(x => x.Value.ToString()).ToArray());
                    }
                }
            }

            void CreateNewRecipes()
            {
                var score = FirstElf.Recipe.Value + SecondElf.Recipe.Value;
                for (var i = score.CountDigits(); i > 0; i--)
                {
                    var newnode = new LinkedNode { Value = score.NumberAt(i - 1), Next = Nodes.Last(), NodeId = Nodes.Count + 1 };
                    Nodes.Last().Connect(newnode); 
                    Nodes.First().ConnectEdge(newnode); 
                    Nodes.Add(newnode);
                }

                FirstElf.Recipe = Move(FirstElf.Recipe, FirstElf.Recipe.Value + 1);
                SecondElf.Recipe = Move(SecondElf.Recipe, SecondElf.Recipe.Value + 1);
            }


            public class Cook
            {
                public LinkedNode Recipe { get; set; }
            }

            IEnumerable<LinkedNode> GetNextTenNodes(LinkedNode relativeNode)
            {
                List<LinkedNode> nextnnodes = new();
                for(int i = 0; i < 10; i++)
                {
                    relativeNode = relativeNode.Next;
                    nextnnodes.Add(relativeNode);
                }
                return nextnnodes;
            }

            LinkedNode Move(LinkedNode node, int steps)
            {
                if (steps == 0) return node;
                return Move(node.Next, steps - 1);
            }
        }

        internal class LinkedNode
        {
            public int NodeId { get; set; }
            public int Value { get; set; }
            public LinkedNode Previous { get; set; } = null;
            public LinkedNode Next { get; set; } = null;
        }
    }

    internal static class Ext14
    {
        public static void Connect(this Day14.LinkedNode prev, Day14.LinkedNode next)
        {
            prev.Next = next;
            next.Previous = prev;
        }

        public static void ConnectEdge(this Day14.LinkedNode first, Day14.LinkedNode last)
        {
            first.Previous = last;
            last.Next = first;
        }
    }
}
