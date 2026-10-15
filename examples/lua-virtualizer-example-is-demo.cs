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
