using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CustomerDb.Models;
using CustomerDb.Services;
using MVVMUtils;

namespace CustomerDb.ViewModels;

public partial class MainViewModel : BaseViewModel, IWindowCloser, IRecipient<SignalCloseMessage>
{
    public INavigationStore<BaseViewModel> Store { get; }

    [ObservableProperty]
    private string? _title;

    public MainViewModel(INavigationStore<BaseViewModel> store, INavigationService<BaseViewModel> navigationService) 
        : base(navigationService)
    {
        Store = store;
        Title = "CustomerDB";
        NavigationService.Navigate<CustomerListViewModel>();
        StrongReferenceMessenger.Default.Register(this);
    }
    
    public Action? Close { get; set; }
    public void Receive(SignalCloseMessage message)
    {
        Close?.Invoke();
        StrongReferenceMessenger.Default.Unregister<SignalCloseMessage>(this);
    }
}