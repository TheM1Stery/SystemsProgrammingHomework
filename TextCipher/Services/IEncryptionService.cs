using System;
using System.IO;

namespace TextCipher.Services;

public interface IEncryptionService
{
    public event Action Encrypting;
    
    public void Encrypt(StreamReader from, StreamWriter to, int key);
}