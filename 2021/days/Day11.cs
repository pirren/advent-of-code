using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;
using advent_of_code_lib.extensions;

namespace advent_of_code_2021.days
{
    [ProblemInfo(11, "Dumbo Octopus")]
    public class Day11 : SolverBase
    {
        public override object PartOne(string[] data)
        {
            var rows = data.Length;
            var cols = data.First().Length;

            var octopus = ModelOctopuses(data, rows, cols).ToList();
            octopus = Link(octopus, rows, cols);

            0.Range(100).ForEach(day =>
            {
                octopus.ForEach(oct => oct.Increase(day));
            });

            return octopus.Sum(oct => oct.Flashes);
        }

        public override object PartTwo(string[] data)
        {
            var rows = data.Length;
            var cols = data.First().Length;

            var octopus = ModelOctopuses(data, rows, cols).ToList();
            octopus = Link(octopus, rows, cols);

            int steps = 0;
            while (true)
            {
                octopus.ForEach(oct => oct.Increase(steps));

                if (octopus.All(s => s.Value == 0))
                    break;

                steps++;
            }
            return steps + 1;
        }

        IEnumerable<Octopus> ModelOctopuses(string[] data, int rows, int cols)
        {
            for (int y = 0; y < rows; y++)
                for (int x = 0; x < cols; x++)
                    yield return new Octopus(data[x][y].ToInt(), x, y);
        }

        public static List<Octopus> Link(IEnumerable<Octopus> octopuses, int rows, int cols)
        {
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    var iter = octopuses.First(oct => oct.X == x && oct.Y == y);

                    // link latitudes and diagonals
                    if (y > 0) // add north 
                    {
                        iter.Linked.Add(octopuses.First(oct => oct.X == x && oct.Y == y - 1));
                        if (x < cols - 1) // add north east
                            iter.Linked.Add(octopuses.First(oct => oct.X == x + 1 && oct.Y == y - 1));
                    }

                    if (x < cols - 1) // add east
                    {
                        iter.Linked.Add(octopuses.First(oct => oct.X == x + 1 && oct.Y == y));
                        if (y < rows - 1) // add south east
                            iter.Linked.Add(octopuses.First(oct => oct.X == x + 1 && oct.Y == y + 1));
                    }

                    if (y < rows - 1) // add south
                    {
                        iter.Linked.Add(octopuses.First(oct => oct.X == x && oct.Y == y + 1));
                        if (x > 0) // add south west
                            iter.Linked.Add(octopuses.First(oct => oct.X == x - 1 && oct.Y == y + 1));
                    }

                    if (x > 0) // add west
                    {
                        iter.Linked.Add(octopuses.First(oct => oct.X == x - 1 && oct.Y == y));
                        if (y > 0) // add north west 
                            iter.Linked.Add(octopuses.First(oct => oct.X == x - 1 && oct.Y == y - 1));
                    }
                }
            }
            return octopuses.ToList();
        }
    }

    public class Octopus
    {
        public int X { get; set; }
        public int Y { get; set; }
        public List<Octopus> Linked { get; set; } = new();
        public int Value { get; private set; }
        public int Flashes { get; private set; }
        protected int LastDayIncreased { get; private set; } = 0;

        public void Increase(int day)
        {
            if (LastDayIncreased >= day && Value == 0)
                return;

            Value = Value == 9 ? 0 : Value + 1;
            if (Value == 0)
                Flashes++;

            LastDayIncreased = day;

            if (Value == 0) // chain on flash
                foreach (var oct in Linked)
                    oct.Increase(day);
        }

        public Octopus(int initialvalue, int x, int y)
        {
            X = x;
            Y = y;
            Value = initialvalue;
            Flashes = 0;
        }
    }
}
