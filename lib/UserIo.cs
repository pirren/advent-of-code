namespace advent_of_code_lib
{
    /// <summary>
    /// Handles all user in and out feed
    /// </summary>
    public class UserIo
    {
        public UserIo(PrintUtils printUtils)
        {
            PrintUtils = printUtils;
        }

        protected PrintUtils PrintUtils { get; }

        protected int CursorTop { get; } = 11;

        private void ClearWindow()
        {
            Console.SetCursorPosition(0, CursorTop);
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(new string(' ', 20));
            }
            Console.SetCursorPosition(0, CursorTop);
        }

        public T? GetMenuInput<T>(CancellationToken stoppingToken)
            where T : struct
        {
            int activechoice = 0;
            var menuOptions = Enum.GetValues(typeof(T));

            Console.CursorVisible = false;

            while (!stoppingToken.IsCancellationRequested)
            {
                ClearWindow();

                for (int i = 0; i < menuOptions.Length; i++)
                {
                    var selected = i == activechoice;

                    if (selected)
                    {
                        Console.ForegroundColor = PrintUtils.AccentColor;
                        Console.Write(" > ");
                    }
                    else
                        Console.Write("   ");

                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write($" {menuOptions.GetValue(i)?.ToString()}\r\n");
                }

                var action = Console.ReadKey();

                if (stoppingToken.IsCancellationRequested)
                    Environment.Exit(0);
                if (action.Key == ConsoleKey.DownArrow && activechoice < menuOptions.Length - 1)
                    activechoice++;
                else if (action.Key == ConsoleKey.UpArrow && activechoice > 0)
                    activechoice--;
                else if (action.Key == ConsoleKey.Enter)
                {
                    Console.CursorVisible = true;
                    ClearWindow();
                    return (T?)menuOptions.GetValue(activechoice);
                }
            }
            Console.CursorVisible = true;

            ClearWindow();
            return null;
        }

        public T Prompt<T>(string prompt, CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine(prompt);
                var input = Console.ReadLine();
                try
                {
                    var returnVal = (T)Convert.ChangeType(input, typeof(T))!;
                    if (returnVal != null)
                        return returnVal;
                }
                catch
                {
                    ClearWindow();
                    if (stoppingToken.IsCancellationRequested)
                        Environment.Exit(0);

                    Console.WriteLine("Bad input format!");
                    continue;
                }
            }
            return default(T);
        }

        public string Prompt(string prompt)
        {
            Console.WriteLine(prompt);
            return Console.ReadLine() ?? string.Empty;
        }
    }

    public enum ModeOption
    {
        All = 0,
        Latest = 1,
        Single = 2
    }

    public enum DataOption
    {
        Indata = 0,
        Sample = 1
    }
}
