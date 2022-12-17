using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;

namespace advent_of_code_2022.days
{
    [Slow]
    [ProblemInfo(14, "Regolith Reservoir")]
    public class Day14 : SolverBase
    {

        public override object PartOne(string[] data)
        {
            // Part 1: How many units of sand come to rest before sand starts flowing into the abyss below?
            var map = new Map(data, infiniteFloor: false);
            return map.ProduceSand(continueFalling: false);
        }

        public override object PartTwo(string[] data)
        {
            // Part 2: How many units of sand come to rest?
            // 29805
            var map = new Map(data, infiniteFloor: true);
            return map.ProduceSand(continueFalling: true);
        }

        internal class Block : IComparable<Block>, IEquatable<Block>
        {
            public bool TryMove(int x, int y)
            {
                if (X == x && Y == y) return false;
                X = x;
                Y = y;
                return true;
            }

            public int X { get; set; }
            public int Y { get; set; }

            public int CompareTo(Block? other)
            {
                return Equals(other) ? 0
                    : Y == other!.Y && X < other.X ? -1
                    : Y == other.Y && X > other.X ? 1
                    : Y > other.Y ? -1 : 1;
            }

            public bool Equals(Block? other) 
                => X == other!.X && Y == other.Y;
        }

        private class Map
        {
            private SortedSet<Block> Graph = new();
            private readonly int Void;

            private SortedSet<((int x, int y) from, (int x, int y) to)> Walls = new();
            private List<(int x, int y)> Directions = new() { (0, 1), (-1, 1), (1, 1) };
            private (int x, int y) Source => (500, 0);

            public Map(string[] data, bool infiniteFloor)
            {
                foreach (var row in data.Select(d => d.Split(" -> ").Select(x => x.Split(',').Select(int.Parse).ToList()).ToList()))
                {
                    for (int i = 1; i < row.Count; i++)
                    {
                        var prev = row[i - 1];
                        var current = row[i];

                        Walls.Add(((Math.Min(prev[0], current[0]), Math.Min(prev[1], current[1]))
                            , (Math.Max(prev[0], current[0]), Math.Max(prev[1], current[1]))));
                    }
                }

                Graph.Add(new Block { X = Source.x, Y = Source.y});
                Void = Walls.Max(wall => Math.Max(wall.to.y, wall.from.y)) + (infiniteFloor ? 2 : 0);

                if (!infiniteFloor) return;

                Walls.Add(((Source.x - 1000, Void), (Source.y + 1000, Void)));
            }

            public bool MoveSand(Block block, bool infinite)
            {
                int triedDirections = 0;
                while (triedDirections < 3)
                {
                    bool moved = false;
                    var (dx, dy) = Directions[triedDirections % 3];

                    var xnext = block.X + dx;
                    var ynext = block.Y + dy;

                    if ((dx, dy) == (0, 1) // heading down, go as low as we can
                        && !Graph.Any(n => n.X == block.X && n.Y == ynext) // we are not standing on something
                        && block.Y < Void - 1) // we are not standing on void
                    {
                        // find walls below
                        var closestFloor = Walls.Where(wall => xnext >= wall.from.x && xnext <= wall.to.x)
                            .Select(wall => wall.from.y)
                            .Where(level => level > block.Y)
                            .Min();
                            //.OrderBy(x => x).FirstOrDefault();

                        var closestBlockBelow = Graph.Where(n => n.X == block.X && n.Y > block.Y && n.Y < closestFloor)
                            .OrderBy(x => x.Y)
                            .FirstOrDefault();

                        moved = closestBlockBelow == null
                            ? block.TryMove(block.X, closestFloor - 1)
                            : block.TryMove(block.X, closestBlockBelow.Y - 1);
                    }
                    else if((dx, dy) != (0, 1)) // move one step diagonally if possible
                    {
                        moved = (!Walls.Any(wall => wall.CrossProductIntersect((block.X + dx, block.Y + dy)))
                            && !Graph.Any(n => n.X == xnext && n.Y == ynext) && block.TryMove(xnext, ynext));
                    }

                    if (!infinite && block.Y == Void) return true;

                    triedDirections = moved ? 0 // reset to zero, after a diagonal step the sand falls
                        : triedDirections + 1; // change direction for next try
                }

                //if (!infinite && block.Y == Void) return true;
                return (infinite && block.X == Source.x && block.Y == Source.y)
                    || (!infinite && block.Y == Void);
            }

            public int ProduceSand(bool continueFalling)
            {
                bool overflowing = false;
                int sandOnScreen = 0;
                while (!overflowing)
                {
                    var block = new Block { X = 500, Y = 0 };
                    overflowing = MoveSand(block, continueFalling);
                    Graph.Add(block);
                    sandOnScreen++;
                }

                return sandOnScreen
                   - (continueFalling ? 0 : 1); // subtract one for the overflow
            }
        }
    }

    internal static class Ext14
    {
        public static bool CrossProductIntersect(this ((int x, int y) min, (int x, int y) max) line, (int x, int y) n)
        {
            var dxc = n.x - line.min.x;
            var dyc = n.y - line.min.y;

            var dxl = line.max.x - line.min.x;
            var dyl = line.max.y - line.min.y;

            if (dxc * dyl - dyc * dxl != 0) return false;

            if (Math.Abs(dxl) >= Math.Abs(dyl))
                return dxl > 0 ?
                  line.min.x <= n.x && n.x <= line.max.x :
                  line.max.x <= n.x && n.x <= line.min.x;
            else
                return dyl > 0 ?
                  line.min.y <= n.y && n.y <= line.max.y :
                  line.max.y <= n.y && n.y <= line.min.y;
        }
    }
}
