using System.IO;
using System.Threading.Tasks;

namespace TextCipher.Services;

public class TextFileGetterService : ITextFileGetterService
{
    public string? GetText(string textFilePath)
    {
        var fileInfo = new FileInfo(textFilePath);

        return fileInfo.Length >= 1e+8 ? null : File.ReadAllText(textFilePath);
    }

    public int GetTextLength(string textFilePath)
    {
        using var stream = new StreamReader(textFilePath);
        var length = 0;
        while (stream.Read() != -1)
        {
            length++;
        }
        return length;
    }
}