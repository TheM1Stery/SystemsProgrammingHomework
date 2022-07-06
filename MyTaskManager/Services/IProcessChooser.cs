using System.Threading.Tasks;
using MyTaskManager.ViewModels;

namespace MyTaskManager.Services;

public interface IProcessChooser
{
    public Task<string?> GetProcessPath();
}