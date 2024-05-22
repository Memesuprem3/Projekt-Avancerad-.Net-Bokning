using Projekt_Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Projekt_Avancerad_.Net_Bokning.Services.Interface
{
    public interface ICustomer
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer> GetByIdAsync(int id);
        Task<Customer> AddAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task<Customer> DeleteAsync(int id);
        Task<int> GetCustomerAppointmentCountWeekAsync(int customerId, DateTime startOfWeek);
        Task<IEnumerable<Customer>> GetCustomersWithAppointmentWeekAsync(DateTime startOfWeek);
        Task<IEnumerable<Customer>> GetCustomersSortedAndFilteredAsync(string sortField, string sortOrder, string filterField, string filterValue);
    }
}