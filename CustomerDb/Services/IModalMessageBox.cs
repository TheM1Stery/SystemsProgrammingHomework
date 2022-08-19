using System.Threading.Tasks;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;

namespace CustomerDb.Services;

public interface IModalMessageBox
{
    public Task<ButtonResult> Show(MessageBoxStandardParams @params);
}