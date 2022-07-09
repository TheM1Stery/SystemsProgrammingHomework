// args = firstNumber secondNumber operation optional: file-path
// if the user starts this code with file path, there will be no logs in the console instead they will be logged to the specified file path
// The optional log will be ignored if the application is ran in Release Mode
using Serilog;
#if DEBUG
Log.Logger = args.Length switch
{
    4 => new LoggerConfiguration().MinimumLevel.Debug()
        .WriteTo.File(args[3])
        .CreateLogger(),
    3 => new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().CreateLogger(),
    _ => Log.Logger
};
#endif

double firstNumber, secondNumber;

try
{
    firstNumber = Convert.ToDouble(args[0]);
    secondNumber = Convert.ToDouble(args[1]);
}
catch (Exception e)
{
#if RELEASE
    Console.WriteLine(e);
#endif
#if DEBUG
    Log.Error(e, "ERROR!");
#endif
    return;
}

try
{
#if DEBUG
    Log.Information("Calculation start");
#endif
    var result = Calculate(firstNumber, secondNumber, args[2][0]);
    Console.WriteLine(result);
    Log.Debug("Got {Result} as the result", result);
}
catch (Exception e)
{
#if RELEASE
    Console.WriteLine(e.Message);
#endif
    Log.Error(e, "Something went wrong...");
}

#if DEBUG
    Log.Information("End of the calculation");
    Log.CloseAndFlush();
#endif


double Calculate(double first, double second, char @operator)
{
    switch (@operator)
    {
        case '+':
#if DEBUG
            Log.Debug("Summing {First} and {Second}", first, second);
#endif
            return first + second;
        case '-':
#if DEBUG
            Log.Debug("Subtracting {First} and {Second}", first, second);
#endif
            return first - second;
        case '*':
#if DEBUG
            Log.Debug("Multiplying {First} and {Second}", first, second);
#endif
            return first * second;
        case '/':
#if DEBUG
            Log.Debug("Dividing {First} and {Second}", first, second);
#endif
            return first / second;
        default:
            throw new ArgumentException("Invalid operator");
    }
}

Console.Write("Press enter to continue...");
Console.ReadLine();