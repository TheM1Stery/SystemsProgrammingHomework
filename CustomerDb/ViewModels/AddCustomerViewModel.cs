using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CustomerDb.Models;
using CustomerDb.Services;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using MVVMUtils;

namespace CustomerDb.ViewModels;

public partial class AddCustomerViewModel : BaseViewModel
{
    private readonly ICustomerDbClient _dbClient;
    private readonly IModalMessageBox _messageBox;

    [ObservableProperty]
    private Customer _customer = new();
    
    public AddCustomerViewModel(ICustomerDbClient dbClient, IModalMessageBox messageBox,
        INavigationService<BaseViewModel> navigationService) 
        : base(navigationService)
    {
        _dbClient = dbClient;
        _messageBox = messageBox;
    }

    [ObservableProperty]
    private List<Gender> _genders = new() { Gender.Male , Gender.Female};

    [ObservableProperty]
    private Gender? _selectedGender;
    
    
    partial void OnSelectedGenderChanged(Gender? value)
    {
        Customer.Gender = value;
    }


    [RelayCommand]
    private async Task AddAsync()
    {
        if (Customer.FirstName is null && Customer.LastName is null
                                           && Customer.Age is null && Customer.Gender is not null &&
                                           Customer.Email is null)
        {
            return;
        }
        
        if (Customer.HasErrors)
        {
            var builder = new StringBuilder();
            foreach (var validationResult in _customer.GetErrors())
            {
                builder.Append(validationResult.ErrorMessage + '\n');
            }
            await _messageBox.Show(new MessageBoxStandardParams()
            {
                ButtonDefinitions = ButtonEnum.Ok,
                CanResize = false,
                ContentMessage = builder.ToString(),
                ContentTitle = "Error",
                Icon = Icon.Error,
                ShowInCenter = true,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            });
            return;
        }
        if (Customer.Email != null && await _dbClient.DoesEmailExistAsync(Customer.Email))
        {
            await _messageBox.Show(new MessageBoxStandardParams
            {
                ButtonDefinitions = ButtonEnum.Ok,
                ContentMessage = "This email already exists",
                Icon = Icon.Error,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            });
            return;
        }
        NavigationService.Navigate<CustomerListViewModel>();
        await _dbClient.AddCustomerAsync(Customer);
    }
    
    
    [RelayCommand]
    private void GoBack()
    {
        NavigationService.Navigate<CustomerListViewModel>();
    }
}