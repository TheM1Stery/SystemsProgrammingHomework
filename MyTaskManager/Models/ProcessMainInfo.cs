using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MyTaskManager.Models;

[INotifyPropertyChanged]
public partial class ProcessMainInfo : IDisposable
{
    private readonly Process _process;
    
    public string? ProcessName { get; }
    public int ProcessId { get; private set; }
    public Status? CurrentStatus { get; }

    [ObservableProperty] 
    private ProcessPriorityClass? _priorityClass;
    
    public ProcessMainInfo(Process process)
    {
        _process = process;
        ProcessName = _process.ProcessName;
        try
        {
            ProcessId = _process.Id;
        }
        catch (Exception)
        {
            ProcessId = 0;
        }
        try
        {
            PriorityClass = _process.PriorityClass;
        }
        catch (Exception)
        {
            PriorityClass = null;
        }
        CurrentStatus = _process.Responding ? Status.Running : Status.Suspended;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _process.Dispose();
    }
}