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
