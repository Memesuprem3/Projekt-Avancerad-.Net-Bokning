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
            var customers = await _customerRepo.GetAllAsync();
            var customerDtos = customers.Select(c => new CustomerDTO
            {
                CustomerId = c.CustomerId,
                FristName = c.FristName,
                LastName = c.LastName,
                Adress = c.Adress,
                Phone = c.Phone,
                Email = c.Email
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
                Adress = customer.Adress,
                Phone = customer.Phone,
                Email = customer.Email
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
                Adress = customerDto.Adress,
                Phone = customerDto.Phone,
                Email = customerDto.Email
            };

            var createdCustomer = await _customerRepo.AddAsync(customer);
            var createdCustomerDto = new CustomerDTO
            {
                CustomerId = createdCustomer.CustomerId,
                FristName = createdCustomer.FristName,
                LastName = createdCustomer.LastName,
                Adress = createdCustomer.Adress,
                Phone = createdCustomer.Phone,
                Email = createdCustomer.Email
            };

            return CreatedAtAction(nameof(GetCustomerById), new { id = createdCustomerDto.CustomerId }, createdCustomerDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, CustomerDTO customerDto)
        {
            if (id != customerDto.CustomerId)
            {
                return NotFound("Customer With That ID Not Found");
            }

            var customer = new Customer
            {
                CustomerId = customerDto.CustomerId,
                FristName = customerDto.FristName,
                LastName = customerDto.LastName,
                Adress = customerDto.Adress,
                Phone = customerDto.Phone,
                Email = customerDto.Email
            };

            await _customerRepo.UpdateAsync(customer);
            return Ok("Updated");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _customerRepo.DeleteAsync(id);
            if (customer == null)
            {
                return NotFound("Customer With That ID Not Found");
            }
            return Ok("Customer Deleted");
        }

        [HttpGet("{id}/appointments")]
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetCustomerAppointments(int id)
        {
            var customer = await _customerRepo.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound("No Appointments With That ID Found");
            }

            var appointmentDtos = customer.Appointments.Select(a => new AppointmentDTO
            {
                Id = a.id,
                AppointDiscription = a.AppointDiscription,
                PlacedApp = a.PlacedApp,
                CustomerId = a.CustomerId,
                CompanyId = a.CompanyId
            }).ToList();

            return Ok(appointmentDtos);
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
                Adress = c.Adress,
                Phone = c.Phone,
                Email = c.Email
            }).ToList();

            return Ok(customerDtos);
        }

        [HttpGet("sorted-filtered")]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetCustomersSortedAndFiltered(string sortField, string sortOrder, string filterField, string filterValue)
        {
            var customers = await _customerRepo.GetCustomersSortedAndFilteredAsync(sortField, sortOrder, filterField, filterValue);

            var customerDtos = customers.Select(c => new CustomerDTO
            {
                CustomerId = c.CustomerId,
                FristName = c.FristName,
                LastName = c.LastName,
                Adress = c.Adress,
                Phone = c.Phone,
                Email = c.Email
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