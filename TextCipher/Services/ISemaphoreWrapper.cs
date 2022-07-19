namespace TextCipher.Services;

public interface ISemaphoreWrapper
{
    public void WaitOne();

    public void Release();
}