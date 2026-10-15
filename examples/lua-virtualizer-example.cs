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

//
// Per-function marker (source syntax, not a library import). The marker is the
// first statement of a function. The engine reads it and strips it.
//
// local function f()
//   LUAVIRTUALIZER(VM(FSA), OBFUSCATION(encrypt_strings, NUMBERS))
//   return 1
// end
//
// VM(VM|FSA|FLAT|NONE) selects the dispatcher; NONE stays native Lua.
// OBFUSCATION(...) is a blacklist: NONE, ALL, group names NAMES, STRINGS,
// NUMBERS, BOOLEANS, SHAPE, BUCKET, CALLS, COMMENTS, PROTECT, or a specific
// snake_case strategy flag.
