using System;
using System.IO;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
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

    
    public EncryptionArgs? EncryptionArgs { get; private set; }


    [ObservableProperty]
    private int _messageLength;
    

    private bool _isCyphered;


    
  
    
    private async void ThreadMethod(object? state)
    {
        if (EncryptionArgs?.FromPath is null || EncryptionArgs.ToPath is null)
            return;
        _isCyphered = true;
        _semaphore.WaitOne();
        if (Message == "Couldn't get the message. But it can be cyphered..")
        {
            MessageLength = _textFileGetterService.GetTextLength(EncryptionArgs.FromPath);
            Message = "Cyphering...";
        }
        Action<int> lamdba = i => Progress = i;
        _encryptionService.OnOnePercent += lamdba;
        try
        {
            using var from = new FileStream(EncryptionArgs.FromPath, FileMode.Open, FileAccess.Read,
                FileShare.Read);
            using var to = new FileStream(EncryptionArgs.ToPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
            _encryptionService.Encrypt(from, to, EncryptionArgs.Key);
            from.Close();
            to.Close();
            Message = _textFileGetterService.GetText(EncryptionArgs.ToPath) ?? "Cypher was done successfully";
            _semaphore.Release();
            _encryptionService.OnOnePercent -= lamdba;
        }
        catch (Exception)
        {
            await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
            {
                ContentTitle = "Error",
                ContentMessage = "Couldn't open the files",
                Icon = Icon.Error,
                ShowInCenter = true,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            }).Show();
        }
        
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

    public async void Receive(ValueChangedMessage<EncryptionArgs> message)
    {
        if (message.Value.FromPath is null || message.Value.ToPath is null)
            return;
        WeakReferenceMessenger.Default.Unregister<ValueChangedMessage<EncryptionArgs>>(this);
        EncryptionArgs = message.Value;
        try
        {
            Message = _textFileGetterService.GetText(EncryptionArgs.FromPath) ?? "Couldn't get the message. But it can be cyphered..";
            if (Message is not null)
                MessageLength = Message.Length;
            if (Message == "Couldn't get the message. But it can be cyphered..")
                MessageLength = 100;
        }
        catch (Exception)
        {
            await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
            {
                ContentTitle = "Error",
                ContentMessage = "Couldn't get the text",
                Icon = Icon.Error,
                ShowInCenter = true,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            }).Show();
        }
        
    }
}