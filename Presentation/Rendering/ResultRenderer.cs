// Unified console rendering for Result / Result<T> so every feature prints the same way.
using Project.Application.Common;

namespace Project.Presentation.Rendering;

/// <summary>
/// Central place where <see cref="Result"/> and <see cref="Result{T}"/> outcomes are
/// translated into console output. All feature screens render through this type so
/// users see consistent success/error treatment.
/// </summary>
public static class ResultRenderer
{
    /// <summary>Renders a generic result: on success, formats via <paramref name="onSuccess"/>; on failure, prints the error.</summary>
    /// <typeparam name="T">Payload type.</typeparam>
    /// <param name="result">The result to render.</param>
    /// <param name="onSuccess">Formats the success value into a display string.</param>
    public static void Render<T>(Result<T> result, Func<T, string> onSuccess)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(onSuccess);

        if (result.IsSuccess)
            ConsoleHelper.WriteSuccess(onSuccess(result.Value));
        else
            RenderError(result.Error!.Value);
    }

    /// <summary>Renders a non-generic result with a fixed success message.</summary>
    /// <param name="result">The result to render.</param>
    /// <param name="successMessage">Message to display on success.</param>
    public static void Render(Result result, string successMessage)
    {
        ArgumentNullException.ThrowIfNull(result);

        if (result.IsSuccess)
            ConsoleHelper.WriteSuccess(successMessage);
        else
            RenderError(result.Error!.Value);
    }

    /// <summary>Renders an <see cref="Error"/>, picking a color based on its code.</summary>
    /// <param name="error">The error to render.</param>
    public static void RenderError(Error error)
    {
        switch (error.Code)
        {
            case Errors.Codes.Validation:
            case Errors.Codes.NotFound:
                ConsoleHelper.WriteWarning(error.Message);
                break;
            default:
                ConsoleHelper.WriteError(error.Message);
                break;
        }
    }
}
