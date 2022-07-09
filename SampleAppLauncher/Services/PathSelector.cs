using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace SampleAppLauncher.Services;

public class PathSelector : IPathSelector
{
    public async Task<string?> GetPath(List<FileDialogFilter> filters)
    {
        var dialog = new OpenFileDialog
        {
            AllowMultiple = false,
            // Filters = new List<FileDialogFilter>
            // {
            //     new()
            //     {
            //         Extensions = new List<string>{"txt"},
            //         Name = "Text file"
            //     },
            // }
            Filters = filters
        };
        Window? window = null;
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            window = desktop.MainWindow;
        }
        if (window is null)
            return null;
        var result = await dialog.ShowAsync(window);
        return result?.FirstOrDefault();
    }
}