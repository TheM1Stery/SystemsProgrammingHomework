using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    private readonly IMessageBoxShower? _messageBox;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(AverageReaction))]
    private int _count;


    [ObservableProperty]
    private IBrush? _buttonColor;
    
    
    [ObservableProperty]
    private bool _isButtonEnabled;


    public string AverageReaction => $"{_sum / _count} ms";



    private double _sum;

    
    
    
    
    public MainViewModel(IMessageBoxShower messageBox)
    {
        _messageBox = messageBox;
        ButtonColor = Brushes.Red;
        IsButtonEnabled = false;
    }

    public MainViewModel() { } // needed for Design.DataContext in MainWindow.axaml.cs


    [RelayCommand]
    private void Start()
    {
        
        IsButtonEnabled = true;
        var timer = new Thread(() =>
        {
            Thread.Sleep(1000);
            ButtonColor = Brushes.Green;
            var start = DateTime.Now;
            var end = start;
            while (_isButtonEnabled)
            {
                end = DateTime.Now;
                Thread.Sleep(1000);
            }
            _sum += (end - start).TotalMilliseconds;
            Count++;
            ButtonColor = Brushes.Red;
        })
        {
            IsBackground = true
        };
        
        timer.Start();
    }
    

    [RelayCommand]
    private void ButtonPress()
    {
        IsButtonEnabled = false;
    }
}