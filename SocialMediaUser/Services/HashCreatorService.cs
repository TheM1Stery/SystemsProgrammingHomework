using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocialMediaUser.Services;

public class HashCreatorService : IHashCreatorService
{
    public string ComputeSalt(int size)
    {
        var str = new StringBuilder();
        var random = new Random();
        for (var i = 0; i < size; i++)
        {
            var value = random.Next(1, 127);
            str.Append(Convert.ToChar(value));
        }
        return str.ToString();
    }

    public string ComputeHash(string? str)
    {
        ArgumentNullException.ThrowIfNull(str, nameof(str));
        HashAlgorithm hashAlgorithm = SHA256.Create();
        var computedHash = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(str));
        return Encoding.UTF8.GetString(computedHash, 0, computedHash.Length);
    }

    public async Task<string> ComputeSaltAsync(int size)
    {
        return await Task.Run(() =>
        {
            var str = new StringBuilder();
            var random = new Random();
            for (var i = 0; i < size; i++)
            {
                var value = random.Next(1, 127);
                str.Append(Convert.ToChar(value));
            }
            return str.ToString();
        });
    }
    
    public async Task<string> ComputeHashAsync(string? str)
    {
        ArgumentNullException.ThrowIfNull(str, nameof(str));
        HashAlgorithm hashAlgorithm = SHA256.Create();
        await using var stream = new MemoryStream(Encoding.UTF8.GetBytes(str));
        await hashAlgorithm.ComputeHashAsync(stream, CancellationToken.None);
        var array = stream.ToArray();
        return Encoding.UTF8.GetString(array, 0, array.Length);
    }
}