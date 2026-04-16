// Adapter that exposes WordServiceUI as an IServiceModule so the registry can dispatch it.
using Project.Application.Abstractions;
using Project.Domain.Enums;
using Project.Presentation.Screens;

namespace Project.Presentation.Modules;

/// <summary>Registers the word-count service as a discoverable <see cref="IServiceModule"/>.</summary>
public sealed class WordServiceModule : IServiceModule
{
    private readonly WordServiceUI _ui;

    /// <summary>Creates a new <see cref="WordServiceModule"/>.</summary>
    public WordServiceModule(WordServiceUI ui) => _ui = ui;

    /// <inheritdoc />
    public string Id => "word-count";

    /// <inheritdoc />
    public string Name => "Word Count";

    /// <inheritdoc />
    public string Description => "Counts how many times a word appears in a text";

    /// <inheritdoc />
    public Role MinimumRole => Role.User;

    /// <inheritdoc />
    public Task RunAsync(CancellationToken cancellationToken) => _ui.RunAsync(cancellationToken);
}
