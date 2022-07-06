using System.Collections.Generic;
using System.Diagnostics;
using MyTaskManager.Models;

namespace MyTaskManager.Services;

public interface IProcessHandlerService
{
    
    /// <summary>
    /// Terminates the process
    /// </summary>
    /// <param name="process">The process to terminate</param>
    public void KillProcess(ProcessMainInfo process);

    /// <summary>
    /// Starts the process from the path
    /// </summary>
    /// <param name="path">path to process</param>
    /// <returns>The created process</returns>
    public ProcessMainInfo StartProcess(string path);

    
    /// <summary>
    /// This methods excepts that the caller will provide all arguments for starting the process
    /// </summary>
    /// <param name="startInfo">Process configuration</param>
    public ProcessMainInfo? StartProcess(ProcessStartInfo startInfo);

    
    /// <summary>
    /// Changes the priority of the given process
    /// </summary>
    /// <param name="processMainInfo">The process itself</param>
    /// <param name="processPriorityClass">the priority to change to</param>
    public void ChangePriorityOfProcess(ProcessMainInfo processMainInfo, ProcessPriorityClass processPriorityClass);

    
    /// <summary>
    /// Gets all the process run on the machine.
    /// </summary>
    /// <returns>All of the processes run on the pc</returns>
    public IEnumerable<ProcessMainInfo> GetAllProcesses();
}