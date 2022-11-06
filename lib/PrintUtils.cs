using advent_of_code.lib.helpers;
using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;
using advent_of_code_lib.configuration;
using advent_of_code_lib.interfaces;

namespace advent_of_code_lib
{
    /// <summary>
    /// Prints solutions in templated format
    /// </summary>
    public class PrintUtils
    {
        public PrintUtils(IConfigurationFactory configFactory)
        {
            ConfigFactory = configFactory;
            CoreSettings = configFactory.Build<CoreSettings>();

            if (Enum.TryParse<ConsoleColor>(CoreSettings.AccentColor, out var color))
                AccentColor = color;
        }

        public ConsoleColor AccentColor { get; private set; } = ConsoleColor.Blue;
        protected IConfigurationFactory ConfigFactory { get; }
        protected CoreSettings CoreSettings { get; }
        protected int HrLength => 80;

        public void PrintHr(char? ch = null) =>
            Console.Write($"{ new string(ch.HasValue ? ch!.Value : '-', HrLength) }\r\n");

        public void PrintAscii()
        {
            using (new ColorScope(ConfigFactory))
            {
                Console.WriteLine(File.ReadAllText(CoreSettings.LogoPath));
            };
        }
    }
}
