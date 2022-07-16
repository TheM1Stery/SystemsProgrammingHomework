using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace MultiThreadedFileReader;

public class FileSelector : IFileSelector
{
    public async Task<string?> SelectFile()
    {
        var dialog = new OpenFileDialog
        {
            AllowMultiple = false
        };
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime lifetime)
        {
            return null;
        }
        var result = await dialog.ShowAsync(lifetime.MainWindow);
        return result?.FirstOrDefault();
    }
}