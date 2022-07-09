using MyTaskManager.Services;
using MyTaskManager.ViewModels;

namespace MyTaskManager;

public static class DesignData
{
    public static MainViewModel ExampleMain { get; } =
        new(new ProcessHandlerService(), new ProcessChooser(), new MessageBoxShower());
}