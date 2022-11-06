using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;

namespace advent_of_code_2021.days
{
    [ProblemInfo(3, "Binary Diagnostic")]
    public class Day03 : SolverBase
    {
        protected int BitLength { get; private set; } = 0;
        public override object PartOne(string[] data)
        {
            BitLength = data[0].Length;
            var intData = data.Select(d => Convert.ToInt32(d, 2)).ToList();

            int gammarate = Enumerable.Range(0, BitLength)
                .Select(pos => intData.Count(b => (b & (1 << pos)) != 0) > intData.Count / 2 ? 1 << pos : 0)
                .Sum();
            var epsilonrate = ~gammarate & (1 << BitLength) - 1;

            return gammarate * epsilonrate;
        }

        public override object PartTwo(string[] data)
        {
            BitLength = data[0].Length;
            var ratingdata = data.ToList();

            var oxygenRating = GetRating(new List<string>(ratingdata), RatingType.Oxygen);
            var coRating = GetRating(new List<string>(ratingdata), RatingType.CarbonDioxide);

            return oxygenRating * coRating;
        }

        int GetRating(List<string> rating, RatingType type)
        {
            for (int pos = 0; pos < BitLength; pos++)
            {
                if (rating.Count == 1) break;

                var numOnes = rating.Select(str => new string(str.Reverse().ToArray())).Count(c => (Convert.ToInt32(c, 2) & (1 << pos)) != 0);
                var keepOnes = false;

                if (type == RatingType.Oxygen)
                    keepOnes = numOnes > rating.Count / 2 || numOnes == rating.Count - numOnes;
                else
                    keepOnes = numOnes < (double)rating.Count / 2 && numOnes != rating.Count - numOnes;

                if (keepOnes) rating.RemoveAll(item => item[pos] != '1');
                else rating.RemoveAll(item => item[pos] != '0');
            }
            return Convert.ToInt32(rating.Single(), 2);
        }

        enum RatingType
        {
            Oxygen,
            CarbonDioxide
        }
    }
}
