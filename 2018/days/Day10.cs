using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;

namespace advent_of_code_2018.days
{
    [ProblemInfo(10, "The Stars Align")]
    public class Day10 : SolverBase
    {
        public override object PartOne(string[] data)
        {
            // Part 1: What message will eventually appear in the sky?
            var pList = GetPoints(data);
            PrintMap(pList);
            return "ZZCBGGCJ";
        }

        public override object PartTwo(string[] data)
        {
            // Part 2: Exactly how many seconds would they have needed to wait for that message to appear?
            var pList = GetPoints(data);
            return PrintMap(pList);
        }

        private int FindSeconds(List<(int pX, int pY, int vX, int vY)> points)
        {
            int latest = int.MaxValue;
            int i = 1;

            while (true)
            {
                var width = Math.Abs(points.GetMaxX(i) - points.GetMinX(i));
                if (width > latest) break;
                latest = width;
                i++;
            }

            return i - 1;
        }

        private int PrintMap(List<(int pX, int pY, int vX, int vY)> points)
        {
            Console.Write("\r\n");
            var time = FindSeconds(points);
            for (int y = points.Min(v => v.pY + v.vY * time); y <= points.Max(v => v.pY + v.vY * time); y++)
            {
                for (int x = points.GetMinX(time); x <= points.GetMaxX(time); x++)
                {
                    if (points.ActiveAtTime(x, y, time)) Console.Write("*");
                    else Console.Write(" ");
                }
                Console.Write("\r\n");
            }

            return time;
        }

        private List<(int pX, int pY, int vX, int vY)> GetPoints(string[] data)
            => data.Select(x => x.Split(',')).Select(s => s.Parse()).ToList();
    }

    public static class Ext10
    {
        public static bool ActiveAtTime(this List<(int pX, int pY, int vX, int vY)> points, int x, int y, int time)
            => points.Any(p => p.pX + (p.vX * time) == x && p.pY + (p.vY * time) == y);

        public static int GetMaxX(this List<(int pX, int pY, int vX, int vY)> points, int time) 
            => points.Max(v => v.pX + v.vX * time);
        public static int GetMinX(this List<(int pX, int pY, int vX, int vY)> points, int time)
            => points.Min(v => v.pX + v.vX * time);

        public static (int i1, int i2, int i3, int i4) Parse(this string[] data)
            => (int.Parse(data[0]), int.Parse(data[1]), int.Parse(data[2]), int.Parse(data[3]));
    }
}
