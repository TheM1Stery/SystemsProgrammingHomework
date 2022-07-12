using TextCipher.ViewModels;
using TextCipher.Views;

namespace TextCipher;

public static class DesignData
{
    public static MainViewModel MainExample { get; } = new();

    public static TabInfoViewModel TabExample { get; } = new("sox icive");
}