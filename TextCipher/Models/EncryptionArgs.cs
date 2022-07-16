namespace TextCipher.Models;

public class EncryptionArgs
{
    public string? FromPath { get; init; }
    
    public string? ToPath { get; init; }
    
    public int Key { get; set; }
}