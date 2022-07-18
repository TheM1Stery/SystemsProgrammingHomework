using System;
using System.Threading.Tasks;

namespace TextCipher.Services;

public interface ITextFileGetterService
{
    public string GetText(string textFilePath);

}