using System;
using System.IO;

namespace TextCipher.Services;

public class CaesarCypherEncryptionService : IEncryptionService
{
    private readonly ITextFileGetterService _service;

    public event Action<int>? OnOnePercent;

    public CaesarCypherEncryptionService(ITextFileGetterService service)
    {
        _service = service;
    }

    public void Encrypt(FileStream fromStream, FileStream toStream, int key)
    {
        int character;

        var from = new StreamReader(fromStream);
        var to = new StreamWriter(toStream);
        var onePercent = _service.GetTextLength(fromStream.Name) / 100.0;
        var progress = 0;
        var tempProgress = 0;
        while ((character = from.Read()) != -1)
        {
            if (!char.IsLetter((char) character))
            {
                to.Write((char)character);
                progress++;
                tempProgress++;
                if (tempProgress >= onePercent)
                {
                    OnOnePercent?.Invoke(progress);
                    tempProgress = 0;
                }
                continue;
            }
            var offset = char.IsUpper((char)character) ? 'A' : 'a';
            to.Write((char)((character + key - offset) % 26 + offset));
            progress++;
            tempProgress++;
            if (tempProgress >= onePercent)
            {
                OnOnePercent?.Invoke(progress);
                tempProgress = 0;
            }
        }
        OnOnePercent?.Invoke(progress);
    }
}