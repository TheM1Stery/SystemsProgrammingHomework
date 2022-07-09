using SampleAppLauncher.Services;
using SampleAppLauncher.ViewModels;

namespace SampleAppLauncher;

public static class DesignData
{
    public static MainViewModel ExampleMain { get; } = new(new PathSelector(), new MessageBoxShower(), new ProcessStarter());
}