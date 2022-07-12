using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using TextCipher.Models;

namespace TextCipher.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    [ObservableProperty]
    private string? _title;

    
    public ObservableCollection<TabItem> Tabs { get; set; }

    public MainViewModel()
    {
        Title = "Text Cipher";
        Tabs = new ObservableCollection<TabItem> { new ()
            {Header = "Siktir", Content = new TabInfoViewModel("Timur siktir")},
            new (){Header = "Salam", Content = new TabInfoViewModel("Timur salam pidor")}
        };
    }
}