using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using AvaloniaEdit.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;
using MyTaskManager.Models;
using MyTaskManager.Services;

namespace MyTaskManager.ViewModels;

public partial class MainViewModel : BaseViewModel, IDisposable, IWindowCloser
{
    private readonly IProcessHandlerService _processHandlerService;
    private readonly IProcessChooser _processChooser;
    private readonly IMessageBoxShower _messageBox;

    [ObservableProperty]
    private ObservableCollection<ProcessMainInfo>? _processInfos;

    [ObservableProperty]
    private ObservableCollection<ProcessPriorityClass> _processPriorities;

    [ObservableProperty]
    private ProcessMainInfo? _selectedProcessHandler;

    [ObservableProperty]
    private ProcessPriorityClass _selectedPriority;

    [ObservableProperty]
    private string? _searchString;
    
    public Action? Close { get; set; }

    
    public MainViewModel(IProcessHandlerService processHandlerService, IProcessChooser processChooser,
        IMessageBoxShower messageBox)
    {
        _processHandlerService = processHandlerService;
        _processChooser = processChooser;
        _messageBox = messageBox;
        ProcessInfos = new ObservableCollection<ProcessMainInfo>(_processHandlerService.GetAllProcesses()
            .OrderBy(x => x.ProcessName));
        _processPriorities = new ObservableCollection<ProcessPriorityClass>();
        ProcessPriorities.AddRange(new List<ProcessPriorityClass>
        {
            ProcessPriorityClass.RealTime,
            ProcessPriorityClass.High,
            ProcessPriorityClass.AboveNormal,
            ProcessPriorityClass.Normal,
            ProcessPriorityClass.BelowNormal,
            ProcessPriorityClass.Idle
        });
    }

    [RelayCommand]
    private void Search()
    {
        if (_searchString is null || ProcessInfos is null)
            return;
        ProcessInfos.Dispose();
        ProcessInfos =
            new ObservableCollection<ProcessMainInfo>(ProcessInfos.Where(x => x.ProcessName != null && 
                                                                              x.ProcessName.Contains(_searchString)));
    }

    [RelayCommand]
    private async Task ChangePriority()
    {
        if (_selectedProcessHandler == null)
            return;
        if (_selectedProcessHandler.PriorityClass is null)
        {
            await _messageBox.ShowMessageAsync("Cannot change the priority of this process.\n" + "You don't have access", 
                "Error", Icon.Error);
            return;
        }
        _processHandlerService.ChangePriorityOfProcess(_selectedProcessHandler, _selectedPriority);
        _selectedProcessHandler.PriorityClass = _selectedPriority;
    }

    [RelayCommand]
    private async Task OpenFileDialog()
    {
        var result = await _processChooser.GetProcessPath();
        if (result == null)
            return;
        try
        {
            var process =_processHandlerService.StartProcess(result);
            ProcessInfos?.Add(process);

        }
        catch (Exception)
        {
            await _messageBox.ShowMessageAsync("Couldn't start process", "Error", Icon.Error);
        }
    }
    
    [RelayCommand]
    private void Refresh()
    {
        // dispose all processes
        _processInfos?.Dispose();
        ProcessInfos = new ObservableCollection<ProcessMainInfo>(_processHandlerService.GetAllProcesses());
    }

    [RelayCommand]
    private async Task Kill()
    {
        if (_selectedProcessHandler is null)
            return;
        try
        {
            _processHandlerService.KillProcess(_selectedProcessHandler);
            Refresh();
        }
        catch (Exception)
        {
            await _messageBox.ShowMessageAsync("Couldn't kill the process", "Error", Icon.Error);
        }
    }
    

    [RelayCommand]
    private void Exit()
    {
        Close?.Invoke();
    }
    
    public void Dispose()
    {
        _processInfos?.Dispose();
        GC.SuppressFinalize(this);
    }

}