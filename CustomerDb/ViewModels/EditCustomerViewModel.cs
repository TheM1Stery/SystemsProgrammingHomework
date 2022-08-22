using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CustomerDb.Models;
using CustomerDb.Services;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using MVVMUtils;

namespace CustomerDb.ViewModels;

public partial class EditCustomerViewModel : BaseViewModel
{
    private readonly ICustomerDbClient _dbClient;
    private readonly IModalMessageBox _messageBox;

    [ObservableProperty] 
    private Customer _customer;

    private readonly string? _oldEmail;
    
    
    [ObservableProperty]
    private List<Gender> _genders = new() { Gender.Male , Gender.Female};

    [ObservableProperty]
    private Gender? _selectedGender;
    
    partial void OnSelectedGenderChanged(Gender? value)
    {
        Customer.Gender = value;
    }
    
    public EditCustomerViewModel(INavigationService<BaseViewModel> navigationService, 
        ICustomerDbClient dbClient, IModalMessageBox messageBox) : base(navigationService)
    {
        _dbClient = dbClient;
        _messageBox = messageBox;
        _customer= StrongReferenceMessenger.Default.Send(new RequestMessage<Customer>());
        _oldEmail = _customer.Email;
        _selectedGender = _customer.Gender;
    }

    [RelayCommand]
    private async Task EditAsync()
    {
        if (Customer.HasErrors)
        {
            var builder = new StringBuilder();
            foreach (var validationResult in Customer.GetErrors())
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
        if (Customer.Email is not null && Customer.Email != _oldEmail 
                                       && await _dbClient.DoesEmailExistAsync(Customer.Email))
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
        await _dbClient.EditCustomerAsync(Customer);
    }

    [RelayCommand]
    private void GoBack()
    {
        NavigationService.Navigate<CustomerListViewModel>();
    }
}