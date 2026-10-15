# Lua Virtualizer — Obfuscate, Virtualize & Protect Python Code

**[Lua Virtualizer](https://www.pelock.com/products/lua-virtualizer)** is a tool to obfuscate, virtualize & protect Lua `.lua` scripts against reverse engineering with a VM engine (or finite-state automata machine), self-defending integrity checks, polymorphic string encryption, anti-debugging, anti-vm, anti-emulation and 50+ total obfuscation strategies!

This repository contains the **C# / .NET WebApi client SDK** (`PELock.LuaVirtualizer`).

Product page:

* https://www.pelock.com/products/lua-virtualizer

Multiple programming APIs available:

* https://www.pelock.com/products/lua-virtualizer/api

## Why Lua scripts need obfuscation?

Scripts are typically distributed as plain `.lua` files or bundled inside packages. That convenience means anyone with file access can read the full logic, hunt for credentials or API keys in strings, and steal your algorithms unless you take extra steps to hide intent.

[Lua](https://www.lua.org/) is one of the most popular general-purpose scripting languages. It is widely used for automation, backend services, data processing, tooling, and licensing/agent code that customers run on their own machines.

## Obfuscation strategies

Lua Virtualizer comes with many advanced obfuscation, virtualization & protection strategies. You can easily tune protection versus size and performance.

### ![Obfuscation](https://raw.githubusercontent.com/PELock/Lua-Virtualizer-CSharp/main/images/obfuscation-32.png) Powerful obfuscation

Polymorphic string encryption is the core of the string pipeline: each build generates a fresh decryptor so literals never share one static algorithm. Integers, floats, and decoy noise join that layer. The result conceals literals and structure while preserving tested runtime behaviour.

### ![Processor](https://raw.githubusercontent.com/PELock/Lua-Virtualizer-CSharp/main/images/processor-32.png) Code virtualization

Code virtualization is the main control-flow defense. Selected statements are lifted into a randomly generated VM engine (or FSA / flattened dispatcher) with shuffled dispatch tables, decoy opcodes, and an obfuscated dispatcher loop. Analysts must interpret the virtual machine instead of reading plain Lua.

### ![Node](https://raw.githubusercontent.com/PELock/Lua-Virtualizer-CSharp/main/images/node-32.png) Finite-state automata (FSA)

Finite-state automata (FSA) obfuscation rewrites linear Lua statement blocks into dual-state automata with opaque schedulers and shuffled dispatch handlers. Instead of reading code top to bottom, analysts must follow numeric states, transition tables, and decoy paths to reconstruct the original order.

### ![Bug](https://raw.githubusercontent.com/PELock/Lua-Virtualizer-CSharp/main/images/bug-32.png) Anti-debugging

Anti-debugging protection inserts polymorphic probes that detect attached debuggers, tracing hooks, and related host signals, together with anti-VM, anti-sandbox and anti-emulator checks. When a check fires, the obfuscated script exits silently instead of revealing protected logic under interactive analysis.

### ![System monitor](https://raw.githubusercontent.com/PELock/Lua-Virtualizer-CSharp/main/images/system-monitor-32.png) Self-integrity checks

Self-defending integrity checks verify that the obfuscated file has not been patched. A bootstrap probe hashes the on-disk script and sets a tamper key when it no longer matches the obfuscated build. String decryptors consume that key, so edited scripts return garbage instead of plaintext.

### ![Brick link](https://raw.githubusercontent.com/PELock/Lua-Virtualizer-CSharp/main/images/brick-link-32.png) Protection linker

The protection linker adds decoy functions and fake calls so a copied fragment still looks like real program code. Hidden traps fire only if someone edits the file or runs a piece of it on its own. When the script starts normally, those extras stay silent and the program runs as usual.

## Before and after obfuscation

Look at this example — the same script becomes harder to read at a glance after obfuscation.

### Sample Lua script before obfuscation

```lua
local function greet(name)
  print("Hello, " .. name)
end
greet("Lua")
```

### After obfuscation

Would you still recognise the original intent if you only had the obfuscated text and no prior copy of the script?

### How does Lua Virtualizer work?

The engine parses Lua source into an AST tree, then applies selectable transforms. Code virtualization (VM, FSA, or flattening) rewrites execution so analysts cannot read the script top to bottom. Polymorphic string encryption hides literals; self-defending integrity probes detect patched files. Also available: identifier renaming, numeric encryption, noise and decoy insertion, the protection linker, and anti-debugging checks. Many techniques are specific to this product; some ideas are shared with our other protection tools.

When all passes finish, the engine emits a new `.lua` file. Edge cases in the Lua grammar and hosting environments mean you should always test the output in your target runtime.

## Lua dialect (`target`)

The client `target` property defaults to `auto`. When `target` is `auto` or empty, the SDK omits the POST field. Sending `target=auto` is also valid. Explicit values are `5.1`, `5.2`, `5.3`, `5.4`, `5.5`, `luajit`, and `luau`.

Omitting `target` or sending `auto` detects the dialect by trying 5.1, then 5.2, 5.3, 5.4, 5.5, then Luau, and keeping the oldest that parses. LuaJIT is never inferred. A file whose only newer syntax is `goto` is detected as 5.2 because `goto` was introduced in Lua 5.2. Pass `luajit` when the script must be treated as LuaJIT. The JSON response field `target` is the dialect actually used.

## Protect your Lua scripts & algorithms

Take no chances, use **Lua Virtualizer** to obfuscate, virtualize and protect your Lua scripts and algorithms.

Our company has a long history in obfuscation technologies and code obfuscators (see our [PowerShell](https://www.pelock.com/products/powershell-pro-obfuscator), [Java](https://www.pelock.com/products/jobfuscator), [AutoIt](https://www.pelock.com/products/autoit-obfuscator), [x86 Assembly](https://www.pelock.com/products/obfuscator) &  obfuscators).

We actively bugfix, research and develop new obfuscation strategies for our tools.

You can count on our expertise and support in this field.

### Installation

The preferred way of WebApi interface installation is via [NuGet](https://www.nuget.org/) (targets **.NET 8**). Package id: **`PELock.LuaVirtualizer`**.

Run:

```bash
dotnet add package PELock.LuaVirtualizer
```

From Visual Studio NuGet Package Manager Console:

```powershell
Install-Package PELock.LuaVirtualizer
```

Or add this entry directly to your `.csproj` file in `ItemGroup` / `PackageReference`:

```xml
<ItemGroup>
  <PackageReference Include="PELock.LuaVirtualizer" Version="1.0.0" />
</ItemGroup>
```

Then import the client namespace and create an instance:

```csharp
using PELock.LuaVirtualizer;

var myLuaVirtualizer = new LuaVirtualizer("ABCD-ABCD-ABCD-ABCD");
```

Package id: `PELock.LuaVirtualizer`. To run the included samples from this repository, use each example project alongside `lua-virtualizer-example*.cs`:

```bash
dotnet run --project examples/lua-virtualizer-example-simple.csproj
```

### Example of obfuscating Lua script source code using default options

```csharp
/******************************************************************************
 * Lua Virtualizer WebApi interface usage example.
 *
 * In this example we will obfuscate sample source with default options.
 *
 * Version        : v1.0.0
 * Language       : C#
 * Author         : Bartosz Wójcik
 * Web page       : https://www.pelock.com
 *
 *****************************************************************************/

using PELock.LuaVirtualizer;

//
// create Lua Virtualizer class instance (we are using our activation key)
//
var myLuaVirtualizer = new LuaVirtualizer("ABCD-ABCD-ABCD-ABCD");

//
// source code in Lua format
//
const string ScriptSourceCode =
    """
    local function greet(name)
      print("Hello, " .. name)
    end
    greet("Lua")
    """;

//
// by default all obfuscation options are enabled, so we can just simply call:
//
var result = await myLuaVirtualizer.ObfuscateScriptSourceAsync(ScriptSourceCode);

//
// it's also possible to pass a Lua script file path instead of a string e.g.
//
// var result = await myLuaVirtualizer.ObfuscateScriptFileAsync("/path/to/project/script.lua");

//
// result object holds the obfuscation results as well as other information
//
// result?.Error           - error code (see LuaVirtualizer.Error*)
// result?.Output          - obfuscated code
// result?.Demo             - demo mode (missing or invalid key); limited to 1000 bytes; the server applies only flat code virtualization (code_virtualization=flat), ignoring every other strategy field
// result?.LicenseExpiration - license end date (Y-m-d), empty if none
// result?.UsagesTotal     - total obfuscations for this activation code
//
if (result is not null)
{
    //
    // display obfuscated code
    //
    if (result.Error == LuaVirtualizer.ErrorSuccess && result.Output is not null)
        Console.WriteLine(result.Output);
    else
        throw new InvalidOperationException("An error occurred, error code: " + result.Error);
}
else
{
    throw new InvalidOperationException("Something unexpected happen while trying to obfuscate the code.");
}

```

### An example of obfuscating Lua script source code with customized obfuscation strategies

```csharp
/******************************************************************************
 * Lua Virtualizer WebApi interface usage example.
 *
 * In this example we will obfuscate sample source with every public option
 * set, so you can see the full client surface and turn flags on or off.
 *
 * Version        : v1.0.0
 * Language       : C#
 * Author         : Bartosz Wójcik
 * Web page       : https://www.pelock.com
 *
 *****************************************************************************/

using PELock.LuaVirtualizer;

//
// create Lua Virtualizer class instance (we are using our activation key)
//
var myLuaVirtualizer = new LuaVirtualizer("ABCD-ABCD-ABCD-ABCD");

//
// should the source code be compressed (both input & compressed)
//
myLuaVirtualizer.EnableCompression = false;

//
// globals
//
// fixed random seed for reproducible obfuscation output (optional)
//
myLuaVirtualizer.Seed = null;

//
// randomization density / intensity (0-100)
//
myLuaVirtualizer.RandomizationDensity = null;

//
// identifier renaming style: RenameStyle.Il, O0, Hex, or Mangled
//
myLuaVirtualizer.RenameStyle = RenameStyle.Il;

//
// Lua dialect. "auto" (default) omits the POST field.
// Explicit values: 5.1, 5.2, 5.3, 5.4, 5.5, luajit, luau.
//
myLuaVirtualizer.Target = "auto";

//
// code virtualization (exactly one): VM, FSA, or FLAT
// set to null to skip virtualization
//
myLuaVirtualizer.CodeVirtualization = CodeVirtualization.VM;

//
// protection (opt-in on the service for --all; shown enabled here so every flag is visible)
//
// protection against tampering with protected code (integrity verification)
//
myLuaVirtualizer.SelfDefending = true;

//
// protection linker (decoy call graph; requires SelfDefending)
//
myLuaVirtualizer.ProtectionLinker = true;

//
// insert anti-debugging detections
//
myLuaVirtualizer.DetectDebugger = true;

//
// insert virtual machine (anti-VM) detections
//
myLuaVirtualizer.AntiVm = true;

//
// insert anti-sandbox detections
//
myLuaVirtualizer.AntiSandbox = true;

//
// insert anti-emulators (CPU) detections
//
myLuaVirtualizer.AntiEmulator = true;

//
// renaming
//
// rename variable names to random string values
//
myLuaVirtualizer.RenameVariables = true;

//
// rename parameter names to random string values
//
myLuaVirtualizer.RenameParameters = true;

//
// rename function names to random string values
//
myLuaVirtualizer.RenameFunctions = true;

//
// rename function call references consistently with renamed functions
//
myLuaVirtualizer.RenameFunctionCalls = true;

//
// shuffle function order in the output source
//
myLuaVirtualizer.ShuffleFunctions = true;

//
// fold/resolve constant expressions at obfuscation time
//
myLuaVirtualizer.ResolveConstants = true;

//
// strings
//
// split strings into concatenated chunks
//
myLuaVirtualizer.SplitStrings = true;

//
// apply light transformations/mutations to string literals
//
myLuaVirtualizer.ModifyStrings = true;

//
// encrypt strings using randomly generated polymorphic encryption algorithms
//
myLuaVirtualizer.EncryptStrings = true;

//
// store string fragments in char-code array vaults
//
myLuaVirtualizer.StringCharArrayVault = true;

//
// numeric
//
// encrypt integers
//
myLuaVirtualizer.EncryptIntegers = true;

//
// encrypt floating point numbers
//
myLuaVirtualizer.EncryptFloating = true;

//
// replace binary operators with mixed boolean-arithmetic (MBA) equivalents
//
myLuaVirtualizer.MbaBinops = true;

//
// represent integers via floating-point math
//
myLuaVirtualizer.IntegersToFloating = true;

//
// move integers to arrays
//
myLuaVirtualizer.IntegersToArrays = true;

//
// move floats to arrays
//
myLuaVirtualizer.FloatsToArrays = true;

//
// apply redundant xor / affine integer masks
//
myLuaVirtualizer.AffineIntegerMask = true;

//
// encrypted array literals
//
// encrypt integer array literals
//
myLuaVirtualizer.ArrayIntCrypt = true;

//
// encrypt character array literals
//
myLuaVirtualizer.ArrayCharCrypt = true;

//
// encrypt floating-point array literals
//
myLuaVirtualizer.ArrayDoubleCrypt = true;

//
// encrypt string array literals
//
myLuaVirtualizer.ArrayStringCrypt = true;

//
// decoy value pools
//
// insert a shared bucket of random noise values used by other strategies
//
myLuaVirtualizer.InsertRandomValueBucket = true;

//
// populate the random value bucket with decoy integers
//
myLuaVirtualizer.RandomBucketIntegers = true;

//
// populate the random value bucket with decoy arrays
//
myLuaVirtualizer.RandomBucketArrays = true;

//
// populate the random value bucket with decoy functions
//
myLuaVirtualizer.RandomBucketFunctions = true;

//
// populate the random value bucket with decoy characters
//
myLuaVirtualizer.RandomBucketCharacters = true;

//
// populate the random value bucket with anti-regex decoy noise
//
myLuaVirtualizer.RandomBucketAntiRegex = true;

//
// populate the random value bucket with autostart decoy stubs
//
myLuaVirtualizer.RandomBucketAutostart = true;

//
// opaque predicates & noise
//
// rewrite selected statements using ternary operators
//
myLuaVirtualizer.InsertTernaryOperators = true;

//
// replace boolean conditions with equivalent complex expressions
//
myLuaVirtualizer.ComplexifyBooleans = true;

//
// insert opaque predicate branches
//
myLuaVirtualizer.OpaqueBranches = true;

//
// insert opaque mixer chains into control flow
//
myLuaVirtualizer.OpaqueMixerChain = true;

//
// insert dead code
//
myLuaVirtualizer.InsertDeadCode = true;

//
// wrap code in try/finally blocks with dead noise
//
myLuaVirtualizer.TryFinallyNoise = true;

//
// decoys
//
// insert decoy functions
//
myLuaVirtualizer.DecoyFunctions = true;

//
// insert decoy lambda expressions
//
myLuaVirtualizer.LambdaDecoys = true;

//
// insert literal padding noise
//
myLuaVirtualizer.LiteralPadding = true;

//
// insert fake import statement markers
//
myLuaVirtualizer.FakeImportMarkers = true;

//
// use dynamic getattr()-based indirect calls
//
myLuaVirtualizer.DynamicGetattrCalls = true;

//
// rewrite absolute imports into __import__ / getattr
//
myLuaVirtualizer.ObfuscateImports = true;

//
// insert dead callback/event registration stubs
//
myLuaVirtualizer.CallbackRegistrationStubs = true;

//
// other
//
// strip comments from the output source
//
myLuaVirtualizer.RemoveComments = true;

//
// source code in Lua format
//
const string ScriptSourceCode =
    """
    local function greet(name)
      print("Hello, " .. name)
    end
    greet("Lua")
    """;

//
// obfuscate the source code with the options set above
//
var result = await myLuaVirtualizer.ObfuscateScriptSourceAsync(ScriptSourceCode);

//
// it's also possible to pass a Lua script file path instead of a string e.g.
//
// var result = await myLuaVirtualizer.ObfuscateScriptFileAsync("/path/to/project/script.lua");

//
// result object holds the obfuscation results as well as other information
//
// result?.Error           - error code (see LuaVirtualizer.Error*)
// result?.Output          - obfuscated code
// result?.Demo             - demo mode (missing or invalid key); limited to 1000 bytes; the server applies only flat code virtualization (code_virtualization=flat), ignoring every other strategy field
// result?.LicenseExpiration - license end date (Y-m-d), empty if none
// result?.UsagesTotal     - total obfuscations for this activation code
//
if (result is not null)
{
    //
    // display obfuscated code
    //
    if (result.Error == LuaVirtualizer.ErrorSuccess && result.Output is not null)
        Console.WriteLine(result.Output);
    else
        throw new InvalidOperationException("An error occurred, error code: " + result.Error);
}
else
{
    throw new InvalidOperationException("Something unexpected happen while trying to obfuscate the code.");
}

```

### Check activation key status

```csharp
/******************************************************************************
 * Lua Virtualizer WebApi interface usage example.
 *
 * In this example we will verify our activation key status.
 *
 * Version        : v1.0.0
 * Language       : C#
 * Author         : Bartosz Wójcik
 * Web page       : https://www.pelock.com
 *
 *****************************************************************************/

using PELock.LuaVirtualizer;

//
// create Lua Virtualizer class instance (we are using our activation key)
//
var myLuaVirtualizer = new LuaVirtualizer("ABCD-ABCD-ABCD-ABCD");

//
// login to the service
//
var result = await myLuaVirtualizer.LoginAsync();

//
// result object holds the information about the license
//
// result?.Demo           - demo mode (missing or invalid key); limited to 1000 bytes; the server applies only flat code virtualization (code_virtualization=flat), ignoring every other strategy field
// result?.LicenseExpiration - license end date (Y-m-d), empty if none
// result?.UsagesTotal     - total obfuscations for this activation code
// result?.StringLimit     - Max. source code size allowed (it's 1000 bytes for demo mode)
//
if (result is not null)
{
    Console.WriteLine("Demo version status - " + (result.Demo == true ? "true" : "false"));
    Console.WriteLine("License expiration - " + result.LicenseExpiration);
    Console.WriteLine("Total obfuscations - " + result.UsagesTotal);
    Console.WriteLine("Max. source code size - " + result.StringLimit);
}
else
{
    throw new InvalidOperationException("Something unexpected happen while trying to login to the service.");
}

```

## Per-function markers

Markers are source syntax, not a library import. There is no separate decorator package. The marker is the first statement of a function. The engine reads it and strips it.

```lua
local function f()
  LUAVIRTUALIZER(VM(RND), ENABLE(STRINGS), DISABLE(encrypt_strings))
  return 1
end
```

`VM(...)` selects the dispatcher for that function. `ENABLE(...)` replaces the function's strategy set with the named groups or strategies. `DISABLE(...)` then removes the named groups or strategies. Clauses apply as VM, then ENABLE, then DISABLE, whatever order they are written in. Names are case-insensitive. `ENABLE(ALL)` is the same set `--all` turns on (every strategy except the four probes). `DISABLE(ALL)` clears every strategy. `FSA` is accepted as an alias of `FSM`. `self_defending` and `protection_linker` are not decorator names. They stay script-wide.

`decoy_functions`, every `BUCKET` strategy, `callback_registration_stubs`, and the `PROTECT` probes are inserted once for the whole script. A function `ENABLE` or `DISABLE` does not turn those passes on or off. A `VM(...)` nested inside `VM(NONE)` stays native Lua.

| Category | Name | Description |
| --- | --- | --- |
| Virtualization | `VM(VM)` | A word is a permuted opcode plus A, B, C, and Bx. The encrypted blob is a read-only code image. The Lua 5.1 shell fetches `__vll_code[pc]` and calls `__vll_hs[__vll_map[opcode]]`. Four decoy handlers share the table. A jump stores an absolute program counter. Exactly one mode applies to a function. |
| Virtualization | `VM(FSM)` | Same compiler, blob, and handler table as `VM(VM)`. The outer loop is shifted, and an opaque gate reads the virtual program counter before the live handler. `FSA` is an alias of `FSM`. |
| Virtualization | `VM(WEAVE)` | Same compiler and encrypted blob. Registers, constants, and instruction words share one table. Three superoperators cover load-and-arithmetic, compare-and-branch, and the call exit. A back-edge rewrites the words it will fetch again. Host calls are the only exit. |
| Virtualization | `VM(FLAT)` | Same compiler and handler table. The loop form is shifted again, and the opaque gate mixes the virtual program counter with the running checksum. |
| Virtualization | `VM(RND)` | Each run picks `VM(VM)`, `VM(FSM)`, or `VM(WEAVE)` for this function. `VM(FLAT)` is not in the draw. The same seed repeats the choice. Nested functions inherit it unless they set their own `VM(...)`. A `VM(RND)` nested inside `VM(NONE)` stays native Lua. |
| Virtualization | `VM(NONE)` | Leave this function, and every function nested inside it, as ordinary Lua. |
| Group | `NAMES` | `rename_variables`, `rename_parameters`, `rename_functions`, `rename_function_calls`, `resolve_constants`. |
| Group | `STRINGS` | `split_strings`, `modify_strings`, `string_char_array_vault`, `encrypt_strings`, `array_string_crypt`. |
| Group | `NUMBERS` | `integers_to_arrays`, `floats_to_arrays`, `affine_integer_mask`, `encrypt_integers`, `integers_to_floating`, `encrypt_floating`, `mba_binops`, `array_int_crypt`, `array_char_crypt`, `array_double_crypt`. |
| Group | `BOOLEANS` | `complexify_booleans`, `insert_ternary_operators`. |
| Group | `SHAPE` | `decoy_functions`, `lambda_decoys`, `insert_dead_code`, `opaque_branches`, `opaque_mixer_chain`, `try_finally_noise`, `literal_padding`, `fake_import_markers`, `shuffle_functions`. `decoy_functions` is inserted for the whole script. |
| Group | `BUCKET` | `insert_random_value_bucket`, `random_bucket_integers`, `random_bucket_arrays`, `random_bucket_functions`, `random_bucket_characters`, `random_bucket_anti_regex`, `random_bucket_autostart`. Inserted for the whole script. |
| Group | `CALLS` | `dynamic_getattr_calls`, `obfuscate_imports`, `callback_registration_stubs`. `callback_registration_stubs` is inserted for the whole script. |
| Group | `COMMENTS` | `remove_comments`. |
| Group | `PROTECT` | `detect_debugger`, `anti_vm`, `anti_sandbox`, `anti_emulator`. Inserted for the whole script. A positive detection ends the process with no error message. |
| Group | `ALL` | `ENABLE(ALL)` selects every strategy except the four probes. `DISABLE(ALL)` clears every strategy, including the probes. |
| Strategy | `rename_variables` | `NAMES`. Local and global variables become random identifiers. Every reference updates together, and scope rules stay intact. |
| Strategy | `rename_parameters` | `NAMES`. Parameter names and in-body bindings are rewritten to opaque tokens. |
| Strategy | `rename_functions` | `NAMES`. Function definitions are renamed and calls are retargeted. Metamethods such as `__index` and built-in library functions stay untouched. |
| Strategy | `rename_function_calls` | `NAMES`. Direct call sites look up the target through a name table instead of the literal identifier. |
| Strategy | `resolve_constants` | `NAMES`. Module-level constants are inlined at every use, and unused declarations are removed. |
| Strategy | `split_strings` | `STRINGS`. Literals are split into chunks and joined at runtime, so the whole token is no longer in one place. |
| Strategy | `modify_strings` | `STRINGS`. Literals are rewritten as shifted characters and rebuilt by a small runtime transform. |
| Strategy | `string_char_array_vault` | `STRINGS`. Strings are rebuilt from numeric character codes instead of quoted text. |
| Strategy | `encrypt_strings` | `STRINGS`. Each run emits a unique per-string decryptor. Two outputs do not share the same algorithm shape. |
| Strategy | `array_string_crypt` | `STRINGS`. String table elements are encrypted independently and rebuilt on demand. |
| Strategy | `integers_to_arrays` | `NUMBERS`. Integers are stored in tables and read by index, with decoy entries mixed in. |
| Strategy | `floats_to_arrays` | `NUMBERS`. Floats are lifted into table slots, with decoy values mixed in. |
| Strategy | `affine_integer_mask` | `NUMBERS`. Reversible bit masks wrap integers without changing the result. The helper matches the selected dialect. |
| Strategy | `encrypt_integers` | `NUMBERS`. Plain integers become equivalent arithmetic or bitwise expressions. |
| Strategy | `integers_to_floating` | `NUMBERS`. Whole numbers are reconstructed from `math` expressions and cast back with `math.floor`. |
| Strategy | `encrypt_floating` | `NUMBERS`. Floating-point literals become equivalent `math` expressions. |
| Strategy | `mba_binops` | `NUMBERS`. Binary operators are replaced with mixed boolean-arithmetic identities. |
| Strategy | `array_int_crypt` | `NUMBERS`. Integer table contents are stored encrypted and decoded at runtime. |
| Strategy | `array_char_crypt` | `NUMBERS`. Character table literals are stored encrypted and restored by generated decoder code. |
| Strategy | `array_double_crypt` | `NUMBERS`. Float table constants are hidden as encoded bit patterns and reconstructed while the program runs. |
| Strategy | `complexify_booleans` | `BOOLEANS`. Simple true/false tests are rewritten into longer equivalent conditions. |
| Strategy | `insert_ternary_operators` | `BOOLEANS`. Simple branches collapse into `and`/`or` conditional expressions. |
| Strategy | `decoy_functions` | `SHAPE`. Plausible functions that are never called. Inserted for the whole script. |
| Strategy | `lambda_decoys` | `SHAPE`. Unused anonymous functions near the top of the script. They are never called. |
| Strategy | `insert_dead_code` | `SHAPE`. Statements under opaque never-true conditions. The result of the script stays the same. |
| Strategy | `opaque_branches` | `SHAPE`. Always-false branches that look like real control flow. |
| Strategy | `opaque_mixer_chain` | `SHAPE`. Mixer functions and guards that never change the returned value. |
| Strategy | `try_finally_noise` | `SHAPE`. Lua has no try/finally, so the pass emits an inert `pcall` plus cleanup. |
| Strategy | `literal_padding` | `SHAPE`. A bulky string constant that live logic never reads. It shifts line numbers and inflates the file. |
| Strategy | `fake_import_markers` | `SHAPE`. Comments that mimic extra `require` calls. `remove_comments` deletes them. |
| Strategy | `shuffle_functions` | `SHAPE`. Permutes top-level function definitions when the execution order stays valid. |
| Strategy | `insert_random_value_bucket` | `BUCKET`. Enables the module-level decoy pools. Inserted for the whole script. |
| Strategy | `random_bucket_integers` | `BUCKET`. Unused integer decoys in the pool. Inserted for the whole script. |
| Strategy | `random_bucket_arrays` | `BUCKET`. Unused table decoys in the pool. Inserted for the whole script. |
| Strategy | `random_bucket_functions` | `BUCKET`. Unused function decoys in the pool. Inserted for the whole script. |
| Strategy | `random_bucket_characters` | `BUCKET`. Unused character decoys in the pool. Inserted for the whole script. |
| Strategy | `random_bucket_anti_regex` | `BUCKET`. Regex-shaped decoy literals. Inserted for the whole script. |
| Strategy | `random_bucket_autostart` | `BUCKET`. Decoy strings that resemble autostart paths and are never used by live code. Inserted for the whole script. |
| Strategy | `dynamic_getattr_calls` | `CALLS`. Call targets are built from fragments and resolved through a table lookup. |
| Strategy | `obfuscate_imports` | `CALLS`. `require` is rewritten so the module name is built at runtime. Those strings stay available for later string encryption. |
| Strategy | `callback_registration_stubs` | `CALLS`. Snippets that resemble callback registration and never wire an event source. Inserted for the whole script. |
| Strategy | `remove_comments` | `COMMENTS`. Removes `--` comments from the emitted script. |
| Strategy | `detect_debugger` | `PROTECT`. Detects a debugger attached to the process (`debug.gethook()`, and `IsDebuggerPresent` on Windows). Inserted for the whole script. A positive detection ends the process with no error message. |
| Strategy | `anti_vm` | `PROTECT`. Detects VMware, VirtualBox, Parallels, KVM, and Linux DMI product or vendor strings. Inserted for the whole script. |
| Strategy | `anti_sandbox` | `PROTECT`. Detects a very low CPU count, Sandboxie, Joe Sandbox, and analysis-host tokens. Inserted for the whole script. |
| Strategy | `anti_emulator` | `PROTECT`. Detects WINE, Bochs, QEMU, XEN, and a short timing self-check. Inserted for the whole script. |

## Product page

https://www.pelock.com/products/lua-virtualizer

Web API:

https://www.pelock.com/products/lua-virtualizer/api

## Windows GUI client and command line version

You can download it at:

https://www.pelock.com/products/lua-virtualizer

#### Lua Virtualizer comes also with full GUI version for Windows

![Lua Virtualizer Windows Client](https://raw.githubusercontent.com/PELock/Lua-Virtualizer-CSharp/main/images/lua-virtualizer-obfuscator-1-0-main.png)

#### Obfuscation options

![Lua Virtualizer obfuscation options](https://raw.githubusercontent.com/PELock/Lua-Virtualizer-CSharp/main/images/lua-virtualizer-obfuscator-1-0-options.png)

#### Command line interface aka CLI

![Lua Virtualizer command-line interface](https://raw.githubusercontent.com/PELock/Lua-Virtualizer-CSharp/main/images/lua-virtualizer-obfuscator-1-0-command-line.png)

Lua Virtualizer ships with a command-line interface for Windows and Linux automation. Use it to integrate obfuscation into build servers, CI jobs, or batch packaging.

## Visual Studio Code / Cursor extension

![Lua code virtualization in Visual Studio Code](https://raw.githubusercontent.com/PELock/Lua-Virtualizer-CSharp/main/images/lua-virtualizer-obfuscator-code-virtualization-vscode-extension.gif)

- [https://marketplace.visualstudio.com/items?itemName=PELock.lua-virtualizer](https://marketplace.visualstudio.com/items?itemName=PELock.lua-virtualizer)
- [https://open-vsx.org/extension/PELock/lua-virtualizer](https://open-vsx.org/extension/PELock/lua-virtualizer)

## Demo mode limitations

A missing or invalid key is limited to 1000 bytes and the server applies only flat code virtualization (`code_virtualization=flat`), ignoring every other strategy field.

Bartosz Wójcik

* Visit my site at — https://www.pelock.com
* X — https://x.com/PELock
* GitHub — https://github.com/PELock
