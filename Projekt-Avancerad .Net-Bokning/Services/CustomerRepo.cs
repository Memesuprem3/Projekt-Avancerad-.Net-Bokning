using Projekt_Avancerad_.Net_Bokning.Data;
using Projekt_Models;

namespace Projekt_Avancerad_.Net_Bokning.Services
{
    public class CustomerRepo : ICustomer
    {
        private  AppDbContext _context;

        public CustomerRepo(AppDbContext context)
        {
            _context = context;
        }

        public Task<Customer> Add(Customer customer)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Customer>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Appointment>> GetAllInfo(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCustomerAppointmentCountForWeekAsync(int customerId, DateTime startOfWeek)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Customer>> GetCustomersSortedAndFilteredAsync(string sortField, string sortOrder, string filterField, string filterValue)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Customer>> GetCustomersWithAppointmentsForWeekAsync(DateTime startOfWeek)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> Update(Customer customer)
        {
            throw new NotImplementedException();
        }
    }
}
