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


    private StreamReader? _from;

    private StreamWriter? _to;

    private int _key;
    
    private readonly Thread _encryptingThread;


    private void ThreadMethod()
    {
        if (_from is null || _to is null) 
            return;
        _encryptionService.Encrypt(_from, _to, _key);
        _from.Dispose();
        _to.Dispose();
    }
    
    public TabInfoViewModel(IEncryptionService encryptionService, ITextFileGetterService textFileGetterService)
    {
        _encryptionService = encryptionService;
        _textFileGetterService = textFileGetterService;
        WeakReferenceMessenger.Default.Register(this);
        _encryptingThread = new Thread(ThreadMethod)
        {
            IsBackground = true
        };
    }

    public async void Receive(ValueChangedMessage<EncryptionArgs> message)
    {
        if (message.Value.FromPath is null || message.Value.ToPath is null)
            return;
        Message = await _textFileGetterService.GetText(message.Value.FromPath);
        _from = new StreamReader(new FileStream(message.Value.FromPath, FileMode.Open));
        _to = new StreamWriter(new FileStream(message.Value.ToPath, FileMode.OpenOrCreate));
        _key = message.Value.Key;
        var onePercent = (int)(Message.Length * 0.01);
        var counter = 0;
        _encryptionService.Encrypting += () =>
        {
            if (counter != onePercent)
            {
                counter++;
                return;
            }
            Progress++;
            counter = 0;
        };
        _encryptingThread.Start();
        WeakReferenceMessenger.Default.Unregister<EncryptionArgs>(this);
    }
}