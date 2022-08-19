using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;

namespace CustomerDb.Services;

public class ModalMessageBox : IModalMessageBox
{
    public async Task<ButtonResult> Show(MessageBoxStandardParams @params)
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
        {
            return ButtonResult.Abort;
        }
        var result = await MessageBoxManager.GetMessageBoxStandardWindow(@params).ShowDialog(desktop.MainWindow);
        return result;
    }
}