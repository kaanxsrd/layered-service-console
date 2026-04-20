// Adapter that exposes PensionServiceUI as an IServiceModule so the registry can dispatch it.
using Project.Application.Abstractions;
using Project.Domain.Enums;
using Project.Presentation.Screens;

namespace Project.Presentation.Modules;

/// <summary>Registers the pension calculator as a discoverable <see cref="IServiceModule"/>.</summary>
public sealed class PensionServiceModule : IServiceModule
{
    private readonly PensionServiceUI _ui;

    /// <summary>Creates a new <see cref="PensionServiceModule"/>.</summary>
    public PensionServiceModule(PensionServiceUI ui) => _ui = ui;

    /// <inheritdoc />
    public string Id => "pension-calculator";

    /// <inheritdoc />
    public string Name => "Pension Calculator";

    /// <inheritdoc />
    public string Description => "Beregner din alder og antal år til pension";

    /// <inheritdoc />
    public Role MinimumRole => Role.User;

    /// <inheritdoc />
    public Task RunAsync(CancellationToken cancellationToken) => _ui.RunAsync(cancellationToken);
}
