/******************************************************************************
 * Lua Virtualizer WebApi interface
 *
 * Version        : v1.0.0
 * Language       : C#
 * Author         : Bartosz Wójcik
 * Web page       : https://www.pelock.com
 *
 *****************************************************************************/

using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PELock.LuaVirtualizer;

/// <summary>Public virtualization mode posted as <c>code_virtualization</c>.</summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CodeVirtualization
{
    VM,
    FSA,
    FLAT,
}

/// <summary>Identifier alphabet posted as <c>rename_style</c> (wire values are lowercase: il, o0, …).</summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RenameStyle
{
    Il,
    O0,
    Hex,
    Mangled,
}

/// <summary>Lua Virtualizer WebApi client.</summary>
public sealed class LuaVirtualizer
{
    private const string DefaultUserAgent = "PELock Lua Virtualizer";

    private readonly HttpClient _httpClient;
    private readonly string? _apiKey;

    /// <summary>Default Lua Virtualizer WebApi endpoint.</summary>
    public static Uri ApiUri { get; } = new("https://www.pelock.com/api/lua-virtualizer/v1");

    /// <summary>Default API endpoint as a string (same resource as <see cref="ApiUri"/>).</summary>
    public static readonly string ApiUrl = ApiUri.ToString();

    /// <summary>Success.</summary>
    public const int ErrorSuccess = 0;

    /// <summary>Invalid source size (e.g. 1000 bytes max for demo).</summary>
    public const int ErrorInputSize = 1;

    /// <summary>Input source is empty.</summary>
    public const int ErrorInput = 2;

    /// <summary>Lua source parsing error.</summary>
    public const int ErrorParsing = 3;

    /// <summary>Obfuscation error.</summary>
    public const int ErrorObfuscation = 4;

    /// <summary>Output generation error.</summary>
    public const int ErrorOutput = 5;

    /// <summary>Whether request/response zlib compression is enabled.</summary>
    public bool EnableCompression { get; set; } = false;

    /// <summary>Fixed random seed for reproducible obfuscation output (optional).</summary>
    public int? Seed { get; set; } = null;

    /// <summary>Randomization density / intensity (0-100).</summary>
    public int? RandomizationDensity { get; set; } = null;

    /// <summary>Identifier renaming style (il, o0, hex, mangled).</summary>
    public PELock.LuaVirtualizer.RenameStyle? RenameStyle { get; set; } = null;

    /// <summary>Lua dialect posted as <c>target</c>. <c>auto</c> or empty omits the field. Explicit: 5.1, 5.2, 5.3, 5.4, 5.5, luajit, luau.</summary>
    public string Target { get; set; } = "auto";

    /// <summary>Public virtualization mode (vm, fsa, or flat). Set to null to skip virtualization.</summary>
    public PELock.LuaVirtualizer.CodeVirtualization? CodeVirtualization { get; set; } = PELock.LuaVirtualizer.CodeVirtualization.VM;

    /// <summary>Protection against tampering with protected code (integrity verification).</summary>
    public bool SelfDefending { get; set; } = false;

    /// <summary>Protection linker (decoy call graph).</summary>
    public bool ProtectionLinker { get; set; } = false;

    /// <summary>Rename variable names to random string values.</summary>
    public bool RenameVariables { get; set; } = true;

    /// <summary>Rename parameter names to random string values.</summary>
    public bool RenameParameters { get; set; } = true;

    /// <summary>Rename function names to random string values.</summary>
    public bool RenameFunctions { get; set; } = true;

    /// <summary>Rename function call references consistently with renamed functions.</summary>
    public bool RenameFunctionCalls { get; set; } = true;

    /// <summary>Shuffle function order in the output source.</summary>
    public bool ShuffleFunctions { get; set; } = true;

    /// <summary>Fold/resolve constant expressions at obfuscation time.</summary>
    public bool ResolveConstants { get; set; } = true;

    /// <summary>Split strings into concatenated chunks.</summary>
    public bool SplitStrings { get; set; } = true;

    /// <summary>Apply light transformations/mutations to string literals.</summary>
    public bool ModifyStrings { get; set; } = true;

    /// <summary>Encrypt strings using randomly generated polymorphic encryption algorithms.</summary>
    public bool EncryptStrings { get; set; } = true;

    /// <summary>Store string fragments in char-code array vaults.</summary>
    public bool StringCharArrayVault { get; set; } = true;

    /// <summary>Encrypt integers.</summary>
    public bool EncryptIntegers { get; set; } = true;

    /// <summary>Encrypt floating point numbers.</summary>
    public bool EncryptFloating { get; set; } = true;

    /// <summary>Replace binary operators with mixed boolean-arithmetic (MBA) equivalents.</summary>
    public bool MbaBinops { get; set; } = true;

    /// <summary>Represent integers via floating-point math.</summary>
    public bool IntegersToFloating { get; set; } = true;

    /// <summary>Move integers to arrays.</summary>
    public bool IntegersToArrays { get; set; } = true;

    /// <summary>Move floats to arrays.</summary>
    public bool FloatsToArrays { get; set; } = true;

    /// <summary>Apply redundant xor / affine integer masks.</summary>
    public bool AffineIntegerMask { get; set; } = true;

    /// <summary>Encrypt integer array literals.</summary>
    public bool ArrayIntCrypt { get; set; } = true;

    /// <summary>Encrypt character array literals.</summary>
    public bool ArrayCharCrypt { get; set; } = true;

    /// <summary>Encrypt floating-point array literals.</summary>
    public bool ArrayDoubleCrypt { get; set; } = true;

    /// <summary>Encrypt string array literals.</summary>
    public bool ArrayStringCrypt { get; set; } = true;

    /// <summary>Insert a shared bucket of random noise values used by other strategies.</summary>
    public bool InsertRandomValueBucket { get; set; } = true;

    /// <summary>Populate the random value bucket with decoy integers.</summary>
    public bool RandomBucketIntegers { get; set; } = true;

    /// <summary>Populate the random value bucket with decoy arrays.</summary>
    public bool RandomBucketArrays { get; set; } = true;

    /// <summary>Populate the random value bucket with decoy functions.</summary>
    public bool RandomBucketFunctions { get; set; } = true;

    /// <summary>Populate the random value bucket with decoy characters.</summary>
    public bool RandomBucketCharacters { get; set; } = true;

    /// <summary>Populate the random value bucket with anti-regex decoy noise.</summary>
    public bool RandomBucketAntiRegex { get; set; } = true;

    /// <summary>Populate the random value bucket with autostart decoy stubs.</summary>
    public bool RandomBucketAutostart { get; set; } = true;

    /// <summary>Rewrite selected statements using ternary operators.</summary>
    public bool InsertTernaryOperators { get; set; } = true;

    /// <summary>Replace boolean conditions with equivalent complex expressions.</summary>
    public bool ComplexifyBooleans { get; set; } = true;

    /// <summary>Insert opaque predicate branches.</summary>
    public bool OpaqueBranches { get; set; } = true;

    /// <summary>Insert opaque mixer chains into control flow.</summary>
    public bool OpaqueMixerChain { get; set; } = true;

    /// <summary>Insert dead code.</summary>
    public bool InsertDeadCode { get; set; } = true;

    /// <summary>Wrap code in try/finally blocks with dead noise.</summary>
    public bool TryFinallyNoise { get; set; } = true;

    /// <summary>Insert decoy functions.</summary>
    public bool DecoyFunctions { get; set; } = true;

    /// <summary>Insert decoy lambda expressions.</summary>
    public bool LambdaDecoys { get; set; } = true;

    /// <summary>Insert literal padding noise.</summary>
    public bool LiteralPadding { get; set; } = true;

    /// <summary>Insert fake import statement markers.</summary>
    public bool FakeImportMarkers { get; set; } = true;

    /// <summary>Use dynamic getattr()-based indirect calls.</summary>
    public bool DynamicGetattrCalls { get; set; } = true;

    /// <summary>Rewrite absolute imports into __import__ / getattr.</summary>
    public bool ObfuscateImports { get; set; } = true;

    /// <summary>Insert dead callback/event registration stubs.</summary>
    public bool CallbackRegistrationStubs { get; set; } = true;

    /// <summary>Insert anti-debugging detections.</summary>
    public bool DetectDebugger { get; set; } = false;

    /// <summary>Insert virtual machine (anti-VM) detections.</summary>
    public bool AntiVm { get; set; } = false;

    /// <summary>Insert anti-sandbox detections.</summary>
    public bool AntiSandbox { get; set; } = false;

    /// <summary>Insert anti-emulators (CPU) detections.</summary>
    public bool AntiEmulator { get; set; } = false;

    /// <summary>Strip comments from the output source.</summary>
    public bool RemoveComments { get; set; } = true;

    /// <summary>
    /// Create a client. Empty or invalid <paramref name="apiKey"/> runs demo mode:
    /// A missing or invalid key is limited to 1000 bytes and the server applies only flat code virtualization (code_virtualization=flat), ignoring every other strategy field.
    /// </summary>
    public LuaVirtualizer(string? apiKey = null)
        : this(SharedClient.Value, apiKey)
    {
    }

    /// <summary>
    /// Uses supplied <see cref="HttpClient"/> (caller owns lifetime).
    /// A missing or invalid <paramref name="apiKey"/> is limited to 1000 bytes and the server applies only flat code virtualization (code_virtualization=flat), ignoring every other strategy field.
    /// </summary>
    public LuaVirtualizer(HttpClient httpClient, string? apiKey = null)
    {
        _httpClient = httpClient;
        _apiKey = apiKey;
    }

    private static readonly Lazy<HttpClient> SharedClient = new(() =>
    {
        var client = new HttpClient { Timeout = TimeSpan.FromMinutes(5) };
        return client;
    });

    /// <summary>Login and read license/quota limits.</summary>
    public Task<LuaVirtualizerResult?> LoginAsync(CancellationToken cancellationToken = default) =>
        PostRequestAsync(CreateLoginParams(), cancellationToken);

    /// <summary>Obfuscate Lua script read from UTF-8 file.</summary>
    public async Task<LuaVirtualizerResult?> ObfuscateScriptFileAsync(string scriptFilePath, CancellationToken cancellationToken = default)
    {
        string source;
        try
        {
            source = await File.ReadAllTextAsync(scriptFilePath, Encoding.UTF8, cancellationToken).ConfigureAwait(false);
        }
        catch
        {
            return null;
        }

        return string.IsNullOrEmpty(source) ? null : await ObfuscateScriptSourceAsync(source, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Obfuscate Lua script source string.</summary>
    public Task<LuaVirtualizerResult?> ObfuscateScriptSourceAsync(string scriptSource, CancellationToken cancellationToken = default) =>
        PostRequestAsync(CreateObfuscateParams(scriptSource), cancellationToken);

    private static IEnumerable<KeyValuePair<string, string>> CreateLoginParams() =>
        new[] { new KeyValuePair<string, string>("command", "login") };

    private static IEnumerable<KeyValuePair<string, string>> CreateObfuscateParams(string scriptSource)
    {
        yield return new("command", "obfuscate");
        yield return new("source", scriptSource);
    }

    private async Task<LuaVirtualizerResult?> PostRequestAsync(
        IEnumerable<KeyValuePair<string, string>> paramsEnumerable,
        CancellationToken cancellationToken)
    {
        var dict = paramsEnumerable.ToDictionary(static e => e.Key, static e => e.Value, StringComparer.Ordinal);

        if (!string.IsNullOrEmpty(_apiKey))
            dict["key"] = _apiKey!;

        if (Seed is not null)
            dict["seed"] = Seed.Value.ToString();
        if (RandomizationDensity is not null)
            dict["randomization_density"] = RandomizationDensity.Value.ToString();
        if (RenameStyle is not null)
            dict["rename_style"] = RenameStyle.Value.ToString().ToLowerInvariant();
        if (CodeVirtualization is not null)
            dict["code_virtualization"] = CodeVirtualization.Value.ToString().ToLowerInvariant();
        if (!string.IsNullOrEmpty(Target) && !Target.Equals("auto", StringComparison.OrdinalIgnoreCase))
            dict["target"] = Target;

        if (SelfDefending)
            dict["self_defending"] = "1";
        if (ProtectionLinker)
            dict["protection_linker"] = "1";
        if (RenameVariables)
            dict["rename_variables"] = "1";
        if (RenameParameters)
            dict["rename_parameters"] = "1";
        if (RenameFunctions)
            dict["rename_functions"] = "1";
        if (RenameFunctionCalls)
            dict["rename_function_calls"] = "1";
        if (ShuffleFunctions)
            dict["shuffle_functions"] = "1";
        if (ResolveConstants)
            dict["resolve_constants"] = "1";
        if (SplitStrings)
            dict["split_strings"] = "1";
        if (ModifyStrings)
            dict["modify_strings"] = "1";
        if (EncryptStrings)
            dict["encrypt_strings"] = "1";
        if (StringCharArrayVault)
            dict["string_char_array_vault"] = "1";
        if (EncryptIntegers)
            dict["encrypt_integers"] = "1";
        if (EncryptFloating)
            dict["encrypt_floating"] = "1";
        if (MbaBinops)
            dict["mba_binops"] = "1";
        if (IntegersToFloating)
            dict["integers_to_floating"] = "1";
        if (IntegersToArrays)
            dict["integers_to_arrays"] = "1";
        if (FloatsToArrays)
            dict["floats_to_arrays"] = "1";
        if (AffineIntegerMask)
            dict["affine_integer_mask"] = "1";
        if (ArrayIntCrypt)
            dict["array_int_crypt"] = "1";
        if (ArrayCharCrypt)
            dict["array_char_crypt"] = "1";
        if (ArrayDoubleCrypt)
            dict["array_double_crypt"] = "1";
        if (ArrayStringCrypt)
            dict["array_string_crypt"] = "1";
        if (InsertRandomValueBucket)
            dict["insert_random_value_bucket"] = "1";
        if (RandomBucketIntegers)
            dict["random_bucket_integers"] = "1";
        if (RandomBucketArrays)
            dict["random_bucket_arrays"] = "1";
        if (RandomBucketFunctions)
            dict["random_bucket_functions"] = "1";
        if (RandomBucketCharacters)
            dict["random_bucket_characters"] = "1";
        if (RandomBucketAntiRegex)
            dict["random_bucket_anti_regex"] = "1";
        if (RandomBucketAutostart)
            dict["random_bucket_autostart"] = "1";
        if (InsertTernaryOperators)
            dict["insert_ternary_operators"] = "1";
        if (ComplexifyBooleans)
            dict["complexify_booleans"] = "1";
        if (OpaqueBranches)
            dict["opaque_branches"] = "1";
        if (OpaqueMixerChain)
            dict["opaque_mixer_chain"] = "1";
        if (InsertDeadCode)
            dict["insert_dead_code"] = "1";
        if (TryFinallyNoise)
            dict["try_finally_noise"] = "1";
        if (DecoyFunctions)
            dict["decoy_functions"] = "1";
        if (LambdaDecoys)
            dict["lambda_decoys"] = "1";
        if (LiteralPadding)
            dict["literal_padding"] = "1";
        if (FakeImportMarkers)
            dict["fake_import_markers"] = "1";
        if (DynamicGetattrCalls)
            dict["dynamic_getattr_calls"] = "1";
        if (ObfuscateImports)
            dict["obfuscate_imports"] = "1";
        if (CallbackRegistrationStubs)
            dict["callback_registration_stubs"] = "1";
        if (DetectDebugger)
            dict["detect_debugger"] = "1";
        if (AntiVm)
            dict["anti_vm"] = "1";
        if (AntiSandbox)
            dict["anti_sandbox"] = "1";
        if (AntiEmulator)
            dict["anti_emulator"] = "1";
        if (RemoveComments)
            dict["remove_comments"] = "1";

        if (EnableCompression && dict.TryGetValue("source", out var sourceValue) && !string.IsNullOrEmpty(sourceValue))
        {
            var compressed = ZCompressUtf8(sourceValue);
            dict["source"] = Convert.ToBase64String(compressed);
            dict["compression"] = "1";
        }

        using var form = new MultipartFormDataContent();
        foreach (var pair in dict)
            form.Add(new StringContent(pair.Value, Encoding.UTF8), pair.Key);

        using var message = new HttpRequestMessage(HttpMethod.Post, ApiUri) { Content = form };
        message.Headers.TryAddWithoutValidation("User-Agent", DefaultUserAgent);

        string responseText;
        try
        {
            using var response = await _httpClient.SendAsync(message, cancellationToken).ConfigureAwait(false);
            responseText = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        }
        catch
        {
            return null;
        }

        if (string.IsNullOrEmpty(responseText))
            return null;

        LuaVirtualizerResult? result;
        try
        {
            result = JsonSerializer.Deserialize<LuaVirtualizerResult>(responseText, JsonOpts);
        }
        catch
        {
            return null;
        }

        if (result is null)
            return null;

        if (!EnableCompression || result.Error != ErrorSuccess || string.IsNullOrEmpty(result.Output))
            return result;

        try
        {
            result.Output = ZDecompressUtf8FromBase64(result.Output);
        }
        catch
        {
            return null;
        }

        return result;
    }

    private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = false };

    /// <summary>Zlib compress UTF-8 (compatible with Node <c>zlib.deflateSync(...)</c> with default zlib format).</summary>
    private static byte[] ZCompressUtf8(string text)
    {
        var utf8Bytes = Encoding.UTF8.GetBytes(text);
        using var ms = new MemoryStream();
        using (var zs = new ZLibStream(ms, CompressionLevel.SmallestSize))
        {
            zs.Write(utf8Bytes, 0, utf8Bytes.Length);
        }

        return ms.ToArray();
    }

    /// <summary>Zlib decompress from base64 (compatible with Node <c>zlib.inflateSync</c>).</summary>
    private static string ZDecompressUtf8FromBase64(string base64)
    {
        var buf = Convert.FromBase64String(base64);
        using var input = new MemoryStream(buf, writable: false);
        using var zs = new ZLibStream(input, CompressionMode.Decompress);
        using var outMs = new MemoryStream();
        zs.CopyTo(outMs);
        return Encoding.UTF8.GetString(outMs.ToArray());
    }
}
