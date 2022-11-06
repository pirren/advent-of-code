using advent_of_code_lib.configuration;
using advent_of_code_lib.interfaces;

namespace advent_of_code.lib.helpers
{
    public class ColorScope : IDisposable
    {
        /// <summary>
        /// Color in use
        /// </summary>
        public ConsoleColor ActiveColor { get; } = ConsoleColor.Gray; // default

        /// <summary>
        /// DefaultColor
        /// </summary>
        protected ConsoleColor DefaultColor { get; } = ConsoleColor.Gray;

        /// <summary>
        /// Uses system default AccentColor in scope
        /// </summary>
        /// <param name="configFactory"></param>
        public ColorScope(IConfigurationFactory configFactory)
        {
            if (Enum.TryParse<ConsoleColor>(configFactory.Build<CoreSettings>().AccentColor, out var color))
                ActiveColor = color;
            Console.ForegroundColor = ActiveColor;
        }

        /// <summary>
        /// Uses given color in scope
        /// </summary>
        public ColorScope(ConsoleColor color)
        {
            ActiveColor = color;
            Console.ForegroundColor = ActiveColor;
        }

        public static ColorScope CreateScope(IConfigurationFactory configFactory) => new(configFactory);
        public static ColorScope CreateScope(ConsoleColor color) => new(color);

        public void Dispose() => Console.ForegroundColor = DefaultColor;
    }
}
