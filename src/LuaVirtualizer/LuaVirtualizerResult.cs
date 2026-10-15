/******************************************************************************
 * Lua Virtualizer WebApi interface
 *
 * Version        : v1.0.0
 * Language       : C#
 * Author         : Bartosz Wójcik
 * Web page       : https://www.pelock.com
 *
 *****************************************************************************/

using System.Text.Json.Serialization;

namespace PELock.LuaVirtualizer;

/// <summary>Server JSON response payload (see ERROR_* on <see cref="LuaVirtualizer"/>).</summary>
public sealed class LuaVirtualizerResult
{
    /// <summary>Error code (<see cref="LuaVirtualizer.ErrorSuccess"/> on success).</summary>
    [JsonPropertyName("error")]
    public int Error { get; init; }

    /// <summary>Obfuscated source when successful.</summary>
    [JsonPropertyName("output")]
    public string? Output { get; set; }

    /// <summary>Whether demo mode was used (missing or invalid key). A missing or invalid key is limited to 1000 bytes and the server applies only flat code virtualization (code_virtualization=flat), ignoring every other strategy field.</summary>
    [JsonPropertyName("demo")]
    public bool? Demo { get; init; }

    /// <summary>License end date (Y-m-d), empty if none.</summary>
    [JsonPropertyName("license_expiration")]
    public string? LicenseExpiration { get; init; }

    /// <summary>Total obfuscations for this activation code.</summary>
    [JsonPropertyName("usages_total")]
    public long? UsagesTotal { get; init; }

    /// <summary>Max. source code size allowed (bytes), e.g. demo limit.</summary>
    [JsonPropertyName("string_limit")]
    public long? StringLimit { get; init; }

    /// <summary>Dialect actually used (5.1, 5.2, 5.3, 5.4, 5.5, luau, or luajit when requested).</summary>
    [JsonPropertyName("target")]
    public string? Target { get; init; }
}
