using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace SampleAppLauncher.Services;

public interface IPathSelector
{
    public Task<string?> GetPath(List<FileDialogFilter> filters);
}