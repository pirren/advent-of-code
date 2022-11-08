using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;

namespace advent_of_code_2018.days
{
    [ProblemInfo(9, "Marble Mania")]
    public class Day09 : SolverBase
    {
        public override object PartOne(string[] data)
        {
            // Part 1: What is the winning Elf's score?
            var (players, lastMarble) = ParseData(data[0]);
            var marbleGame = new MarbleGame
            {
                Target = lastMarble
            };
            return marbleGame.Play(players); //GetWinningScore(players, lastMarble);
        }

        public override object PartTwo(string[] data)
        {
            // Part 2: What would the new winning Elf's score be if the number of the last marble were 100 times larger?
            var (players, lastMarble) = ParseData(data[0]);
            var marbleGame = new MarbleGame
            {
                Target = lastMarble * 100
            };
            return marbleGame.Play(players); //GetWinningScore(players, lastMarble);
        }

        private (int players, int lastMarble) ParseData(string data)
        {
            var parts = data.Split(' ').Select(int.Parse).ToArray();
            return new(parts[0], parts[1]);
        }

        internal class MarbleGame
        {
            public HashSet<MarbleNode> Marbles { get; set; }
            public Dictionary<int, long> Players { get; } = new();
            public int Target { get; set; }
            public MarbleNode CurrentNode { get; set; } // tracking CurrentNode as node obj instead of int was key to performance
            public long Winningscore => Players.Max(x => x.Value);

            public long Play(int players)
            {
                for (int i = 0; i < players; i++)
                    Players.Add(i, 0);

                int currentPlayer = 0;
                int currentMarble = 1;

                CurrentNode = new MarbleNode { Value = 0 };
                Marbles = new HashSet<MarbleNode>() { CurrentNode };

                while (true)
                {
                    Place(new MarbleNode() { Value = currentMarble }, currentPlayer);

                    currentMarble++;

                    if (currentMarble == Target)
                        break;

                    currentPlayer = currentPlayer + 1 == players
                        ? 0 : currentPlayer + 1;
                }


                return Players.Max(x => x.Value);
            }

            public void Place(MarbleNode node, int player)
            {
                if (!Marbles.Any())
                {
                    Place(node);
                    return;
                }

                if (Marbles.Count == 1)
                {
                    node.ConnectForward(CurrentNode);
                    node.ConnectBack(CurrentNode);
                    Place(node);
                    return;
                }

                PlaceNext(node, player);
            }

            private void Place(MarbleNode node)
            {
                CurrentNode = node; // tracking CurrentNode as node obj instead of int was key to performance
                Marbles.Add(node);
            }

            private MarbleNode GetScoreMarble(MarbleNode source, int steps = 9)
                => steps == 0 ? source : GetScoreMarble(source.Previous, steps - 1);

            private void PlaceNext(MarbleNode node, int player)
            {
                var target = CurrentNode.Next.Next;
                if (node.Value % 23 == 0)
                {
                    var scoreMarble = GetScoreMarble(target);

                    CurrentNode = scoreMarble.Next;
                    Players[player] += node.Value + scoreMarble.Value;

                    Remove(scoreMarble);
                    return;
                }

                node.ConnectBack(target.Previous);
                node.ConnectForward(target);

                Place(node);
            }

            public void Remove(MarbleNode node)
            {
                node.Previous.ConnectForward(node.Next);

                Marbles.Remove(node);
            }
        }

        public class MarbleNode
        {
            public int Value { get; set; }
            public MarbleNode Previous { get; set; } = null;
            public MarbleNode Next { get; set; } = null;
        }
    }

    public static class Ext9
    {
        public static void ConnectForward(this Day09.MarbleNode node, Day09.MarbleNode next)
        {
            next.Previous = node;
            node.Next = next;
        }

        public static void ConnectBack(this Day09.MarbleNode node, Day09.MarbleNode prevNode)
        {
            prevNode.Next = node;
            node.Previous = prevNode;
        }
    }
}
