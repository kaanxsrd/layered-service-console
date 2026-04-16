// Unifying contract for every interactive console screen.
namespace Project.Presentation.Abstractions;

/// <summary>
/// A single interactive console screen. Screens are composed by higher-level screens and
/// invoked as leaves of the navigation tree.
/// </summary>
public interface IScreen
{
    /// <summary>Runs the screen to completion.</summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task RunAsync(CancellationToken cancellationToken);
}
