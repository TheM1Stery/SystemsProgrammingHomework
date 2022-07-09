using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MessageBox.Avalonia.Enums;
using SampleAppLauncher.Services;

namespace SampleAppLauncher.ViewModels;

public partial class MainViewModel : BaseViewModel, IWindowCloser
{
    private readonly IPathSelector _pathSelector;
    private readonly IMessageBoxShower _messageBox;
    private readonly IProcessStarter _processStarter;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Range(0.0,double.MaxValue)]
    [Required]
    private double _firstNumber;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Range(0.0,double.MaxValue)]
    [Required]
    private double _secondNumber;


    [ObservableProperty]
    private double _increment;

    public ObservableCollection<char> Operators { get; }

    [ObservableProperty]
    private bool _isDebugChecked;

    [ObservableProperty]
    private string? _path;

    [ObservableProperty]
    private char _selectedOperator;
    
    public Action? Close { get; set; }
    
    public MainViewModel(IPathSelector pathSelector, IMessageBoxShower messageBox, IProcessStarter processStarter)
    {
        _pathSelector = pathSelector;
        _messageBox = messageBox;
        _processStarter = processStarter;
        Increment = 0.1;
        Operators = new ObservableCollection<char> {'+', '-', '/', '*'};
    }


    [RelayCommand]
    private void Start()
    {
        var debug = "../../../SampleApp/DebugMode/net6.0/SampleApp.exe";
        var release = "../../../SampleApp/ReleaseMode/net6.0/SampleApp.exe";
        var arguments = $"{_firstNumber} {_secondNumber} {_selectedOperator}";
        if (_isDebugChecked)
        {
            if (_path is not null)
            {
                arguments += $" {_path}";
            }
            _processStarter.StartProcess(debug, arguments);
            return;
        }
        _processStarter.StartProcess(release, arguments);
    }

    [RelayCommand]
    private async Task SelectPath()
    {
        var path = await _pathSelector.GetPath(new List<FileDialogFilter>
        {
            new()
            {
                Extensions = new List<string>{"txt"},
                Name = "Text file"
            },
        });
        if (path is null)
        {
            await _messageBox.ShowMessageAsync("Error getting path", "Error", Icon.Error);
        }
        Path = path;
    }
    
    [RelayCommand]
    private void Exit()
    {
        Close?.Invoke();
    }

}