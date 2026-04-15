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
        
        string? text = Console.In.ReadToEnd();
        text = text.Replace("\r", "").Replace("\n", "");
        Console.WriteLine();

        Result<int> countResult = _service.WordCount(word, text);

        if (!countResult.IsSuccess)
        {
            ConsoleHelper.WriteWarning(countResult.Error);
        }
        else
        {
            int count = countResult.Value;

            if (count < 10)
                ConsoleHelper.WriteInfo($"Ordet '{word}' forekommer {count} gange.");
            else
                ConsoleHelper.WriteSuccess($"Ordet '{word}' forekommer {count} gange.");

            Console.WriteLine();

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
            Console.Write("  Dit valg (1-{0}): ", folders.Length);

            string option = Console.ReadLine() ?? string.Empty;
            string folderPath;

            if (int.TryParse(option, out int choice) && choice >= 1 && choice <= folders.Length)
            {
                folderPath = Environment.GetFolderPath(folders[choice - 1].Folder);
                if (folders[choice - 1].Folder == Environment.SpecialFolder.UserProfile)
                    folderPath = Path.Combine(folderPath, "Downloads");
            }
            else
            {
                ConsoleHelper.WriteWarning("Ugyldigt valg. Bruger Desktop.");
                folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }

            string fileName   = DateTime.Now.ToString("dd-MM-yyyy-HH:mm") + ".txt";
            string filePath   = Path.Combine(folderPath, fileName);
            string logContent = $"Ord    {word}   -   {count} gange{Environment.NewLine}{Environment.NewLine}{text}";

            Result<string> saveResult = FileService.Save(filePath, logContent);
            Console.WriteLine();

            if (saveResult.IsSuccess)
                ConsoleHelper.WriteSuccess($"Logfil gemt: {saveResult.Value}");
            else
                ConsoleHelper.WriteError(saveResult.Error);
        }

        Console.WriteLine();
        ConsoleHelper.Pause();
    }
}
