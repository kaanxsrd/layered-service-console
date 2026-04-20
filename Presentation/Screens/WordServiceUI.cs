// Console UI for the word-count feature, with optional file logging.
using Project.Application.Services.Files;
using Project.Application.Services.Text;
using Project.Presentation.Abstractions;
using Project.Presentation.Rendering;

namespace Project.Presentation.Screens;

/// <summary>Interactive console UI for the word-count feature.</summary>
public sealed class WordServiceUI : IScreen
{
    private readonly WordService  _words;
    private readonly IFileService _files;

    /// <summary>Creates a new <see cref="WordServiceUI"/>.</summary>
    public WordServiceUI(WordService words, IFileService files)
    {
        _words = words;
        _files = files;
    }

    /// <inheritdoc />
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        ConsoleHelper.WriteHeader("WORD COUNT SERVICE");

        Console.Write("  Hvilket ord skal der søges på? ");
        string word = ConsoleHelper.ReadLine();

        Console.WriteLine("  Lim eller skriv din tekst herunder.");
        Console.WriteLine("  Tryk Enter to gange på en tom linje for at afslutte:");
        Console.WriteLine();

        string text = (Console.In.ReadToEnd() ?? string.Empty)
            .Replace("\r", string.Empty, StringComparison.Ordinal)
            .Replace("\n", string.Empty, StringComparison.Ordinal);
        Console.WriteLine();

        var countResult = _words.WordCount(word, text);

        if (countResult.IsFailure)
        {
            ResultRenderer.RenderError(countResult.Error!.Value);
            Console.WriteLine();
            ConsoleHelper.Pause();
            return;
        }

        int count = countResult.Value;
        if (count < 10)
            ConsoleHelper.WriteInfo($"Ordet '{word}' forekommer {count} gange.");
        else
            ConsoleHelper.WriteSuccess($"Ordet '{word}' forekommer {count} gange.");

        Console.WriteLine();
        string folderPath = PickFolder();

        string fileName   = DateTime.Now.ToString("dd-MM-yyyy-HH-mm", System.Globalization.CultureInfo.InvariantCulture) + ".txt";
        string filePath   = Path.Combine(folderPath, fileName);
        string logContent = $"Ord    {word}   -   {count} gange{Environment.NewLine}{Environment.NewLine}{text}";

        var saveResult = await _files.SaveAsync(filePath, logContent, cancellationToken).ConfigureAwait(false);
        Console.WriteLine();
        ResultRenderer.Render(saveResult, p => $"Logfil gemt: {p}");

        Console.WriteLine();
        ConsoleHelper.Pause();
    }

    private static string PickFolder()
    {
        var folders = new (string Label, Environment.SpecialFolder Folder)[]
        {
            ("Desktop",   Environment.SpecialFolder.Desktop),
            ("Documents", Environment.SpecialFolder.MyDocuments),
            ("Downloads", Environment.SpecialFolder.UserProfile),
            ("Pictures",  Environment.SpecialFolder.MyPictures),
            ("Music",     Environment.SpecialFolder.MyMusic),
            ("Videos",    Environment.SpecialFolder.MyVideos),
        };

        ConsoleHelper.WriteInfo("Vælg en mappe at gemme logfilen i:");
        Console.WriteLine();
        for (int i = 0; i < folders.Length; i++)
            Console.WriteLine($"    [{i + 1}] {folders[i].Label}  ({Environment.GetFolderPath(folders[i].Folder)})");
        Console.WriteLine();
        Console.Write($"  Dit valg (1-{folders.Length}): ");

        string option = ConsoleHelper.ReadLine();

        if (int.TryParse(option, out int choice) && choice >= 1 && choice <= folders.Length)
        {
            string path = Environment.GetFolderPath(folders[choice - 1].Folder);
            if (folders[choice - 1].Folder == Environment.SpecialFolder.UserProfile)
                path = Path.Combine(path, "Downloads");
            return path;
        }

        ConsoleHelper.WriteWarning("Ugyldigt valg. Bruger Desktop.");
        return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    }
}
