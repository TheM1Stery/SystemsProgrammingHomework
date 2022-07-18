﻿using System;
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

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(MessageLength))]
    private string? _message;
    
    [ObservableProperty]
    private double _progress;

    
    private EncryptionArgs? _encryptionArgs;


    public double MessageLength => Message?.Length ?? default;
    

    private bool _isCyphered;

  
    
    private void ThreadMethod(object? state)
    {
        if (_encryptionArgs?.FromPath is null || _encryptionArgs.ToPath is null)
            return;
        var progress = 0;
        var percent = MessageLength * 0.01;
        _encryptionService.Encrypting += () =>
        {
            if (progress >= percent)
            {
                Progress++;
                progress = 0;
            }
            progress++;
        };
        using var from = new StreamReader(new FileStream(_encryptionArgs.FromPath, FileMode.Open, FileAccess.Read,
            FileShare.Read));
        using var to = new StreamWriter(new FileStream(_encryptionArgs.ToPath, FileMode.OpenOrCreate));
        _encryptionService.Encrypt(from, to, _encryptionArgs.Key);
        _isCyphered = true;
        Message = _textFileGetterService.GetText(_encryptionArgs.FromPath);
    }
    
    
    public TabInfoViewModel(IEncryptionService encryptionService, ITextFileGetterService textFileGetterService)
    {
        _encryptionService = encryptionService;
        _textFileGetterService = textFileGetterService;
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
    }
}