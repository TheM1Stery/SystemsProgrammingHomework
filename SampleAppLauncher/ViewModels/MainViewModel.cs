using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MessageBox.Avalonia;
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
    [Required]
    private double _firstNumber;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private double _secondNumber;
    

    public ObservableCollection<char> Operators { get; }

    [ObservableProperty]
    private bool _isDebugChecked;

    [ObservableProperty]
    private string? _logPath;

    [ObservableProperty]
    private char _selectedOperator;
    
    public Action? Close { get; set; }
    
    public MainViewModel(IPathSelector pathSelector, IMessageBoxShower messageBox, IProcessStarter processStarter)
    {
        _pathSelector = pathSelector;
        _messageBox = messageBox;
        _processStarter = processStarter;
        Operators = new ObservableCollection<char> {'+', '-', '/', '*'};
    }


    [RelayCommand]
    private async Task Start()
    {
        if (HasErrors)
        {
            var builder = new StringBuilder();
            foreach (var validationResult in GetErrors())
            {
                builder.Append(validationResult + "\n");
            }
            await _messageBox.ShowMessageAsync($"There were errors: \n{builder}", "Errors", Icon.Error);
            return;
        }
        var debug = "SampleApp/DebugMode/net6.0/SampleApp.exe";
        var release = "SampleApp/ReleaseMode/net6.0/SampleApp.exe";
        var arguments = $"{_firstNumber} {_secondNumber} {_selectedOperator}";
        if (_isDebugChecked)
        {
            if (_logPath is not null)
            {
                var args = arguments + $"{_logPath}";
                if (!Uri.IsWellFormedUriString(_logPath, UriKind.Absolute))
                {
                    await _messageBox.ShowMessageAsync("The path was not valid, console log will be used", 
                        "Warning", Icon.Warning);
                }
                else
                {
                    arguments = args;
                }
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
        LogPath = path;
    }
    
    [RelayCommand]
    private void Exit()
    {
        Close?.Invoke();
    }

}