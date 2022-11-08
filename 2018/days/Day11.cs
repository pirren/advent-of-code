using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;
using advent_of_code_lib.extensions;

namespace advent_of_code_2018.days
{
    [ProblemInfo(11, "Chronal Charge")]
    public class Day11 : SolverBase
    {
        public int GraphSize { get; set; } = 300;
        public override object PartOne(string[] data)
        {
            // Part 1: What is the X,Y coordinate of the top-left fuel cell of the 3x3 square with the largest total power?
            var map = new Graph
            {
                SerialNo = data[0].ToInt(),
                SideLength = GraphSize
            };

            var (x, y, _) = map.HighestFuel(3);
            return $"({x},{y})";
        }

        public override object PartTwo(string[] data)
        {
            // Part 2: What is the X,Y,size identifier of the square with the largest total power?
            var map = new Graph
            {
                SerialNo = data[0].ToInt(),
                SideLength = GraphSize,
            };

            var (x, y, size) = map.HighestFuel(300);
            return $"({x},{y},{size})";
        }

        internal class Graph
        {
            public int SideLength { get; set; }
            public int SerialNo { get; set; }
            public Dictionary<(int x, int y), int> PowerMap { get; set; } = new();

            int Power(int serialno, int x, int y)
            {
                var rackId = x + 10;
                return (((rackId * y) + serialno) * rackId).NumberAt(2) - 5;
            }

            void Print()
            {
                for (int y = 0; y < SideLength; y++)
                {
                    for (int x = 0; x < SideLength; x++)
                    {
                        Console.Write(PowerMap[(x, y)].ToString().PadLeft(4));
                    }
                    Console.Write("\r\n");
                }
            }

            public (int x, int y, int size) HighestFuel(int maxsize)
            {
                // serialnumber 18, the largest total 3x3 top-left corner of 33,45 - total power of 29

                for (int y = 0; y < SideLength; y++)
                    for (int x = 0; x < SideLength; x++)
                        PowerMap.Add((x, y), Power(SerialNo, x, y));

                //Print();

                var areas = new Dictionary<(int x, int y, int size), int>();

                for (int s = 2; s < maxsize + 2; s++)
                {
                    for (int y = s; y <= SideLength - s; y++)
                    {
                        for (int x = s; x <= SideLength - s; x++)
                        {
                            int newsum = 0;

                            if(areas.TryGetValue((x - s, y - s, s - 2), out var iarea)) 
                            {
                                for (var dx = x - s; dx < x - 2; dx++)
                                    newsum += PowerMap[(dx, y - 2)];

                                for (var dy = y - s; dy < y - 2; dy++)
                                    newsum += PowerMap[(x - 2, dy)];

                            }
                            areas[(x - s, y - s, s - 1)] = PowerMap[(x - 2,y - 2)] + iarea + newsum;
                        }
                    }
                }

                return areas.OrderByDescending(x => x.Value).First().Key;
            }
        }
    }
}
