using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using FluentAvalonia.UI.Controls;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using TextCipher.Factories;
using TextCipher.Models;
using TextCipher.Services;
using TabItem = TextCipher.Models.TabItem;

namespace TextCipher.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    private readonly IFileSelector _fileSelector;
    private readonly ITabFactory _tabFactory;

    [ObservableProperty]
    private string? _title;

    
    public ObservableCollection<TabItem> Tabs { get; set; }

    public MainViewModel(IFileSelector fileSelector, ITabFactory tabFactory)
    {
        _fileSelector = fileSelector;
        _tabFactory = tabFactory;
        Title = "Text Cipher";
        Tabs = new ObservableCollection<TabItem>();
    }
    
    [RelayCommand]
    private async Task AddTab()
    { 
        var paths = await _fileSelector.GetFiles(new List<FileDialogFilter>
        {
            new ()
            {
                Extensions = new List<string>{"txt"},
                Name = "text files"
            }
        });
        if (paths is {Count: 0})
        {
            await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
            {
                ContentTitle = "Error",
                ContentMessage = "Couldn't get text files",
                Icon = Icon.Error,
                ShowInCenter = true,
            }).Show();
            return;
        }

        foreach (var path in paths)
        {
            var fileName = path.Split(@"\")[^1];
            Tabs.Add(new TabItem{Header = fileName, 
                Content = _tabFactory.Create()});
            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<EncryptionArgs>(new EncryptionArgs
            {
                FromPath = path,
                ToPath = path[..path.LastIndexOf(@"\", StringComparison.Ordinal)] + $@"\{fileName}.ffe",
                Key = 3
            }));
        }
    }

    [RelayCommand]
    private void DeleteTab(TabViewTabCloseRequestedEventArgs tabCloseArgs)
    {
        Tabs.Remove((TabItem)tabCloseArgs.Item);
    }
}