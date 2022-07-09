using System.Diagnostics;

namespace SampleAppLauncher.Services;

public interface IProcessStarter
{
    /// <summary>
    /// Starts the process from the path
    /// </summary>
    /// <param name="path">path to process</param>
    /// <param name="arguments">arguments to the process</param>
    /// <returns>The created process</returns>
    public Process StartProcess(string path, string arguments);

    
    /// <summary>
    /// This methods excepts that the caller will provide all arguments for starting the process
    /// </summary>
    /// <param name="startInfo">Process configuration</param>
    /// <returns>The created process</returns>
    public Process? StartProcess(ProcessStartInfo startInfo);
}