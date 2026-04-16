// Typed input + colored-output helpers for the console UI. No business logic lives here.
using System.Text;

namespace Project.Presentation;

/// <summary>Static utility class for common console UI tasks: colors, input, pauses.</summary>
public static class ConsoleHelper
{
    private const int PanelWidth = 52;

    /// <summary>Writes text in the specified color, without a trailing newline.</summary>
    public static void WriteColored(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ResetColor();
    }

    /// <summary>Writes a line of text in the specified color.</summary>
    public static void WriteLineColored(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    /// <summary>Clears the console and writes a centered title inside a box.</summary>
    public static void WriteHeader(string title)
    {
        Console.Clear();
        string border = new('═', PanelWidth - 2);
        string padded = title.PadLeft((PanelWidth - 2 + title.Length) / 2).PadRight(PanelWidth - 2);
        WriteLineColored($"╔{border}╗", ConsoleColor.Cyan);
        WriteLineColored($"║{padded}║", ConsoleColor.Cyan);
        WriteLineColored($"╚{border}╝", ConsoleColor.Cyan);
        Console.WriteLine();
    }

    /// <summary>Draws a horizontal gray line as a separator.</summary>
    public static void WriteSeparator() =>
        WriteLineColored("  " + new string('─', PanelWidth - 4), ConsoleColor.DarkGray);

    /// <summary>Writes a success message in green.</summary>
    public static void WriteSuccess(string msg) => WriteLineColored("  [OK] " + msg, ConsoleColor.Green);

    /// <summary>Writes an error message in red.</summary>
    public static void WriteError(string msg) => WriteLineColored("  [!!] " + msg, ConsoleColor.Red);

    /// <summary>Writes a warning message in yellow.</summary>
    public static void WriteWarning(string msg) => WriteLineColored("  [**] " + msg, ConsoleColor.Yellow);

    /// <summary>Writes an info message in dark cyan.</summary>
    public static void WriteInfo(string msg) => WriteLineColored("  [>>] " + msg, ConsoleColor.DarkCyan);

    /// <summary>Reads a single line, returning an empty string when input is closed.</summary>
    public static string ReadLine() => Console.ReadLine() ?? string.Empty;

    /// <summary>
    /// Prompts until the user enters a non-empty string. Writes <paramref name="prompt"/>
    /// verbatim each attempt.
    /// </summary>
    public static string ReadNonEmpty(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string value = ReadLine();
            if (!string.IsNullOrWhiteSpace(value)) return value;
            WriteWarning("Input cannot be empty.");
        }
    }

    /// <summary>Prompts until the user enters a parseable int.</summary>
    public static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(ReadLine(), out int value)) return value;
            WriteWarning("Please enter a whole number.");
        }
    }

    /// <summary>Reads a password without echoing characters; asterisks are shown in their place.</summary>
    public static string ReadPassword()
    {
        var sb = new StringBuilder();
        ConsoleKeyInfo key;

        while ((key = Console.ReadKey(intercept: true)).Key != ConsoleKey.Enter)
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

    /// <summary>Pauses execution and waits for the user to press Enter.</summary>
    public static void Pause(string message = "  Press Enter to continue...")
    {
        WriteLineColored(message, ConsoleColor.DarkGray);
        Console.ReadLine();
    }
}
