using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using MyTaskManager.ViewModels;
using ReactiveUI;

namespace MyTaskManager.Services;

public class ProcessChooser : IProcessChooser
{

    
    public async Task<string?> GetProcessPath()
    {
        var dialog = new OpenFileDialog
        {
            AllowMultiple = false,
            Filters = new List<FileDialogFilter>
            {
                new()
                {
                    Extensions = new List<string>{"exe"},
                    Name = "Executable file"
                },
                new()
                {
                    Extensions = new List<string>{"lnk"},
                    Name = "Shortcut"
                }
            }
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