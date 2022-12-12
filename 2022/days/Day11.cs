using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;
using advent_of_code_lib.extensions;
using System.Numerics;

namespace advent_of_code_2022.days
{
    [ProblemInfo(11, "Monkey in the Middle")]
    public class Day11 : SolverBase
    {

        public override object PartOne(string[] _)
        {
            // Part 1: What is the level of monkey business after 20 rounds of stuff-slinging simian shenanigans?
            var monkeys = ParseInput(reduce: true);
            return MonkeyBusiness(monkeys, 20);
        }

        public override object PartTwo(string[] _)
        {
            // Part 2: What is the level of monkey business after 10000 rounds?
            var monkeys = ParseInput(reduce: false);
            return MonkeyBusiness(monkeys, 10000);
        }

        private long MonkeyBusiness(List<Monkey> monkeys, int turns)
        {
            int round = 0;
            var lcm = monkeys.Select(x => x.Divisor).Aggregate((a, b) => a * b);

            while (round < turns)
            {
                foreach (var monkey in monkeys)
                {
                    while (monkey.Items.Count > 0)
                    {
                        var item = monkey.Items.Dequeue();
                        item = monkey.Execute(item, lcm);

                        monkeys[item % monkey.Divisor == 0 ? monkey.OnTrue : monkey.OnFalse].Items.Enqueue(item);

                        monkey.HandleCount++;
                    }
                }
                round++;
            }

            return monkeys.Select(x => x.HandleCount)
                .OrderByDescending(x => x)
                .Take(2)
                .Aggregate((x, y) => x * y);
        }

        private List<Monkey> ParseInput(bool reduce)
        {
            List<Monkey> monkeys = new();
            var monkeyData = GetAllData(Folder)
                .Split("\r\n\r\n")
                .Select(x => x.Split("\r\n"));
            foreach (var monkeydata in monkeyData)
            {
                monkeys.Add(new Monkey
                {
                    Items = new(monkeydata[1].Split(',').Select(long.Parse)),
                    ExecutionType = monkeydata[2].Split()[0][0],
                    ExecutionValue = long.Parse(monkeydata[2].Split()[^1]),
                    Divisor = Convert.ToInt32(monkeydata[3]),
                    OnTrue = Convert.ToInt32(monkeydata[4]),
                    OnFalse = Convert.ToInt32(monkeydata[5]),
                    Reduce = reduce
                });
            }
            return monkeys;
        }

        internal record Monkey
        {
            public Queue<long> Items { get; set; }
            public long ExecutionValue { get; set; }
            public char ExecutionType { get; set; }
            public int Divisor { get; set; }
            public int OnFalse { get; set; }
            public int OnTrue { get; set; }
            public bool Reduce { get; set; }
            public long HandleCount { get; set; }

            public int GetReceivingMonkey(long worryLevel) 
                => worryLevel % Divisor == 0
                    ? OnTrue
                    : OnFalse;

            public long Execute(long item, int lcm)
            {
                var val = ExecutionValue == -1 ? item : ExecutionValue;

                var returnValue = ExecutionType switch
                {
                    '*' => item * val,
                    '+' => item + val,
                    _ => throw new ArgumentException("Unknown execution type")
                };

                return Reduce ? returnValue /= 3L : returnValue % lcm;
            }
        }
    }
}
