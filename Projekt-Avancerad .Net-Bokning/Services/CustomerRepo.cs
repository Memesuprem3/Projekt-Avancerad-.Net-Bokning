using Microsoft.EntityFrameworkCore;
using Projekt_Avancerad_.Net_Bokning.Data;
using Projekt_Avancerad_.Net_Bokning.Services.Interface;
using Projekt_Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt_Avancerad_.Net_Bokning.Services
{
    public class CustomerRepo : ICustomer
    {
        private readonly AppDbContext _context;
        private readonly IBookingHistory _bookingHistoryRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomerRepo(AppDbContext context, IBookingHistory bookingHistoryRepo, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _bookingHistoryRepo = bookingHistoryRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.customer.Include(c => c.Appointments).ToListAsync();
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await _context.customer.Include(c => c.Appointments).FirstOrDefaultAsync(c => c.CustomerId == id);
        }

        public async Task<Customer> AddAsync(Customer customer)
        {
            var result = await _context.customer.AddAsync(customer);
            await _context.SaveChangesAsync();
            await LogChange("Add", customer.CustomerId, null);
            return result.Entity;
        }

        public async Task UpdateAsync(Customer customer)
        {
            var existingCustomer = await _context.customer.FindAsync(customer.CustomerId);
            if (existingCustomer != null)
            {
                existingCustomer.FristName = customer.FristName;
                existingCustomer.LastName = customer.LastName;
                existingCustomer.Adress = customer.Adress;
                existingCustomer.Phone = customer.Phone;
                existingCustomer.Email = customer.Email;
                _context.customer.Update(existingCustomer);
                await _context.SaveChangesAsync();
                await LogChange("Update", customer.CustomerId, null);
            }
        }

        public async Task<Customer> DeleteAsync(int id)
        {
            var customer = await _context.customer.FindAsync(id);
            if (customer != null)
            {
                _context.customer.Remove(customer);
                await _context.SaveChangesAsync();
                await LogChange("Delete", customer.CustomerId, null);
            }
            return customer;
        }

        private async Task LogChange(string changeType, int? customerId, int? appointmentId)
        {
            var bookingHistory = new BookingHistory
            {
                AppointmentId = appointmentId ?? 0,
                ChangeType = changeType,
                ChangedAt = DateTime.UtcNow,
                ChangedBy = _httpContextAccessor.HttpContext.User.Identity.Name
            };
            await _bookingHistoryRepo.AddBookingHistoryAsync(bookingHistory);
        }

        public async Task<int> GetCustomerAppointmentCountWeekAsync(int customerId, DateTime startOfWeek)
        {
            var endOfWeek = startOfWeek.AddDays(7);
            return await _context.Appointments.CountAsync(a => a.CustomerId == customerId && a.PlacedApp >= startOfWeek && a.PlacedApp < endOfWeek);
        }

        public async Task<IEnumerable<Customer>> GetCustomersWithAppointmentWeekAsync(DateTime startOfWeek)
        {
            var endOfWeek = startOfWeek.AddDays(7);
            return await _context.customer.Include(c => c.Appointments)
                .Where(c => c.Appointments.Any(a => a.PlacedApp >= startOfWeek && a.PlacedApp < endOfWeek))
                .ToListAsync();
        }

        public async Task<IEnumerable<Customer>> GetCustomersSortedAndFilteredAsync(string sortField, string sortOrder, string filterField, string filterValue)
        {
            var query = _context.customer.Include(c => c.Appointments).AsQueryable();

            if (!string.IsNullOrEmpty(filterField) && !string.IsNullOrEmpty(filterValue))
            {
                switch (filterField.ToLower())
                {
                    case "firstname":
                        query = query.Where(c => c.FristName.Contains(filterValue));
                        break;
                    case "lastname":
                        query = query.Where(c => c.LastName.Contains(filterValue));
                        break;
                    case "email":
                        query = query.Where(c => c.Email.Contains(filterValue));
                        break;
                    default:
                        break;
                }
            }

            if (!string.IsNullOrEmpty(sortField))
            {
                switch (sortField.ToLower())
                {
                    case "firstname":
                        query = sortOrder.ToLower() == "desc" ? query.OrderByDescending(c => c.FristName) : query.OrderBy(c => c.FristName);
                        break;
                    case "lastname":
                        query = sortOrder.ToLower() == "desc" ? query.OrderByDescending(c => c.LastName) : query.OrderBy(c => c.LastName);
                        break;
                    case "email":
                        query = sortOrder.ToLower() == "desc" ? query.OrderByDescending(c => c.Email) : query.OrderBy(c => c.Email);
                        break;
                    default:
                        break;
                }
            }

            return await query.ToListAsync();
        }
    }
}