using System.Threading.Tasks;

namespace SocialMediaUser.Services;

public interface IHashCreatorService 
{
    public string ComputeSalt(int size);

    public string ComputeHash(string? str);
    
    public Task<string> ComputeSaltAsync(int size);

    public Task<string> ComputeHashAsync(string? str);
}