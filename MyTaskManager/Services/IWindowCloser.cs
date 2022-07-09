using System;

namespace MyTaskManager.Services;

public interface IWindowCloser
{
    public Action? Close { get; set; }
}