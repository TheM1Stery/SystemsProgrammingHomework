using System;

namespace CustomerDb.Services;

public interface IWindowCloser
{
    public Action? Close { get; set; }
}