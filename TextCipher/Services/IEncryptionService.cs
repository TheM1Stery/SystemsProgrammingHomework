using System;
using System.IO;

namespace TextCipher.Services;

public interface IEncryptionService
{
    public event Action<int> OnOnePercent;
    
    public void Encrypt(FileStream from, FileStream to, int key);
}