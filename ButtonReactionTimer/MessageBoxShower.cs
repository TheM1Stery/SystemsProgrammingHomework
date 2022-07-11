using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;

namespace ButtonReactionTimer;

public class MessageBoxShower : IMessageBoxShower
{
    public async Task<ButtonResult> ShowMessageAsync(string message, string title, Icon icon, ButtonEnum buttons)
    {
        var box = MessageBoxManager.GetMessageBoxStandardWindow(title, message, 
            buttons, icon, WindowStartupLocation.CenterOwner);
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
        {
            return ButtonResult.Abort;
        }
        return await box.ShowDialog(desktop.MainWindow);
    }
}
