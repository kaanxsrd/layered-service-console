// Word counting + inspection helpers used by the text module.
using Project.Application.Common;

namespace Project.Application.Services.Text;

/// <summary>Text-processing utilities: word counting and trivial inspection.</summary>
public sealed class WordService
{
    /// <summary>
    /// Counts the tokens in <paramref name="text"/> that contain <paramref name="searchWord"/>
    /// (case-insensitive). Behavior is preserved from the original implementation.
    /// </summary>
    /// <param name="searchWord">Word to search for.</param>
    /// <param name="text">Text to search within.</param>
    /// <returns>
    /// A <see cref="Result{T}"/> carrying the match count, or a validation/not-found failure.
    /// </returns>
    public Result<int> WordCount(string searchWord, string text)
    {
        if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(searchWord))
            return Result<int>.Failure(Errors.Validation("Det søgte ord eller teksten må ikke være tom."));

        int count = text
            .ToLowerInvariant()
            .Split(' ')
            .Count(x => x.Contains(searchWord.ToLowerInvariant(), StringComparison.Ordinal));

        if (count == 0)
            return Result<int>.Failure(Errors.NotFound($"Ordet '{searchWord}' findes ikke i den angivne tekst."));

        return Result<int>.Success(count);
    }

    /// <summary>
    /// Reports whether a word count is high, low, or zero. Preserved for completeness.
    /// </summary>
    /// <param name="wordCount">The count to inspect.</param>
    /// <returns><c>true</c> when high (≥ 10), <c>false</c> when low (1-9), <c>null</c> when zero.</returns>
    public bool? InspectWordCount(int wordCount) => wordCount switch
    {
        0                     => null,
        >= 1 and < 10         => false,
        _                     => true,
    };
}
