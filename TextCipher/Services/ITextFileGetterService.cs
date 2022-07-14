using System;
using System.Threading.Tasks;

namespace TextCipher.Services;

public interface ITextFileGetterService
{
    public Task<string> GetText(string textFilePath);
}