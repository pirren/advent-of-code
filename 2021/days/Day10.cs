using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;

namespace advent_of_code_2021.days
{
    [ProblemInfo(10, "Syntax Scoring")]
    public class Day10 : SolverBase
    {
        protected Dictionary<char, char> Tags { get; private set; } = new()
        {
            { '}', '{' },
            { ']', '[' },
            { ')', '(' },
            { '>', '<' },
        };

        public override object PartOne(string[] data)
        {
            Dictionary<char, int> scoretable = new() { { ')', 3 }, { ']', 57 }, { '}', 1197 }, { '>', 25137 } };

            return GetCorruptLines(data).Select(ch => scoretable[ch.Item2]).Aggregate((a, b) => a + b);
        }

        public override object PartTwo(string[] data)
        {
            var corruptlines = GetCorruptLines(data).ToArray();
            var scores = AutocompleteScores(data, corruptlines.Select(s => s.Item1));
            return scores.OrderBy(s => s).Skip((data.Length - corruptlines.Length) / 2).First();
        }

        IEnumerable<long> AutocompleteScores(string[] syntaxlines, IEnumerable<string> corruptlines)
        {
            Dictionary<char, int> scoretable = new() { { ')', 1 }, { ']', 2 }, { '}', 3 }, { '>', 4 }, };

            foreach (var line in syntaxlines.Where(line => !corruptlines.Contains(line)))
            {
                var syntax = string.Empty;

                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i].IsStartTag())
                        syntax += line[i];

                    if (line[i].IsEndTag())
                    {
                        if (Tags[line[i]] == syntax.Last())
                            syntax = syntax[0..^1];
                    }
                }

                long totalscore = 0;
                foreach (var match in syntax.Select(s => Tags.FirstOrDefault(x => x.Value == s).Key).Reverse()) // reverse order
                    totalscore = (totalscore * 5) + scoretable[match];

                yield return totalscore;
            }
        }

        IEnumerable<(string, char)> GetCorruptLines(string[] syntaxlines)
        {
            foreach (var line in syntaxlines)
            {
                var syntax = string.Empty;
                char firstcorruptsign = '\0';

                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i].IsStartTag())
                        syntax += line[i];

                    if (line[i].IsEndTag())
                    {
                        if (Tags[line[i]] == syntax.Last())
                            syntax = syntax[0..^1];
                        else
                        {
                            firstcorruptsign = line[i];
                            break;
                        }
                    }
                }
                if (firstcorruptsign != '\0')
                    yield return (line, firstcorruptsign);
            }
        }
    }

    internal static partial class Ext
    {
        public static bool IsStartTag(this char c) => new[] { '{', '[', '(', '<' }.Contains(c);
        public static bool IsEndTag(this char c) => new[] { '}', ']', ')', '>' }.Contains(c);
    }
}
