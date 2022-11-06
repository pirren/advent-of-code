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
            return GetWinningScore(players, lastMarble);
        }

        public override object PartTwo(string[] data)
        {
            // Part 2: What would the new winning Elf's score be if the number of the last marble were 100 times larger?
            var (players, lastMarble) = ParseData(data[0]);
            return GetWinningScore(players, lastMarble * 100);
        }

        private long GetWinningScore(int players, int lastMarble)
        {
            int currentPlayer = 0;
            int currentMarble = 1;

            LinkedMarbleGame marbleGame = new(players);

            while (true)
            {
                marbleGame.Place(new MarbleNode() { Value = currentMarble }, currentPlayer);

                currentMarble++;

                if (currentMarble == lastMarble)
                    break;

                currentPlayer = currentPlayer + 1 == players
                    ? 0 : currentPlayer + 1;
            }


            return marbleGame.Winningscore;
        }

        private (int players, int lastMarble) ParseData(string data)
        {
            var parts = data.Split(' ').Select(int.Parse).ToArray();
            return new(parts[0], parts[1]);
        }

        internal class LinkedMarbleGame
        {
            protected HashSet<MarbleNode> Marbles { get; }
            protected Dictionary<int, long> Players { get; } = new();
            protected MarbleNode CurrentNode { get; private set; } // tracking CurrentNode as node obj instead of int was key to performance
            public long Winningscore => Players.Max(x => x.Value);

            public LinkedMarbleGame(int players)
            {
                CurrentNode = new MarbleNode { Value = 0 };
                Marbles = new HashSet<MarbleNode> { CurrentNode };
                for (int i = 0; i < players; i++)
                    Players.Add(i, 0);
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
