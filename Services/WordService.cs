using System.Text.RegularExpressions;

namespace Project.Services;

/// <summary>
/// Provides text processing and word counting utilities.
/// </summary>
public class WordService
{
    /// <summary>
    /// Normalizes input text by handling various line endings and whitespace characters.
    /// </summary>
    /// <param name="input">The string to normalize.</param>
    /// <returns>A normalized version of the input string.</returns>
    public static string Normalize(string input)
    {
        input = input.Replace("\r\n", "\n").Replace('\r', '\n');
        
        input = Regex.Replace(input, @"[\u00A0\u1680\u2000-\u200A\u202F\u205F\u3000]", " ");

        input = Regex.Replace(input, @"[\u200B-\u200D\u2028\u2029\uFEFF]", "");

        return input;
    }

    /// <summary>
    /// Counts occurrences of a specific word in a text.
    /// </summary>
    /// <param name="searchWord">The word to search for.</param>
    /// <param name="text">The text to search within.</param>
    /// <returns>Number of occurrences found.</returns>
    public int WordCount(string searchWord, string text)
    {
        if (string.IsNullOrEmpty(searchWord) || string.IsNullOrEmpty(text))
            return 0;

        text       = Normalize(text);
        searchWord = Normalize(searchWord).Trim();

        string pattern = $@"\b{Regex.Escape(searchWord)}\b";
        return Regex.Matches(text, pattern, RegexOptions.IgnoreCase).Count;
    }
    
    /// <summary>
    /// Inspects if a word count is within an acceptable range.
    /// </summary>
    /// <param name="wordCount">The count to inspect.</param>
    /// <returns>True if count is high, False if low but present, Null if zero.</returns>
    public bool? InspectWordCount(int wordCount)
    {
        if (wordCount == 0)
            return null;

        if (wordCount >= 1 && wordCount < 10)
            return false;

        return true;
    }
}
