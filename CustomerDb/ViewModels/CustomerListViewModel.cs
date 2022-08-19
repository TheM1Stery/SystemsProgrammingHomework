using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CustomerDb.Models;
using CustomerDb.Services;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using MVVMUtils;

namespace CustomerDb.ViewModels;

public partial class CustomerListViewModel : BaseViewModel, IRecipient<RequestMessage<Customer>>
{
    private readonly ICustomerDbClient _dbClient;
    private readonly IModalMessageBox _modalMessageBox;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SearchCommand))]
    private string? _searchString;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
    [NotifyCanExecuteChangedFor(nameof(EditCommand))]
    private Customer? _selectedCustomer;

    [ObservableProperty]
    private ObservableCollection<Customer> _customers = new();
    
    [ObservableProperty]
    private ObservableCollection<int> _pages = new();

    [ObservableProperty]
    private int _selectedPage;
    
    public CustomerListViewModel(ICustomerDbClient dbClient, IModalMessageBox modalMessageBox,
        INavigationService<BaseViewModel> navigationService) 
        : base(navigationService)
    {
        _dbClient = dbClient;
        _modalMessageBox = modalMessageBox;
        StrongReferenceMessenger.Default.Register(this);
    }

    // because this is a event handler, async void is valid
    async partial void OnSelectedPageChanged(int value)
    {
        Customers = new ObservableCollection<Customer>(await _dbClient.GetCustomersByPageAsync(SelectedPage, 100));
    }

    private bool CanSearch => !string.IsNullOrWhiteSpace(_searchString);

    private bool CanDoWorkWithCustomer => _selectedCustomer is not null;

    [RelayCommand]
    private async Task InitializedAsync()
    {
        // testing if it will connect, if not show the message and close the application
        try
        {
            await _dbClient.ConnectAsync();
        }
        catch (Exception)
        {
            await _modalMessageBox.Show(new MessageBoxStandardParams()
            {
                ButtonDefinitions = ButtonEnum.Ok, Icon = Icon.Error,
                ContentMessage = "Couldn't connect to the database. Please check your connection " +
                                 "\nor your connection string in appsettings.json",
                ShowInCenter = true,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            });
            StrongReferenceMessenger.Default.Send(new SignalCloseMessage());
            return;
        }
        
        
        var count = await _dbClient.GetCustomerCountAsync();
        var pageCount = (int)Math.Ceiling(count / 100.0);
        Pages = new ObservableCollection<int>(Enumerable.Range(1, pageCount));
    }
    
    [RelayCommand(CanExecute = nameof(CanSearch))]
    private async Task SearchAsync()
    {
        var enumerable = await _dbClient.SearchCustomersAsync(_searchString!);
        Customers = new ObservableCollection<Customer>(enumerable);
    }

    [RelayCommand(CanExecute = nameof(CanDoWorkWithCustomer))]
    private async Task DeleteAsync()
    {
        var customerToDelete = _selectedCustomer;
        Customers.Remove(customerToDelete!);
        await _dbClient.RemoveCustomerAsync(customerToDelete!);
    }

    [RelayCommand(CanExecute = nameof(CanDoWorkWithCustomer))]
    private void Edit()
    {
        NavigationService.Navigate<EditCustomerViewModel>();
    }

    [RelayCommand]
    private void Add()
    {
        NavigationService.Navigate<AddCustomerViewModel>();
    }

    public void Receive(RequestMessage<Customer> message)
    {
        message.Reply(_selectedCustomer!);
        StrongReferenceMessenger.Default.Unregister<RequestMessage<Customer>>(this);
    }
}