using System.Text.Json;
using System.Text.Json.Nodes;
using Project.Application.Abstractions;
using Project.Application.Common;

namespace Project.Infrastructure.Configuration;

/// <summary>
/// <see cref="IConfigurationService"/> backed by a JSON file on disk.
/// Dot-separated keys navigate nested objects: <c>"A.B.C"</c> → <c>{ "A": { "B": { "C": … } } }</c>.
/// </summary>
public sealed class JsonConfigurationService : IConfigurationService
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _writeOptions = new()
    {
        WriteIndented = true,
        Converters    = { new System.Text.Json.Serialization.JsonStringEnumConverter() },
    };
    private JsonObject _root;

    /// <param name="filePath">Absolute or relative path to the JSON configuration file.</param>
    public JsonConfigurationService(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        _filePath = filePath;
        _root = LoadOrCreate();
    }

    /// <inheritdoc />
    public Result<string?> Get(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return Result<string?>.Failure(Errors.Validation("Key must not be empty."));

        var node = Resolve(key);
        return Result<string?>.Success(node?.GetValue<string?>());
    }

    /// <inheritdoc />
    public Result<T> Get<T>(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return Result<T>.Failure(Errors.Validation("Key must not be empty."));

        var node = Resolve(key);
        if (node is null)
            return Result<T>.Failure(Errors.NotFound($"Key '{key}' was not found."));

        try
        {
            var value = node.Deserialize<T>(_writeOptions);
            if (value is null)
                return Result<T>.Failure(Errors.NotFound($"Key '{key}' resolved to null."));
            return Result<T>.Success(value);
        }
        catch (Exception ex)
        {
            return Result<T>.Failure(Errors.Unexpected($"Failed to deserialize '{key}': {ex.Message}"));
        }
    }

    /// <inheritdoc />
    public Result Set(string key, object? value)
    {
        if (string.IsNullOrWhiteSpace(key))
            return Result.Failure(Errors.Validation("Key must not be empty."));

        var segments = key.Split('.');
        var current = _root;

        for (var i = 0; i < segments.Length - 1; i++)
        {
            var segment = segments[i];
            if (current[segment] is not JsonObject nested)
            {
                nested = new JsonObject();
                current[segment] = nested;
            }
            current = nested;
        }

        current[segments[^1]] = value is null ? null : JsonSerializer.SerializeToNode(value, _writeOptions);

        return Persist();
    }

    /// <inheritdoc />
    public Result Remove(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return Result.Failure(Errors.Validation("Key must not be empty."));

        var segments = key.Split('.');
        var current = _root;

        for (var i = 0; i < segments.Length - 1; i++)
        {
            if (current[segments[i]] is not JsonObject nested)
                return Result.Success();
            current = nested;
        }

        current.Remove(segments[^1]);
        return Persist();
    }

    /// <inheritdoc />
    public IReadOnlyList<string> GetKeys() => _root.Select(kv => kv.Key).ToList();

    /// <inheritdoc />
    public Result Reload()
    {
        try
        {
            _root = LoadOrCreate();
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(Errors.Unexpected($"Reload failed: {ex.Message}"));
        }
    }

    // ── helpers ──────────────────────────────────────────────────────────────

    private JsonNode? Resolve(string key)
    {
        JsonNode? current = _root;
        foreach (var segment in key.Split('.'))
        {
            if (current is not JsonObject obj || !obj.TryGetPropertyValue(segment, out current))
                return null;
        }
        return current;
    }

    private Result Persist()
    {
        try
        {
            var dir = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);

            File.WriteAllText(_filePath, _root.ToJsonString(_writeOptions));
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(Errors.Unexpected($"Failed to write config: {ex.Message}"));
        }
    }

    private JsonObject LoadOrCreate()
    {
        if (!File.Exists(_filePath))
            return new JsonObject();

        try
        {
            var text = File.ReadAllText(_filePath);
            return JsonNode.Parse(text) as JsonObject ?? new JsonObject();
        }
        catch
        {
            return new JsonObject();
        }
    }
}
