using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerDb.Models;

namespace CustomerDb.Services;

public interface ICustomerDbClient
{
    public Task ConnectAsync();

    public Task AddCustomerAsync(Customer customer);

    public Task RemoveCustomerAsync(Customer customer);

    public Task EditCustomerAsync(Customer customer);

    public Task<Customer?> GetCustomerByIdAsync(int id);

    public Task<IEnumerable<Customer>> SearchCustomersAsync(string searchString);


    public Task<IEnumerable<Customer>> GetCustomersByPageAsync(int page, int customerPerPage);

    public Task<int> GetCustomerCountAsync();

    public Task<bool> DoesEmailExistAsync(string Email);

}