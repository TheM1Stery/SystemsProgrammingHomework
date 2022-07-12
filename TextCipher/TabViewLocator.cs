using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using TextCipher.ViewModels;

namespace TextCipher;

public class TabViewLocator : IDataTemplate
{
    public IControl Build(object data)
    {
        var name = data.GetType().FullName!.Replace("ViewModel", "View");
        var type = Type.GetType(name);

        if (type != null)
        {
            return (Control)Activator.CreateInstance(type)!;
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object data)
    {
        return data is BaseTabInfoViewModel;
    }
}