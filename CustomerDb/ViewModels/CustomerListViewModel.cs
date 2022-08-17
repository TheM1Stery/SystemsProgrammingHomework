using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CustomerDb.Models;
using CustomerDb.Services;
using MessageBox.Avalonia;
using MVVMUtils;

namespace CustomerDb.ViewModels;

public partial class CustomerListViewModel : BaseViewModel
{
    private readonly ICustomerDbClient _dbClient;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SearchCommand))]
    private string? _searchString;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
    [NotifyCanExecuteChangedFor(nameof(EditCommand))]
    private Customer? _selectedCustomer;

    [ObservableProperty]
    private ObservableCollection<Customer> _customers = new();
    
    // public ObservableCollection<int> Pages { get; private set; } = new();
    [ObservableProperty]
    private ObservableCollection<int> _pages = new();

    [ObservableProperty]
    private int _selectedPage;
    
    public CustomerListViewModel(ICustomerDbClient dbClient, INavigationService<BaseViewModel> navigationService) 
        : base(navigationService)
    {
        _dbClient = dbClient;
        PropertyChanged += OnPropertyChanged;
    }

    // because this is a event handler, async void is valid
    private async void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(SelectedPage)) 
            return;
        Customers = new ObservableCollection<Customer>(await _dbClient.GetCustomersByPage(SelectedPage, 100));
    }

    private bool CanSearch => !string.IsNullOrWhiteSpace(_searchString);

    private bool CanDoWorkWithCustomer => _selectedCustomer is not null;

    [RelayCommand]
    private async Task InitializedAsync()
    {
        var count = await _dbClient.GetCustomerCount();
        var pageCount = (int)Math.Ceiling(count / 100.0);
        Pages = new ObservableCollection<int>(Enumerable.Range(1, pageCount));
    }
    
    [RelayCommand(CanExecute = nameof(CanSearch))]
    private async Task SearchAsync()
    {
        var enumerable = await _dbClient.SearchCustomers(_searchString!);
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
    private async Task EditAsync()
    {
        await MessageBoxManager.GetMessageBoxStandardWindow("error", "Not implemented yet").Show();
    }
}