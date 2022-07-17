using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MessageBox.Avalonia.Enums;
using SampleAppLauncher.Services;
using Path = System.IO.Path;

namespace SampleAppLauncher.ViewModels;

public partial class MainViewModel : BaseViewModel, IWindowCloser, IDisposable
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

    private Semaphore _semaphore;
    
    public MainViewModel(IPathSelector pathSelector, IMessageBoxShower messageBox, IProcessStarter processStarter)
    {
        _pathSelector = pathSelector;
        _messageBox = messageBox;
        _processStarter = processStarter;
        _semaphore = new Semaphore(3, 3);
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
        if (!_semaphore.WaitOne(100))
        {
            await _messageBox.ShowMessageAsync("You reached the limit of processes", "erorr", Icon.Error);
            return;
        }
        if (_isDebugChecked)
        {
            if (_logPath is not null)
            {
                var args = arguments + $" {_logPath}";
                if (!Path.IsPathFullyQualified(_logPath))
                {
                    await _messageBox.ShowMessageAsync("The path was not valid, console log will be used", 
                        "Warning", Icon.Warning);
                }
                else
                {
                    arguments = args;
                }
            }
            var process = _processStarter.StartProcess(debug, arguments);
            process.EnableRaisingEvents = true;
            process.Exited += (_, _) =>
            {
                _semaphore.Release(1);
            };
            return;
        }
        var secondProcess = _processStarter.StartProcess(release, arguments);
        secondProcess.EnableRaisingEvents = true;
        secondProcess.Exited += (_, _) =>
        {
            _semaphore.Release(1);
        };
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

    public void Dispose()
    {
        _semaphore.Dispose();
        GC.SuppressFinalize(this);
    }
}