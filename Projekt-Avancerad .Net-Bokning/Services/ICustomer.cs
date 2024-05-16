using Projekt_Models;

namespace Projekt_Avancerad_.Net_Bokning.Services
{
    public interface ICustomer
    {
        Task<IEnumerable<Customer>> GetAll();
        Task<Customer> GetById(int id);
        Task<Customer> Add(Customer customer);
        Task<Customer> Update(Customer customer);
        Task<Customer> Delete(int id);
        Task<IEnumerable<Appointment>> GetAllInfo(int id);
        Task<IEnumerable<Customer>> GetCustomersWithAppointmentsForWeekAsync(DateTime startOfWeek);
        Task<int> GetCustomerAppointmentCountForWeekAsync(int customerId, DateTime startOfWeek);
        Task<IEnumerable<Customer>> GetCustomersSortedAndFilteredAsync(string sortField, string sortOrder, string filterField, string filterValue);


    }
}
