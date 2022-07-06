using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MyTaskManager.Models;

namespace MyTaskManager.Services;

public class ProcessHandlerService : IProcessHandlerService
{
    public void KillProcess(ProcessMainInfo processMainInfo)
    {
        using var process = Process.GetProcessById(processMainInfo.ProcessId);
        process.Kill();
    }

    public ProcessMainInfo StartProcess(string path)
    {
        return new ProcessMainInfo(Process.Start(path));
    }

    public ProcessMainInfo? StartProcess(ProcessStartInfo startInfo)
    {
        var process = Process.Start(startInfo);
        return process is null ? null : new ProcessMainInfo(process);
    }

    public void ChangePriorityOfProcess(ProcessMainInfo processMainInfo, ProcessPriorityClass processPriorityClass)
    {
        using var process = Process.GetProcessById(processMainInfo.ProcessId);
        process.PriorityClass = processPriorityClass;
    }

    public IEnumerable<ProcessMainInfo> GetAllProcesses()
    {
        return from x in Process.GetProcesses() select new ProcessMainInfo(x);
    }
}