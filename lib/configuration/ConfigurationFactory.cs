using advent_of_code_lib.interfaces;
using Microsoft.Extensions.Configuration;

namespace advent_of_code_lib.configuration
{
    public class ConfigurationFactory : IConfigurationFactory
    {
        private readonly IConfiguration config;

        public ConfigurationFactory(IConfiguration config)
        {
            this.config = config;
        }

        public T Build<T>() where T : class
            => config.GetRequiredSection(typeof(T).Name).Get<T>();
    }
}
