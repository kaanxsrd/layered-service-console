using Project.Services;

namespace Project.UI;

/// <summary>
/// Provides a console-based user interface for the <see cref="WordService"/>.
/// </summary>
public static class WordServiceUI
{
    private static readonly WordService _service = new();

    /// <summary>
    /// Executes the word count service interaction.
    /// </summary>
    public static void Run()
    {
        ConsoleHelper.WriteHeader("WORD COUNT SERVICE");

        Console.Write("  Hvilket ord skal der søges på? ");
        string word = Console.ReadLine() ?? string.Empty;

        Console.WriteLine("  Lim eller skriv din tekst herunder.");
        Console.WriteLine("  Tryk Enter to gange på en tom linje for at afslutte:");
        Console.WriteLine();
        var sb = new System.Text.StringBuilder();
        string? line;
        bool prevWasEmpty = false;
        while ((line = Console.ReadLine()) != null)
        {
            bool isEmpty = string.IsNullOrWhiteSpace(line);
            if (isEmpty && prevWasEmpty)
                break;
            prevWasEmpty = isEmpty;
            if (isEmpty) continue;
            
            string normalized = WordService.Normalize(line);
            if (sb.Length > 0) sb.Append(' ');
            sb.Append(normalized);
        }
        string text = sb.ToString().Trim();

        Console.WriteLine();

        int count    = _service.WordCount(word, text);
        bool? result = _service.InspectWordCount(count);

        if (result == null)
        {
            ConsoleHelper.WriteWarning("Det søgte ord findes ikke i det angivet tekst.");
        }
        else if (result == false)
        {
            ConsoleHelper.WriteInfo($"Ordet '{word}' forekommer {count} gange.");
        }
        else
        {
            ConsoleHelper.WriteSuccess($"Ordet '{word}' forekommer {count} gange.");
            string outputPath = Path.Combine(AppContext.BaseDirectory, "word_count_result.txt");
            File.WriteAllText(outputPath, text, System.Text.Encoding.UTF8);
            ConsoleHelper.WriteInfo($"Tekst gemt til: {outputPath}");
        }

        Console.WriteLine();
        ConsoleHelper.Pause();
    }
}
