using System.Threading;

namespace TextCipher.Services;

public class SemaphoreWrapper : ISemaphoreWrapper
{
    private readonly Semaphore _semaphore = new(5, 5);
    
    
    public void WaitOne()
    {
        _semaphore.WaitOne();
    }

    public void Release()
    {
        _semaphore.Release();
    }
}