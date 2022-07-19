using System;
using System.IO;
using System.Threading;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using TextCipher.Models;
using TextCipher.Services;

namespace TextCipher.ViewModels;

public partial class TabInfoViewModel : BaseViewModel, IRecipient<ValueChangedMessage<EncryptionArgs>>
{
    private readonly IEncryptionService _encryptionService;
    private readonly ITextFileGetterService _textFileGetterService;
    private readonly ISemaphoreWrapper _semaphore;
    
    
    [ObservableProperty]
    private string? _message;
    
    [ObservableProperty]
    private double _progress;

    
    private EncryptionArgs? _encryptionArgs;


    [ObservableProperty]
    private int _messageLength;
    

    private bool _isCyphered;

  
    
    private void ThreadMethod(object? state)
    {
        if (_encryptionArgs?.FromPath is null || _encryptionArgs.ToPath is null)
            return;
        _semaphore.WaitOne();
        var progress = 0;
        var percent = Math.Round(MessageLength * 0.03);
        _encryptionService.Encrypting += () =>
        {
            if (progress >= percent)
            {
                Progress += progress;
                progress = 0;
            }
            progress++;
        };
        using var from = new StreamReader(new FileStream(_encryptionArgs.FromPath, FileMode.Open, FileAccess.Read,
            FileShare.Read));
        using var to = new StreamWriter(new FileStream(_encryptionArgs.ToPath, FileMode.OpenOrCreate));
        _encryptionService.Encrypt(from, to, _encryptionArgs.Key);
        Progress += progress;
        from.Close();
        to.Close();
        Message = _textFileGetterService.GetText(_encryptionArgs.ToPath);
        _isCyphered = true;
        _semaphore.Release();
    }
    
    
    public TabInfoViewModel(IEncryptionService encryptionService, ITextFileGetterService textFileGetterService,
        ISemaphoreWrapper semaphore)
    {
        _encryptionService = encryptionService;
        _textFileGetterService = textFileGetterService;
        _semaphore = semaphore;
        WeakReferenceMessenger.Default.Register(this);
    }

    public void Cypher()
    {
        if (_isCyphered)
            return;
        ThreadPool.QueueUserWorkItem(ThreadMethod);
    }

    public void Receive(ValueChangedMessage<EncryptionArgs> message)
    {
        if (message.Value.FromPath is null || message.Value.ToPath is null)
            return;
        WeakReferenceMessenger.Default.Unregister<ValueChangedMessage<EncryptionArgs>>(this);
        _encryptionArgs = message.Value;
        Message = _textFileGetterService.GetText(_encryptionArgs.FromPath);
        MessageLength = Message.Length;
    }
}