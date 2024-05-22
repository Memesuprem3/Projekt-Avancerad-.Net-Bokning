using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Projekt_Avancerad_.Net_Bokning.DTO;
using Projekt_Avancerad_.Net_Bokning.Services.Interface;
using Projekt_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetAllCustomers()
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
            var customerDtos = customers.Select(c => new CustomerDTO
            {
                CustomerId = c.CustomerId,
                FristName = c.FristName,
                LastName = c.LastName,
                Email = c.Email,
                Phone = c.Phone,
                Address = c.Adress
            }).ToList();
            return Ok(customerDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDTO>> GetCustomerById(int id)
        {
            var customer = await _customerRepo.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            var customerDto = new CustomerDTO
            {
                CustomerId = customer.CustomerId,
                FristName = customer.FristName,
                LastName = customer.LastName,
                Email = customer.Email,
                Phone = customer.Phone,
                Address = customer.Adress
            };
            return Ok(customerDto);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDTO>> AddCustomer(CustomerDTO customerDto)
        {
            var customer = new Customer
            {
                FristName = customerDto.FristName,
                LastName = customerDto.LastName,
                Email = customerDto.Email,
                Phone = customerDto.Phone,
                Adress = customerDto.Address
            };

            var createdCustomer = await _customerRepo.AddAsync(customer);

            customerDto.CustomerId = createdCustomer.CustomerId;

            return CreatedAtAction(nameof(GetCustomerById), new { id = customerDto.CustomerId }, customerDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, CustomerDTO customerDto)
        {
            if (id != customerDto.CustomerId)
            {
                return BadRequest();
            }

            var customer = new Customer
            {
                CustomerId = customerDto.CustomerId,
                FristName = customerDto.FristName,
                LastName = customerDto.LastName,
                Email = customerDto.Email,
                Phone = customerDto.Phone,
                Adress = customerDto.Address
            };

            await _customerRepo.UpdateAsync(customer);

            return Ok(customerDto);
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
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetCustomerAppointments(int id)
        {
            var customer = await _customerRepo.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            var appointments = customer.Appointments.Select(a => new AppointmentDTO
            {
                Id = a.id,
                AppointDiscription = a.AppointDiscription,
                PlacedApp = a.PlacedApp,
                CustomerId = a.CustomerId,
                CompanyId = a.CompanyId
            }).ToList();

            return Ok(appointments);
        }

        [HttpGet("{id}/appointments/week/{startOfWeek}")]
        public async Task<ActionResult<int>> GetCustomerAppointmentCountForWeek(int id, DateTime startOfWeek)
        {
            var count = await _customerRepo.GetCustomerAppointmentCountWeekAsync(id, startOfWeek);
            return Ok(count);
        }

        [HttpGet("appointments/current-week")]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetCustomersWithAppointmentsForWeek()
        {
            var startOfWeek = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
            var customers = await _customerRepo.GetCustomersWithAppointmentWeekAsync(startOfWeek);
            var customerDtos = customers.Select(c => new CustomerDTO
            {
                CustomerId = c.CustomerId,
                FristName = c.FristName,
                LastName = c.LastName,
                Email = c.Email,
                Phone = c.Phone,
                Address = c.Adress
            }).ToList();
            return Ok(customerDtos);
        }

        [HttpGet("sorted-filtered")]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetCustomersSortedAndFiltered(string sortField, string sortOrder, string filterField, string filterValue)
        {
            var customers = await _customerRepo.GetAllAsync();

            if (!string.IsNullOrEmpty(filterField) && !string.IsNullOrEmpty(filterValue))
            {
                switch (filterField.ToLower())
                {
                    case "firstname":
                        customers = customers.Where(c => c.FristName.Contains(filterValue, StringComparison.OrdinalIgnoreCase));
                        break;
                    case "lastname":
                        customers = customers.Where(c => c.LastName.Contains(filterValue, StringComparison.OrdinalIgnoreCase));
                        break;
                    case "email":
                        customers = customers.Where(c => c.Email.Contains(filterValue, StringComparison.OrdinalIgnoreCase));
                        break;
                    case "phone":
                        customers = customers.Where(c => c.Phone.Contains(filterValue, StringComparison.OrdinalIgnoreCase));
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
                        customers = sortOrder.ToLower() == "desc" ?
                            customers.OrderByDescending(c => c.FristName) :
                            customers.OrderBy(c => c.FristName);
                        break;
                    case "lastname":
                        customers = sortOrder.ToLower() == "desc" ?
                            customers.OrderByDescending(c => c.LastName) :
                            customers.OrderBy(c => c.LastName);
                        break;
                    case "email":
                        customers = sortOrder.ToLower() == "desc" ?
                            customers.OrderByDescending(c => c.Email) :
                            customers.OrderBy(c => c.Email);
                        break;
                    case "phone":
                        customers = sortOrder.ToLower() == "desc" ?
                            customers.OrderByDescending(c => c.Phone) :
                            customers.OrderBy(c => c.Phone);
                        break;
                    default:
                        break;
                }
            }

            var customerDtos = customers.Select(c => new CustomerDTO
            {
                CustomerId = c.CustomerId,
                FristName = c.FristName,
                LastName = c.LastName,
                Email = c.Email,
                Phone = c.Phone,
                Address = c.Adress
            }).ToList();

            return Ok(customerDtos);
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