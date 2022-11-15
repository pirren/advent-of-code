using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;
using static advent_of_code_2018.days.Day13;

namespace advent_of_code_2018.days
{
    [ProblemInfo(13, "Mine Cart Madness")]
    public class Day13 : SolverBase
    {
        public override object PartOne(string[] data)
        {
            // Part 1: To help prevent crashes, you'd like to know the location of the first crash. (eg. 7,3)
            var (positions, carts) = ParseData(data);
            var map = new Map
            {
                Positions = positions,
                Carts = carts,
                MapSizeX = data[0].Length,
                MapSizeY = data.GetLength(0)
            };
            return map.SimulateCollision();
        }

        public override object PartTwo(string[] data)
        {
            // Part 2: What is the location of the last cart at the end of the first tick where it is the only cart left?
            var (positions, carts) = ParseData(data);
            var map = new Map
            {
                Positions = positions,
                Carts = carts,
                MapSizeX = data[0].Length,
                MapSizeY = data.GetLength(0)
            };
            return map.SimulateCollision(lookforlast: true);
        }

        (char[,] positions, List<Cart> carts) ParseData(string[] data)
        {
            var p = new char[data.Length, data[0].Length];
            var c = new List<Cart>();
            for (var y = 0; y < data.Length; y++)
            {
                for (var x = 0; x < data[0].Length; x++)
                {
                    var d = data[y][x];
                    if (char.IsWhiteSpace(d)) continue;
                    p[y, x] = d;

                    var cart = d.GetCart(x, y);
                    if (cart != null) c.Add(cart);
                }
            }

            return (p, c);
        }

        public class Map
        {
            public char[,] Positions { get; set; }
            public List<Cart> Carts { get; set; }
            public int MapSizeX { get; set; }
            public int MapSizeY { get; set; }

            public void SortCarts()
            {
                Carts = Carts.OrderBy(c => c.Y).ThenBy(c => c.X).ToList();
            }

            public (int x, int y) SimulateCollision(bool lookforlast = false)
            {
                SortCarts();   
                while (Carts.Count(x => x.Alive) > 1)
                {
                    foreach (var cart in Carts)
                    {
                        if (!cart.Alive) continue;
                        var pos = cart.NextPosition(); 

                        if (Positions[pos.y, pos.x] == '+') cart.TurnAtIntersection();
                        else if (Positions[pos.y, pos.x].IsCurve()) cart.Turn(Positions[pos.y, pos.x]);


                        cart.X = pos.x;
                        cart.Y = pos.y;

                        if (!Carts.Any(c => c.Alive && c.X == pos.x && c.Y == pos.y && c != cart)) continue;

                        cart.Alive = false;
                        foreach (var c2 in Carts.Where(c => (c.X, c.Y) == pos && c.Alive)) c2.Alive = false;

                        if (!lookforlast && Carts.Any(x => !x.Alive)) return new(pos.x, pos.y);

                    }
                    SortCarts();
                }
                return Carts.Where(x => x.Alive).Select(cart => (cart.X, cart.Y)).Single();
            }
        }

        public class Cart
        {
            public Dictionary<TurnFor, TurnFor> TurnMap = new()
            {
                { TurnFor.Left, TurnFor.Straight },
                { TurnFor.Straight, TurnFor.Right },
                { TurnFor.Right, TurnFor.Left }
            };

            public bool Alive { get; set; } = true;
            public int X { get; set; }
            public int Y { get; set; }
            public int Dx { get; set; }
            public int Dy { get; set; }

            private TurnFor nextTurn = TurnFor.Left;
            public TurnFor NextTurn()
            {
                var turn = nextTurn;
                nextTurn = TurnMap[nextTurn];
                return turn;
            }

            public (int x, int y) NextPosition() => (X + Dx, Y + Dy);

            public void TurnAtIntersection()
            {
                void T(int i)
                {
                    if (Dx == -1) { Dy = 1 * i; Dx = 0; }
                    else if (Dx == 1) { Dy = -1 * i; Dx = 0; }
                    else if (Dy == -1) { Dy = 0; Dx = -1 * i; }
                    else if (Dy == 1) { Dy = 0; Dx = 1 * i; }
                }

                var direction = NextTurn();
                if (direction == TurnFor.Left) T(1);
                else if (direction == TurnFor.Right) T(-1);
            }

            public void Turn(char turn)
            {
                void T(int i)
                {
                    if (Dx == -1) { Dy = 1 * i; Dx = 0; }
                    else if (Dx == 1) { Dy = -1 * i; Dx = 0; }
                    else if (Dy == -1) { Dy = 0; Dx = 1 * i; }
                    else if (Dy == 1) { Dy = 0; Dx = -1 * i; }
                }

                if (turn == '/') T(1);
                else if (turn == '\\') T(-1);
            }
        }

        public enum TurnFor
        {
            Left,
            Straight,
            Right
        }
    }

    internal static class Ext13
    {
        public static char[] CartTypes { get; } = new char[] { '>', 'v', '<', '^' };
        public static char[] AllTracks { get; } = new char[] { '|', '-', '+', '/', '\\' };
        public static char[] CurveTracks { get; } = new char[] { '/', '\\' };

        public static bool IsCurve(this char ch) => CurveTracks.Contains(ch);
        public static Cart? GetCart(this char ch, int x, int y)
        {
            if (!CartTypes.Contains(ch)) return null;
            return ch switch
            {
                '>' => new Cart { Dx = 1, Dy = 0, X = x, Y = y },
                'v' => new Cart { Dx = 0, Dy = 1, X = x, Y = y },
                '<' => new Cart { Dx = -1, Dy = 0, X = x, Y = y },
                '^' => new Cart { Dx = 0, Dy = -1, X = x, Y = y },
                _ => throw new NotImplementedException()
            };
        }
    }
}
