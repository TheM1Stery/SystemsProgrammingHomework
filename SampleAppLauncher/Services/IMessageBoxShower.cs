using System.Threading.Tasks;
using MessageBox.Avalonia.Enums;

namespace SampleAppLauncher.Services;

public interface IMessageBoxShower
{
    public Task ShowMessageAsync(string message, string title, Icon icon);
}