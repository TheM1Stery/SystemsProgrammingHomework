using System.Threading.Tasks;

namespace MultiThreadedFileReader;

public interface IFileSelector
{
    public Task<string?> SelectFile();
}