namespace StupidCalculationLibrary;

public class Sum
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="first">first number</param>
    /// <param name="second">second number</param>
    /// <param name="operator">operation to perform</param>
    /// <returns>returns the result, if invalid operator is provided, null will be returned</returns>
    public double? Calculate(double first, double second, char @operator)
    {
        switch (@operator)
        {
            case '+':
                return first + second;
            case '-':
                return first - second;
            case '/':
                return first / second;
            case '*':
                return first * second;
            default:
                return null; 
        }
    }
}