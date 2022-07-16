using System;
using System.IO;

namespace TextCipher.Services;

public class CaesarCypherEncryptionService : IEncryptionService
{
    public event Action? Encrypting;
    
    public void Encrypt(StreamReader from, StreamWriter to, int key)
    {
        int character;
        while ((character = from.Read()) != -1)
        {
            if (!char.IsLetter((char) character))
            {
                to.Write((char)character);
                continue;
            }
            var offset = char.IsUpper((char)character) ? 'A' : 'a';
            to.Write((char)((character + key - offset) % 26 + offset));
            Encrypting?.Invoke();
        }
    }
}