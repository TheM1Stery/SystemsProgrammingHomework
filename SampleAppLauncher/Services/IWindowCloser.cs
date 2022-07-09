using System;

namespace SampleAppLauncher.Services;

public interface IWindowCloser
{
    public Action? Close { get; set; }
}