using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;

namespace advent_of_code_2022.days
{
    [ProblemInfo(2, "Rock Paper Scissors")]
    public class Day02 : SolverBase
    {
        public override object PartOne(string[] data)
        {
            // Part 1: What would your total score be if everything goes exactly according to your strategy guide?
            return data.Sum(x =>
            {
                var other = x[0] - 64;
                var you = x[1] - 87;
                return (you == other 
                    ? 3 : you - other == 2 || (other - you != 2 && you < other) 
                    ? 0 : 6
                ) + you;
            });
        }

        public override object PartTwo(string[] data)
        {
            // Part 2: Following the Elf's instructions for the second column,
            // what would your total score be if everything goes exactly according to your strategy guide?
            int IndicatedMove(int o, int result) => result switch
            {
                0 => o == 1 ? o + 2 : o - 1,    // loss
                1 => o,                         // draw
                2 => o == 3 ? o - 2 : o + 1,    // win
                _ => throw new InvalidDataException("Indata not correct or not correctly parsed")
            };

            return data.Sum(x =>
            {
                var other = x[0] - 64;
                var result = x[1] - 88; // 0 = lose, 1 = draw, 2 = win
                return IndicatedMove(other, result) + (result * 3);
            });
        }
    }
}
