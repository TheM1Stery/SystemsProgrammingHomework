using System.Runtime.Loader;

var loadContext = new AssemblyLoadContext("Calculation", true);

var assembly = loadContext.
    LoadFromAssemblyPath(Path.Combine(Environment.CurrentDirectory + @"\CalculationLibrary"));

var type = assembly.GetType("CalculationLibrary.Sum");

if (type is null)
{
    Console.WriteLine("Failed init");
    return;
}

var myClass = Activator.CreateInstance(type);
var method = type.GetMethod("Calculate", new[] {typeof(double), typeof(double), typeof(char)});

var result = method?.Invoke(myClass, new object[] {25.2, 5.8, '+'});

if (result is null)
{
    Console.WriteLine("Error trying to calculate the expression");
    return;
}
Console.WriteLine($"Result: {result}");


loadContext.Unload();