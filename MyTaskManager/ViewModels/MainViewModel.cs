using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
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

public partial class MainViewModel : BaseViewModel, IDisposable
{
    private readonly IProcessHandlerService _processHandlerService;
    private readonly IProcessChooser _processChooser;

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
    
    

    public MainViewModel(IProcessHandlerService processHandlerService, IProcessChooser processChooser)
    {
        _processHandlerService = processHandlerService;
        _processChooser = processChooser;
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
            var box = MessageBoxManager.GetMessageBoxStandardWindow("Error", "Cannot change the priority of this process.\n" +
                                                                   "You don't have access", ButtonEnum.Ok, Icon.Error,
                WindowStartupLocation.CenterOwner);
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                await box.ShowDialog(desktop.MainWindow);
                return;
            }
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
            var box = MessageBoxManager.GetMessageBoxStandardWindow("Error", "Couldn't start the process", 
                ButtonEnum.Ok, Icon.Error, WindowStartupLocation.CenterOwner);
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                await box.ShowDialog(desktop.MainWindow);
            }
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
    private void Kill()
    {
        if (_selectedProcessHandler is null)
            return;
        _processHandlerService.KillProcess(_selectedProcessHandler);
        Refresh();
    }
    

    [RelayCommand]
    private void Exit()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow.Close();
        }
    }
    
    public void Dispose()
    {
        _processInfos?.Dispose();
        GC.SuppressFinalize(this);
    }

}