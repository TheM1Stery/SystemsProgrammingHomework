using System.Threading.Tasks;
using MessageBox.Avalonia.Enums;

namespace ButtonReactionTimer;

public interface IMessageBoxShower
{
    public Task<ButtonResult> ShowMessageAsync(string message, string title, Icon icon, ButtonEnum buttons);
}