using System.IO;
using System.Threading.Tasks;

namespace TextCipher.Services;

public class TextFileGetterService : ITextFileGetterService
{
    public async Task<string> GetText(string textFilePath)
    {
        return await File.ReadAllTextAsync(textFilePath);
    }
}