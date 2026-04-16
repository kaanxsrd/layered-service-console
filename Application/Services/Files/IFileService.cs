// Contract for writing textual content to the filesystem.
using Project.Application.Common;

namespace Project.Application.Services.Files;

/// <summary>Writes textual content to the local filesystem.</summary>
public interface IFileService
{
    /// <summary>Saves <paramref name="content"/> to <paramref name="path"/>.</summary>
    /// <param name="path">Destination file path.</param>
    /// <param name="content">Text to write.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="Result{T}"/> with the written path on success.</returns>
    Task<Result<string>> SaveAsync(string path, string content, CancellationToken cancellationToken);
}
