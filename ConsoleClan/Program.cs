using ConsoleClan;
using ConsoleClan.Interfaces;

using Microsoft.Extensions.DependencyInjection;

var startup = new Startup();
var clash = startup.Provider.GetService<IClash>();
if (clash == null)
{
	Console.WriteLine("Null parser");
	return;
}
if (args.Length < 2)
{
	Console.WriteLine("Usage: <fileName=example.csv> <key=home|office>");
	return;
}
var fileNameArgument = args[0].Split("=");
if (string.Compare(fileNameArgument[0].Trim(), "fileName", true) != 0 || fileNameArgument.Length != 2)
{
	Console.WriteLine("<fileName=example.csv> is missing");
	return;
}
string? fileName = fileNameArgument[1]?.Trim();
if (string.IsNullOrEmpty(fileName))
{
	Console.WriteLine("fileName can not be null");
	return;
}
var keyArgument = args[1].Split("=");
string? key;
AuthenticationTokeReference authenticationTokeReference = AuthenticationTokeReference.Office;
if (string.Compare(keyArgument[0].Trim(), "key", true) != 0 || fileNameArgument.Length != 2)
{
	Console.WriteLine("<key=home|office> is missing");
	return;
}
key = keyArgument[1]?.Trim();
if (!Enum.TryParse(key, true, out authenticationTokeReference))
{
	Console.WriteLine("key must be: home|office");
	return;
}
await clash.ProcessAsync(authenticationTokeReference, fileName);
Console.WriteLine("Finished.");
