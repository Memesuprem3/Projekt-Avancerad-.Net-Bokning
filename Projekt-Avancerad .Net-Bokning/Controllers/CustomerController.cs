using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Projekt_Avancerad_.Net_Bokning.Services.Interface;
using Projekt_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Projekt_Avancerad_.Net_Bokning.Controllers
{
    namespace Projekt_Avancerad_.Net_Bokning.Controllers
    {
        [Authorize(Roles = "Admin,Company,Customer")]
        [ApiController]
        [Route("api/[controller]")]
        public class CustomerController : ControllerBase
        {
            private readonly ICustomer _customerRepo;
            private readonly IAppointment _appointmentRepo;
            private readonly ILogger<CustomerController> _logger;

            public CustomerController(ICustomer customerRepo, IAppointment appointmentRepo, ILogger<CustomerController> logger)
            {
                _customerRepo = customerRepo;
                _appointmentRepo = appointmentRepo;
                _logger = logger;
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<Customer>>> GetAllCustomers()
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    var claims = identity.Claims.ToList();
                    foreach (var claim in claims)
                    {
                        _logger.LogInformation($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
                    }
                }

                var customers = await _customerRepo.GetAllAsync();
                return Ok(customers);
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<Customer>> GetCustomerById(int id)
            {
                var customer = await _customerRepo.GetByIdAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }
                return Ok(customer);
            }

            [HttpPost]
            public async Task<ActionResult<Customer>> AddCustomer(Customer customer)
            {
                var createdCustomer = await _customerRepo.AddAsync(customer);
                return CreatedAtAction(nameof(GetCustomerById), new { id = createdCustomer.CustomerId }, createdCustomer);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateCustomer(int id, Customer customer)
            {
                if (id != customer.CustomerId)
                {
                    return BadRequest();
                }
                await _customerRepo.UpdateAsync(customer);
                return NoContent();
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteCustomer(int id)
            {
                var customer = await _customerRepo.DeleteAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }
                return NoContent();
            }

            [HttpGet("{id}/appointments")]
            public async Task<ActionResult<IEnumerable<Appointment>>> GetCustomerAppointments(int id)
            {
                var customer = await _customerRepo.GetByIdAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }
                return Ok(customer.Appointments);
            }

            [HttpGet("{id}/appointments/week/{startOfWeek}")]
            public async Task<ActionResult<int>> GetCustomerAppointmentCountForWeek(int id, DateTime startOfWeek)
            {
                var count = await _customerRepo.GetCustomerAppointmentCountWeekAsync(id, startOfWeek);
                return Ok(count);
            }

            [HttpGet("appointments/current-week")]
            public async Task<ActionResult<IEnumerable<Customer>>> GetCustomersWithAppointmentsForWeek()
            {
                var startOfWeek = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
                var customers = await _customerRepo.GetCustomersWithAppointmentWeekAsync(startOfWeek);
                return Ok(customers);
            }

            [HttpGet("sorted-filtered")]
            public async Task<ActionResult<IEnumerable<Customer>>> GetCustomersSortedAndFiltered(string sortField, string sortOrder, string filterField, string filterValue)
            {
                var customers = await _customerRepo.GetCustomersSortedAndFilteredAsync(sortField, sortOrder, filterField, filterValue);
                return Ok(customers);
            }
        }

        public static class DateTimeExtensions
        {
            public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
            {
                int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
                return dt.AddDays(-1 * diff).Date;
            }
        }
    }
}