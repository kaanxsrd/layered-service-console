using System.Text.RegularExpressions;

namespace Project.Services;

/// <summary>
/// Provides text processing and word counting utilities.
/// </summary>
public class WordService
{
    /// <summary>
    /// Counts occurrences of a specific word in a text.
    /// </summary>
    /// <param name="searchWord">The word to search for.</param>
    /// <param name="text">The text to search within.</param>
    /// <returns>
    /// <see cref="Result{T}"/> with the count on success, or a failure message if the word was not found.
    /// </returns>
    public Result<int> WordCount(string searchWord, string text)
    {
        if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(searchWord))
            return Result<int>.Failure("Det søgte ord eller teksten må ikke være tom.");

        int count = text.ToLower().Split(' ').Count(x => x.Contains(searchWord.ToLower()));

        if (count == 0)
            return Result<int>.Failure($"Ordet '{searchWord}' findes ikke i den angivne tekst.");

        return Result<int>.Success(count);
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


    /// <summary>
    /// Handles a dynamic parameter, performing different operations based on its type.
    /// </summary>
    /// <param name="parameter">The dynamic parameter to handle.</param>
    /// <returns>The result of the operation.</returns>
    public dynamic HandleParameter(dynamic parameter)
    {
        if (parameter is int intValue || parameter is double doubleValue || parameter is float floatValue)
        {
            return parameter + 100;
        }
        else if (parameter is string strValue)
        {
            return "Følgende tekst modtaget: " + parameter;
        }
        else
        {
            return false;
        }
    }
}
