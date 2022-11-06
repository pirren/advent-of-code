using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;

namespace advent_of_code_2018.days
{
    [ProblemInfo(2, "Inventory Management System")]
    public class Day02 : SolverBase
    {
        public override object PartOne(string[] data)
        {
            var letterCounts = GetBoxInformation(data).Select(x => x.count);
            int checksum = letterCounts.Count(x => x == 2) * letterCounts.Count(x => x == 3);
            return checksum;
        }

        public override object PartTwo(string[] data)
        {
            var allBoxIds = GetBoxInformation(data).Select(x => x.boxId);

            foreach (var boxId in allBoxIds)
            {
                foreach (var otherboxId in allBoxIds.Where(x => x != boxId))
                {
                    string lettersInCommon = "";
                    for (int i = 0; i < boxId.Length; i++)
                        if (boxId[i] == otherboxId[i]) lettersInCommon += boxId[i];

                    if (lettersInCommon.Length == boxId.Length - 1)
                        return lettersInCommon;
                }
            }

            return 0;
        }

        private IEnumerable<(int count, string boxId)> GetBoxInformation(string[] indata)
        {
            return indata.SelectMany(
                    (str, internalId) =>
                        str.GroupBy(ch => ch)
                        .Where(x => new[] { 2, 3 }.Contains(x.Count()))
                        .Select(x => new { Count = x.Count(), BoxId = indata[internalId] })
                    ).DistinctBy(x => new { x.BoxId, x.Count })
                    .Select(x => (x.Count, x.BoxId));
        }
    }
}
