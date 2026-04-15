namespace Project.UI;

/// <summary>
/// Static utility class for common console UI tasks.
/// </summary>
public static class ConsoleHelper
{
    private const int PanelWidth = 52;

    /// <summary>
    /// Writes text in the specified color.
    /// </summary>
    /// <param name="text">The string to display.</param>
    /// <param name="color">The color to use.</param>
    public static void WriteColored(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ResetColor();
    }

    /// <summary>
    /// Writes a line of text in the specified color.
    /// </summary>
    /// <param name="text">The string to display.</param>
    /// <param name="color">The color to use.</param>
    public static void WriteLineColored(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    /// <summary>
    /// Clears the console and writes a centered title inside a box.
    /// </summary>
    /// <param name="title">The text to display as a header.</param>
    public static void WriteHeader(string title)
    {
        Console.Clear();
        string border = new string('═', PanelWidth - 2);
        string padded = title.PadLeft((PanelWidth - 2 + title.Length) / 2).PadRight(PanelWidth - 2);
        WriteLineColored($"╔{border}╗", ConsoleColor.Cyan);
        WriteLineColored($"║{padded}║", ConsoleColor.Cyan);
        WriteLineColored($"╚{border}╝", ConsoleColor.Cyan);
        Console.WriteLine();
    }

    /// <summary>
    /// Draws a horizontal gray line as a separator.
    /// </summary>
    public static void WriteSeparator() =>
        WriteLineColored("  " + new string('─', PanelWidth - 4), ConsoleColor.DarkGray);

    /// <summary>Writes a success message in green.</summary>
    public static void WriteSuccess(string msg) => WriteLineColored("  [OK] " + msg, ConsoleColor.Green);
    /// <summary>Writes an error message in red.</summary>
    public static void WriteError(string msg)   => WriteLineColored("  [!!] " + msg, ConsoleColor.Red);
    /// <summary>Writes a warning message in yellow.</summary>
    public static void WriteWarning(string msg) => WriteLineColored("  [**] " + msg, ConsoleColor.Yellow);
    /// <summary>Writes an info message in cyan.</summary>
    public static void WriteInfo(string msg)    => WriteLineColored("  [>>] " + msg, ConsoleColor.DarkCyan);

    /// <summary>
    /// Reads user input from the console without echoing characters (shows * instead).
    /// </summary>
    /// <returns>The entered password as a plain string.</returns>
    public static string ReadPassword()
    {
        var sb = new System.Text.StringBuilder();
        ConsoleKeyInfo key;

        while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
        {
            if (key.Key == ConsoleKey.Backspace)
            {
                if (sb.Length > 0)
                {
                    sb.Remove(sb.Length - 1, 1);
                    Console.Write("\b \b");
                }
            }
            else if (!char.IsControl(key.KeyChar))
            {
                sb.Append(key.KeyChar);
                Console.Write('*');
            }
        }

        Console.WriteLine();
        return sb.ToString();
    }

    /// <summary>
    /// Pauses execution and waits for the user to press Enter.
    /// </summary>
    /// <param name="message">The instruction message to display.</param>
    public static void Pause(string message = "  Press Enter to continue...")
    {
        WriteLineColored(message, ConsoleColor.DarkGray);
        Console.ReadLine();
    }
}
