using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;
using advent_of_code_lib.configuration;
using advent_of_code_lib.extensions;
using advent_of_code_lib.interfaces;
using System.Reflection;

namespace advent_of_code_lib
{
    public class Archive
    {
        protected List<Solution> Solutions { get; private set; }
        protected IConfigurationFactory ConfigurationFactory { get; }
        protected CoreSettings CoreSettings { get; }

        public Archive(IConfigurationFactory configurationFactory)
        {
            ConfigurationFactory = configurationFactory;
            CoreSettings = ConfigurationFactory.Build<CoreSettings>();

            var assembly = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(x => x.FullName!.Contains(CoreSettings.ProjectName))
                .FirstOrDefault();

            if (assembly == null)
                throw new ArgumentException($"Could not find assembly for startup project: {CoreSettings.ProjectName}");

            Solutions = LoadArchives(assembly)
                .Select(solver => new Solution(solver, solver.GetProblemInfo()))
                .OrderBy(x => x.Info.Day)
                .ToList();
        }

        public Solution? GetSolution(int day) => Solutions.FirstOrDefault(x => x.Info.Day == day);

        public List<Solution> GetAllSolutions() => Solutions;

        public Solution? GetLatestSolution() => Solutions.Last();

        private List<SolverBase> LoadArchives(Assembly assembly)
            => assembly
                .GetTypes()
                .Where(type => type.IsClass && type.Namespace == CoreSettings.SolverNamespace && type.Name.StartsWith("Day"))
                .Select(type => (SolverBase?)Activator.CreateInstance(type) ?? null)
                .NotNull()
                .ToList();

        public record Solution(SolverBase Solver, ProblemInfo Info) { }
    }
}
