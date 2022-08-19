using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerDb.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CustomerDb.Services;

public class CustomerDbClient : ICustomerDbClient
{

    private string _connectionString;
    
    public CustomerDbClient(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public async Task ConnectAsync()
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
    }

    public async Task AddCustomerAsync(Customer customer)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync("INSERT INTO Customers(FirstName, LastName, Email, Gender, Age) " +
                                  "VALUES(@FirstName, @LastName, @Email, @Gender, @Age)",
            new { customer.FirstName, customer.LastName, customer.Email, customer.Gender, customer.Age });
    }

    public async Task RemoveCustomerAsync(Customer customer)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync("DELETE FROM Customers WHERE Id = @Id", new { customer.Id });
    }

    public async Task EditCustomerAsync(Customer customer)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync(
            "UPDATE Customers SET FirstName = @FirstName, LastName = @LastName, Email = @Email, " +
            "Gender = @Gender, Age = @Age WHERE Id = @Id", new
            {
                customer.FirstName, customer.LastName, customer.Email, customer.Gender, customer.Age, customer.Id
            });
    }

    public async Task<Customer?> GetCustomerByIdAsync(int id)
    {
        await using var connection = new SqlConnection(_connectionString);
        var customer = await connection.QuerySingleOrDefaultAsync<Customer>
            ("SELECT * FROM Customers WHERE Id = @Id", new { Id = id });
        return customer;
    }

    public async Task<IEnumerable<Customer>> SearchCustomersAsync(string searchString)
    {
        await using var connection = new SqlConnection(_connectionString);
        var parameter = $"{searchString}%";
        var customers = await connection
            .QueryAsync<Customer>(
                "SELECT * FROM Customers WHERE FirstName LIKE @searchString OR LastName LIKE @searchString",
                new { searchString = parameter});
        return customers;
    }

    public async Task<IEnumerable<Customer>> GetCustomersByPageAsync(int page, int customerPerPage)
    {
        await using var connection = new SqlConnection(_connectionString);
        var count = await GetCustomerCountAsync();
        var pageCount = (int)Math.Ceiling(count / (double)customerPerPage);
        if (page > pageCount)
        {
            return Enumerable.Empty<Customer>();
        }
        var from = (page - 1) * customerPerPage;
        var sql = "SELECT * FROM (SELECT *, ROW_NUMBER() OVER(ORDER BY Id) AS row FROM Customers) temp " +
                  "WHERE row >= @From AND row <= @To";
        var customers = await connection.QueryAsync<Customer>(sql, 
            new {From = from, To = from + customerPerPage});
        return customers;
    }

    public async Task<int> GetCustomerCountAsync()
    {
        await using var connection = new SqlConnection(_connectionString);
        return await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Customers");
    }

    public async Task<bool> DoesEmailExistAsync(string email)
    {
        await using var connection = new SqlConnection(_connectionString);
        var count = connection.ExecuteScalar<int>("SELECT COUNT(1) FROM Customers WHERE Email = @Email",
            new {Email = email});
        return count > 0;
    }
}