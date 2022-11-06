using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;
using advent_of_code_lib.extensions;
using System.Text.RegularExpressions;

namespace advent_of_code_2018.days
{
    [ProblemInfo(4, "Repose Record")]
    public class Day04 : SolverBase
    {
        public override object PartOne(string[] data)
        {
            // Strategy 1: Find the guard that has the most minutes asleep. What minute does that guard spend asleep the most?
            var logs = GetOrderedLogs(data);
            var guardId = SleepiestGuardId(new(logs));

            return guardId * MostAsleepAt(GetSleepWindowsForGuard(new(logs), guardId));
        }

        public override object PartTwo(string[] data)
        {
            // Strategy 2: Of all guards, which guard is most frequently asleep on the same minute?
            var logs = GetOrderedLogs(data);
            Dictionary<int, List<(DateTime, DateTime)>> sleepWindowsForGuards = new();
            foreach (var guardId in logs.Select(x => x.Id).Where(x => x != 0).Distinct())
            {
                sleepWindowsForGuards.Add(guardId, GetSleepWindowsForGuard(new(logs), guardId));
            }

            Dictionary<int, int> guardsMostAsleepAt = new();
            foreach (var kvp in sleepWindowsForGuards.Where(x => x.Value.Count != 0))
            {
                guardsMostAsleepAt.Add(kvp.Key, MostFrequentSleepingMinute(kvp.Value));
            }
            var guardWithMostFrequentMinute = guardsMostAsleepAt.OrderByDescending(x => x.Value).First();

            var mostAsleepAt = MostAsleepAt(sleepWindowsForGuards[guardWithMostFrequentMinute.Key]);
            return guardWithMostFrequentMinute.Key * mostAsleepAt;
        }

        private int MostFrequentSleepingMinute(List<(DateTime fallAsleep, DateTime wakeUp)> windows)
            => windows.SelectMany(x => Enumerable.Range(x.fallAsleep.Minute, x.wakeUp.Minute - x.fallAsleep.Minute))
                .GroupBy(x => x).Select(x => x.Count()).OrderByDescending(x => x).First();

        private int MostAsleepAt(List<(DateTime fallAsleep, DateTime wakeUp)> windows)
            => windows.SelectMany(x => Enumerable.Range(x.fallAsleep.Minute, x.wakeUp.Minute - x.fallAsleep.Minute))
                .GroupBy(x => x).OrderByDescending(x => x.Count()).Select(grp => grp.Key).First();

        private List<(DateTime, DateTime)> GetSleepWindowsForGuard(Queue<LogEntry> logs, int guardId)
        {
            List<(DateTime, DateTime)> sleepWindowsForGuard = new();
            var guardIdLogs = new Queue<LogEntry>((logs.Where(x => x.Id.Equals(guardId))));
            while (guardIdLogs.Any())
            {
                var shiftStartlog = guardIdLogs.Dequeue(); // discard the shift start log
                if (shiftStartlog.Type != LogType.BeginShift)
                {
                    throw new Exception($"Log was of unexpected type: {shiftStartlog.Type}. Expected type was: {LogType.BeginShift}");
                }
                var shiftLogs = guardIdLogs.DequeueGuardLogs(guardId);
                while (shiftLogs.Any())
                {
                    var (sleepLog, wakeLog) = shiftLogs.NextSleepLogs();
                    sleepWindowsForGuard.Add((sleepLog.TimeStamp, wakeLog.TimeStamp));
                }
            }
            return sleepWindowsForGuard;
        }

        private int SleepiestGuardId(Queue<LogEntry> logs)
        {
            Dictionary<int, int> guardSumSleepTime = new();
            while (logs.Any())
            {
                var guardId = logs.Dequeue().Id;
                var shiftLogs = logs.DequeueGuardLogs(guardId);
                while (shiftLogs.Any())
                    guardSumSleepTime.AddIncrement(guardId, Math.Abs(shiftLogs.SumNextSleepWindow()));
            }
            return guardSumSleepTime.OrderByDescending(x => x.Value).First().Key;
        }

        private Queue<LogEntry> GetOrderedLogs(string[] indata)
        {
            var logs = indata.Select(x =>
            {
                var (timestamp, id, type) = x.GetLogInfo();
                return new LogEntry
                {
                    TimeStamp = timestamp,
                    Type = type,
                    Id = id
                };
            }).OrderBy(x => x.TimeStamp).ToList();
            int currentId = logs.First().Id;
            for (int i = 1; i < logs.Count; i++)
            {
                if (logs[i].Id != 0)
                    currentId = logs[i].Id;
                logs[i].Id = currentId;
            }
            return new Queue<LogEntry>(logs);
        }

        public enum LogType
        {
            BeginShift = 0,
            FallsAsleep = 1,
            WakesUp = 2
        }

        public class LogEntry
        {
            public LogType Type { get; set; }
            public int Id { get; set; }
            public DateTime TimeStamp { get; set; }
        }
    }

    public static class Ext4
    {
        public static (Day04.LogEntry sleepLog, Day04.LogEntry wakeLog) NextSleepLogs(this Queue<Day04.LogEntry> logs)
            => (logs.Dequeue(), logs.Dequeue());

        public static int SumNextSleepWindow(this Queue<Day04.LogEntry> logs)
        {
            if (!logs.Any()) return 0;
            var (sleepLog, wakeLog) = logs.NextSleepLogs();
            return (int)(sleepLog.TimeStamp - wakeLog.TimeStamp).TotalMinutes;
        }

        public static Queue<Day04.LogEntry> DequeueGuardLogs(this Queue<Day04.LogEntry> logs, int guardId)
        {
            Queue<Day04.LogEntry> guardLogs = new();
            while (logs.Any() && logs.Peek().Id.Equals(guardId) && !logs.Peek().Type.Equals(Day04.LogType.BeginShift))
                guardLogs.Enqueue(logs.Dequeue());
            return guardLogs;
        }

        public static void AddIncrement<TKey>(this Dictionary<TKey, int> dict, TKey key, int value)
        {
            if (dict.ContainsKey(key))
                dict[key] += value;
            else
                dict.TryAdd(key, value);
        }

        private static readonly string pattern = @"\[(.*?)\]";
        public static (DateTime timestamp, int id, Day04.LogType type) GetLogInfo(this string data)
        {
            var timestamp = Convert.ToDateTime(Regex.Matches(data, pattern).Single().Groups[1].Value);
            if (data.Contains("begins shift"))
            {
                var id = data.Split(' ').Where(x => x.StartsWith('#')).Single().TrimStart('#').ToInt();
                return (timestamp, id, Day04.LogType.BeginShift);
            }
            if (data.Contains("falls asleep")) return (timestamp, 0, Day04.LogType.FallsAsleep);
            if (data.Contains("wakes up")) return (timestamp, 0, Day04.LogType.WakesUp);

            throw new ArgumentException($"Unhandled log type. Data: {data}");
        }
    }
}
