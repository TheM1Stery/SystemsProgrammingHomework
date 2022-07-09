using System.Diagnostics;

namespace SampleAppLauncher.Services;

public class ProcessStarter : IProcessStarter
{
    public Process StartProcess(string path, string arguments)
    {
        return Process.Start(path, arguments);
    }

    public Process? StartProcess(ProcessStartInfo startInfo)
    {
        return Process.Start(startInfo);
    }
}