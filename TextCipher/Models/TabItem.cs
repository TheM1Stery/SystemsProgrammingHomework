using TextCipher.ViewModels;

namespace TextCipher.Models;

public class TabItem
{
    public string? Header { get; set; }
    public TabInfoViewModel? Content { get; set; }
}