using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TextCipher.Services;

public class TextFileGetterService : ITextFileGetterService
{
    private Dictionary<string, int> _fileLengthCache = new();
    
    public string? GetText(string textFilePath)
    {
        var fileInfo = new FileInfo(textFilePath);

        return fileInfo.Length >= 1e+8 ? null : File.ReadAllText(textFilePath);
    }

    public int GetTextLength(string textFilePath)
    {
        if (_fileLengthCache.TryGetValue(textFilePath, out var value))
        {
            return value;
        }
        using var stream = new StreamReader(textFilePath);
        var length = 0;
        while (stream.Read() != -1)
        {
            length++;
        }
        _fileLengthCache.Add(textFilePath, length);
        return length;
    }
}