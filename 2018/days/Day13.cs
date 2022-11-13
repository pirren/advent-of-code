using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;
using Serilog;
using static advent_of_code_2018.days.Day13;

namespace advent_of_code_2018.days
{
    [ProblemInfo(13, "Mine Cart Madness")]
    public class Day13 : SolverBase
    {
        public override object PartOne(string[] data)
        {
            // Part 1: To help prevent crashes, you'd like to know the location of the first crash. (eg. 7,3)
            var (positions, carts) = Parse(data);
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
            // 61,74 not the right answer
            // 126,45 not the right answer
            var (positions, carts) = Parse(data);
            var map = new Map
            {
                Positions = positions,
                Carts = carts,
                MapSizeX = data[0].Length,
                MapSizeY = data.GetLength(0)
            };
            return map.SimulateCollision(lookforlast: true);
        }

        public class Map
        {
            public List<Position> Positions { get; set; }
            public List<Cart> Carts { get; set; }
            public int MapSizeX { get; set; }
            public int MapSizeY { get; set; }

            private int tick = 0;

            //void PrintState()
            //{
            //    Console.WriteLine($"Iteration: {iteration}");
            //    for (int y = 100; y < MapSizeY; y++)
            //    {
            //        for (int x = 0; x < MapSizeX; x++)
            //        {
            //            if (!Positions.Any(p => p.P.x == x && p.P.y == y)) {
            //                Console.Write(" ");
            //                continue;
            //            }
            //            var cart = Carts.FirstOrDefault(c => c.At.P.x == x && c.At.P.y == y);
            //            if (cart != null) Console.Write(cart.Figure);
            //            else Console.Write(Positions.FirstOrDefault(p => p.P.x == x && p.P.y == y)?.Type);
            //        }
            //        Console.Write("\r\n");
            //    }
            //    Console.Write("\r\n");
            //}

            public (int x, int y) SimulateCollision(bool lookforlast = false)
            {
                
                while (Carts.Count(x => x.Alive) > 1)
                {
                    //PrintState();
                    //Console.ReadLine();
                    
                    foreach(var cart in Carts.Where(x => x.Alive).OrderBy(x => x.At.P.x).ThenBy(x => x.At.P.y))
                    {
                        if (!cart.Alive) continue;
                        var nextcartpos = cart.GetNextPosition(Positions);

                        if (nextcartpos.IsIntersection()) cart.TurnAtIntersection();
                        else if (nextcartpos.IsCurve()) cart.Turn(nextcartpos.Type);

                        //Log.Debug($"Cart {cart.Id} {cart.Figure} prev pos: {cart.At.P} - {cart.At.Type}, next pos: {nextcartpos.P} - {nextcartpos.Type}");

                        cart.At = nextcartpos;
                        if(Carts.Any(c => c.At == cart.At && c.Id != cart.Id))
                        {
                            if (!lookforlast) return cart.At.P;

                            foreach(var c2 in Carts.Where(x => x.At == cart.At))
                                c2.Alive = false;
                        }
                    }

                    tick++;
                }
                return Carts.Single(x => x.Alive).At.P;
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

            public int Id { get; set; } // debug
            public char Figure { get; set; }
            public bool Alive { get; set; } = true;
            public Position At { get; set; } = null;

            private TurnFor nextTurn = TurnFor.Left;
            public TurnFor NextTurn()
            {
                var turn = nextTurn;
                nextTurn = TurnMap[nextTurn];
                return turn;
            }

            public Position GetNextPosition(List<Position> positions)
                => Figure switch
                {
                    '>' => positions.First(x => x.P.y == At.P.y && x.P.x == At.P.x + 1),
                    'v' => positions.First(x => x.P.y == At.P.y + 1 && x.P.x == At.P.x),
                    '<' => positions.First(x => x.P.y == At.P.y && x.P.x == At.P.x - 1),
                    '^' => positions.First(x => x.P.y == At.P.y - 1 && x.P.x == At.P.x),
                };

            public void TurnAtIntersection()
            {
                var direction = NextTurn();
                if (direction == TurnFor.Left)
                {
                    Figure = Figure switch
                    {
                        '>' => '^',
                        'v' => '>',
                        '<' => 'v',
                        '^' => '<',
                        _ => throw new Exception()
                    };
                }
                else if (direction == TurnFor.Right)
                {
                    Figure = Figure switch
                    {
                        '>' => 'v',
                        'v' => '<',
                        '<' => '^',
                        '^' => '>',
                        _ => throw new Exception()
                    };
                }
            }

            public void Turn(char turn)
            {
                if (turn == '/')
                {
                    Figure = Figure switch
                    {
                        '>' => '^',
                        'v' => '<',
                        '<' => 'v',
                        '^' => '>',
                        _ => throw new Exception()
                    };
                }
                else if (turn == '\\')
                {
                    Figure = Figure switch
                    {
                        '>' => 'v',
                        'v' => '>',
                        '<' => '^',
                        '^' => '<',
                        _ => throw new Exception()
                    };
                }
            }
        }

        public class Position
        {
            public (int x, int y) P { get; set; }
            public char Type { get; set; }
            public static char[] CurveTracks { get; } = new char[] { '/', '\\' };
            public bool IsIntersection() => Type == '+';
            public bool IsCurve() => CurveTracks.Contains(Type);
        }

        public enum TurnFor
        {
            Left,
            Straight,
            Right
        }

        (List<Position> positions, List<Cart> carts) Parse(string[] data)
        {
            var p = new List<Position>();
            var c = new List<Cart>();
            var cartcount = 0;
            for (var y = 0; y < data.Length; y++)
            {
                for (var x = 0; x < data[0].Length; x++)
                {
                    var d = data[y][x];
                    if (char.IsWhiteSpace(d)) continue;

                    var position = new Position
                    {
                        P = (x, y),
                        Type = d.UnderlyingTrack()
                    };
                    p.Add(position);

                    var cart = d.GetCart(position);
                    if (cart != null)
                    {
                        cartcount++;
                        cart.Id = cartcount;
                        c.Add(cart);
                    }
                }
            }

            return (p, c);
        }
    }

    static class Ext13
    {
        public static char[] CartTypes { get; } = new char[] { '>', 'v', '<', '^' };
        public static char[] AllTracks { get; } = new char[] { '|', '-', '+', '/', '\\' };
        public static char[] PathTracks { get; } = new char[] { '|', '-' };

        public static Cart? GetCart(this char ch, Position pos) => CartTypes.Contains(ch) ? new Cart { Figure = ch, At = pos } : null;

        public static char UnderlyingTrack(this char ch)
        {
            if (AllTracks.Contains(ch)) return ch;
            if (new char[] { '^', 'v' }.Contains(ch)) return '|';
            if (new char[] { '<', '>' }.Contains(ch)) return '-';
            throw new Exception("Unhandled track part");
        }
    }
}
