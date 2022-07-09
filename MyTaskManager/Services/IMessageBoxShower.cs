using System.Threading.Tasks;
using MessageBox.Avalonia.Enums;

namespace MyTaskManager.Services;

public interface IMessageBoxShower
{
    public Task ShowMessageAsync(string message, string title, Icon icon);
}