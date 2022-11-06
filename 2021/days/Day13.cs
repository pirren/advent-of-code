using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;
using advent_of_code_lib.extensions;
using System.Drawing;

namespace advent_of_code_2021.days
{
    [ProblemInfo(13, "Transparent Origami")]
    public class Day13 : SolverBase
    {
        protected Sheet Paper { get; set; } = new();
        protected List<Instruction> Instructions { get; set; } = new();

        public override object PartOne(string[] data)
        {
            ParseInput(data);
            return ExecuteFolds(1);
        }

        public override object PartTwo(string[] data)
        {
            ParseInput(data);
            return ExecuteFolds();
        }

        private int ExecuteFolds() => ExecuteFolds(Instructions.Count);

        private int ExecuteFolds(int count)
        {
            for (int i = 0; i < count; i++)
            {
                FoldPaper(Instructions[i]);

                //if(i == 11)
                //    PrintPaper();
            }
            return Paper.Dots.Count;
        }

        private void PrintPaper()
        {
            for (var y = 0; y < Paper.SizeY; y++)
            {
                for (var x = 0; x < Paper.SizeX; x++)
                {
                    if (Paper.Dots.Any(p => p.X == x && p.Y == y))
                    {
                        Console.Write("*");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine("");
            }
        }

        private Sheet GetHalf(Instruction ins)
            => ins.Direction switch
            {
                Direction.Vertical => new Sheet()
                {
                    Dots = new List<Point>(Paper.Dots).Where(s => s.Y > ins.Lines).ToList(),
                    SizeX = Paper.SizeX,
                    SizeY = ins.Lines
                },
                Direction.Horizontal => new Sheet()
                {
                    Dots = new List<Point>(Paper.Dots).Where(s => s.X > ins.Lines).ToList(),
                    SizeX = ins.Lines,
                    SizeY = Paper.SizeY
                },
                _ => throw new Exception("Bad instruction")
            };

        private void FoldPaper(Instruction ins)
        {
            Sheet half = GetHalf(ins);
            Paper.Erase(half);

            if (ins.Direction == Direction.Vertical)
                half.ShiftVertical();
            else if (ins.Direction == Direction.Horizontal)
                half.ShiftHorizontal();

            Paper.Fold(half);
        }

        void ParseInput(string[] indata)
        {
            var split = Array.IndexOf(indata, "");
            Paper.Dots = indata.Take(split).Select(x => x.Split(','))
                .Select(x => new Point(x[0].ToInt(), x[1].ToInt()))
                .ToList();
            Paper.SizeX = Paper.Dots.Max(s => s.X) + 1;
            Paper.SizeY = Paper.Dots.Max(s => s.Y) + 1;

            Instructions = indata.Skip(split + 1).Select(s => s.Split('='))
                .Select(s => new Instruction
                {
                    Direction = s[0].Last() == 'x' ? Direction.Horizontal : Direction.Vertical,
                    Lines = s[1].ToInt()
                }).ToList();
        }

        public enum Direction
        {
            Vertical,
            Horizontal
        }

        public class Instruction
        {
            public Direction Direction { get; set; }
            public int Lines { get; set; }
        }

        public class Sheet
        {
            public void Fold(Sheet half)
            {
                Dots.AddRange(half.Dots.Where(s => !Dots.Contains(s)));
            }

            public void Erase(Sheet negative)
            {
                Dots.RemoveAll(s => negative.Dots.Contains(s));
                SizeX = negative.SizeX;
                SizeY = negative.SizeY;
            }

            public void ShiftVertical()
            {
                Dots = Dots.Select(s => new Point
                {
                    Y = ~Convert.ToInt32(s.Y) + SizeY * 2 + 1,
                    X = s.X
                }).ToList();
            }

            public void ShiftHorizontal()
            {
                Dots = Dots.Select(s => new Point
                {
                    Y = s.Y,
                    X = ~Convert.ToInt32(s.X) + SizeX * 2 + 1,
                }).ToList();
            }

            public int SizeX { get; set; }
            public int SizeY { get; set; }
            public List<Point> Dots { get; set; } = new List<Point>();
        }
    }
}
