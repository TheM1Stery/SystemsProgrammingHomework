using System;
using System.IO;
using System.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using TextCipher.Models;
using TextCipher.Services;

namespace TextCipher.ViewModels;

public partial class TabInfoViewModel : BaseViewModel, IRecipient<ValueChangedMessage<EncryptionArgs>>
{
    private readonly IEncryptionService _encryptionService;
    private readonly ITextFileGetterService _textFileGetterService;

    [ObservableProperty]
    private string? _message;
    
    [ObservableProperty]
    private double _progress;

    
    private EncryptionArgs? _encryptionArgs;
    
    
    private void ThreadMethod(object? state)
    {
        if (_encryptionArgs?.FromPath is null || _encryptionArgs.ToPath is null) 
            return;
        using var from = new StreamReader(new FileStream(_encryptionArgs.FromPath, FileMode.Open));
        using var to = new StreamWriter(new FileStream(_encryptionArgs.ToPath, FileMode.OpenOrCreate));
        _encryptionService.Encrypt(from, to, _encryptionArgs.Key);
    }
    
    public TabInfoViewModel(IEncryptionService encryptionService, ITextFileGetterService textFileGetterService)
    {
        _encryptionService = encryptionService;
        _textFileGetterService = textFileGetterService;
        WeakReferenceMessenger.Default.Register(this);
        _encryptionService.Encrypting += () =>
        {
            Progress++;
        };
    }

    public async void Receive(ValueChangedMessage<EncryptionArgs> message)
    {
        if (message.Value.FromPath is null || message.Value.ToPath is null)
            return;
        WeakReferenceMessenger.Default.Unregister<ValueChangedMessage<EncryptionArgs>>(this);
        Message = await _textFileGetterService.GetText(message.Value.FromPath);
        _encryptionArgs = message.Value;
        ThreadPool.QueueUserWorkItem(ThreadMethod);
    }
}