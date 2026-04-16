// Default IFileService writing through System.IO.File.
using Microsoft.Extensions.Logging;
using Project.Application.Common;

namespace Project.Application.Services.Files;

/// <summary>
/// Default <see cref="IFileService"/> implementation backed by <see cref="System.IO.File"/>.
/// </summary>
public sealed class FileService : IFileService
{
    private readonly ILogger<FileService> _logger;

    /// <summary>Creates a new <see cref="FileService"/>.</summary>
    public FileService(ILogger<FileService> logger) => _logger = logger;

    /// <inheritdoc />
    public async Task<Result<string>> SaveAsync(string path, string content, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(path))
            return Result<string>.Failure(Errors.Validation("Path is required."));

        try
        {
            await File.WriteAllTextAsync(path, content, cancellationToken).ConfigureAwait(false);
            return Result<string>.Success(path);
        }
        catch (IOException ex)
        {
            _logger.LogWarning(ex, "Failed to write file {Path}", path);
            return Result<string>.Failure(Errors.Unexpected(ex.Message));
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Access denied writing {Path}", path);
            return Result<string>.Failure(Errors.Unauthorized(ex.Message));
        }
    }
}
