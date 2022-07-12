using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MessageBox.Avalonia.Enums;

namespace ButtonReactionTimer;

[ObservableObject]
public partial class MainViewModel
{

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(AverageReaction))]
    private int _count;


    [ObservableProperty]
    private IBrush? _buttonColor;
    
    
    [ObservableProperty]
    private bool _isReactionButtonEnabled;

    [ObservableProperty]
    private bool _isStartButtonEnabled;


    private bool _isButtonPressed;


    public string AverageReaction => $"Average reaction time: {_sum / _count} ms";


    [ObservableProperty]
    private string? _lastCount;


    private List<Thread> _threads;

    private double _sum;
    
    public MainViewModel()
    {
        ButtonColor = Brushes.Red;
        IsReactionButtonEnabled = false;
        IsStartButtonEnabled = true;
        _threads = new List<Thread>();
    }
    
    
    [RelayCommand]
    private void Start()
    {
        IsReactionButtonEnabled = true;
        IsStartButtonEnabled = false;
        _threads.Clear();
        for (var i = 0; i < 5; i++)
        {
            _threads.Add(new Thread(() =>
            {
                Thread.Sleep(Random.Shared.Next(500, 3000));
                ButtonColor = Brushes.Green;
                var start = DateTime.Now;
                var end = start;
                while (!_isButtonPressed)
                {
                    end = DateTime.Now;
                    Thread.Sleep(1000);
                }
                var timeSpan = end - start;
                _sum += timeSpan.TotalMilliseconds;
                LastCount = $"Last Attempt: {timeSpan.TotalMilliseconds} ms";
                Count++;
                ButtonColor = Brushes.Red;
                _isButtonPressed = false;
            })
            {
                IsBackground = true
            });
        }
        var thread = new Thread(() =>
        {
            foreach (var item in _threads)
            {
                item.Start();
                item.Join();
            }
            IsStartButtonEnabled = true;
        });
        thread.Start();
    }
    

    [RelayCommand]
    private void ButtonPress()
    {
        if (Equals(ButtonColor, Brushes.Red))
            return;
        _isButtonPressed = true;
    }
}