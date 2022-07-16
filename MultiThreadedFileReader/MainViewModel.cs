using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MessageBox.Avalonia;

namespace MultiThreadedFileReader;

[ObservableObject]
public partial class MainViewModel
{
    private readonly IFileSelector _fileSelector;

    [ObservableProperty]
    private string? _message;


    [ObservableProperty]
    private bool _progressBar;

    private byte[]? _file;

    public ObservableCollection<int> NumberOfThreads { get; }

    [ObservableProperty]
    private int _selectedCount;

    public MainViewModel(IFileSelector fileSelector)
    {
        _fileSelector = fileSelector;
        NumberOfThreads = new ObservableCollection<int>();
        for (var i = 2; i < 9; i++)
            NumberOfThreads.Add(i);
    }

    [RelayCommand]
    private async Task UploadFile()
    {
        var path = await _fileSelector.SelectFile();
        if (path is null)
        {
            await MessageBoxManager.GetMessageBoxStandardWindow("Error", "Couldn't get " +
                                                                   "the file").Show();
            return;
        }
        var fileInfo = new FileInfo(path);
        var size = fileInfo.Length;
        _file = Array.Empty<byte>();
        var threadWorkSize = fileInfo.Length / _selectedCount;
        // example: if size is 917 and there are only 3 working threads.
        // 917 / 3 = 305(with floor division).
        // 305 * 3 = 915.
        // If we don't calculate the remainder(in this example it is 2), there will be 2 left out bytes, that is not good.
        // So we calculate the remainder and sum it up in the last thread. 917 - 915 = 2
        // Formula: $fileSize - $threadWorkSize * $numberOfThreads
        var remainder = size - threadWorkSize * _selectedCount;
        var threadList = new List<Thread>();
        var byteArrayList = Enumerable.Repeat<byte[]?>(null, _selectedCount).ToList();
        Message = "Uploading..";
        long initPosition = 0;
        _progressBar = true;
        for (var i = 0; i < _selectedCount; i++)
        {
            var position = initPosition;
            var index = i;
            if (i == _selectedCount - 1)
                threadWorkSize += remainder;
            var workSize = threadWorkSize;
            threadList.Add(new Thread(() =>
            {
                using var stream = new FileStream(path, new FileStreamOptions
                {
                    Share = FileShare.Read,
                    Mode = FileMode.Open,
                    Access = FileAccess.Read
                });
                stream.Position = position;
                byteArrayList[index] = new byte[workSize];
                var read = stream.Read(byteArrayList[index]!,0, (int)workSize);
            }));
            initPosition += threadWorkSize;
        }
        var thread = new Thread(() =>
        {
            threadList.ForEach(thread => thread.Start());
            threadList.ForEach(x => x.Join());
            _file = byteArrayList.SelectMany(x => x!).ToArray();
            Message = "Upload finished";
        });
        thread.Start();
    }
}