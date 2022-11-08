using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;
using advent_of_code_lib.extensions;

namespace advent_of_code_2021.days
{
    [ProblemInfo(4, "Giant Squid")]
    public class Day04 : SolverBase
    {
        public override object PartOne(string[] data)
        {
            var (draws, boards) = ParseData(data);
            //var draws = new Queue<int>(data[0].Split(',').Select(int.Parse).ToList());
            var gameresults = GetBingoResult(boards, draws, stop: true);
            return gameresults[0].score;
        }

        public override object PartTwo(string[] data)
        {
            var (draws, boards) = ParseData(data);
            var gameresults = GetBingoResult(boards, draws);
            return gameresults.OrderByDescending(x => x.totalDraws).First().score;
        }

        private List<(int totalDraws, int score)> GetBingoResult(List<Board> boards, Queue<int> draws, bool stop = false)
        {
            List<(int, int)> results = new();
            int totalDraws = 0;

            while (draws.Any() && boards.Any())
            {
                var number = draws.Dequeue();
                totalDraws++;

                for (int b = 0; b < boards.Count; b++)
                {
                    boards[b].Mark(number);

                    if (boards[b].Bingo())
                    {
                        results.Add((totalDraws, boards[b].UnmarkedSum() * number));
                        boards.Remove(boards[b]);
                        b--;

                        if (stop) return results;
                    }
                }
            }

            return results;
        }

        public class Board
        {
            public List<Row> Rows { get; set; } = new();

            public bool Bingo()
            {
                if (Rows.Any(x => x.Values.All(v => v == -1)))
                    return true;

                if (0.Range(5).Any(x => Rows.All(f => f.Values[x] == -1)))
                    return true;

                return false;
            }

            public int UnmarkedSum() => Rows.Sum(x => x.Values.Where(y => y != -1).Sum());

            public void Mark(int number)
            {
                if (!Rows.Any(r => r.Values.Contains(number)))
                    return;

                foreach (var row in Rows.Where(x => x.Values.Contains(number)).Select(x => x.Values))
                {
                    for (int i = 0; i < row.Count; i++)
                    {
                        if (row[i] != number) 
                            continue;
                        row[i] = -1;
                    }
                }
            }

            public class Row { public List<int> Values { get; set; } = new(); }
        }

        public (Queue<int> draws, List<Board> boards) ParseData(string[] indata)
        {
            var draws = new Queue<int>(indata[0].Split(',').Select(int.Parse).ToList());
            var lb = new List<Board>();

            for (var b = 2; b < indata.Length; b += 6)
            {
                var board = new Board();
                for (int y = b; y < b + 5; y++)
                {
                    board.Rows.Add(new Board.Row
                    {
                        Values = indata[y].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList()
                    });
                }
                lb.Add(board);
            }

            return (draws, lb);
        }
    }
}
