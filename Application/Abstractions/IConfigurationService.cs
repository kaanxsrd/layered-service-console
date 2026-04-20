using Project.Application.Common;

namespace Project.Application.Abstractions;

/// <summary>
/// Read/write access to a persistent JSON configuration store.
/// Keys use dot-notation sections, e.g. <c>"Logging.MinLevel"</c>.
/// </summary>
public interface IConfigurationService
{
    /// <summary>Returns the value for <paramref name="key"/>, or <c>null</c> when absent.</summary>
    Result<string?> Get(string key);

    /// <summary>Returns the value for <paramref name="key"/> cast to <typeparamref name="T"/>.</summary>
    Result<T> Get<T>(string key);

    /// <summary>Sets <paramref name="key"/> to <paramref name="value"/> and persists the file.</summary>
    Result Set(string key, object? value);

    /// <summary>Removes <paramref name="key"/> from the store and persists the file.</summary>
    Result Remove(string key);

    /// <summary>Returns every top-level key in the store.</summary>
    IReadOnlyList<string> GetKeys();

    /// <summary>Reloads the configuration from disk, discarding any unsaved in-memory changes.</summary>
    Result Reload();
}
