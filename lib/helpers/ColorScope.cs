using advent_of_code_lib.configuration;
using advent_of_code_lib.interfaces;

namespace advent_of_code.lib.helpers
{
    public class ColorScope : IDisposable
    {
        /// <summary>
        /// Uses system default AccentColor
        /// </summary>
        /// <param name="configFactory"></param>
        public ColorScope(IConfigurationFactory configFactory)
        {
            if (Enum.TryParse<ConsoleColor>(configFactory.Build<CoreSettings>().AccentColor, out var color))
                Color = color;
            Console.ForegroundColor = Color;
        }

        /// <summary>
        /// Color in use
        /// </summary>
        public ConsoleColor Color { get; } = ConsoleColor.Gray; // Gray default

        /// <summary>
        /// Uses given color
        /// </summary>
        public ColorScope(ConsoleColor color)
        {
            Color = color;
            Console.ForegroundColor = Color;
        }

        public void Dispose()
            => Console.ForegroundColor = ConsoleColor.Gray;
    }
}
