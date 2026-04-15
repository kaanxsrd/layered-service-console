namespace Project.Services;

/// <summary>
/// File Service for saving the texts
/// </summary>
public class FileService
{
    /// <summary>
    /// Saves the content to the specified file path.
    /// </summary>
    /// <param name="path">The file path where the content will be saved.</param>
    /// <param name="content">The content to save.</param>
    /// <returns>A status message indicating success or an error message if saving fails.</returns>
    public static Result<string> Save(string path, string content)
    {
        try
        {
            File.WriteAllText(path, content);
            return Result<string>.Success(path);
        }
        catch (Exception e)
        {
            return Result<string>.Failure(e.Message);
        }
    }
}