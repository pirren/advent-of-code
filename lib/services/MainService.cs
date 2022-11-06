using advent_of_code_lib.bases;
using advent_of_code_lib.configuration;
using advent_of_code_lib.interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace advent_of_code_lib.services
{
    public class MainService : ServiceBase
    {
        protected Archive Archive { get; }
        protected CoreSettings CoreSettings { get; }
        protected UserIo UserIo { get; }
        protected PrintUtils PrintUtils { get; }

        public MainService(IServiceScopeFactory scopeFactory, IConfigurationFactory configurationFactory, UserIo userIo,
            Archive archive, PrintUtils printUtils)
            : base(scopeFactory, configurationFactory)
        {
            CoreSettings = configurationFactory.Build<CoreSettings>();
            UserIo = userIo;
            Archive = archive;
            PrintUtils = printUtils;
        }

        public override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.Clear();
                PrintUtils.PrintAscii();

                var modeOpt = UserIo.GetMenuInput<ModeOption>(stoppingToken);
                if (!modeOpt.HasValue) continue;

                int? day = null;
                if (modeOpt.Value == ModeOption.Single)
                    day = UserIo.Prompt<int>("Enter day: ", stoppingToken);

                var dataOpt = UserIo.GetMenuInput<DataOption>(stoppingToken);
                if (!dataOpt.HasValue) continue;

                ExecuteWork(modeOpt, GetFolder(dataOpt), day);

                _ = UserIo.Prompt("Press any key to continue");
            }

            return Task.CompletedTask;
        }

        private string GetFolder(DataOption? option)
            => option == DataOption.Sample
                ? CoreSettings.SampleIndataFolder
                : CoreSettings.IndataFolder;

        private Task ExecuteWork(ModeOption? mode, string folder, int? day) =>
            mode switch
            {
                ModeOption.Single => RunSolution(day, folder),
                ModeOption.All => RunAllSolutions(folder),
                ModeOption.Latest => RunLatestSolution(folder),
                _ => Task.CompletedTask
            };

        private Task RunLatestSolution(string folder)
        {
            var solution = Archive.GetLatestSolution();
            if (solution == null)
                return Task.CompletedTask;

            solution.Solver.Solve(folder);

            Console.Write("\r\n");

            return Task.CompletedTask;
        }

        private Task RunAllSolutions(string folder)
        {
            var solutions = Archive.GetAllSolutions();
            if (solutions.Count == 0)
                return Task.CompletedTask;

            foreach (var solution in solutions)
            {
                solution.Solver.Solve(folder);

                PrintUtils.PrintHr();
            }
            Console.Write("\r\n");

            return Task.CompletedTask;
        }

        private Task RunSolution(int? day, string folder)
        {
            if (!day.HasValue)
                return Task.CompletedTask;

            var solution = Archive.GetSolution(day.Value);
            if (solution == null)
                return Task.CompletedTask;

            solution.Solver.Solve(folder);

            Console.Write("\r\n");

            return Task.CompletedTask;
        }
    }
}
