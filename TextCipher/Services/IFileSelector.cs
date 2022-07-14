using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace TextCipher.Services;

public interface IFileSelector
{
    public Task<List<string>> GetFiles(List<FileDialogFilter> filters);
}