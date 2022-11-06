using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;
using advent_of_code_lib.extensions;
using System.Text.RegularExpressions;

namespace advent_of_code_2018.days
{
    [ProblemInfo(7, "The Sum of Its Parts")]
    public class Day07 : SolverBase
    {
        public override object PartOne(string[] data)
        {
            // Part 1: In what order should the steps in your instructions be completed?
            var tree = new Schematic(ParseSteps(data));
            return tree.ChainSequence();
        }

        public override object PartTwo(string[] data)
        {
            // Part 2: With 5 workers and the 60+ second step durations described above,
            //      how long will it take to complete all of the steps?
            var tree = new Schematic(ParseSteps(data));
            return tree.ChainTime();
        }

        public List<SchematicStep> ParseSteps(string[] indata)
        {
            string prevStepPattern = @"Step (.*?) must";
            string stepPattern = @"step (.*?) can";

            var data = indata.Select(x => new
            {
                Prev = Regex.Matches(x, prevStepPattern).Single().Groups[1].Value,
                Step = Regex.Matches(x, stepPattern).Single().Groups[1].Value
            });

            var steps = data.GroupBy(x => x.Step).Select(x => new SchematicStep
            {
                Step = x.Key,
                PrevSteps = x.Select(x => x.Prev).ToList()
            }).ToList();

            // add start steps on top
            steps.AddRange(data.SelectMany(x => x.Prev)
                .Distinct()
                .Where(n => !steps.Select(x => x.Step).Contains(n.ToString()))
                .Select(x => new SchematicStep(x.ToString(), new())));

            return steps;
        }

        public class Schematic
        {
            protected int AmountWorkers { get; } = 5;
            public Schematic(List<SchematicStep> steps)
            {
                Steps = steps;
                Workers = Enumerable.Range(0, AmountWorkers).Select(x => new Worker()).ToList();
            }

            protected List<Worker> Workers { get; set; } = new();
            protected List<SchematicStep> Steps { get; set; } = new();
            protected List<string> CompletedSteps { get; set; } = new();

            private List<Worker> AvailableWorkers => Workers.Where(x => string.IsNullOrEmpty(x.Step)).ToList();
            private List<Worker> BusyWorkers => Workers.Except(AvailableWorkers).ToList();

            public int ChainTime(int time = 0)
            {
                if (!Steps.Any() && !BusyWorkers.Any()) return time;

                while (true)
                {
                    var work = Steps.AvailableSteps(CompletedSteps);
                    if (!work.Any()) break;

                    var worker = AvailableWorkers.FirstOrDefault();
                    if (worker == null) break;

                    worker.Step = Steps.Pop(work.First()).Step;
                }

                foreach (var worker in BusyWorkers)
                {
                    var (success, step) = worker.Work();
                    if (!success) continue;

                    CompletedSteps.Add(step);
                }

                time++;
                return ChainTime(time);
            }

            public string ChainSequence()
            {
                if (!Steps.Any()) return string.Concat(CompletedSteps);
                SchematicStep next;

                if (Steps.Count == 1)
                {
                    next = Steps.Single();
                }
                else
                {
                    next = Steps.AvailableSteps(CompletedSteps).First();
                }

                CompletedSteps.Add(Steps.Pop(next).Step);

                return ChainSequence();
            }
        }

        public record struct SchematicStep(string Step, List<string> PrevSteps) { }

        public class Worker
        {
            public bool Available => string.IsNullOrEmpty(Step);
            public int Time { get; set; } = 0;
            public string Step { get; set; } = string.Empty;

            public (bool success, string task) Work()
            {
                Time++;
                if (Time < Step.TaskTime()) 
                    return (false, string.Empty);

                var returnTask = Step; // capture this before reset

                Time = 0;
                Step = string.Empty;

                return (true, returnTask);
            }
        }
    }

    public static class Ext7
    {
        public static int TaskTime(this string str)
            => Convert.ToInt32(char.Parse(str) - 64) + 60; // letter position in alphabet + 60 (1 minute)

        public static List<Day07.SchematicStep> AvailableSteps(this List<Day07.SchematicStep> remaining, List<string> visited)
            => remaining.Where(remain => !remain.PrevSteps.Any() || remain.PrevSteps.All(x => visited.Contains(x)))
                .OrderBy(a => a.Step).ToList();
    }
}
