using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;

namespace SampleAppLauncher.Services;

public class MessageBoxShower : IMessageBoxShower
{
    public async Task ShowMessageAsync(string message, string title, Icon icon)
    {
        var box = MessageBoxManager.GetMessageBoxStandardWindow(title, message, 
            ButtonEnum.Ok, icon, WindowStartupLocation.CenterOwner);
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            await box.ShowDialog(desktop.MainWindow);
        }
    }
}