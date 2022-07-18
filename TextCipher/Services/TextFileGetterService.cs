using System.IO;
using System.Threading.Tasks;

namespace TextCipher.Services;

public class TextFileGetterService : ITextFileGetterService
{
    public string GetText(string textFilePath)
    {
        return File.ReadAllText(textFilePath);
    }
    
}