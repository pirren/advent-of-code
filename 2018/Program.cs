using advent_of_code_lib;
using advent_of_code_lib.configuration;
using advent_of_code_lib.interfaces;
using advent_of_code_lib.services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("configuration.json")
            .AddEnvironmentVariables()
            .Build();

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug)
            .WriteTo.File("..\\..\\..\\logs\\log_.log",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug)
            .CreateLogger();

        services.AddSingleton<IConfigurationFactory>(x => new ConfigurationFactory(config));
        services.AddSingleton<PrintUtils>();
        services.AddScoped<UserIo>();
        services.AddScoped<Archive>();

        Log.Verbose($"Configuring advent of code {config.GetValue(typeof(int), "aoc-version")}.");

        services.AddHostedService<MainService>();
    })
    .Build();

await host.RunAsync();
