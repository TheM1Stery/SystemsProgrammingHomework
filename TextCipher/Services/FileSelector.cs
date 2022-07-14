using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Dialogs;

namespace TextCipher.Services;

public class FileSelector : IFileSelector
{
    public async Task<List<string>> GetFiles(List<FileDialogFilter> filters)
    {
        var dialog = new OpenFileDialog()
        {
            Filters = filters,
            AllowMultiple = true
        };
        var list = new List<string>();
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime lifetime)
        {
            return list;
        }
        var result = await dialog.ShowAsync(lifetime.MainWindow);
        return result is null ? list : result.ToList();
    }
}