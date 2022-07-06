using System;
using System.Collections.ObjectModel;

namespace MyTaskManager;

public static class ObservableCollectionsExtensions
{
    public static void Dispose<T>(this ObservableCollection<T> source) where T : IDisposable
    {
        foreach (var disposable in source)
        {
            disposable.Dispose();
        }
    }
}