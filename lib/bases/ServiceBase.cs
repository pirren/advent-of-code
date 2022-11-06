using advent_of_code_lib.interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace advent_of_code_lib.bases
{
    public abstract class ServiceBase : IHostedService
    {
        protected AutoResetEvent AutoResetEvent => new(false);
        protected IServiceScopeFactory ScopeFactory { get; }
        protected IConfigurationFactory ConfigurationFactory { get; }

        public ServiceBase(IServiceScopeFactory scopeFactory, IConfigurationFactory configurationFactory)
        {
            ScopeFactory = scopeFactory;
            ConfigurationFactory = configurationFactory;
        }

        /// <summary>
        /// Executing task that must be implemented
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        public abstract Task ExecuteAsync(CancellationToken stoppingToken);

        public virtual async Task StartAsync(CancellationToken stoppingToken)
        {
            if (stoppingToken.IsCancellationRequested)
                return;

            Log.Verbose($"Starting up service {GetType().Name}");

            await Task.Run(async () =>
            {
                await ExecuteAsync(stoppingToken);
            }, stoppingToken);
        }

        public virtual Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}
