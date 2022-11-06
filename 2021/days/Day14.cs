using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;
using advent_of_code_lib.extensions;

namespace advent_of_code_2021.days
{
    [ProblemInfo(14, "Extended Polymerization")]
    public class Day14 : SolverBase
    {
        protected char polymerStart { get; set; } = '\0';
        protected char polymerEnd { get; set; } = '\0';

        protected Dictionary<string, List<string>> InsertionRules { get; set; } = new();
        protected Dictionary<string, long> Polymer { get; set; } = new();

        public override object PartOne(string[] data)
        {
            ParseInput(data);
            return QuantifyElements(10);
        }

        public override object PartTwo(string[] data)
        {
            ParseInput(data);
            return QuantifyElements(40);
        }

        private long QuantifyElements(int steps)
        {
            Polymerize(steps);

            var elementCount = InsertionRules.SelectMany(x => x.Key).Distinct().ToDictionary(s => s, v => 0L);
            foreach (var keySet in Polymer)
            {
                string poly = keySet.Key;
                long amount = keySet.Value;
                elementCount[poly[0]] += amount;
                elementCount[poly[1]] += amount;
            }
            elementCount[polymerStart]++;
            elementCount[polymerEnd]++;

            var abs = elementCount.Max(s => s.Value) - elementCount.Min(s => s.Value);

            return abs / 2L;
        }

        private void Polymerize(int steps)
        {
            if (steps == 0) return;

            var newPolymer = Polymer.Select(s => s.Key).ToDictionary(s => s, v => 0L);

            Polymer.ForEach(set => { Next(set.Key).ForEach(newset => { newPolymer[newset] += set.Value; }); });
            Polymer = newPolymer;
            Polymerize(steps - 1);
        }

        public List<string> Next(string idx) => InsertionRules[idx];

        private void ParseInput(string[] data)
        {
            var split = Array.IndexOf(data, "");
            var template = data[0];

            InsertionRules = data.Skip(split + 1).Select(s => s.Split(" -> "))
                .ToDictionary(s => s[0], v => new List<string>());

            data.Skip(split + 1).Select(s => s.Split(" -> ")).ToArray().ForEach(row =>
            {
                InsertionRules[row[0]].Add(new string(row[0][0].ToString().Concat(row[1]).ToArray()));
                InsertionRules[row[0]].Add(new string(row[1].Concat(new string(row[0][1].ToString())).ToArray()));
            });

            Polymer = InsertionRules.Select(rule => rule.Key).ToDictionary(k => k, v => 0L);
            Enumerable.Range(0, template.Length - 1).ForEach(idx =>
            {
                Polymer[new string(new[] { template[idx], template[idx + 1] })] += 1L;
            });

            polymerStart = template.First();
            polymerEnd = template.Last();
        }
    }
}
